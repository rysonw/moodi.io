# moodi.io

### Installation:

1. Go to Solution Explorer in Visual Studio
2. Click on "Restore NuGet Packages"
3. Build as x64 Platform
4. Install Emgu.CV
5. Run the application

*** Application requires a Google Cloud account and verification. For more information, please visit this link: https://cloud.google.com/vision/ ***

### Overview and Purpose:

This app is a WPF-based application that uses facial and mood detection to recommend entertainment based on your mood. Using the Emgu.CV module, we are able to use AI and build our own models to recognize faces present in the picture. This app can also take images and generate real-time mood detection info through the Google Cloud Vision API. The application displays the results and recommends music, books and movies based on your mood.
<br>

### Application Startup:

Startup Page:
![startup](https://user-images.githubusercontent.com/91065673/201472299-c957e4fc-7f39-40be-bdcf-ca38dce8059e.png)
<br>
TODO

### Face Detection:

TODO

### Mood Detection:

After the image is saved to the directory, the image is then sent to the Google Vision API for classification. Before each image is saved, the folder used to store the images to be sent is cleared.

### Results:
TODO

### Entertainment Recommendations:
TODO
