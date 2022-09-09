using System;
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
using System.Diagnostics;
namespace WindowsFormsApp1
{
    public partial class Form1 : Form

    {
        #region Variables
        private Capture videoCapture = null;
        private Image<Bgr, Byte> currentFrame = null;
        Mat frame = new Mat();
        private bool faceDetectionEnabled = false;
        CascadeClassifier facsCasadeClassifier = new CascadeClassifier(@"C:\Users\SURYAJI\Desktop\C# Project\Face\WindowsFormsApp1\haarcascade_frontalface_alt.xml");
       // Image<Bgr, Byte> faceResult = null;
        List<int> PersonsLabes = new List<int>();
        private bool isTrained = false;
        EigenFaceRecognizer recognizer;
        List<Image<Gray, Byte>> TrainedFaces = new List<Image<Gray, byte>>();
        bool EnableSaveImage = false;
        List<string> PersonsNames = new List<string>();

        #endregion
        public Form1()
        {
            InitializeComponent();
        }

        private void CaptureBttn_Click(object sender, EventArgs e)
        {
            videoCapture = new Capture();
            //videoCapture.ImageGrabbed += ProcessFrame;
            Application.Idle += ProcessFrame;
            videoCapture.Start();
        }

        private void ProcessFrame(object sender, EventArgs e)
        {
            if (videoCapture != null && videoCapture.Ptr != IntPtr.Zero)
            {//2
                videoCapture.Retrieve(frame, 0);
                currentFrame = frame.ToImage<Bgr, Byte>().Resize(CaptureImg.Width, CaptureImg.Height, Inter.Cubic);
                //step2.detected face
                if (faceDetectionEnabled)
                {//3
                 //Convert from Bgr to Gray Image
                    Mat grayImage = new Mat();
                    CvInvoke.CvtColor(currentFrame, grayImage, ColorConversion.Bgr2Gray);
                    //Enhance the image to get better result
                    CvInvoke.EqualizeHist(grayImage, grayImage);
                    Rectangle[] faces = facsCasadeClassifier.DetectMultiScale(grayImage, 1.1, 3, Size.Empty, Size.Empty);
                    //if face detected
                    if (faces.Length > 0)
                    {//4
                        foreach (var face in faces)
                        {//5
                         //Draw Square around each face
                         CvInvoke.Rectangle(currentFrame, face, new Bgr(Color.Red).MCvScalar, 2);
                         //step 3: Add Person
                            Image<Bgr, Byte> resultImage = currentFrame.Convert<Bgr, Byte>();
                            resultImage.ROI = face;
                            picDetected.SizeMode = PictureBoxSizeMode.StretchImage;
                            picDetected.Image = resultImage.Bitmap;

                            if (EnableSaveImage)
                            {//6
                                //create a directry if does not exists!
                                string path = Directory.GetCurrentDirectory() + @"\TrainedImages";
                                if (!Directory.Exists(path))
                                    Directory.CreateDirectory(path);
                                Task.Factory.StartNew(() =>
                                {//7
                                    for (int i = 0; i < 10; i++)
                                    {//8
                                        resultImage.Resize(200, 200, Inter.Cubic).Save(path + @"\" + PersonName.Text + "_" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".bmp");
                                        Thread.Sleep(1000);
                                    }//1
                                    // MessageBox.Show(path);
                                });//2
                            }//3     
                            EnableSaveImage = false;
                            if (btnSave.InvokeRequired)
                            {//9
                                btnSave.Invoke(new ThreadStart(delegate
                                {//10
                                    btnSave.Enabled = true;
                                }));//4
                            }//5
                            if (isTrained)
                            {
                                Image<Gray, Byte> grayFaceResult = resultImage.Convert<Gray, Byte>().Resize(200, 200, Inter.Cubic);
                                CvInvoke.EqualizeHist(grayFaceResult, grayFaceResult);
                                var result = recognizer.Predict(grayFaceResult);
                                pictureBox1.Image = grayFaceResult.Bitmap;
                               // pictureBox2.Image = TrainedFaces[result.Label].Bitmap;
                                Debug.WriteLine(result.Label + ". " + result.Distance);
                                //Here results found known faces
                                if (result.Label != -1 && result.Distance < 2000)
                                {
                                    CvInvoke.PutText(currentFrame, PersonsNames[result.Label], new Point(face.X - 2, face.Y - 2),
                                        FontFace.HersheyComplex, 1.0, new Bgr(Color.Orange).MCvScalar);
                                    CvInvoke.Rectangle(currentFrame, face, new Bgr(Color.Green).MCvScalar, 2);
                                }
                                //here results did not found any know faces
                                else
                                {
                                    CvInvoke.PutText(currentFrame, "Unknown", new Point(face.X - 2, face.Y - 2),
                                        FontFace.HersheyComplex, 1.0, new Bgr(Color.Orange).MCvScalar);
                                    CvInvoke.Rectangle(currentFrame, face, new Bgr(Color.Red).MCvScalar, 2);

                                }
                            }
                        }//6
                    }//7
                }//8
                CaptureImg.Image = currentFrame.Bitmap;
            }//9
            if (currentFrame != null)
                currentFrame.Dispose();
        }//10
        private void CaptureImg_Click(object sender, EventArgs e)
        {

        }

        private void DetectFaceBtn_Click(object sender, EventArgs e)
        {
            faceDetectionEnabled = true;
        }



        private void btnSave_Click(object sender, EventArgs e)
        {
            //  btnSave.Enabled = false;

            EnableSaveImage = true;
        }

        private void picDetected_Click(object sender, EventArgs e)
        {

        }

        private void RecognizeBtn_Click(object sender, EventArgs e)
        {
            //isTrained = true;
        }

        private void MatchImgBtn_Click(object sender, EventArgs e)
        {
            TrainImagesFromDir();
        }
        //Step 4: train Images .. we will use the saved images from the previous example 
        private bool TrainImagesFromDir()
        {
            int ImagesCount = 0;
            double Threshold = 2000;
            TrainedFaces.Clear();
            PersonsLabes.Clear();
            PersonsNames.Clear();
            try
            {
                string path = Directory.GetCurrentDirectory() + @"\TrainedImages";
                string[] files = Directory.GetFiles(path, "*.bmp", SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    Image<Gray, byte> trainedImage = new Image<Gray, byte>(file).Resize(200, 200, Inter.Cubic);
                    CvInvoke.EqualizeHist(trainedImage, trainedImage);
                    TrainedFaces.Add(trainedImage);
                    PersonsLabes.Add(ImagesCount);
                    string name = file.Split('\\').Last().Split('_')[0];
                    PersonsNames.Add(name);
                    ImagesCount++;
                    Debug.WriteLine(ImagesCount + ". " + name);
                   // pictureBox2.Image = trainedImage.Bitmap;
                }

                if (TrainedFaces.Count() > 0)
                {
                    // recognizer = new EigenFaceRecognizer(ImagesCount,Threshold);
                    recognizer = new EigenFaceRecognizer(ImagesCount, Threshold);
                    recognizer.Train(TrainedFaces.ToArray(), PersonsLabes.ToArray());
                   //  pictureBox2.Image = TrainedFaces[13].Bitmap;

                    isTrained = true;
                    //Debug.WriteLine(ImagesCount);
                    //Debug.WriteLine(isTrained);
                    return true;
                }
                else
                {
                    isTrained = false;
                    return false;
                }
            }
            catch (Exception ex)
            {
                isTrained = false;
                MessageBox.Show("Error in Train Images: " + ex.Message);
                return false;
            }
        }
    }
}