using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Coding4Fun.Kinect.Wpf;
using System.Windows.Media.Imaging;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Kinect_Experiments
{
    public partial class Form1 : Form
    {
        private KinectSensor kinectSensor;
        public Form1()
        {
            InitializeComponent();

            //initialize Kinect sensor
            //1. find connected kinect sensor
            foreach (var sensor in KinectSensor.KinectSensors)
            {
                if(sensor.Status == KinectStatus.Connected)
                {
                    this.kinectSensor = sensor;
                    break;
                }
            }
            //2. enable data stream
            if(kinectSensor != null)
            {
                this.kinectSensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                this.kinectSensor.Start();
                this.kinectSensor.ColorFrameReady += KinectSensor_ColorFrameReady;
            }
        }

        private void KinectSensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using(ColorImageFrame cif = e.OpenColorImageFrame())
            {
                if(cif == null)
                {
                    return;
                }

                BitmapSource bs = cif.ToBitmapSource();
                Bitmap bmp = GetBitmapFromBitmapSource(bs);
                Image<Bgr, byte> currentFrame = new Image<Bgr, byte>(bmp);
                imageBox1.Image = currentFrame;
            }
        }

        private Bitmap GetBitmapFromBitmapSource(BitmapSource bs)
        {
            Bitmap bmp = null;
            using(MemoryStream ms = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bs));
                enc.Save(ms);
                bmp = new Bitmap(ms);
            }
            return bmp;
        }
    }
}
