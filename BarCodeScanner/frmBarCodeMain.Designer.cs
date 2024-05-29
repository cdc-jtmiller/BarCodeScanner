﻿namespace BarCodeScanner
{
    partial class frmBarCodeMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pbScan = new PictureBox();
            btnScan = new Button();
            tbScanBarCode = new TextBox();
            lblScannedBCode = new Label();
            cmboCameras = new ComboBox();
            ((System.ComponentModel.ISupportInitialize) pbScan).BeginInit();
            SuspendLayout();
            // 
            // pbScan
            // 
            pbScan.BorderStyle = BorderStyle.FixedSingle;
            pbScan.Location = new Point(46, 98);
            pbScan.Name = "pbScan";
            pbScan.Size = new Size(438, 329);
            pbScan.TabIndex = 0;
            pbScan.TabStop = false;
            // 
            // btnScan
            // 
            btnScan.Font = new Font("Lucida Sans", 11F, FontStyle.Regular, GraphicsUnit.Point,  0);
            btnScan.Location = new Point(589, 98);
            btnScan.Name = "btnScan";
            btnScan.Size = new Size(84, 35);
            btnScan.TabIndex = 1;
            btnScan.Text = "Scan";
            btnScan.UseVisualStyleBackColor = true;
            btnScan.Click += btnScan_Click;
            // 
            // tbScanBarCode
            // 
            tbScanBarCode.Location = new Point(43, 468);
            tbScanBarCode.Name = "tbScanBarCode";
            tbScanBarCode.Size = new Size(627, 25);
            tbScanBarCode.TabIndex = 2;
            // 
            // lblScannedBCode
            // 
            lblScannedBCode.AutoSize = true;
            lblScannedBCode.Location = new Point(46, 446);
            lblScannedBCode.Name = "lblScannedBCode";
            lblScannedBCode.Size = new Size(222, 17);
            lblScannedBCode.TabIndex = 3;
            lblScannedBCode.Text = "Scanned Barcode Information";
            // 
            // cmboCameras
            // 
            cmboCameras.FormattingEnabled = true;
            cmboCameras.Location = new Point(49, 25);
            cmboCameras.Name = "cmboCameras";
            cmboCameras.Size = new Size(310, 25);
            cmboCameras.TabIndex = 4;
            // 
            // frmBarCodeMain
            // 
            AutoScaleDimensions = new SizeF(9F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(703, 519);
            Controls.Add(cmboCameras);
            Controls.Add(lblScannedBCode);
            Controls.Add(tbScanBarCode);
            Controls.Add(btnScan);
            Controls.Add(pbScan);
            Font = new Font("Lucida Sans", 11.25F, FontStyle.Regular, GraphicsUnit.Point,  0);
            Name = "frmBarCodeMain";
            Text = "Bar Code Scanner";
            ((System.ComponentModel.ISupportInitialize) pbScan).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pbScan;
        private Button btnScan;
        private TextBox tbScanBarCode;
        private Label lblScannedBCode;
        private ComboBox cmboCameras;
    }
}
