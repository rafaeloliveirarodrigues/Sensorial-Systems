using System;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;


namespace SS_OpenCV
{
    public partial class MainForm : Form
    {
        Image<Bgr, Byte> img = null; // working image
        Image<Bgr, Byte> imgUndo = null; // undo backup image - UNDO
        string title_bak = "";

        public MainForm()
        {
            InitializeComponent();
            title_bak = Text;
        }

        /// <summary>
        /// Opens a new image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                img = new Image<Bgr, byte>(openFileDialog1.FileName);
                Text = title_bak + " [" +
                        openFileDialog1.FileName.Substring(openFileDialog1.FileName.LastIndexOf("\\") + 1) +
                        "]";
                imgUndo = img.Copy();
                ImageViewer.Image = img.Bitmap;
                ImageViewer.Refresh();
            }
        }

        /// <summary>
        /// Saves an image with a new name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ImageViewer.Image.Save(saveFileDialog1.FileName);
            }
        }

        /// <summary>
        /// Closes the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// restore last undo copy of the working image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imgUndo == null) // verify if the image is already opened
                return; 
            Cursor = Cursors.WaitCursor;
            img = imgUndo.Copy();

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        /// <summary>
        /// Chaneg visualization mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoZoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // zoom
            if (autoZoomToolStripMenuItem.Checked)
            {
                ImageViewer.SizeMode = PictureBoxSizeMode.Zoom;
                ImageViewer.Dock = DockStyle.Fill;
            }
            else // with scroll bars
            {
                ImageViewer.Dock = DockStyle.None;
                ImageViewer.SizeMode = PictureBoxSizeMode.AutoSize;
            }
        }

        /// <summary>
        /// Show authors form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AuthorsForm form = new AuthorsForm();
            form.ShowDialog();
        }

        /// <summary>
        /// Calculate the image negative
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void negativeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor 

            //copy Undo Image
            imgUndo = img.Copy();

            ImageClass.Negative(img);

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        private void evalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EvalForm eval = new EvalForm();
            eval.ShowDialog();
        }

        private void grayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor 

            //copy Undo Image
            imgUndo = img.Copy();

            ImageClass.ConvertToGray(img);

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        private void redChannelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor 

            //copy Undo Image
            imgUndo = img.Copy();

            ImageClass.RedChannel(img);

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        private void brilhoEContrasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;

            InputBox form = new InputBox("brilho?");
            form.ShowDialog();
            int bright = Convert.ToInt32(form.textBoxBright.Text);
            double contrast = Convert.ToInt64(form.textBoxConstrast.Text);


            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor

            ImageClass.BrightContrast(img, bright, contrast);

            //copy Undo Image
            imgUndo = img.Copy();

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 

        }

        private void translationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;

            InputBox form = new InputBox("translact");
            form.ShowDialog();
            int posX = Convert.ToInt32(form.textBoxBright.Text);
            int posY = Convert.ToInt32(form.textBoxConstrast.Text);
            Cursor = Cursors.WaitCursor; // clock cursor

            imgUndo = img.Copy();
            ImageClass.Translation(img,imgUndo, posX, posY);
            //copy Undo Image
    

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 


        }

        private void rotationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;

            InputBox form = new InputBox("rotate");
            form.ShowDialog();
            double rot = Convert.ToInt64(form.textBoxBright.Text);
            Cursor = Cursors.WaitCursor; // clock cursor

            imgUndo = img.Copy();
            ImageClass.Rotation(img, imgUndo, rot);
            //copy Undo Image


            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 

        }

        int mouseX, mouseY;
        bool mouseFlag = false;
       
        private void zoomToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

        }

        private void simpleToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (img == null) // verify if the image is already opened
                return;

            InputBox form = new InputBox("simple_zoom");
            form.ShowDialog();
            float scaleFactor = Convert.ToSingle(form.textBoxBright.Text);
            imgUndo = img.Copy();
            ImageClass.Scale(img, imgUndo, scaleFactor);

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        private void pointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //get mouse coordinates
            mouseFlag = true;
            while (mouseFlag)//WAIT FOR MOUSE CLICK
            {
                Application.DoEvents();
            }
            if (img == null) // verify if the image is already opened
                return;



            InputBox form = new InputBox("Zoom in point");
            form.ShowDialog();
            float scaleFactor = Convert.ToSingle(form.textBoxBright.Text);
            imgUndo = img.Copy();
            ImageClass.Scale_point_xy(img, imgUndo, scaleFactor, mouseX, mouseY);


            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

     
        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void medToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor 

            //copy Undo Image
            imgUndo = img.Copy();

            ImageClass.Mean(img, imgUndo);

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor

        }

        private void nonUniformToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor 
            
            //copy Undo Image
            imgUndo = img.Copy();
            Matrix form = new Matrix();
            form.ShowDialog();

            float matrixWeight;



            float [,] matrix =  new float[3,3];
            if ((matrixWeight = Convert.ToInt64(form.textBox10.Text)) != 0)
            {
                matrix[0, 0] = Convert.ToInt32(form.textBox1.Text);
                matrix[0, 1] = Convert.ToInt32(form.textBox2.Text);
                matrix[0, 2] = Convert.ToInt32(form.textBox3.Text);
                matrix[1, 0] = Convert.ToInt32(form.textBox4.Text);
                matrix[1, 1] = Convert.ToInt32(form.textBox5.Text);
                matrix[1, 2] = Convert.ToInt32(form.textBox6.Text);
                matrix[2, 0] = Convert.ToInt32(form.textBox7.Text);
                matrix[2, 1] = Convert.ToInt32(form.textBox8.Text);
                matrix[2, 2] = Convert.ToInt32(form.textBox9.Text);
                matrixWeight = Convert.ToInt64(form.textBox10.Text);

                ImageClass.NonUniform(img, imgUndo, matrix, matrixWeight);


                ImageViewer.Image = img.Bitmap;
                ImageViewer.Refresh(); // refresh image on the screen
            }
            Cursor = Cursors.Default; // normal cursor
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void sobelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor 

            //copy Undo Image
            imgUndo = img.Copy();

            
                ImageClass.Sobel(img, imgUndo);

            ImageViewer.Image = img.Bitmap;
                ImageViewer.Refresh(); // refresh image on the screen
            
            Cursor = Cursors.Default; // normal cursor



        }

        private void medianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor 

            //copy Undo Image
            imgUndo = img.Copy();


            ImageClass.Median(img, imgUndo);

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor

        }

        private void grayToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor 
            
            int []histogram =  ImageClass.Histogram_Gray(img);

            Histogram form = new Histogram(histogram);
            form.ShowDialog();
            Cursor = Cursors.Default; // normal cursor
        }

        private void rGBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor 

            int[,] histogram = ImageClass.Histogram_RGB(img);

            histogram_rgb form = new histogram_rgb(histogram);
            form.ShowDialog();
            Cursor = Cursors.Default; // normal cursor
        }

        private void thresholdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor 
            InputBox form = new InputBox("Threshold");
            form.ShowDialog();
            int tresh = Convert.ToInt32(form.textBoxBright.Text);
            ImageClass.ConvertToBW(img, tresh);

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor
        }

        private void otsuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor 
           
           
            ImageClass.ConvertToBW_Otsu(img);

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor
        }

        private void chessRecognitionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor 
            imgUndo = img.Copy();
            System.Drawing.Rectangle BD_Location;
            char[,] Pieces = new char[64, 20];
            //ImageClass.Chess_Recognition(img, imgUndo,BD_Location,Angle,Pieces);

            ImageViewer.Image = img.Bitmap;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor
        }

        private void ImageViewer_MouseClick(object sender, MouseEventArgs e)
        {
            if (mouseFlag)
            {
                mouseX = e.X; //get x coordinate
                mouseY = e.Y; //get y coordinate

                mouseFlag = false; //unlock
            }

        }
        
    }
}