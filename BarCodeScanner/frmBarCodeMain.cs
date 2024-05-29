using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Markup;
using ZXing;
using ZXing.Common;
using ZXing.Windows.Compatibility;
using AForge.Video.DirectShow;
using ZXing.QrCode;
using AForge.Video;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.PortableExecutable;

namespace BarCodeScanner
{
    public partial class frmBarCodeMain : Form
    {

        private VideoCaptureDevice? _captureDevice;
        private BarcodeReader? _barcodeReader;

        public frmBarCodeMain()
        {
            InitializeComponent();
            _captureDevice = null;
            _barcodeReader = new BarcodeReader();

            // Populate the comboBoxCameras with available video devices
            var videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo device in videoDevices)
            {
                cmboCameras.Items.Add(device.Name);
            }

            // Select the first video device by default
            if (cmboCameras.Items.Count > 0)
            {
                cmboCameras.SelectedIndex = 0;
            }

            // Subscribe to the Load event
            this.Load += new EventHandler(Form_Load);
        }

        private void Form_Load(object sender, EventArgs e)
        {
            if (cmboCameras.SelectedIndex >= 0)
            {
                try
                {
                    // Get the selected video device
                    var videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                    var selectedDevice = videoDevices[cmboCameras.SelectedIndex];

                    // Start webcam capture
                    _captureDevice = new VideoCaptureDevice(selectedDevice.MonikerString);

                    // Set the video resolution
                    var videoCapabilities = _captureDevice.VideoCapabilities;
                    foreach (var capability in videoCapabilities)
                    {
                        if (capability.FrameSize.Width == 800 && capability.FrameSize.Height == 600)
                        {
                            _captureDevice.VideoResolution = capability;
                            break;
                        }
                    }

                    _captureDevice.NewFrame += CaptureDevice_NewFrame;
                    _captureDevice.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error starting the webcam: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnScan_Click(object sender, EventArgs e)
        {
            if (pbScan.Image != null)
            {
                // Scan barcode from the current frame displayed in the PictureBox
                var result = _barcodeReader.Decode((Bitmap)pbScan.Image);

                if (result != null)
                {
                    // Barcode found, display the contents in the textbox
                    tbScanBarCode.Text = result.Text;
                }
                else
                {
                    tbScanBarCode.Text = "No barcode found";
                }
            }
        }

        private void CaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (eventArgs.Frame != null)
            {
                // Get the captured frame
                Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();

                // Display the frame in the PictureBox
                pbScan.Invoke((MethodInvoker) delegate
                {
                    pbScan.SizeMode = PictureBoxSizeMode.Zoom;
                    pbScan.Image = bitmap;
                });
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_captureDevice != null && _captureDevice.IsRunning)
            {
                _captureDevice.SignalToStop();
                _captureDevice.WaitForStop();
            }
            base.OnFormClosing(e);
        }
    }

}
