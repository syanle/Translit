namespace translit
{
    partial class TransForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TransForm));
            this.tbClipboardText = new System.Windows.Forms.TextBox();
            this.hiddenNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // tbClipboardText
            // 
            this.tbClipboardText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbClipboardText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.tbClipboardText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbClipboardText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbClipboardText.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tbClipboardText.Location = new System.Drawing.Point(4, -1);
            this.tbClipboardText.Multiline = true;
            this.tbClipboardText.Name = "tbClipboardText";
            this.tbClipboardText.ReadOnly = true;
            this.tbClipboardText.Size = new System.Drawing.Size(290, 33);
            this.tbClipboardText.TabIndex = 0;
            this.tbClipboardText.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tbClipboardText_MouseMove);
            this.tbClipboardText.Resize += new System.EventHandler(this.tbClipboardText_Resize);
            // 
            // hiddenNotifyIcon
            // 
            this.hiddenNotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("hiddenNotifyIcon.Icon")));
            this.hiddenNotifyIcon.Text = "Translit!";
            this.hiddenNotifyIcon.Visible = true;
            // 
            // TransForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(295, 25);
            this.Controls.Add(this.tbClipboardText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TransForm";
            this.ShowInTaskbar = false;
            this.Text = "Translit!";
            this.Deactivate += new System.EventHandler(this.TransForm_Deactivate);
            this.Load += new System.EventHandler(this.TransForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NotifyIcon hiddenNotifyIcon;
        private System.Windows.Forms.TextBox tbClipboardText;
    }
}

