using System;
using System.Drawing;
using System.Windows.Forms;
using ZXing;
using ZXing.Common;
using ZXing.Windows.Compatibility;
using AForge.Video.DirectShow;
using AForge.Video;
using AForge.Imaging;
using AForge.Imaging.Filters;
using ZXing.QrCode;
using static System.Windows.Forms.Design.AxImporter;


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
            _barcodeReader = new BarcodeReader
            {
                Options = new DecodingOptions
                {
                    PossibleFormats = new List<BarcodeFormat>
                    {
                        BarcodeFormat.MAXICODE,
                        BarcodeFormat.QR_CODE,
                        BarcodeFormat.CODE_128,
                        BarcodeFormat.UPC_A,
                        BarcodeFormat.CODE_39,
                        BarcodeFormat.PDF_417,
                        BarcodeFormat.All_1D
                    },
                    TryHarder = true,
                    PureBarcode = false
                }
            };



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

            // Subscribe to the Load & Closing events
            this.Load += new EventHandler(Form_Load);
            this.FormClosing += new FormClosingEventHandler(Form_FormClosing);

            // Allows the user to click a link and have the app open a web browser
            this.rtbScanBarcode.LinkClicked += new LinkClickedEventHandler(rtbScanBarcode_LinkClicked);
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
                        if (capability.FrameSize.Width == 520 && capability.FrameSize.Height == 329)
                        {
                            _captureDevice.VideoResolution = capability;
                            pbScan.Width = capability.FrameSize.Width;
                            pbScan.Height = capability.FrameSize.Height;
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

                // Preprocess the image before decoding
                // Bitmap processedImage = PreprocessImage((Bitmap)pbScan.Image);
                
                // Scan barcode from the current frame displayed in the PictureBox (No pre-processing)
                var result = _barcodeReader.Decode((Bitmap)pbScan.Image);

                // Using preprocessing
                //var result = _barcodeReader.Decode(processedImage);

                if (result != null)
                {
                    // Barcode found, display the contents in the textbox
                    rtbScanBarcode.Text = result.Text;
                }
                else
                {
                    rtbScanBarcode.Text = "No barcode found";
                }
            }
        }

        private void CaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (eventArgs.Frame != null)
            {
                // Get the captured frame
                Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();

                // Preprocess the image  NOTE: doesn't help so commenting out
                //bitmap = PreprocessImage(bitmap);

                // Process the frame on a separate thread
                Task.Run(() =>
                {
                    // Scale the frame based on a zoom factor
                    // Note: Zoom code slows things down too much
                    //float zoomFactor = 1.5f; // Adjust the zoom factor as needed
                    //int newWidth = (int)(bitmap.Width * zoomFactor);
                    //int newHeight = (int)(bitmap.Height * zoomFactor);

                    //Bitmap zoomedFrame = new Bitmap(bitmap, newWidth, newHeight);

                    // Display the frame in the PictureBox on the UI thread
                    pbScan.Invoke((MethodInvoker) delegate
                    {
                        pbScan.SizeMode = PictureBoxSizeMode.Zoom;
                        pbScan.Image = (Bitmap) bitmap.Clone();
                    });

                    // Dispose the bitmap
                    bitmap.Dispose();
                });
            }
        }

        private Bitmap PreprocessImage(Bitmap image)
        {
            // Convert the image to grayscale
            //Grayscale gsfilter = new Grayscale(0.2125, 0.7154, 0.0721);
            //Bitmap grayscaleImage = gsfilter.Apply(image);
            Bitmap grayscaleImage = Grayscale.CommonAlgorithms.BT709.Apply(image);

            // Adjust the contrast and brightness
            ContrastCorrection contrastFilter = new ContrastCorrection(10);
            Bitmap adjustedImage = contrastFilter.Apply(grayscaleImage);

            // Apply a sharpening filter
            Sharpen sharpenFilter = new Sharpen();
            Bitmap sharpenedImage = sharpenFilter.Apply(adjustedImage);

            // Dispose the intermediate bitmaps
            grayscaleImage.Dispose();
            adjustedImage.Dispose();

            return sharpenedImage;
        }

        private void rtbScanBarcode_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            // Open the URL in the default web browser
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.LinkText) { UseShellExecute = true });
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_captureDevice != null && _captureDevice.IsRunning)
            {
                _captureDevice.SignalToStop();
                _captureDevice.WaitForStop();
                _captureDevice.NewFrame -= CaptureDevice_NewFrame;
                _captureDevice = null;
            }
        }
    }
}
