namespace DesktopRemoteApplication
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
            this.components = new System.ComponentModel.Container();
            this.executeButton = new System.Windows.Forms.Button();
            this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.pathBox = new System.Windows.Forms.TextBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.appStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.processChecker = new System.Windows.Forms.Timer(this.components);
            this.processValidation = new System.Windows.Forms.Timer(this.components);
            this.receiveButton = new System.Windows.Forms.Button();
            this.dataInBox = new System.Windows.Forms.TextBox();
            this.clearButton = new System.Windows.Forms.Button();
            this.udpStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // executeButton
            // 
            this.executeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.executeButton.Location = new System.Drawing.Point(32, 78);
            this.executeButton.Name = "executeButton";
            this.executeButton.Size = new System.Drawing.Size(103, 33);
            this.executeButton.TabIndex = 0;
            this.executeButton.Text = "EXECUTE";
            this.executeButton.UseVisualStyleBackColor = true;
            this.executeButton.Click += new System.EventHandler(this.executeButton_Click);
            // 
            // OpenFileDialog
            // 
            this.OpenFileDialog.FileName = "OpenFileDialog";
            // 
            // pathBox
            // 
            this.pathBox.Location = new System.Drawing.Point(170, 84);
            this.pathBox.Name = "pathBox";
            this.pathBox.Size = new System.Drawing.Size(624, 20);
            this.pathBox.TabIndex = 2;
            this.pathBox.DoubleClick += new System.EventHandler(this.pathBox_DoubleClick);
            this.pathBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.pathBox_KeyDown);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.appStatus,
            this.udpStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 263);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(855, 22);
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "StatusStrip";
            // 
            // appStatus
            // 
            this.appStatus.Name = "appStatus";
            this.appStatus.Size = new System.Drawing.Size(404, 17);
            this.appStatus.Spring = true;
            this.appStatus.Text = "Application Status: ";
            // 
            // receiveButton
            // 
            this.receiveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.receiveButton.Location = new System.Drawing.Point(32, 135);
            this.receiveButton.Name = "receiveButton";
            this.receiveButton.Size = new System.Drawing.Size(103, 33);
            this.receiveButton.TabIndex = 0;
            this.receiveButton.Text = "RECEIVE";
            this.receiveButton.UseVisualStyleBackColor = true;
            this.receiveButton.Click += new System.EventHandler(this.receiveButton_Click);
            // 
            // dataInBox
            // 
            this.dataInBox.Location = new System.Drawing.Point(170, 191);
            this.dataInBox.Name = "dataInBox";
            this.dataInBox.Size = new System.Drawing.Size(624, 20);
            this.dataInBox.TabIndex = 2;
            // 
            // clearButton
            // 
            this.clearButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.clearButton.Location = new System.Drawing.Point(32, 185);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(103, 33);
            this.clearButton.TabIndex = 0;
            this.clearButton.Text = "CLEAR";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // udpStatus
            // 
            this.udpStatus.Name = "udpStatus";
            this.udpStatus.Size = new System.Drawing.Size(404, 17);
            this.udpStatus.Spring = true;
            this.udpStatus.Text = "Connection Status:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(855, 285);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.dataInBox);
            this.Controls.Add(this.pathBox);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.receiveButton);
            this.Controls.Add(this.executeButton);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button executeButton;
        private System.Windows.Forms.OpenFileDialog OpenFileDialog;
        private System.Windows.Forms.TextBox pathBox;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel appStatus;
        private System.Windows.Forms.Timer processChecker;
        private System.Windows.Forms.Timer processValidation;
        private System.Windows.Forms.Button receiveButton;
        private System.Windows.Forms.TextBox dataInBox;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.ToolStripStatusLabel udpStatus;
    }
}

