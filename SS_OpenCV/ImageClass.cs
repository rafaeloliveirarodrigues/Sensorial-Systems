using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Linq;
using System.Drawing;

namespace SS_OpenCV
{
    class ImageClass
    {
        private static int minIndex;
        private static int x_origin_int;
        private static int y_origin_int;
        private static int sum_y;
        private static int sum_x;
        private static int y_origin;
        private static int x_origin;
        private static int offsetX;
        private static int offsetY;
        private static int blue;
        private static int green;
        private static int red;

        /// <summary>
        /// Image Negative using EmguCV library
        /// Slower method
        /// </summary>
        /// <param name="img">Image</param>
        public static void Negative(Image<Bgr, byte> img)
        {
            unsafe
            {
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // numero de canais 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)
                int x, y;


                for (y = 0; y < img.Height; y++)
                {
                    for (x = 0; x < img.Width; x++)
                    {

                        // store in the image
                        dataPtr[0] = (byte)(255 - dataPtr[0]);
                        dataPtr[1] = (byte)(255 - dataPtr[1]);
                        dataPtr[2] = (byte)(255 - dataPtr[2]);

                        // advance the pointer to the next pixel
                        dataPtr += nChan;
                    }
                    //at the end of the line advance the pointer by the aligment bytes (padding)
                    dataPtr += padding;
                }
            }
        }
        /// <summary>
        /// Convert to gray
        /// Direct access to memory
        /// </summary>
        /// <param name="img">image</param>
        public static void ConvertToGray(Image<Bgr, byte> img)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte blue, green, red, gray;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            //obtém as 3 componentes
                            blue = dataPtr[0];
                            green = dataPtr[1];
                            red = dataPtr[2];

                            // convert to gray
                            gray = (byte)(((int)blue + green + red) / 3);

                            // store in the image
                            dataPtr[0] = gray;
                            dataPtr[1] = gray;
                            dataPtr[2] = gray;

                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }
            }
        }

        public static void RedChannel(Image<Bgr, byte> img)
        {
            unsafe
            {
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte blue, green, red;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            //obtém as 3 componentes
                            blue = dataPtr[0];
                            green = dataPtr[1];
                            red = dataPtr[2];

                            // store in the image
                            dataPtr[0] = red;
                            dataPtr[1] = red;


                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }



            }
        }

        public static void BrightContrast(Image<Bgr, byte> img, int bright, double contrast)
        {
            unsafe
            {
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte blue, green, red;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            //obtém as 3 componentes
                            blue = dataPtr[0];
                            green = dataPtr[1];
                            red = dataPtr[2];

                            double newBlue = ((blue * contrast) + bright);
                            double newGreen = ((green * contrast) + bright);
                            double newRed = ((red * contrast) + bright);

                            if (newBlue > 255)
                                newBlue = 255;
                            if (newBlue < 0)
                                newBlue = 0;

                            if (newGreen > 255)
                                newGreen = 255;
                            if (newGreen < 0)
                                newGreen = 0;

                            if (newRed > 255)
                                newRed = 255;
                            if (newRed < 0)
                                newRed = 0;

                            dataPtr[0] = (byte)newBlue;
                            dataPtr[1] = (byte)newGreen;
                            dataPtr[2] = (byte)newRed;


                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }

            }


        }


        public static void Translation(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, int dX, int dY)
        {
            unsafe
            {
                MIplImage mDest = img.MIplImage;
                MIplImage mOrig = imgCopy.MIplImage;

                byte* dataPtrDest = (byte*)mDest.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrOrig = (byte*)mOrig.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrOrigAux;




                int width = img.Width;
                int height = img.Height;
                int nChan = mOrig.nChannels; // number of channels - 3
                int padding = mOrig.widthStep - mOrig.nChannels * mOrig.width; // alinhament bytes (padding)
                int x, y, y0, x0;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            y0 = y - dY;
                            x0 = x - dX;

                            if (y0 > 0 && y0 < height && x0 > 0 && x0 < width)
                            {
                                dataPtrOrigAux = dataPtrOrig + y0 * mOrig.widthStep + (x0) * mOrig.nChannels;
                                dataPtrDest[0] = (dataPtrOrigAux)[0];
                                dataPtrDest[1] = (dataPtrOrigAux)[1];
                                dataPtrDest[2] = (dataPtrOrigAux)[2];
                            }
                            else
                            {
                                dataPtrDest[0] = 0;
                                dataPtrDest[1] = 0;
                                dataPtrDest[2] = 0;
                            }

                            //at the end of the line advance the pointer by the aligment bytes (padding)
                            //dataPtrOrig += mOrig.nChannels;
                            dataPtrDest += mDest.nChannels;
                        }
                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtrDest += padding;
                    }

                }
            }
        }
        /*
        public static void Rotation(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, double rot)
        {
            unsafe
            {
                MIplImage mDest = img.MIplImage;
                MIplImage mOrig = imgCopy.MIplImage;

                byte* dataPtrDest = (byte*)mDest.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrOrig = (byte*)mOrig.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrOrigAux;

                //rot = rot * (Math.PI / 180);


                int width = img.Width;
                int height = img.Height;
                int nChan = mOrig.nChannels; // number of channels - 3
                int padding = mOrig.widthStep - mOrig.nChannels * mOrig.width; // alinhament bytes (padding)
                int x, y, y0, x0;
                int halfwidth = width / 2;
                int halfheigth = height / 2;
                //rot = rot * (180.0 / Math.PI);
                double cosrot = Math.Cos(rot);
                double senrot = Math.Sin(rot);

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            x0 = (int)Math.Round((x - halfwidth) * cosrot - (halfheigth - y) * senrot+ halfwidth);
                            y0 = (int)Math.Round(halfheigth - (x - halfheigth) * senrot - (halfheigth - y) * cosrot);

                            if (x0 < 0 || y0< 0 || x0 >= width || y0 >= height)
                            {
                                dataPtrDest[0] = 0;
                                dataPtrDest[1] = 0;
                                dataPtrDest[2] = 0;
                                
                            }
                            else
                            {
                                dataPtrOrigAux = dataPtrOrig + y0 * mOrig.widthStep + (x0) * mOrig.nChannels;
                                dataPtrDest[0] = (dataPtrOrigAux)[0];
                                dataPtrDest[1] = (dataPtrOrigAux)[1];
                                dataPtrDest[2] = (dataPtrOrigAux)[2];
                            }

                            //at the end of the line advance the pointer by the aligment bytes (padding)
                           
                            dataPtrDest += mDest.nChannels;
                        }
                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtrDest += padding;
                    }

                }
            }
        }*/

        public static void Rotation(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, double angle)
        {
            unsafe
            {

                MIplImage m = img.MIplImage;
                MIplImage mOrig = imgCopy.MIplImage;

                byte* dataPtrDest = (byte*)m.imageData.ToPointer(); // Pointer to the begining of the destiny image
                byte* dataPtrOrig = (byte*)mOrig.imageData.ToPointer();// Pointer to the begining of the original image

                int width = img.Width;
                int height = img.Height; //numero de pixeis que a imagem ocupas
                double halfWidth = width / 2.0;
                double halfHeight = height / 2.0;
                double cosAngle = Math.Cos(angle);
                double sinAngle = Math.Sin(angle);

                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes, acerto (padding)
                int xDestino, yDestino, xOrigem, yOrigem;
                int widthStep = m.widthStep;


                if (nChan == 3) // image in RGB
                {
                    for (yDestino = 0; yDestino < height; yDestino++)
                    {
                        for (xDestino = 0; xDestino < width; xDestino++)
                        {

                            xOrigem = (int)Math.Round((xDestino - halfWidth) * cosAngle - (halfHeight - yDestino) * sinAngle + (halfWidth));
                            yOrigem = (int)Math.Round(halfHeight - (xDestino - halfWidth) * sinAngle - (halfHeight - yDestino) * cosAngle);


                            if ((xOrigem < 0) || (yOrigem < 0) || xOrigem >= width || yOrigem >= height)
                            {
                                //por o pixel a preto
                                dataPtrDest[0] = (byte)0;
                                dataPtrDest[1] = (byte)0;
                                dataPtrDest[2] = (byte)0;
                            }
                            else
                            {

                                ////Colocar o Pixel(RGB) no ponto (x,y) definido
                                dataPtrDest[0] = (dataPtrOrig + (yOrigem * widthStep) + (xOrigem * nChan))[0];
                                dataPtrDest[1] = (dataPtrOrig + (yOrigem * widthStep) + (xOrigem * nChan))[1];
                                dataPtrDest[2] = (dataPtrOrig + (yOrigem * widthStep) + (xOrigem * nChan))[2];
                            }

                            // advance the pointer to the next pixel
                            dataPtrDest += nChan;

                            //at the end of the line advance the pointer by the aligment bytes (padding)
                        }
                        dataPtrDest += padding;
                    }

                }

            }


        }

        public static void Scale(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float scaleFactor)
        {
            unsafe
            {
                MIplImage mDest = img.MIplImage;
                MIplImage mOrig = imgCopy.MIplImage;

                byte* dataPtrDest = (byte*)mDest.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrOrig = (byte*)mOrig.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrOrigAux;



                int width = img.Width;
                int height = img.Height;
                int nChan = mOrig.nChannels; // number of channels - 3
                int padding = mOrig.widthStep - mOrig.nChannels * mOrig.width; // alinhament bytes (padding)
                int x, y, newX, newY;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {

                            newX = (int)Math.Round(x / scaleFactor);
                            newY = (int)Math.Round(y / scaleFactor);

                            if (newY > 0 && newY < height && newX > 0 && newX < width)
                            {
                                dataPtrOrigAux = dataPtrOrig + newY * mOrig.widthStep + newX * mOrig.nChannels;
                                dataPtrDest[0] = (dataPtrOrigAux)[0];
                                dataPtrDest[1] = (dataPtrOrigAux)[1];
                                dataPtrDest[2] = (dataPtrOrigAux)[2];
                            }
                            else
                            {
                                dataPtrDest[0] = 0;
                                dataPtrDest[1] = 0;
                                dataPtrDest[2] = 0;
                            }

                            //at the end of the line advance the pointer by the aligment bytes (padding)
                            //dataPtrOrig += mOrig.nChannels;
                            dataPtrDest += mDest.nChannels;
                        }
                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtrDest += padding;
                    }

                }
            }

        }

        public static void Scale_point_xy(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float scaleFactor, int xP, int yP)
        {
            unsafe
            {
                MIplImage mDest = img.MIplImage;
                MIplImage mOrig = imgCopy.MIplImage;

                byte* dataPtrDest = (byte*)mDest.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrOrig = (byte*)mOrig.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrOrigAux;


                int width = img.Width;
                int height = img.Height;
                int nChan = mOrig.nChannels; // number of channels - 3
                int padding = mOrig.widthStep - mOrig.nChannels * mOrig.width; // alinhament bytes (padding)
                int x, y, newX, newY;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            newX = (int)Math.Round((x / scaleFactor) + xP - ((width / 2) / scaleFactor));
                            newY = (int)Math.Round((y / scaleFactor) + yP - ((height / 2) / scaleFactor));

                            if (newY > 0 && newY < height && newX > 0 && newX < width)
                            {
                                dataPtrOrigAux = dataPtrOrig + newY * mOrig.widthStep + newX * mOrig.nChannels;
                                dataPtrDest[0] = (dataPtrOrigAux)[0];
                                dataPtrDest[1] = (dataPtrOrigAux)[1];
                                dataPtrDest[2] = (dataPtrOrigAux)[2];
                            }
                            else
                            {
                                dataPtrDest[0] = 0;
                                dataPtrDest[1] = 0;
                                dataPtrDest[2] = 0;
                            }

                            //at the end of the line advance the pointer by the aligment bytes (padding)
                            //dataPtrOrig += mOrig.nChannels;
                            dataPtrDest += mDest.nChannels;
                        }
                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtrDest += padding;
                    }

                }
            }

        }

        //Versao mais rapida (completa)
        // Filtro 3x3, 
        public static void Mean(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {
            unsafe
            {

                MIplImage m = img.MIplImage;
                MIplImage mOrig = imgCopy.MIplImage;

                byte* dataPtrDest = (byte*)m.imageData.ToPointer(); // Pointer to the begining of the destiny image
                byte* dataPtrOrig = (byte*)mOrig.imageData.ToPointer();// Pointer to the begining of the original image
                byte* dataPtrAux;

                int width = img.Width;
                int height = img.Height; //numero de pixeis que a imagem ocupas
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes, acerto (padding)

                int xDestino, yDestino;
                int widthStep = m.widthStep; //
                int somaB, somaG, somaR, somaRGB, i, j;

                dataPtrDest += widthStep + nChan;
                if (nChan == 3) // image in RGB
                {
                    //Processar os pixeis no interior 
                    for (yDestino = 1; yDestino < height - 1; yDestino++)
                    {
                        for (xDestino = 1; xDestino < width - 1; xDestino++)
                        {
                            somaB = 0;
                            somaG = 0;
                            somaR = 0;

                            dataPtrAux = dataPtrOrig + ((yDestino - 1) * widthStep) + ((xDestino - 1) * nChan);


                            for (j = 0; j < 3; j++) // Fitlro 3x3 ou outra dimensao qualquer
                            {
                                for (i = 0; i < 3; i++)
                                {
                                    somaB += (dataPtrAux + i * nChan)[0];
                                    somaG += (dataPtrAux + i * nChan)[1];
                                    somaR += (dataPtrAux + i * nChan)[2];
                                }
                                dataPtrAux += widthStep;
                            }

                            ////Colocar o Pixel(RGB) no ponto (x,y) definido pixel do meio
                            dataPtrDest[0] = (byte)Math.Round(somaB / 9.0);
                            dataPtrDest[1] = (byte)Math.Round(somaG / 9.0);
                            dataPtrDest[2] = (byte)Math.Round(somaR / 9.0);


                            // advance the pointer to the next pixel
                            dataPtrDest += nChan;

                        }
                        dataPtrDest += padding + 2 * nChan; //Por causa das margens

                    }

                    dataPtrDest = (byte*)m.imageData.ToPointer();
                    dataPtrOrig = (byte*)mOrig.imageData.ToPointer();// Pointer to the begining of the original image
                    dataPtrAux = dataPtrOrig;

                    //canto (0,0)

                    //somaRGB = 0;
                    //for (i = 0; i < 3; i++)
                    //{
                    //    somaRGB = 4 * (byte)(dataPtrAux)[i];
                    //    somaRGB += 2 * (byte)(dataPtrAux + nChan)[i];
                    //    somaRGB += 2 * (byte)(dataPtrAux + widthStep)[i];
                    //    somaRGB += (byte)(dataPtrAux + nChan + widthStep)[i];

                    //    dataPtrDest[i] = (byte)Math.Round(somaRGB / 9.0);
                    //}


                    //canto (0,0)
                    dataPtrDest[0] = (byte)((4 * dataPtrAux[0] + 2 * (dataPtrAux + nChan)[0] + 2 * (dataPtrAux + widthStep)[0] + (dataPtrAux + nChan + widthStep)[0]) / 9.0);
                    dataPtrDest[1] = (byte)((4 * dataPtrAux[1] + 2 * (dataPtrAux + nChan)[1] + 2 * (dataPtrAux + widthStep)[1] + (dataPtrAux + nChan + widthStep)[1]) / 9.0);
                    dataPtrDest[2] = (byte)((4 * dataPtrAux[2] + 2 * (dataPtrAux + nChan)[2] + 2 * (dataPtrAux + widthStep)[2] + (dataPtrAux + nChan + widthStep)[2]) / 9.0);



                    //Linha de Topo
                    dataPtrDest += nChan;
                    dataPtrAux += nChan;
                    somaRGB = 0;

                    for (i = 1; i < (width - 1); i++)
                    {
                        for (j = 0; j < 3; j++)
                        {
                            somaRGB = 2 * (byte)(dataPtrAux - nChan)[j];
                            somaRGB += (byte)(dataPtrAux - nChan + widthStep)[j];
                            somaRGB += 2 * (byte)(dataPtrAux)[j];
                            somaRGB += (byte)(dataPtrAux + widthStep)[j];
                            somaRGB += 2 * (byte)(dataPtrAux + nChan)[j];
                            somaRGB += (byte)(dataPtrAux + nChan + widthStep)[j];

                            dataPtrDest[j] = (byte)Math.Round(somaRGB / 9.0);

                        }

                        //dataPtrDest[0] = (byte)((dataPtrAux[0 - nChan] * 2 + dataPtrAux[0] * 2 + dataPtrAux[0 + nChan] * 2 + dataPtrAux[0 - nChan + widthStep] + dataPtrAux[0 + widthStep] + dataPtrAux[0 + nChan + widthStep]) / 9.0);
                        //dataPtrDest[1] = (byte)((dataPtrAux[1 - nChan] * 2 + dataPtrAux[1] * 2 + dataPtrAux[1 + nChan] * 2 + dataPtrAux[1 - nChan + widthStep] + dataPtrAux[1 + widthStep] + dataPtrAux[1 + nChan + widthStep]) / 9.0);
                        //dataPtrDest[2] = (byte)((dataPtrAux[2 - nChan] * 2 + dataPtrAux[2] * 2 + dataPtrAux[2 + nChan] * 2 + dataPtrAux[2 - nChan + widthStep] + dataPtrAux[2 + widthStep] + dataPtrAux[2 + nChan + widthStep]) / 9.0);

                        //dataPtrDest[0] = (byte)(((2 * (dataPtrAux - nChan)[0]) + (2 * dataPtrAux[0]) + (2 * (dataPtrAux + nChan)[0]) + (dataPtrAux - nChan + widthStep)[0]) + (dataPtrAux + widthStep)[0] + (dataPtrAux + nChan + widthStep)[0] / 9.0);
                        //dataPtrDest[1] = (byte)(((2 * (dataPtrAux - nChan)[1]) + (2 * dataPtrAux[1]) + (2 * (dataPtrAux + nChan)[1]) + (dataPtrAux - nChan + widthStep)[1]) + (dataPtrAux + widthStep)[1] + (dataPtrAux + nChan + widthStep)[1] / 9.0);
                        //dataPtrDest[2] = (byte)(((2 * (dataPtrAux - nChan)[2]) + (2 * dataPtrAux[2]) + (2 * (dataPtrAux + nChan)[2]) + (dataPtrAux - nChan + widthStep)[2]) + (dataPtrAux + widthStep)[2] + (dataPtrAux + nChan + widthStep)[2] / 9.0);

                        dataPtrDest += nChan;
                        dataPtrAux += nChan;
                    }


                    //canto (width,0)

                    //somaRGB = 0;
                    //for (i = 0; i < 3; i++)
                    //{
                    //    somaRGB = 4 * (byte)(dataPtrAux)[i];
                    //    somaRGB += 2 * (byte)(dataPtrAux - nChan)[i];
                    //    somaRGB += 2 * (byte)(dataPtrAux + widthStep)[i];
                    //    somaRGB += (byte)(dataPtrAux - nChan + widthStep)[i];

                    //    dataPtrDest[i] = (byte)Math.Round(somaRGB / 9.0);

                    //}
                    //canto (width,0)
                    dataPtrDest[0] = (byte)((4 * dataPtrAux[0] + 2 * (dataPtrAux - nChan)[0] + 2 * (dataPtrAux + widthStep)[0] + (dataPtrAux - nChan + widthStep)[0]) / 9.0);
                    dataPtrDest[1] = (byte)((4 * dataPtrAux[1] + 2 * (dataPtrAux - nChan)[1] + 2 * (dataPtrAux + widthStep)[1] + (dataPtrAux - nChan + widthStep)[1]) / 9.0);
                    dataPtrDest[2] = (byte)((4 * dataPtrAux[2] + 2 * (dataPtrAux - nChan)[2] + 2 * (dataPtrAux + widthStep)[2] + (dataPtrAux - nChan + widthStep)[2]) / 9.0);


                    //linha da direita
                    dataPtrDest += widthStep;
                    dataPtrAux += widthStep;

                    for (i = 1; i < (height - 1); i++)
                    {
                        for (j = 0; j < 3; j++)
                        {
                            somaRGB = 2 * (byte)(dataPtrAux - widthStep)[j];
                            somaRGB += (byte)(dataPtrAux - widthStep - nChan)[j];
                            somaRGB += 2 * (byte)(dataPtrAux)[j];
                            somaRGB += (byte)(dataPtrAux - nChan)[j];
                            somaRGB += 2 * (byte)(dataPtrAux + widthStep)[j];
                            somaRGB += (byte)(dataPtrAux - nChan + widthStep)[j];

                            dataPtrDest[j] = (byte)Math.Round(somaRGB / 9.0);

                        }
                        dataPtrDest += widthStep;
                        dataPtrAux += widthStep;
                    }


                    //canto (width,height)
                    //somaRGB = 0;
                    //for (i = 0; i < 3; i++)
                    //{
                    //    somaRGB = 4 * (byte)(dataPtrAux)[i];
                    //    somaRGB += 2 * (byte)(dataPtrAux - nChan)[i];
                    //    somaRGB += 2 * (byte)(dataPtrAux - widthStep)[i];
                    //    somaRGB += (byte)(dataPtrAux - nChan - widthStep)[i];

                    //    dataPtrDest[i] = (byte)Math.Round(somaRGB / 9.0);

                    //}
                    dataPtrDest[0] = (byte)((4 * dataPtrAux[0] + 2 * (dataPtrAux - nChan)[0] + 2 * (dataPtrAux - widthStep)[0] + (dataPtrAux - nChan - widthStep)[0]) / 9.0);
                    dataPtrDest[1] = (byte)((4 * dataPtrAux[1] + 2 * (dataPtrAux - nChan)[1] + 2 * (dataPtrAux - widthStep)[1] + (dataPtrAux - nChan - widthStep)[1]) / 9.0);
                    dataPtrDest[2] = (byte)((4 * dataPtrAux[2] + 2 * (dataPtrAux - nChan)[2] + 2 * (dataPtrAux - widthStep)[2] + (dataPtrAux - nChan - widthStep)[2]) / 9.0);


                    //linha de baixo
                    dataPtrDest -= nChan;
                    dataPtrAux -= nChan;

                    for (i = 1; i < (width - 1); i++)
                    {
                        for (j = 0; j < 3; j++)
                        {
                            somaRGB = 2 * (byte)(dataPtrAux + nChan)[j];
                            somaRGB += (byte)(dataPtrAux - widthStep + nChan)[j];
                            somaRGB += 2 * (byte)(dataPtrAux)[j];
                            somaRGB += (byte)(dataPtrAux - widthStep)[j];
                            somaRGB += 2 * (byte)(dataPtrAux - nChan)[j];
                            somaRGB += (byte)(dataPtrAux - nChan - widthStep)[j];

                            dataPtrDest[j] = (byte)Math.Round(somaRGB / 9.0);

                        }
                        dataPtrDest -= nChan;
                        dataPtrAux -= nChan;
                    }

                    //canto (0,height)
                    somaRGB = 0;

                    for (i = 0; i < 3; i++)
                    {
                        somaRGB = 4 * (byte)(dataPtrAux)[i];
                        somaRGB += 2 * (byte)(dataPtrAux + nChan)[i];
                        somaRGB += 2 * (byte)(dataPtrAux - widthStep)[i];
                        somaRGB += (byte)(dataPtrAux + nChan - widthStep)[i];

                        dataPtrDest[i] = (byte)Math.Round(somaRGB / 9.0);

                    }
                    //dataPtrDest[0] = (byte)((4 * dataPtrAux[0] + 2 * (dataPtrAux + nChan)[0] + 2 * (dataPtrAux - widthStep)[0] + (dataPtrAux + nChan - widthStep)[0]) / 9.0);
                    //dataPtrDest[1] = (byte)((4 * dataPtrAux[1] + 2 * (dataPtrAux + nChan)[1] + 2 * (dataPtrAux - widthStep)[1] + (dataPtrAux + nChan - widthStep)[1]) / 9.0);
                    //dataPtrDest[2] = (byte)((4 * dataPtrAux[2] + 2 * (dataPtrAux + nChan)[2] + 2 * (dataPtrAux - widthStep)[2] + (dataPtrAux + nChan - widthStep)[2]) / 9.0);

                    //dataPtrDest[0] = (byte)((dataPtrAux[0] * 4 + dataPtrAux[0 + nChan] * 2 + dataPtrAux[0 - widthStep] * 2 + dataPtrAux[0 + nChan - widthStep]) / 9.0);
                    //dataPtrDest[1] = (byte)((dataPtrAux[1] * 4 + dataPtrAux[1 + nChan] * 2 + dataPtrAux[1 - widthStep] * 2 + dataPtrAux[1 + nChan - widthStep]) / 9.0);
                    //dataPtrDest[2] = (byte)((dataPtrAux[2] * 4 + dataPtrAux[2 + nChan] * 2 + dataPtrAux[2 - widthStep] * 2 + dataPtrAux[2 + nChan - widthStep]) / 9.0);


                    //linha da esquerda
                    dataPtrDest -= widthStep;
                    dataPtrAux -= widthStep;

                    for (i = 1; i < (height - 1); i++)
                    {
                        for (j = 0; j < 3; j++)
                        {
                            somaRGB = 2 * (byte)(dataPtrAux - widthStep)[j];
                            somaRGB += (byte)(dataPtrAux - widthStep + nChan)[j];
                            somaRGB += 2 * (byte)(dataPtrAux)[j];
                            somaRGB += (byte)(dataPtrAux + nChan)[j];
                            somaRGB += 2 * (byte)(dataPtrAux + widthStep)[j];
                            somaRGB += (byte)(dataPtrAux + nChan + widthStep)[j];

                            dataPtrDest[j] = (byte)Math.Round(somaRGB / 9.0);

                        }
                        dataPtrDest -= widthStep;
                        dataPtrAux -= widthStep;
                    }

                }

            }


        }

        public static void NonUniform(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float[,] matrix, float matrixWeight)
        {
            unsafe
            {
                MIplImage mDest = img.MIplImage;
                MIplImage mOrig = imgCopy.MIplImage;

                byte* dataPtrDest = (byte*)mDest.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrOrig = (byte*)mOrig.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrAux = (byte*)mOrig.imageData.ToPointer();

                int width = img.Width;
                int height = img.Height;
                int nChan = mOrig.nChannels; // number of channels - 3
                int padding = mOrig.widthStep - mOrig.nChannels * mOrig.width; // alinhament bytes (padding)
                int x, y, x_f, y_f, sum_x, sum_y, i, j;

                byte* auxPtr;

                int size = 3;
                int margin = (int)(size / 2);
                int area = size * size;
                int blue = 0, green = 0, red = 0;

                if (nChan == 3)
                { // imagem em RGB
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {

                            for (x_f = -1 * margin, i = 0; x_f <= margin; x_f++, i++)
                            {
                                for (y_f = -1 * margin, j = 0; y_f <= margin; y_f++, j++)
                                {
                                    if (y + y_f < 0)
                                    {
                                        sum_y = 0;
                                    }
                                    else
                                    {
                                        if ((y + y_f) > (height - 1))
                                        {
                                            sum_y = height - 1;
                                        }
                                        else sum_y = y + y_f;
                                    }

                                    if (x + x_f < 0)
                                    {
                                        sum_x = 0;
                                    }
                                    else
                                    {
                                        if ((x + x_f) > (width - 1))
                                        {
                                            sum_x = width - 1;
                                        }
                                        else sum_x = x + x_f;
                                    }


                                    auxPtr = (dataPtrOrig + sum_y * mOrig.widthStep + sum_x * nChan);

                                    blue += (int)(matrix[i, j] * auxPtr[0]); //blue
                                    green += (int)(matrix[i, j] * auxPtr[1]); //green
                                    red += (int)(matrix[i, j] * auxPtr[2]); //red
                                }
                            }

                            blue = (blue < 0 ? -blue : blue) / (int)matrixWeight;
                            green = (green < 0 ? -green : green) / (int)matrixWeight;
                            red = (red < 0 ? -red : red) / (int)matrixWeight;

                            if (blue > 255)
                                blue = 255;

                            if (green > 255)
                                green = 255;

                            if (red > 255)
                                red = 255;

                            //guarda na imagem
                            dataPtrDest[0] = (byte)blue; //blue
                            dataPtrDest[1] = (byte)green; //green
                            dataPtrDest[2] = (byte)red; //red
                            blue = 0;
                            green = 0;
                            red = 0;

                            // avança apontador para próximo pixel
                            dataPtrDest += nChan;
                        }

                        //no fim da linha avança alinhamento (padding)
                        dataPtrDest += padding;
                    }

                }

            }
        }

        public static void Sobel(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {
            unsafe
            {
                float[,] matrix = new float[3, 3];

                matrix[0, 0] = 1;
                matrix[0, 1] = 0;
                matrix[0, 2] = -1;
                matrix[1, 0] = 2;
                matrix[1, 1] = 0;
                matrix[1, 2] = -2;
                matrix[2, 0] = 1;
                matrix[2, 1] = 0;
                matrix[2, 2] = -1;

                float[,] matrix1 = new float[3, 3];

                matrix1[0, 0] = -1;
                matrix1[0, 1] = -2;
                matrix1[0, 2] = -1;
                matrix1[1, 0] = 0;
                matrix1[1, 1] = 0;
                matrix1[1, 2] = 0;
                matrix1[2, 0] = 1;
                matrix1[2, 1] = 2;
                matrix1[2, 2] = 1;

                MIplImage mDest = img.MIplImage;
                MIplImage mOrig = imgCopy.MIplImage;

                byte* dataPtrDest = (byte*)mDest.imageData.ToPointer(); // Pointer to the image
                byte* dataPtr = (byte*)mOrig.imageData.ToPointer(); // Pointer to the image

                int width = img.Width;
                int height = img.Height;
                int nChan = mOrig.nChannels; // number of channels - 3
                int padding = mOrig.widthStep - mOrig.nChannels * mOrig.width; // alinhament bytes (padding)
                int x, y, x_f, y_f, sum_x, sum_y, i, j;

                byte* auxPtr;

                int size = 3;
                int margin = (int)(size / 2);
                int area = size * size;
                int blue = 0, green = 0, red = 0, blue2 = 0, green2 = 0, red2 = 0;

                if (nChan == 3)
                { // imagem em RGB
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {

                            for (x_f = -1 * margin, i = 0; x_f <= margin; x_f++, i++)
                            {
                                for (y_f = -1 * margin, j = 0; y_f <= margin; y_f++, j++)
                                {
                                    if (y + y_f < 0)
                                    {
                                        sum_y = 0;
                                    }
                                    else
                                    {
                                        if ((y + y_f) > (height - 1))
                                        {
                                            sum_y = height - 1;
                                        }
                                        else sum_y = y + y_f;
                                    }

                                    if (x + x_f < 0)
                                    {
                                        sum_x = 0;
                                    }
                                    else
                                    {
                                        if ((x + x_f) > (width - 1))
                                        {
                                            sum_x = width - 1;
                                        }
                                        else sum_x = x + x_f;
                                    }


                                    auxPtr = (dataPtr + sum_y * mOrig.widthStep + sum_x * nChan);


                                    blue += (int)(matrix[i, j] * auxPtr[0]); //blue
                                    green += (int)(matrix[i, j] * auxPtr[1]); //green
                                    red += (int)(matrix[i, j] * auxPtr[2]); //red

                                    blue2 += (int)(matrix1[i, j] * auxPtr[0]); //blue
                                    green2 += (int)(matrix1[i, j] * auxPtr[1]); //green
                                    red2 += (int)(matrix1[i, j] * auxPtr[2]); //red
                                }
                            }

                            blue = (blue < 0 ? -blue : blue) + (blue2 < 0 ? -blue2 : blue2);
                            green = (green < 0 ? -green : green) + (green2 < 0 ? -green2 : green2);
                            red = (red < 0 ? -red : red) + (red2 < 0 ? -red2 : red2);

                            if (blue > 255)
                                blue = 255;

                            if (green > 255)
                                green = 255;

                            if (red > 255)
                                red = 255;

                            //guarda na imagem
                            dataPtrDest[0] = (byte)blue; //blue
                            dataPtrDest[1] = (byte)green; //green
                            dataPtrDest[2] = (byte)red; //red
                            blue = 0;
                            green = 0;
                            red = 0;
                            blue2 = 0;
                            green2 = 0;
                            red2 = 0;

                            // avança apontador para próximo pixel
                            dataPtrDest += nChan;
                        }

                        //no fim da linha avança alinhamento (padding)
                        dataPtrDest += padding;
                    }

                }

            }
        }

        public static void Diferentiation(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {
            unsafe
            {
                MIplImage mDest = img.MIplImage;
                MIplImage mOrig = imgCopy.MIplImage;

                byte* dataPtrDest = (byte*)mDest.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrAux = (byte*)mOrig.imageData.ToPointer(); // Pointer to the image

                byte* auxPtr, auxPtrRight, auxPtrDown;
                int width = img.Width;
                int height = img.Height;
                int nChan = mOrig.nChannels; // number of channels - 3
                int padding = mOrig.widthStep - mOrig.nChannels * mOrig.width; // alinhament bytes (padding)
                int x, y, sum_x, sum_y;
                int blue_right, green_right, red_right, blue_down, green_down, red_down, blue, red, green;
                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {

                            sum_y = ((y + 1) > (height - 1) ? (y - 1) : (y + 1));
                            sum_x = ((x + 1) > (width - 1) ? (x - 1) : (x + 1));

                            auxPtr = (dataPtrAux + y * mDest.widthStep + x * nChan);
                            auxPtrRight = (dataPtrAux + (sum_y - 1) * mDest.widthStep + sum_x * nChan);
                            auxPtrDown = (dataPtrAux + sum_y * mDest.widthStep + (sum_x - 1) * nChan);

                            blue_right = auxPtr[0] - auxPtrRight[0]; //blue
                            green_right = auxPtr[1] - auxPtrRight[1]; //green
                            red_right = auxPtr[2] - auxPtrRight[2]; //red

                            blue_down = auxPtr[0] - auxPtrDown[0]; //blue
                            green_down = auxPtr[1] - auxPtrDown[1]; //green
                            red_down = auxPtr[2] - auxPtrDown[2]; //red

                            blue = (blue_right < 0 ? -blue_right : blue_right) + (blue_down < 0 ? -blue_down : blue_down);
                            green = (green_right < 0 ? -green_right : green_right) + (green_down < 0 ? -green_down : green_down);
                            red = (red_right < 0 ? -red_right : red_right) + (red_down < 0 ? -red_down : red_down);

                            if (blue > 255) blue = 255;
                            if (green > 255) green = 255;
                            if (red > 255) red = 255;

                            dataPtrDest[0] = (byte)blue;
                            dataPtrDest[1] = (byte)green;
                            dataPtrDest[2] = (byte)red;

                            dataPtrDest += nChan;
                        }
                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtrDest += padding;
                    }

                }
            }
        }

        public static void Roberts(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {
            unsafe
            {
                MIplImage mDest = img.MIplImage;
                MIplImage mOrig = imgCopy.MIplImage;

                byte* dataPtrDest = (byte*)mDest.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrAux = (byte*)mOrig.imageData.ToPointer(); // Pointer to the image

                byte* auxPtr, auxPtrRight, auxPtrDown, auxPtrDownRight;
                int width = img.Width;
                int height = img.Height;
                int nChan = mOrig.nChannels; // number of channels - 3
                int padding = mOrig.widthStep - mOrig.nChannels * mOrig.width; // alinhament bytes (padding)
                int x, y, sum_x, sum_y;
                int blue_right, green_right, red_right, blue_down, green_down, red_down, blue, red, green;
                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {

                            sum_y = ((y + 1) > (height - 1) ? (y - 1) : (y + 1));
                            sum_x = ((x + 1) > (width - 1) ? (x - 1) : (x + 1));

                            auxPtr = (dataPtrAux + y * mDest.widthStep + x * nChan);
                            auxPtrRight = (dataPtrAux + (sum_y - 1) * mDest.widthStep + sum_x * nChan);
                            auxPtrDown = (dataPtrAux + sum_y * mDest.widthStep + (sum_x - 1) * nChan);
                            auxPtrDownRight = (dataPtrAux + sum_y * mDest.widthStep + sum_x * nChan);

                            blue_right = auxPtr[0] - auxPtrDownRight[0]; //blue
                            green_right = auxPtr[1] - auxPtrDownRight[1]; //green
                            red_right = auxPtr[2] - auxPtrDownRight[2]; //red

                            blue_down = auxPtrRight[0] - auxPtrDown[0]; //blue
                            green_down = auxPtrRight[1] - auxPtrDown[1]; //green
                            red_down = auxPtrRight[2] - auxPtrDown[2]; //red

                            blue = (blue_right < 0 ? -blue_right : blue_right) + (blue_down < 0 ? -blue_down : blue_down);
                            green = (green_right < 0 ? -green_right : green_right) + (green_down < 0 ? -green_down : green_down);
                            red = (red_right < 0 ? -red_right : red_right) + (red_down < 0 ? -red_down : red_down);

                            if (blue > 255) blue = 255;
                            if (green > 255) green = 255;
                            if (red > 255) red = 255;

                            dataPtrDest[0] = (byte)blue;
                            dataPtrDest[1] = (byte)green;
                            dataPtrDest[2] = (byte)red;

                            dataPtrDest += nChan;
                        }
                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtrDest += padding;
                    }

                }
            }
        }

        public static void Median(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {
            unsafe
            {
                MIplImage mDest = img.MIplImage;
                MIplImage mOrig = imgCopy.MIplImage;

                byte* dataPtrDest = (byte*)mDest.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrAux = (byte*)mOrig.imageData.ToPointer(); // Pointer to the image
                byte* auxPtr;

                int min;
                int width = img.Width;
                int height = img.Height;
                int nChan = mOrig.nChannels; // number of channels - 3
                int padding = mOrig.widthStep - mOrig.nChannels * mOrig.width; // alinhament bytes (padding)
                int x, y, sum_x, sum_y, x_f, y_f, i, j, k = 0, l = 0;
                int size = 3;
                int margin = (int)(size / 2);
                int[] blue = new int[9];
                int[] red = new int[9];
                int[] green = new int[9];
                int[] aux = new int[9];

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            for (x_f = -1 * margin, i = 0; x_f <= margin; x_f++, i++)
                            {
                                for (y_f = -1 * margin, j = 0; y_f <= margin; y_f++, j++)
                                {
                                    if (y + y_f < 0)
                                    {
                                        sum_y = 0;
                                    }
                                    else
                                    {
                                        if ((y + y_f) > (height - 1))
                                        {
                                            sum_y = height - 1;
                                        }
                                        else sum_y = y + y_f;
                                    }

                                    if (x + x_f < 0)
                                    {
                                        sum_x = 0;
                                    }
                                    else
                                    {
                                        if ((x + x_f) > (width - 1))
                                        {
                                            sum_x = width - 1;
                                        }
                                        else sum_x = x + x_f;
                                    }


                                    auxPtr = (dataPtrAux + sum_y * mOrig.widthStep + sum_x * nChan);

                                    blue[k] = auxPtr[0];
                                    green[k] = auxPtr[1];
                                    red[k] = auxPtr[2];
                                    k++;

                                }
                            }

                            k = 0;

                            for (i = 0; i < 9; i++)
                            {
                                for (j = 0; j < 9; j++)
                                {
                                    if (i !=j)
                                    {
                                        aux[i] += (blue[i] - blue[j] < 0 ? -(blue[i] - blue[j]) : blue[i] - blue[j]) + (green[i] - green[j] < 0 ? -(green[i] - green[j]) : green[i] - green[j]) + (red[i] - red[j] < 0 ? -(red[i] - red[j]) : red[i] - red[j]);
                                    }
                                }
                            }

                            min = aux.Min();

                            minIndex = aux.ToList().IndexOf(min);
                            dataPtrDest[0] = (byte)blue[minIndex];
                            dataPtrDest[1] = (byte)green[minIndex];
                            dataPtrDest[2] = (byte)red[minIndex];

                            for (i = 0; i < 9; i++) aux[i] = 0;

                            dataPtrDest += nChan;
                            //dataPtrAux += nChan;
                        }
                        dataPtrDest += padding;
                        //dataPtrAux += padding;
                    }
                }
            }
        }

        public static int[] Histogram_Gray(Emgu.CV.Image<Bgr, byte> img)
        {
            unsafe
            {
                MIplImage mOrig = img.MIplImage;
                byte* dataPtr = (byte*)mOrig.imageData.ToPointer(); // Pointer to the image

                int[] histogram = new int[256];
                int width = img.Width;
                int height = img.Height;
                int nChan = mOrig.nChannels; // number of channels - 3
                int padding = mOrig.widthStep - mOrig.nChannels * mOrig.width; // alinhament bytes (padding)
                int x, y, gray;
                for(x= 0; x < 255; x++)
                {
                    histogram[x] = 0;
                }

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            gray = (int)Math.Round((dataPtr[0] + dataPtr[1] + dataPtr[2]) / 3.0);
                            histogram[gray]++;
                            dataPtr += nChan;
                        }
                        dataPtr += padding;
                    }
                }

                return histogram;
            }
        }

        public static int[,] Histogram_RGB(Emgu.CV.Image<Bgr, byte> img)
        {
            unsafe
            {
                MIplImage mOrig = img.MIplImage;
                byte* dataPtr = (byte*)mOrig.imageData.ToPointer(); // Pointer to the image

                int[,] histogram = new int[3,256];
                int width = img.Width;
                int height = img.Height;
                int nChan = mOrig.nChannels; // number of channels - 3
                int padding = mOrig.widthStep - mOrig.nChannels * mOrig.width; // alinhament bytes (padding)
                int x, y;
                for (x = 0; x < 255; x++)
                {
                    histogram[0,x] = 0;
                    histogram[1, x] = 0;
                    histogram[2, x] = 0;
                }

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            histogram[0, dataPtr[0]]++;
                            histogram[0, dataPtr[1]]++;
                            histogram[0, dataPtr[2]]++;
                            dataPtr += nChan;
                        }
                        dataPtr += padding;
                    }
                }

                return histogram;
            }

        }

        public static void ConvertToBW(Emgu.CV.Image<Bgr, byte> img, int threshold)
        {
            unsafe
            {
                MIplImage mOrig = img.MIplImage;
                byte* dataPtr = (byte*)mOrig.imageData.ToPointer(); // Pointer to the image
                
                int width = img.Width;
                int height = img.Height;
                int nChan = mOrig.nChannels; // number of channels - 3
                int padding = mOrig.widthStep - mOrig.nChannels * mOrig.width; // alinhament bytes (padding)
                int x, y, gray;
                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            gray = (int)(dataPtr[0] + dataPtr[1] + dataPtr[2]) / 3;
                            if (gray <= threshold)
                            {
                                dataPtr[0] = (byte)0;
                                dataPtr[1] = (byte)0;
                                dataPtr[2] = (byte)0;
                            }
                            else
                            {
                                dataPtr[0] = (byte)255;
                                dataPtr[1] = (byte)255;
                                dataPtr[2] = (byte)255;
                            }
                            dataPtr += nChan;
                        }
                        dataPtr += padding;
                    }
                }

            }
        }

        public static void ConvertToBW_Otsu(Emgu.CV.Image<Bgr, byte> img)
         {
             unsafe
             {
                 // acesso directo à memoria da imagem (sequencial)
                 // direcção top left -> bottom right

                 MIplImage m = img.MIplImage;
                 byte* dataPtr = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem

                 int width = img.Width;
                 int height = img.Height;
                 int nChan = m.nChannels; // numero de canais 3
                 int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)
                 int x, y, gray, i, n;
                 double q1 = 0, q2 = 0, u1 = 0, u2 = 0, var, varAux = 0;
                
                 int thres = 0;

                 //histogram array
                 int[] arrayGray = new int[256];
                 //probability array
                 float[] arrayProb = new float[256];
                 //sigma array
                 float[] arraySigma = new float[256];

                 //amount of pixels
                 int pixel_area = width * height;

                 //make a histogram
                 if (nChan == 3)
                 { // imagem em RGB
                     for (y = 0; y < height; y++)
                     {
                         for (x = 0; x < width; x++)
                         {

                             gray = (dataPtr[0] + dataPtr[1] + dataPtr[2]) / 3;
                             arrayGray[gray]++;

                             // avança apontador para próximo pixel
                             dataPtr += nChan;
                         }

                         //no fim da linha avança alinhamento (padding)
                         dataPtr += padding;
                     }
                 }
                 
                 //algoritm
                 for (n = 0; n < 256; n++)
                 {

                     //sum of probabilities left
                     for (i = 0; i <= n; i++)
                     { //q1
                         q1 += arrayGray[i];
                     }

                     

                     for (i = 0; i <= n; i++)
                     { //u1
                         u1 += i * arrayGray[i];
                     }

                     for (i = n + 1; i < 256; i++)
                     { //u2
                         q2 += arrayGray[i];
                         u2 += i * arrayGray[i];
                     }

                    q1 = q1 / pixel_area;
                    u1 = (u1 / pixel_area) / q1;
                    q2 = q2/ pixel_area;
                    u2 = (u2 / pixel_area) / q2;

                    var = q1 * q2 * Math.Pow((u1 - u2), 2);

                    if(var > varAux)
                    {
                        varAux = var;
                        thres = n;
                    }
                 }
                //apply threshold

                ConvertToBW(img, thres);

             }
         }

        /*public static void Rotation_Bilinear(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float angle)
        {
            unsafe
            {
                MIplImage mDest = img.MIplImage;
                MIplImage mOrig = imgCopy.MIplImage;

                byte* dataPtrDest = (byte*)mDest.imageData.ToPointer(); // Pointer to the image
                byte* dataPtrOrig = (byte*)mOrig.imageData.ToPointer(); // Pointer to the image
                byte* auxPtr11, auxPtr12, auxPtr21, auxPtr22;

                //rot = rot * (Math.PI / 180);


                int width = img.Width;
                int height = img.Height;
                int nChan = mOrig.nChannels; // number of channels - 3
                int padding = mOrig.widthStep - mOrig.nChannels * mOrig.width; // alinhament bytes (padding)
                int x, y, y0, x0;
                int r1_g, r1_b, r1_r, r2_b, r2_g, r2_r;
                int halfwidth = width / 2;
                int halfheigth = height / 2;
                //rot = rot * (180.0 / Math.PI);
                double cosrot = Math.Cos(angle);
                double senrot = Math.Sin(angle);

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            x0 = (int)Math.Round((x - halfwidth) * cosrot - (halfheigth - y) * senrot + halfwidth);
                            y0 = (int)Math.Round(halfheigth - (x - halfheigth) * senrot - (halfheigth - y) * cosrot);

                            offsetX = ((x0 - (int)x0) < 0 ? -(x0- (int)x0) : (x0 - (int)x0));
                            offsetY = ((y0 - (int)y0) < 0 ? -(y0 - (int)y0) : (y0 - (int)y0));

                            if (x0 < 0 || y0 < 0 || x0 >= width || y0 >= height)
                            {
                                dataPtrDest[0] = 0;
                                dataPtrDest[1] = 0;
                                dataPtrDest[2] = 0;

                            }
                            else
                            {
                                x_origin_int = (int)x0;
                                y_origin_int = (int)y0;

                                sum_y = (y_origin_int + 1 > (height - 1) ? y_origin_int : y_origin_int + 1);
                                sum_x = (x_origin_int + 1 > (width - 1) ? x_origin_int : x_origin_int + 1);

                                auxPtr11 = (dataPtrOrig + y_origin_int * mDest.widthStep + x_origin_int * nChan);
                                auxPtr12 = (dataPtrOrig + sum_y * mDest.widthStep + x_origin_int * nChan);
                                auxPtr21 = (dataPtrOrig + y_origin_int * mDest.widthStep + sum_x * nChan);
                                auxPtr22 = (dataPtrOrig + sum_y * mDest.widthStep + sum_x * nChan);

                                //bilinear
                                r1_b = (1 - offsetX) * auxPtr11[0] + offsetX * auxPtr21[0];
                                r1_g = (1 - offsetX) * auxPtr11[1] + offsetX * auxPtr21[1];
                                r1_r = (1 - offsetX) * auxPtr11[2] + offsetX * auxPtr21[2];

                                r2_b = (1 - offsetX) * auxPtr12[0] + offsetX * auxPtr22[0];
                                r2_g = (1 - offsetX) * auxPtr12[1] + offsetX * auxPtr22[1];
                                r2_r = (1 - offsetX) * auxPtr12[2] + offsetX * auxPtr22[2];

                                blue = (int)((1 - offsetY) * r1_b + offsetY * r2_b);
                                green = (int)((1 - offsetY) * r1_g + offsetY * r2_g);
                                red = (int)((1 - offsetY) * r1_r + offsetY * r2_r);

                                // guarda na imagem
                                dataPtrDest[0] = (byte)blue; //blue
                                dataPtrDest[1] = (byte)green; //green
                                dataPtrDest[2] = (byte)red; //red
                            }

                            //at the end of the line advance the pointer by the aligment bytes (padding)

                            dataPtrDest += mDest.nChannels;
                        }
                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtrDest += padding;
                    }

                }
            }
        }*/

        public static void Chess_Recognition(Image<Bgr, byte> img,Image<Bgr, byte> imgCopy, out Rectangle BD_Location,out string Angle,out string[,] Pieces)
        {
            unsafe
            {
               
                    MIplImage m = img.MIplImage;
                    byte* dataPtr = (byte*)m.imageData.ToPointer(); // obter apontador do inicio da imagem
                    byte* dataPtrOrig = (byte*)m.imageData.ToPointer();
                    int width = img.Width;
                    int height = img.Height;
                    int widthStep = m.widthStep;
                    int nChan = m.nChannels; // numero de canais 3
                    int padding = m.widthStep - m.nChannels * m.width; // alinhamento (padding)
                    int x, y;
                    int x11 = 0, y11 = 0, heightrect = 0, widthrect = 0, x22 = 0, y22 = 0, x12 = 0, y12 = 0, x21 = 0, y21 = 0, aux = 0;
                    Angle = 10.ToString();
                    int halfwidth = width / 2;
                    int halfheigth = height / 2;

                    double ang;
                    BD_Location = new Rectangle(400, 400, 480, 480);
                    if (nChan == 3)
                    {
                        
                        Roberts(img, imgCopy);
                        //Median(img, imgCopy);
                        ConvertToBW(img, 10);
                        
                    //check for upper corners positions

                    dataPtr = dataPtrOrig;
                    
                        for (y = 0; y < height ; y++)
                        {
                            for (x = 0; x < width ; x++)
                            {
                                if (((int)dataPtr[0] + dataPtr[1] + dataPtr[2]) != 0)
                                {
                                    x11 = x;
                                    y11 = y;
                                    x = width;
                                    y = height;
                                
                                }
                                dataPtr += nChan;
                            }
                            dataPtr += padding;
                        }

                    dataPtr = dataPtrOrig +  height  * widthStep;
                    x = 0;

                    for (x = 0; x < width; x++)
                    {
                        for (y = height; y > 0; y--)
                        {
                            if (((int)dataPtr[0] + dataPtr[1] + dataPtr[2]) != 0)
                            {
                                x12 = x;
                                y12 = y;
                                x = width;
                                y = 0;

                            }
                            dataPtr -= widthStep;
                        }
                        dataPtr = dataPtrOrig + widthStep * height +  (x - 1)* nChan;
                    }
                    
                    
                    //check for lower corners positions
                    dataPtr = dataPtrOrig + (height - 1) * widthStep + (width - 1) * nChan;
                    for (y = height -1 ; y >= 0; y--)
                        {
                            for (x = width; x > 0 ; x--)
                            {
                                if (((int)dataPtr[0] + dataPtr[1] + dataPtr[2]) != 0)
                                {
                                    x22 = x;
                                    y22 = y;
                                    y = 0;
                                    x = 0;
                                }
                                dataPtr -= nChan;
                            }
                            dataPtr -= padding;
                        }

                    dataPtr = dataPtrOrig + width * nChan;

                    for (x = width; x > 0; x--)
                    {
                        for (y = 0; y < height; y++)
                        {
                            if (((int)dataPtr[0] + dataPtr[1] + dataPtr[2]) != 0)
                            {
                                x21 = x;
                                y21 = y;
                                y = height;
                                x = 0;
                            }
                            dataPtr += widthStep;
                        }
                        dataPtr = dataPtrOrig + x * nChan;
                    }

                    if(x11 > x12 && y12 > y11)
                    {
                        aux = x11;
                        x11 = x12;
                        x12 = x22;
                        x22 = x21;
                        x21 = aux;
                        aux = y11;
                        y11 = y12;
                        y12 = y22;
                        y22 = y21;
                        y21 = aux;
                    }
                    heightrect = y22 - y11;
                    widthrect = x22 - x11;
                    int larg = x21 - x12;
                    int comp = y12 -  y21;
                    ang = Math.Atan(comp / larg);
                    ang = ang * (180.0 / Math.PI);
                    //Angle = (int)ang.ToString();
                    widthrect = (int)Math.Round(Math.Sqrt(Math.Pow(x12 - x11, 2) + Math.Pow(y12 - y11, 2)));
                    heightrect = (int)Math.Round(Math.Sqrt(Math.Pow(x11 - x21, 2) + Math.Pow(y11 - y21, 2)));


                    if (ang != 45)
                    {
                        ang = 45 - ang;
                        double cosrot = Math.Cos(ang);
                        double senrot = Math.Sin(ang);
                        int xaux = x11;
                        int yaux = y11;
                        imgCopy = img.Copy();
                        Rotation(img, imgCopy, ang);
                       
                        x11 = (int)Math.Round((xaux - halfwidth) * cosrot - (halfheigth - yaux) * senrot + halfwidth);
                        y11 = (int)Math.Round(halfheigth - (xaux - halfwidth) * senrot - (halfheigth - yaux) * cosrot);

                    }
                    //widthrect = (int)Math.Round(Math.Sqrt(Math.Pow(x12 - x11, 2) + Math.Pow(y12 - y11, 2)));
                    //heightrect = (int)Math.Round(Math.Sqrt(Math.Pow(x11 - x21, 2) + Math.Pow(y11 - y21, 2)));


                    BD_Location = new Rectangle(0, 0, width, height);
                    //BD_Location = new Rectangle(0, 0, 500, 500);

                }
                    string[,] tmp = new string[8, 8];
                    for (int l = 0; l < 8; l++)
                    {
                        for (int k = 0; k < 8; k++)
                        {
                            tmp[l, k] = "K_w";
                        }

                    }
                Pieces = tmp;
            }
        }
    }
}




