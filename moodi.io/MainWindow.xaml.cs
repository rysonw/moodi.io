#region Packages
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using moodi.io.Views;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Face;
using Emgu.CV.CvEnum;
using System.IO;
using System.Threading;
using System.Drawing.Text;
using System.Diagnostics;
using Google.Cloud.Vision.V1;
using Newtonsoft.Json;

#endregion

namespace moodi.io
{

    public partial class MainWindow : Window
    {

        //#region Global Variables
        //private Capture vidCapture = null; //Declare a new Capture Object
        //private Image<Bgr, Byte> currentFrame = null; //Bgr is a color scheme, OpenCV uses BGR as their default. Here we are creating an image and defining that it will be in Bgr and represented as bytes
        //private bool faceDetectionEnabled = false;
        ////private string cascadeFileLocation = ""
        //public string currentPhotoPath = "";

        ////Image<Bgr, Byte> faceResult = null;
        //List<Image<Gray, Byte>> TrainedFaces = new List<Image<Gray, byte>>();
        //List<int> PersonsLabes = new List<int>();
        //List<string> PersonsNames = new List<string>();
        //bool enableSaveImage = false;
        //bool isTrained = false;

        //public string pathTrain = Directory.GetCurrentDirectory() + @"\Train_Images"; //Change directory for saving images for training
        //public string pathToSend = Directory.GetCurrentDirectory() + @"\Send_Images"; //Directory for saving images to send to Google Cloud Vision
        //public string pathToFace;
        //DirectoryInfo dirToSend = new DirectoryInfo(Directory.GetCurrentDirectory() + @"\Send_Images");
        
        //CascadeClassifier faceCascadeClassifier = new CascadeClassifier("Insert"); //Declaring Capture Rectangle; Static for now will add secret later
        //Mat frame = new Mat();
        //EigenFaceRecognizer recognizer;

        //#endregion

        #region Window cliping under task bar fix

        private static IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
            }

            return (IntPtr)0;
        }

        private static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));
            int MONITOR_DEFAULTTONEAREST = 0x00000002;
            IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
            if (monitor != IntPtr.Zero)
            {
                MONITORINFO monitorInfo = new MONITORINFO();
                GetMonitorInfo(monitor, monitorInfo);
                RECT rcWorkArea = monitorInfo.rcWork;
                RECT rcMonitorArea = monitorInfo.rcMonitor;
                mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
                mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
                mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
                mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
            }

            Marshal.StructureToPtr(mmi, lParam, true);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            /// <summary>x coordinate of point.</summary>
            public int x;

            /// <summary>y coordinate of point.</summary>
            public int y;

            /// <summary>Construct a point of coordinates (x,y).</summary>
            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MONITORINFO
        {
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
            public RECT rcMonitor = new RECT();
            public RECT rcWork = new RECT();
            public int dwFlags = 0;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
            public static readonly RECT Empty = new RECT();

            public int Width
            {
                get { return Math.Abs(right - left); }
            }

            public int Height
            {
                get { return bottom - top; }
            }

            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            public RECT(RECT rcSrc)
            {
                left = rcSrc.left;
                top = rcSrc.top;
                right = rcSrc.right;
                bottom = rcSrc.bottom;
            }

            public bool IsEmpty
            {
                get { return left >= right || top >= bottom; }
            }

            public override string ToString()
            {
                if (this == Empty)
                {
                    return "RECT {Empty}";
                }

                return "RECT { left : " + left + " / top : " + top + " / right : " + right + " / bottom : " + bottom +
                       " }";
            }

            public override bool Equals(object obj)
            {
                if (!(obj is Rect))
                {
                    return false;
                }

                return (this == (RECT)obj);
            }

            /// <summary>Return the HashCode for this struct (not garanteed to be unique)</summary>
            public override int GetHashCode() =>
                left.GetHashCode() + top.GetHashCode() + right.GetHashCode() + bottom.GetHashCode();

            /// <summary> Determine if 2 RECT are equal (deep compare)</summary>
            public static bool operator ==(RECT rect1, RECT rect2)
            {
                return (rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right &&
                        rect1.bottom == rect2.bottom);
            }

            /// <summary> Determine if 2 RECT are different(deep compare)</summary>
            public static bool operator !=(RECT rect1, RECT rect2)
            {
                return !(rect1 == rect2);
            }
        }

        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("User32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

        #endregion

        private FaceDetectionView _faceDetectionView;
        private MoodResultView _moodResultView;
        
        public MainWindow()
        {
            InitializeComponent();

            //vidCapture = new Capture();
            //vidCapture.ImageGrabbed += ProcessFrame;
            //vidCapture.Start();

            SourceInitialized += (s, e) =>
            {
                var handle = new WindowInteropHelper(this).Handle;
                HwndSource.FromHwnd(handle)?.AddHook(WindowProc);
            };

            _faceDetectionView = new FaceDetectionView(this);
            _moodResultView = new MoodResultView(this);

            ShowFaceDetection();
            //ShowMoodResults();
        }

        //private void ProcessFrame(object sender, EventArgs e) //Handles ProcessFrame which will be rendered into picture box
        //{
        //    //Step 1: Capture the Video and render it into main picture box
        //    vidCapture.Retrieve(frame, 0);
        //    currentFrame = frame.ToImage<Bgr, Byte>().Resize(CameraComponent.Width, CameraComponent.Height, Inter.Cubic);

        //    //Step 2: Facial Recognition while capturing video
        //    if (faceDetectionEnabled)
        //    {
        //        //Convert Bgr Image to a Gray Image
        //        Mat grayImage = new Mat();
        //        CvInvoke.CvtColor(currentFrame, grayImage, ColorConversion.Bgr2Gray);
        //        CvInvoke.EqualizeHist(grayImage, grayImage); //Enhancing Image for easier recognition

        //        Rectangle[] faces = faceCascadeClassifier.DetectMultiScale(grayImage, 1.1, 3, Size.Empty, Size.Empty);

        //        //Detect Faces
        //        if (faces.Length > 0)
        //        {
        //            foreach (var face in faces)
        //            {
        //                // Draw Rectangle around the faces
        //                // CvInvoke.Rectangle(currentFrame, face, new Bgr(Color.Blue).MCvScalar, 3);

        //                //Step 3: Add Person
        //                Image<Bgr, Byte> resultImage = currentFrame.Convert<Bgr, Byte>();
        //                resultImage.ROI = face;
        //                CameraComponent.SizeMode = PictureBoxSizeMode.StretchImage; //Resizing capture to fit in mini capture box on the left
        //                SamplePictureBox.Image = resultImage.Bitmap;

        //                if (enableSaveImage)
        //                {

        //                    if (!Directory.Exists(pathTrain)) //We create this directory if it does not exist 
        //                    {
        //                        Directory.CreateDirectory(pathTrain);
        //                    }

        //                    if (!Directory.Exists(pathToSend)) //We create this directory if it does not exist 
        //                    {
        //                        Directory.CreateDirectory(pathToSend);
        //                    }

        //                    ////Tasks are basically threads; Is this thread needed if we are only taking one picture?; Google Cloud API implementation starts here. Have it save image to directory then send to Google for classification
        //                    Task.Factory.StartNew(() => {
        //                        for (int i = 0; i < 10; i++)
        //                        {
        //                            //resize the image then saving it
        //                            resultImage.Resize(200, 200, Inter.Cubic).Save(pathTrain + @"\" + textPersonName.Text + "_" + DateTime.Now.ToString("dd-mm-yyyy-hh-mm-ss") + ".jpg");
        //                            Thread.Sleep(1000);
        //                        }
        //                    });



        //                    if (dirToSend.GetDirectories() == null) //Checking to see if pathSending Directory is empty
        //                    {
        //                        continue;
        //                    }
        //                    else
        //                    {
        //                        FileInfo[] files = dirToSend.GetFiles();
        //                        foreach (FileInfo file in files)
        //                        {
        //                            file.Delete();
        //                        }
        //                    }

        //                    enableSaveImage = false;

        //                    resultImage.Resize(200, 200, Inter.Cubic).Save(pathToSend + @"\" + textPersonName.Text + "_" + DateTime.Now.ToString("dd-mm-yyyy-hh-mm-ss") + ".jpg");
        //                    pathToFace = pathToSend + @"\" + textPersonName.Text + "_" + DateTime.Now.ToString("dd-mm-yyyy-hh-mm-ss") + ".jpg";




        //                    if (AddPersonButton.InvokeRequired) //Invokes make sure you are interacting with the right element in the correct thread
        //                    {
        //                        AddPersonButton.Invoke(new ThreadStart(delegate
        //                        {
        //                            AddPersonButton.Enabled = true;

        //                        }));
        //                    }



        //                    //Step 5: Recognize Known Faces
        //                    if (isTrained)
        //                    {
        //                        Image<Gray, Byte> grayFaceResult = resultImage.Convert<Gray, Byte>().Resize(200, 200, Inter.Cubic);
        //                        CvInvoke.EqualizeHist(grayFaceResult, grayFaceResult);
        //                        var result = recognizer.Predict(grayFaceResult);
        //                        Debug.WriteLine(result.Label + ". " + result.Distance);

        //                        //Here results found known faces
        //                        if (result.Label != -1 && result.Distance < 2000)
        //                        {
        //                            CvInvoke.PutText(currentFrame, PersonsNames[result.Label], new Point(face.X - 2, face.Y - 2),
        //                                FontFace.HersheyComplex, 1.0, new Bgr(Color.Blue).MCvScalar);
        //                            CvInvoke.Rectangle(currentFrame, face, new Bgr(Color.Blue).MCvScalar, 3);
        //                        }
        //                        //here results did not found any know faces
        //                        else
        //                        {
        //                            CvInvoke.PutText(currentFrame, "????", new Point(face.X - 2, face.Y - 2),
        //                                FontFace.HersheyComplex, 1.0, new Bgr(Color.Red).MCvScalar);
        //                            CvInvoke.Rectangle(currentFrame, face, new Bgr(Color.Red).MCvScalar, 3);

        //                        }
        //                    }

        //                }


        //            }
        //        }
        //    }

        //    picCapture.Image = currentFrame.Bitmap;

        //}


        private void MinimizeBTN_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeBTN_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void CloseBTN_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }


        public void ShowFaceDetection()
        {
            MainContent.Children.Clear();
            MainContent.Children.Add(_faceDetectionView);
        }


        public void ShowMoodResults()
        {
            MainContent.Children.Clear();
            MainContent.Children.Add(_moodResultView);
        }

        //private void DetectButton_Click(object sender, EventArgs e)
        //{
        //    //Step 2: Facial Recognition while capturing video; NEEDS TO BE CLICKED BEFORE ADD PERSON or ERROR
        //    faceDetectionEnabled = true;
        //}

        ////Wrap in try catch block
        //private void AddPersonButton_Click(object sender, EventArgs e)
        //{
        //    SaveAndSendButton.Enabled = true;
        //    AddPersonButton.Enabled = true;
        //    enableSaveImage = true;

        //    //Create seperate function??
        //    var client = ImageAnnotatorClient.Create();
        //    var image = Google.Cloud.Vision.V1.Image.FromFile(pathToFace); //How to save image and send it to Google Cloud API; Create stats??
        //    var response = client.DetectFaces(image).ToString(); //JSON response

        //    //System.Diagnostics.Debug.Write(response.ToString());
        //    Thread.Sleep(1000);
        //    dynamic dynJson = JsonConvert.DeserializeObject(response);

        //    //TODO: Create a one-liner for this code
        //    double confidence = Convert.ToDouble((dynJson[0]["detectionConfidence"]) * 100);
        //    string confidenceString = confidence.ToString();
        //    string splicedconfidenceString = confidenceString.Substring(0, 5);

        //    confidenceOutput.Text = $"{splicedconfidenceString}%";
        //    joyOutput.Text = $"{dynJson[0]["joyLikelihood"].ToString()}";
        //    sorrowOutput.Text = $"{dynJson[0]["sorrowLikelihood"].ToString()}";
        //    angerOutput.Text = $"{dynJson[0]["angerLikelihood"].ToString()}";
        //    surpriseOutput.Text = $"{dynJson[0]["surpriseLikelihood"].ToString()}";

        //    //Updating Pie Chart; VERY_LIKELY will get 80% of the chart, LIKELY will get 20 and be split.
        //    moodChart.Series["moods"].Points.AddXY("Joy", dynJson[0]["joyLikelihood"].ToString());
        //    moodChart.Series["moods"].Points.AddXY("Anger", dynJson[0]["sorrowLikelihood"].ToString());
        //    moodChart.Series["moods"].Points.AddXY("Sorrow", dynJson[0]["angerLikelihood"].ToString());
        //    moodChart.Series["moods"].Points.AddXY("Suprised", dynJson[0]["surpriseLikelihood"].ToString());

        //    //if ()
        //    //moodChart.Series["moods"].Points.AddXY("Neutral", 10);

        //}

        //private void SaveAndSendButton_Click(object sender, EventArgs e)
        //{
        //    SaveAndSendButton.Enabled = false;
        //    AddPersonButton.Enabled = true;
        //    enableSaveImage = false;

        //}


        //private void TrainButton_Click(object sender, EventArgs e)
        //{
        //    TrainImagesFromDirectory();
        //}



        //private bool TrainImagesFromDirectory()
        //{
        //    int imageCount = 0;
        //    double threshold = 2000;
        //    TrainedFaces.Clear();
        //    PersonsLabes.Clear();
        //    PersonsNames.Clear();

        //    try
        //    {
        //        if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\Trained_Faces")) //We create this directory if it does not exist 
        //        {
        //            Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Trained_Faces");
        //        }

        //        string pathTrained = Directory.GetCurrentDirectory() + @"\Trained_Faces";

        //        string[] files = Directory.GetFiles(pathTrained, "*.jpg", SearchOption.AllDirectories); //Creating array of all files

        //        foreach (var file in files)
        //        {
        //            Image<Gray, Byte> trainedImage = new Image<Gray, byte>(file);
        //            CvInvoke.EqualizeHist(trainedImage, trainedImage);
        //            TrainedFaces.Add(trainedImage);
        //            PersonsLabes.Add(imageCount);
        //            string name = file.Split('\\').Last().Split('_')[0];
        //            PersonsNames.Add(name);
        //            imageCount++;
        //            Debug.WriteLine(imageCount + ". " + name);
        //        }

        //        if (TrainedFaces.Count() > 0)
        //        {
        //            // recognizer = new EigenFaceRecognizer(ImagesCount,Threshold);
        //            recognizer = new EigenFaceRecognizer(imageCount, threshold);
        //            recognizer.Train(TrainedFaces.ToArray(), PersonsLabes.ToArray());
        //            isTrained = true;
        //            Debug.WriteLine(imageCount);
        //            //Debug.WriteLine(isTrained);
        //            return true;
        //        }
        //        else
        //        {
        //            isTrained = false;
        //            return false;
        //        }

        //    }

        //    catch (Exception ex)
        //    {
        //        isTrained = false;
        //        MessageBox.Show("Error in Train Images: " + ex.Message);
        //        return false;
        //    }


        //}

        //private double SetMoodPercentages()
        //{

        //}
    }
}
