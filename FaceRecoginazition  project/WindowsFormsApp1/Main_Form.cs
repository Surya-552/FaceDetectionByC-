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

using System.Data.SqlClient;

namespace WindowsFormsApp1
{
    public partial class Main_Form : Form
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
        
        List<string> PersonsNames = new List<string>();

        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\SURYAJI\Documents\MyDataBase.mdf;Integrated Security=True;");
        #endregion


        public Main_Form()
        {
            InitializeComponent();
            BackgroundImageLayout = ImageLayout.Stretch;
            timer1.Start();
            CaptureFn();

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime time = DateTime.Now;
            timeHM.Text = time.ToString("hh:mm");
            TimeSS.Text = time.ToString("ss");
            AMPM.Text = time.ToString("tt");
            DateLable.Text = time.ToString("dd:MM:yyyy");
            Day.Text = time.ToString("dddd");
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
        private void CaptureFn()
        {
            videoCapture = new Capture();
            //videoCapture.ImageGrabbed += ProcessFrame;
           Application.Idle += ProcessFrame;
          
            TrainImagesFromDir();
            videoCapture.Start();

        }
        private void ProcessFrame(object sender, EventArgs e)
        {
            if (videoCapture != null && videoCapture.Ptr != IntPtr.Zero)
            {//2
                videoCapture.Retrieve(frame, 0);
                currentFrame = frame.ToImage<Bgr, Byte>().Resize(pictureBox2.Width, pictureBox2.Height, Inter.Cubic);
                //step2.detected face
                faceDetectionEnabled=true;
                if (faceDetectionEnabled)
                {//3
                 //Convert from Bgr to Gray Image
                    Mat grayImage = new Mat();
                    CvInvoke.CvtColor(currentFrame, grayImage, ColorConversion.Bgr2Gray);
                    //Enhance the image to get better result
                    CvInvoke.EqualizeHist(grayImage, grayImage);
                    Rectangle[] faces = facsCasadeClassifier.DetectMultiScale(grayImage, 1.1, 3, Size.Empty, Size.Empty);
                   // Rectangle face=facsCasadeClassifier.
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
                           // picDetected.SizeMode = PictureBoxSizeMode.StretchImage;
                           // picDetected.Image = resultImage.Bitmap;

                               
                        
                            if (isTrained)
                            {
                                Image<Gray, Byte> grayFaceResult = resultImage.Convert<Gray, Byte>().Resize(200, 200, Inter.Cubic);
                                CvInvoke.EqualizeHist(grayFaceResult, grayFaceResult);
                                var result = recognizer.Predict(grayFaceResult);
                               // pictureBox1.Image = grayFaceResult.Bitmap;
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
                                        FontFace.HersheyComplex, 1.0, new Bgr(Color.Orange).MCvScalar) ; 
                                    CvInvoke.Rectangle(currentFrame, face, new Bgr(Color.Red).MCvScalar, 2);

                                }
                            }
                        }//6
                    }//7
                }//8
                pictureBox2.Image = currentFrame.Bitmap;
            }//9
            if (currentFrame != null)
                currentFrame.Dispose();
        }//10

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

                }

                if (TrainedFaces.Count() > 0)
                {
                    // recognizer = new EigenFaceRecognizer(ImagesCount,Threshold);
                    recognizer = new EigenFaceRecognizer(ImagesCount, Threshold);
                    recognizer.Train(TrainedFaces.ToArray(), PersonsLabes.ToArray());

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

        private void label1_Click(object sender, EventArgs e)
        {

            pictureBox1.Visible = false;

            groupBox3.Visible = true;
          
        }

        private void label2_Click_1(object sender, EventArgs e)
        {
             Application.Exit();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void label11_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("select * from Admin where User_name=@User_name and Password=@Password  ", con);
            cmd.Parameters.AddWithValue("@User_name", IdBox.Text);
            cmd.Parameters.AddWithValue("@Password", PsdBox.Text);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (dt.Rows.Count > 0)
            {
                Login_Reg obj = new Login_Reg();

                this.Hide();
                MessageBox.Show("Login Successfully");
                videoCapture.Dispose();
                obj.Show();

            }
            else
            {
                MessageBox.Show("Invalid User");
            }
        }

        private void PsdBox_TextChanged(object sender, EventArgs e)
        {
            PsdBox.PasswordChar = '*';
        }
    }
}
