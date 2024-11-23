namespace CHAT_LENAM
{
    partial class server
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
            this.richTextBoxMessages = new System.Windows.Forms.RichTextBox();
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.checkedListBoxClients = new System.Windows.Forms.CheckedListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonSendImage = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBoxMessages
            // 
            this.richTextBoxMessages.Location = new System.Drawing.Point(151, 12);
            this.richTextBoxMessages.Name = "richTextBoxMessages";
            this.richTextBoxMessages.Size = new System.Drawing.Size(563, 305);
            this.richTextBoxMessages.TabIndex = 0;
            this.richTextBoxMessages.Text = "";
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.Location = new System.Drawing.Point(151, 336);
            this.textBoxMessage.Multiline = true;
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.Size = new System.Drawing.Size(563, 55);
            this.textBoxMessage.TabIndex = 1;
            // 
            // checkedListBoxClients
            // 
            this.checkedListBoxClients.FormattingEnabled = true;
            this.checkedListBoxClients.Location = new System.Drawing.Point(1, 12);
            this.checkedListBoxClients.Name = "checkedListBoxClients";
            this.checkedListBoxClients.Size = new System.Drawing.Size(144, 259);
            this.checkedListBoxClients.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(644, 349);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(57, 33);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonSendImage
            // 
            this.buttonSendImage.Location = new System.Drawing.Point(49, 336);
            this.buttonSendImage.Name = "buttonSendImage";
            this.buttonSendImage.Size = new System.Drawing.Size(76, 46);
            this.buttonSendImage.TabIndex = 4;
            this.buttonSendImage.Text = "Image";
            this.buttonSendImage.UseVisualStyleBackColor = true;
            this.buttonSendImage.Click += new System.EventHandler(this.buttonSendImage_Click);
            // 
            // server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 437);
            this.Controls.Add(this.buttonSendImage);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkedListBoxClients);
            this.Controls.Add(this.textBoxMessage);
            this.Controls.Add(this.richTextBoxMessages);
            this.Name = "server";
            this.Text = "server";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxMessages;
        private System.Windows.Forms.TextBox textBoxMessage;
        private System.Windows.Forms.CheckedListBox checkedListBoxClients;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonSendImage;
    }
}

