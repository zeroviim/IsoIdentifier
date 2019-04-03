namespace ISOIdentifier
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
            this.btn_Open = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lsbx_Sector16 = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbl_desc_BinLocation = new System.Windows.Forms.Label();
            this.lbl_BinLocation = new System.Windows.Forms.Label();
            this.lbl_OutputLocation = new System.Windows.Forms.Label();
            this.lbl_desc_OutputLocation = new System.Windows.Forms.Label();
            this.btn_BinLocation = new System.Windows.Forms.Button();
            this.btn_Output = new System.Windows.Forms.Button();
            this.fbd_Output = new System.Windows.Forms.FolderBrowserDialog();
            this.lbl_Status = new System.Windows.Forms.Label();
            this.lbl_desc_Status = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_Open
            // 
            this.btn_Open.Location = new System.Drawing.Point(23, 185);
            this.btn_Open.Name = "btn_Open";
            this.btn_Open.Size = new System.Drawing.Size(61, 33);
            this.btn_Open.TabIndex = 0;
            this.btn_Open.Text = "Extract";
            this.btn_Open.UseVisualStyleBackColor = true;
            this.btn_Open.Click += new System.EventHandler(this.btn_Open_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(473, 214);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Sector 16 Information";
            // 
            // lsbx_Sector16
            // 
            this.lsbx_Sector16.FormattingEnabled = true;
            this.lsbx_Sector16.Location = new System.Drawing.Point(476, 230);
            this.lsbx_Sector16.Name = "lsbx_Sector16";
            this.lsbx_Sector16.Size = new System.Drawing.Size(185, 147);
            this.lsbx_Sector16.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(275, 230);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(192, 283);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // lbl_desc_BinLocation
            // 
            this.lbl_desc_BinLocation.AutoSize = true;
            this.lbl_desc_BinLocation.Location = new System.Drawing.Point(20, 26);
            this.lbl_desc_BinLocation.Name = "lbl_desc_BinLocation";
            this.lbl_desc_BinLocation.Size = new System.Drawing.Size(68, 13);
            this.lbl_desc_BinLocation.TabIndex = 4;
            this.lbl_desc_BinLocation.Text = "BIN location:";
            // 
            // lbl_BinLocation
            // 
            this.lbl_BinLocation.AutoSize = true;
            this.lbl_BinLocation.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_BinLocation.Location = new System.Drawing.Point(94, 26);
            this.lbl_BinLocation.Name = "lbl_BinLocation";
            this.lbl_BinLocation.Size = new System.Drawing.Size(117, 15);
            this.lbl_BinLocation.TabIndex = 5;
            this.lbl_BinLocation.Text = "[bin location path here]";
            // 
            // lbl_OutputLocation
            // 
            this.lbl_OutputLocation.AutoSize = true;
            this.lbl_OutputLocation.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_OutputLocation.Location = new System.Drawing.Point(108, 99);
            this.lbl_OutputLocation.Name = "lbl_OutputLocation";
            this.lbl_OutputLocation.Size = new System.Drawing.Size(133, 15);
            this.lbl_OutputLocation.TabIndex = 7;
            this.lbl_OutputLocation.Text = "[output location path here]";
            // 
            // lbl_desc_OutputLocation
            // 
            this.lbl_desc_OutputLocation.AutoSize = true;
            this.lbl_desc_OutputLocation.Location = new System.Drawing.Point(20, 99);
            this.lbl_desc_OutputLocation.Name = "lbl_desc_OutputLocation";
            this.lbl_desc_OutputLocation.Size = new System.Drawing.Size(82, 13);
            this.lbl_desc_OutputLocation.TabIndex = 6;
            this.lbl_desc_OutputLocation.Text = "Output location:";
            // 
            // btn_BinLocation
            // 
            this.btn_BinLocation.Location = new System.Drawing.Point(23, 51);
            this.btn_BinLocation.Name = "btn_BinLocation";
            this.btn_BinLocation.Size = new System.Drawing.Size(48, 25);
            this.btn_BinLocation.TabIndex = 8;
            this.btn_BinLocation.Text = "Bin";
            this.btn_BinLocation.UseVisualStyleBackColor = true;
            this.btn_BinLocation.Click += new System.EventHandler(this.btn_BinLocation_Click);
            // 
            // btn_Output
            // 
            this.btn_Output.Location = new System.Drawing.Point(23, 127);
            this.btn_Output.Name = "btn_Output";
            this.btn_Output.Size = new System.Drawing.Size(48, 25);
            this.btn_Output.TabIndex = 9;
            this.btn_Output.Text = "Output";
            this.btn_Output.UseVisualStyleBackColor = true;
            this.btn_Output.Click += new System.EventHandler(this.btn_Output_Click);
            // 
            // lbl_Status
            // 
            this.lbl_Status.AutoSize = true;
            this.lbl_Status.Location = new System.Drawing.Point(142, 182);
            this.lbl_Status.Name = "lbl_Status";
            this.lbl_Status.Size = new System.Drawing.Size(43, 13);
            this.lbl_Status.TabIndex = 10;
            this.lbl_Status.Text = "[Status]";
            // 
            // lbl_desc_Status
            // 
            this.lbl_desc_Status.AutoSize = true;
            this.lbl_desc_Status.Location = new System.Drawing.Point(105, 182);
            this.lbl_desc_Status.Name = "lbl_desc_Status";
            this.lbl_desc_Status.Size = new System.Drawing.Size(40, 13);
            this.lbl_desc_Status.TabIndex = 11;
            this.lbl_desc_Status.Text = "Status:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(751, 483);
            this.Controls.Add(this.lbl_desc_Status);
            this.Controls.Add(this.lbl_Status);
            this.Controls.Add(this.btn_Output);
            this.Controls.Add(this.btn_BinLocation);
            this.Controls.Add(this.lbl_OutputLocation);
            this.Controls.Add(this.lbl_desc_OutputLocation);
            this.Controls.Add(this.lbl_BinLocation);
            this.Controls.Add(this.lbl_desc_BinLocation);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lsbx_Sector16);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_Open);
            this.Name = "Form1";
            this.Text = "PSX Iso Identifier";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Open;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lsbx_Sector16;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbl_desc_BinLocation;
        private System.Windows.Forms.Label lbl_BinLocation;
        private System.Windows.Forms.Label lbl_OutputLocation;
        private System.Windows.Forms.Label lbl_desc_OutputLocation;
        private System.Windows.Forms.Button btn_BinLocation;
        private System.Windows.Forms.Button btn_Output;
        private System.Windows.Forms.FolderBrowserDialog fbd_Output;
        private System.Windows.Forms.Label lbl_Status;
        private System.Windows.Forms.Label lbl_desc_Status;
    }
}

