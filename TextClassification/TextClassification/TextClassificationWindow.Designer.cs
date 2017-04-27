namespace TextClassification
{
    partial class TextClassificationWindow
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
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.TrainingDataDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTrainingDataSetLocation = new System.Windows.Forms.Label();
            this.lnkChangeDataSet = new System.Windows.Forms.LinkLabel();
            this.btnTrainData = new System.Windows.Forms.Button();
            this.lnkShowhidefeature = new System.Windows.Forms.LinkLabel();
            this.rtbLogger = new System.Windows.Forms.RichTextBox();
            this.btnClassifyDocument = new System.Windows.Forms.Button();
            this.btnClassifyFolder = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(128, 24);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(232, 17);
            this.linkLabel1.TabIndex = 0;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Switch to Feature Selection Window";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // TrainingDataDialog
            // 
            this.TrainingDataDialog.ShowNewFolderButton = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(131, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Training Data:";
            // 
            // lblTrainingDataSetLocation
            // 
            this.lblTrainingDataSetLocation.AutoSize = true;
            this.lblTrainingDataSetLocation.Location = new System.Drawing.Point(263, 94);
            this.lblTrainingDataSetLocation.Name = "lblTrainingDataSetLocation";
            this.lblTrainingDataSetLocation.Size = new System.Drawing.Size(35, 17);
            this.lblTrainingDataSetLocation.TabIndex = 3;
            this.lblTrainingDataSetLocation.Text = "Text";
            // 
            // lnkChangeDataSet
            // 
            this.lnkChangeDataSet.AutoSize = true;
            this.lnkChangeDataSet.Location = new System.Drawing.Point(592, 96);
            this.lnkChangeDataSet.Name = "lnkChangeDataSet";
            this.lnkChangeDataSet.Size = new System.Drawing.Size(112, 17);
            this.lnkChangeDataSet.TabIndex = 4;
            this.lnkChangeDataSet.TabStop = true;
            this.lnkChangeDataSet.Text = "Change DataSet";
            this.lnkChangeDataSet.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkChangeDataSet_LinkClicked);
            // 
            // btnTrainData
            // 
            this.btnTrainData.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnTrainData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTrainData.ForeColor = System.Drawing.Color.DarkRed;
            this.btnTrainData.Location = new System.Drawing.Point(134, 143);
            this.btnTrainData.Name = "btnTrainData";
            this.btnTrainData.Size = new System.Drawing.Size(570, 38);
            this.btnTrainData.TabIndex = 6;
            this.btnTrainData.Text = "Train Data with Selected Features";
            this.btnTrainData.UseVisualStyleBackColor = false;
            this.btnTrainData.Click += new System.EventHandler(this.btnTrainData_Click);
            // 
            // lnkShowhidefeature
            // 
            this.lnkShowhidefeature.AutoSize = true;
            this.lnkShowhidefeature.Location = new System.Drawing.Point(550, 24);
            this.lnkShowhidefeature.Name = "lnkShowhidefeature";
            this.lnkShowhidefeature.Size = new System.Drawing.Size(154, 17);
            this.lnkShowhidefeature.TabIndex = 7;
            this.lnkShowhidefeature.TabStop = true;
            this.lnkShowhidefeature.Text = "Show Selected Feature";
            this.lnkShowhidefeature.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkShowhidefeature_LinkClicked);
            // 
            // rtbLogger
            // 
            this.rtbLogger.Location = new System.Drawing.Point(35, 276);
            this.rtbLogger.Name = "rtbLogger";
            this.rtbLogger.Size = new System.Drawing.Size(792, 172);
            this.rtbLogger.TabIndex = 8;
            this.rtbLogger.Text = "";
            // 
            // btnClassifyDocument
            // 
            this.btnClassifyDocument.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.btnClassifyDocument.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClassifyDocument.ForeColor = System.Drawing.Color.DarkRed;
            this.btnClassifyDocument.Location = new System.Drawing.Point(131, 218);
            this.btnClassifyDocument.Name = "btnClassifyDocument";
            this.btnClassifyDocument.Size = new System.Drawing.Size(275, 38);
            this.btnClassifyDocument.TabIndex = 9;
            this.btnClassifyDocument.Text = "Classify Single Document";
            this.btnClassifyDocument.UseVisualStyleBackColor = false;
            this.btnClassifyDocument.Visible = false;
            this.btnClassifyDocument.Click += new System.EventHandler(this.btnClassifyDocument_Click);
            // 
            // btnClassifyFolder
            // 
            this.btnClassifyFolder.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.btnClassifyFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClassifyFolder.ForeColor = System.Drawing.Color.DarkRed;
            this.btnClassifyFolder.Location = new System.Drawing.Point(412, 218);
            this.btnClassifyFolder.Name = "btnClassifyFolder";
            this.btnClassifyFolder.Size = new System.Drawing.Size(292, 38);
            this.btnClassifyFolder.TabIndex = 10;
            this.btnClassifyFolder.Text = "Classify Multiple Documents";
            this.btnClassifyFolder.UseVisualStyleBackColor = false;
            this.btnClassifyFolder.Visible = false;
            this.btnClassifyFolder.Click += new System.EventHandler(this.btnClassifyFolder_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // TextClassificationWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 460);
            this.Controls.Add(this.btnClassifyFolder);
            this.Controls.Add(this.btnClassifyDocument);
            this.Controls.Add(this.rtbLogger);
            this.Controls.Add(this.lnkShowhidefeature);
            this.Controls.Add(this.btnTrainData);
            this.Controls.Add(this.lnkChangeDataSet);
            this.Controls.Add(this.lblTrainingDataSetLocation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.linkLabel1);
            this.Name = "TextClassificationWindow";
            this.Text = "Text Classification";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.FolderBrowserDialog TrainingDataDialog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTrainingDataSetLocation;
        private System.Windows.Forms.LinkLabel lnkChangeDataSet;
        private System.Windows.Forms.Button btnTrainData;
        private System.Windows.Forms.LinkLabel lnkShowhidefeature;
        private System.Windows.Forms.RichTextBox rtbLogger;
        private System.Windows.Forms.Button btnClassifyDocument;
        private System.Windows.Forms.Button btnClassifyFolder;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}