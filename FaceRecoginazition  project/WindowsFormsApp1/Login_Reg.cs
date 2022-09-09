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
    public partial class Login_Reg : Form
    {

        #region Variables
        private Capture videoCapture = null;
        private Image<Bgr, Byte> currentFrame = null;
        Mat frame = new Mat();
        private bool faceDetectionEnabled = false;
        CascadeClassifier facsCasadeClassifier = new CascadeClassifier(@"C:\Users\SURYAJI\Desktop\C# Project\Face\WindowsFormsApp1\haarcascade_frontalface_alt.xml");
       // Image<Bgr, Byte> faceResult = null;
        List<int> PersonsLabes = new List<int>();
      //  private bool isTrained = false;
      //  EigenFaceRecognizer recognizer;
        List<Image<Gray, Byte>> TrainedFaces = new List<Image<Gray, byte>>();
        bool EnableSaveImage = false;
        List<string> PersonsNames = new List<string>();
        public int Sid=0;
        #endregion


        public Login_Reg()
        {
            InitializeComponent();
            timer1.Start();
        }
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\SURYAJI\Documents\MyDataBase.mdf;Integrated Security=True;");

        private void adminLogin1_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
            timer1.Start();
            AddStudent.Visible = true;
            groupBox1.Visible = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            ViewUpdate.Visible = false;
            ReportBox.Visible = false;
            groupBox1.Visible = true;
            pictureBox1.Visible = true;
            AddStudent.Visible = false;

        }

        private void label2_Click(object sender, EventArgs e)
        {
            Main_Form obj = new Main_Form();
            this.Hide();
            obj.Show();

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("insert into Admin values(@User_name,@Name,@Mobile,@Email,@Password,@Date)", con);
            cmd.Parameters.AddWithValue("@User_name", U_ID.Text);
            cmd.Parameters.AddWithValue("@Name", NameBox.Text);

            cmd.Parameters.AddWithValue("@Mobile", MobileNO.Text);

            cmd.Parameters.AddWithValue("@Email", EmailID.Text);

            cmd.Parameters.AddWithValue("@Password", PasswordBox.Text);

            cmd.Parameters.AddWithValue("@Date", DateBox.Text);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            if (i != 0)
            {
                MessageBox.Show(i + " Data Saved");
                con.Close();
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void PasswordBox_TextChanged(object sender, EventArgs e)
        {
            PasswordBox.PasswordChar = '*';

        }

        private void DateBox_TextChanged(object sender, EventArgs e)
        {

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void Login_Reg_Load(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {
            
            Application.Exit();
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            DateTime time = DateTime.Now;
            DateBox.Text = time.ToString("dd-MM-yyyy");
        }

        private void label14_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("insert into Register_User values(@Roll_No,@Full_Name,@Course,@contact,@Email,@Address,@Date) ",con);
            cmd.Parameters.AddWithValue("@Roll_No", RollNoBox.Text);
            cmd.Parameters.AddWithValue("@Full_Name", SNameBox.Text);

            cmd.Parameters.AddWithValue("@Course", CourseBox.Text);

            cmd.Parameters.AddWithValue("@contact",MobileNoBox.Text);

            cmd.Parameters.AddWithValue("@Email", EmailBox.Text);

            cmd.Parameters.AddWithValue("@Address",AddressBox.Text);
            DateTime time = DateTime.Now;
            string dt = time.ToString("dd-MM-yyyy hh:mm:ss");
            cmd.Parameters.AddWithValue("@Date", dt);
            con.Open();

            int i = cmd.ExecuteNonQuery();
            if (i != 0)
            {
                EnableSaveImage = true;
                con.Close();
            }
        }

        private void label22_Click(object sender, EventArgs e)
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
                currentFrame = frame.ToImage<Bgr, Byte>().Resize(CaptureImgBox.Width, CaptureImgBox.Height, Inter.Cubic);
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
                            DetectImg.SizeMode = PictureBoxSizeMode.StretchImage;
                            DetectImg.Image = resultImage.Bitmap;

                            if (EnableSaveImage)
                            {//6
                                label14.Enabled = false;

                                //create a directry if does not exists!
                                string path = Directory.GetCurrentDirectory() + @"\TrainedImages";
                                if (!Directory.Exists(path))
                                    Directory.CreateDirectory(path);
                                Task.Factory.StartNew(() =>
                                {//7
                                    
                                    for (int i = 0; i < 5; i++)
                                    {//8
                                        resultImage.Resize(200, 200, Inter.Cubic).Save(path + @"\" + SNameBox.Text + "_" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".bmp");
                                        Thread.Sleep(1000);
                                        if (i == 4)
                                        {
                                            MessageBox.Show(i + " Image with data Saved");

                                        }
                                    }//1
                                   
                                    // MessageBox.Show(path);
                                });//2

                            }//3     
                            EnableSaveImage = false;
                            if (label14.InvokeRequired)
                            {//9
                                label14.Invoke(new ThreadStart(delegate
                                {//10
                                    label14.Enabled = true;
                                }));//4
                            }//5
                           
                        }//6
                    }//7
                }//8
                CaptureImgBox.Image = currentFrame.Bitmap;
            }//9
            if (currentFrame != null)
                currentFrame.Dispose();
        }//10

        private void label23_Click(object sender, EventArgs e)
        {
            RollNoBox.Clear();
            SNameBox.Clear();
            CourseBox.Clear();
            MobileNoBox.Clear();
            EmailBox.Clear();
            AddressBox.Clear();
            CaptureImgBox.Image = null;
            DetectImg.Image = null;
            faceDetectionEnabled = false;
        }

        private void label12_Click(object sender, EventArgs e)
        {
            ViewUpdate.Visible = false;
            ReportBox.Visible = false;
            groupBox1.Visible = false;
            pictureBox1.Visible = false;
            AddStudent.Visible = true;
        }

        private void label21_Click(object sender, EventArgs e)
        {
            faceDetectionEnabled = true;
        }

        private void Login_Reg_Load_1(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'myDataBaseDataSet.Register_User' table. You can move, or remove it, as needed.
            this.register_UserTableAdapter.Fill(this.myDataBaseDataSet.Register_User);
            // TODO: This line of code loads data into the 'myDataBaseStudentDataSet.Register_User' table. You can move, or remove it, as needed.

            SData.ForeColor = Color.Red;
        }
        public void ViewData()
        {


            this.register_UserTableAdapter.Fill(this.myDataBaseDataSet.Register_User);


        }
        private void label11_Click(object sender, EventArgs e)
        {
            ViewUpdate.Visible = true;
            ReportBox.Visible = false;
            groupBox1.Visible = false;
            pictureBox1.Visible = false;
            AddStudent.Visible = false;
        }

        private void SData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void SData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Sid = Convert.ToInt32(SData.SelectedRows[0].Cells[0].Value);
            textBoxRollno.Text = SData.SelectedRows[0].Cells[1].Value.ToString();
             textBoxName.Text= SData.SelectedRows[0].Cells[2].Value.ToString();

            textBoxCourse.Text= SData.SelectedRows[0].Cells[3].Value.ToString();

            Contact.Text= SData.SelectedRows[0].Cells[4].Value.ToString();

            textBoxEmail.Text= SData.SelectedRows[0].Cells[5].Value.ToString();

            textBoxAddress.Text= SData.SelectedRows[0].Cells[6].Value.ToString();

        }

        private void Update_Click(object sender, EventArgs e)
        {
            if (Sid > 0)
            {

                SqlCommand cmd = new SqlCommand("UPDATE Register_User SET Roll_No=@Roll_No1,Full_Name=@Full_Name1,Course=@Course1,contact=@contact1,Email=@Email1,Address=@Address1 WHERE Id=@Stid  ", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Roll_No1", textBoxRollno.Text);
                cmd.Parameters.AddWithValue("@Full_Name1", textBoxName.Text);

                cmd.Parameters.AddWithValue("@Course1", textBoxCourse.Text);

                cmd.Parameters.AddWithValue("@contact1", Contact.Text);

                cmd.Parameters.AddWithValue("@Email1", textBoxEmail.Text);

                cmd.Parameters.AddWithValue("@Address1", textBoxAddress.Text);

                cmd.Parameters.AddWithValue("@Stid", this.Sid);
                con.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    MessageBox.Show("Data Update Successfully", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ViewData();
                    ResetFn();
                    con.Close();
                }
            }
            else
            {
                MessageBox.Show("Plese Select an Student to Update", "Select?", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        public void ResetFn()
        {
            textBoxRollno.Clear();
            textBoxName.Clear();
            textBoxCourse.Clear();
            Contact.Clear();
            textBoxEmail.Clear();
            textBoxAddress.Clear();
            Sid = 0;
        }

        private void label3_Click_1(object sender, EventArgs e)
        {
            ReportBox.Visible = true;
            ViewUpdate.Visible = false;
            
            groupBox1.Visible = false;
            pictureBox1.Visible = false;
            AddStudent.Visible = false;

        }

        private void DeleteData_Click(object sender, EventArgs e)
        {
            if (Sid > 0)
            {

                SqlCommand cmd = new SqlCommand("delete from Register_User  WHERE Id=@Stid  ", con);
                cmd.CommandType = CommandType.Text;
                

                cmd.Parameters.AddWithValue("@Stid", this.Sid);
                con.Open();
                int i = cmd.ExecuteNonQuery();
                if (i != 0)
                {
                    MessageBox.Show("Student Deleted Successfully", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ViewData();
                    ResetFn();
                    con.Close();
                }
            }
            else
            {
                MessageBox.Show("Plese Select an Student to Update", "Select?", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            ResetFn();
        }

        private void label26_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label25_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
