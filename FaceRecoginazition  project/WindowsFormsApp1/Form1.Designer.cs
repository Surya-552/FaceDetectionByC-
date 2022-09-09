
namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.CaptureImg = new System.Windows.Forms.PictureBox();
            this.CaptureBttn = new System.Windows.Forms.Button();
            this.DetectFaceBtn = new System.Windows.Forms.Button();
            this.picDetected = new System.Windows.Forms.PictureBox();
            this.PersonName = new System.Windows.Forms.TextBox();
            this.MatchImgBtn = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.CaptureImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDetected)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // CaptureImg
            // 
            this.CaptureImg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CaptureImg.Location = new System.Drawing.Point(16, 12);
            this.CaptureImg.Name = "CaptureImg";
            this.CaptureImg.Size = new System.Drawing.Size(448, 367);
            this.CaptureImg.TabIndex = 0;
            this.CaptureImg.TabStop = false;
            this.CaptureImg.Click += new System.EventHandler(this.CaptureImg_Click);
            // 
            // CaptureBttn
            // 
            this.CaptureBttn.Location = new System.Drawing.Point(491, 30);
            this.CaptureBttn.Name = "CaptureBttn";
            this.CaptureBttn.Size = new System.Drawing.Size(141, 38);
            this.CaptureBttn.TabIndex = 1;
            this.CaptureBttn.Text = "CaptureBtn";
            this.CaptureBttn.UseVisualStyleBackColor = true;
            this.CaptureBttn.Click += new System.EventHandler(this.CaptureBttn_Click);
            // 
            // DetectFaceBtn
            // 
            this.DetectFaceBtn.Location = new System.Drawing.Point(491, 74);
            this.DetectFaceBtn.Name = "DetectFaceBtn";
            this.DetectFaceBtn.Size = new System.Drawing.Size(141, 39);
            this.DetectFaceBtn.TabIndex = 2;
            this.DetectFaceBtn.Text = "Detect Faces";
            this.DetectFaceBtn.UseVisualStyleBackColor = true;
            this.DetectFaceBtn.Click += new System.EventHandler(this.DetectFaceBtn_Click);
            // 
            // picDetected
            // 
            this.picDetected.Location = new System.Drawing.Point(491, 134);
            this.picDetected.Name = "picDetected";
            this.picDetected.Size = new System.Drawing.Size(141, 118);
            this.picDetected.TabIndex = 3;
            this.picDetected.TabStop = false;
            this.picDetected.Click += new System.EventHandler(this.picDetected_Click);
            // 
            // PersonName
            // 
            this.PersonName.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.PersonName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PersonName.Location = new System.Drawing.Point(488, 279);
            this.PersonName.Name = "PersonName";
            this.PersonName.Size = new System.Drawing.Size(144, 22);
            this.PersonName.TabIndex = 4;
            // 
            // MatchImgBtn
            // 
            this.MatchImgBtn.Location = new System.Drawing.Point(491, 364);
            this.MatchImgBtn.Name = "MatchImgBtn";
            this.MatchImgBtn.Size = new System.Drawing.Size(141, 31);
            this.MatchImgBtn.TabIndex = 5;
            this.MatchImgBtn.Text = "Match Images";
            this.MatchImgBtn.UseVisualStyleBackColor = true;
            this.MatchImgBtn.Click += new System.EventHandler(this.MatchImgBtn_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(491, 327);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(138, 31);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(472, 398);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(80, 88);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(558, 399);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(80, 88);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 7;
            this.pictureBox2.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(660, 555);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.MatchImgBtn);
            this.Controls.Add(this.PersonName);
            this.Controls.Add(this.picDetected);
            this.Controls.Add(this.DetectFaceBtn);
            this.Controls.Add(this.CaptureBttn);
            this.Controls.Add(this.CaptureImg);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Face Recognization System";
            ((System.ComponentModel.ISupportInitialize)(this.CaptureImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDetected)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox CaptureImg;
        private System.Windows.Forms.Button CaptureBttn;
        private System.Windows.Forms.Button DetectFaceBtn;
        private System.Windows.Forms.PictureBox picDetected;
        private System.Windows.Forms.TextBox PersonName;
        private System.Windows.Forms.Button MatchImgBtn;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}

