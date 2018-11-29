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
            this.plusButton = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbClipboardText
            // 
            this.tbClipboardText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.tbClipboardText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbClipboardText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbClipboardText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbClipboardText.ForeColor = System.Drawing.SystemColors.WindowText;
            this.tbClipboardText.Location = new System.Drawing.Point(5, 1);
            this.tbClipboardText.Multiline = true;
            this.tbClipboardText.Name = "tbClipboardText";
            this.tbClipboardText.ReadOnly = true;
            this.tbClipboardText.Size = new System.Drawing.Size(281, 201);
            this.tbClipboardText.TabIndex = 0;
            this.tbClipboardText.EnabledChanged += new System.EventHandler(this.tbClipboardText_EnabledChanged);
            this.tbClipboardText.MouseEnter += new System.EventHandler(this.tbClipboardText_MouseEnter);
            this.tbClipboardText.MouseLeave += new System.EventHandler(this.tbClipboardText_MouseLeave);
            this.tbClipboardText.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tbClipboardText_MouseMove);
            // 
            // hiddenNotifyIcon
            // 
            this.hiddenNotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("hiddenNotifyIcon.Icon")));
            this.hiddenNotifyIcon.Text = "Translit!";
            this.hiddenNotifyIcon.Visible = true;
            // 
            // plusButton
            // 
            this.plusButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.plusButton.AutoSize = true;
            this.plusButton.BackColor = System.Drawing.Color.Transparent;
            this.plusButton.Font = new System.Drawing.Font("SansSerif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.plusButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.plusButton.Location = new System.Drawing.Point(265, 175);
            this.plusButton.Name = "plusButton";
            this.plusButton.Size = new System.Drawing.Size(33, 28);
            this.plusButton.TabIndex = 2;
            this.plusButton.Text = "   ";
            this.plusButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.plusButton_MouseClick);
            this.plusButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.plusButton_MouseDown);
            this.plusButton.MouseEnter += new System.EventHandler(this.plusButton_MouseEnter);
            this.plusButton.MouseLeave += new System.EventHandler(this.plusButton_MouseLeave);
            this.plusButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.plusButton_MouseUp);
            // 
            // TransForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(288, 202);
            this.Controls.Add(this.plusButton);
            this.Controls.Add(this.tbClipboardText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TransForm";
            this.Padding = new System.Windows.Forms.Padding(5, 1, 2, 0);
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
        private System.Windows.Forms.Label plusButton;
    }
}

