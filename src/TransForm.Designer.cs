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
            this.hiddenNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.titlePanel = new System.Windows.Forms.Panel();
            this.iconContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.iCbonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.iPnonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iCeonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.iShowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iAboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.iExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wordsLengthUpperLimit = new System.Windows.Forms.Label();
            this.miniHide = new System.Windows.Forms.PictureBox();
            this.miniEditNQuery = new System.Windows.Forms.PictureBox();
            this.miniClose = new System.Windows.Forms.PictureBox();
            this.miniPin = new System.Windows.Forms.PictureBox();
            this.splitPanel = new System.Windows.Forms.Panel();
            this.textBoxPanel = new System.Windows.Forms.Panel();
            this.tbClipboardText = new System.Windows.Forms.RichTextBox();
            this.tbContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.edExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.edToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.cpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cpAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cpSrcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel.SuspendLayout();
            this.titlePanel.SuspendLayout();
            this.iconContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.miniHide)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.miniEditNQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.miniClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.miniPin)).BeginInit();
            this.textBoxPanel.SuspendLayout();
            this.tbContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // hiddenNotifyIcon
            // 
            this.hiddenNotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("hiddenNotifyIcon.Icon")));
            this.hiddenNotifyIcon.Text = "Translit!";
            this.hiddenNotifyIcon.Visible = true;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(253)))), ((int)(((byte)(253)))));
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.titlePanel, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.splitPanel, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.textBoxPanel, 0, 2);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 2F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(124, 68);
            this.tableLayoutPanel.TabIndex = 3;
            // 
            // titlePanel
            // 
            this.titlePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.titlePanel.ContextMenuStrip = this.iconContextMenu;
            this.titlePanel.Controls.Add(this.wordsLengthUpperLimit);
            this.titlePanel.Controls.Add(this.miniHide);
            this.titlePanel.Controls.Add(this.miniEditNQuery);
            this.titlePanel.Controls.Add(this.miniClose);
            this.titlePanel.Controls.Add(this.miniPin);
            this.titlePanel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.titlePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titlePanel.Location = new System.Drawing.Point(0, 0);
            this.titlePanel.Margin = new System.Windows.Forms.Padding(0);
            this.titlePanel.MinimumSize = new System.Drawing.Size(0, 30);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.Size = new System.Drawing.Size(124, 30);
            this.titlePanel.TabIndex = 0;
            this.titlePanel.Visible = false;
            this.titlePanel.VisibleChanged += new System.EventHandler(this.titlePanel_VisibleChanged);
            this.titlePanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.titlePanel_MouseClick);
            this.titlePanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.titlePanel_MouseDown);
            this.titlePanel.MouseEnter += new System.EventHandler(this.titlePanel_MouseEnter);
            this.titlePanel.MouseLeave += new System.EventHandler(this.titlePanel_MouseLeave);
            this.titlePanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.titlePanel_MouseMove);
            this.titlePanel.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.titlePanel_MouseWheel);
            // 
            // iconContextMenu
            // 
            this.iconContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iCbonToolStripMenuItem,
            this.toolStripSeparator1,
            this.iPnonToolStripMenuItem,
            this.iCeonToolStripMenuItem,
            this.toolStripSeparator2,
            this.iShowToolStripMenuItem,
            this.iAboutToolStripMenuItem,
            this.toolStripSeparator3,
            this.iExitToolStripMenuItem});
            this.iconContextMenu.Name = "iconContextMenu";
            this.iconContextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.iconContextMenu.Size = new System.Drawing.Size(181, 176);
            // 
            // iCbonToolStripMenuItem
            // 
            this.iCbonToolStripMenuItem.Checked = true;
            this.iCbonToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.iCbonToolStripMenuItem.Name = "iCbonToolStripMenuItem";
            this.iCbonToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F1)));
            this.iCbonToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.iCbonToolStripMenuItem.Text = "剪切板翻译";
            this.iCbonToolStripMenuItem.Click += new System.EventHandler(this.iCbonToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // iPnonToolStripMenuItem
            // 
            this.iPnonToolStripMenuItem.Checked = true;
            this.iPnonToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.iPnonToolStripMenuItem.Name = "iPnonToolStripMenuItem";
            this.iPnonToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.iPnonToolStripMenuItem.Text = "单词发音";
            this.iPnonToolStripMenuItem.Click += new System.EventHandler(this.iPnonToolStripMenuItem_Click);
            // 
            // iCeonToolStripMenuItem
            // 
            this.iCeonToolStripMenuItem.Name = "iCeonToolStripMenuItem";
            this.iCeonToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.iCeonToolStripMenuItem.Text = "中译英";
            this.iCeonToolStripMenuItem.Click += new System.EventHandler(this.iCeonToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // iShowToolStripMenuItem
            // 
            this.iShowToolStripMenuItem.Name = "iShowToolStripMenuItem";
            this.iShowToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.iShowToolStripMenuItem.Text = "显示界面";
            this.iShowToolStripMenuItem.Click += new System.EventHandler(this.iShowToolStripMenuItem_Click);
            // 
            // iAboutToolStripMenuItem
            // 
            this.iAboutToolStripMenuItem.Name = "iAboutToolStripMenuItem";
            this.iAboutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.iAboutToolStripMenuItem.Text = "关于";
            this.iAboutToolStripMenuItem.Click += new System.EventHandler(this.iAboutToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(177, 6);
            // 
            // iExitToolStripMenuItem
            // 
            this.iExitToolStripMenuItem.Name = "iExitToolStripMenuItem";
            this.iExitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.iExitToolStripMenuItem.Text = "退出";
            this.iExitToolStripMenuItem.Click += new System.EventHandler(this.iExitToolStripMenuItem_Click);
            // 
            // wordsLengthUpperLimit
            // 
            this.wordsLengthUpperLimit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.wordsLengthUpperLimit.AutoSize = true;
            this.wordsLengthUpperLimit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.wordsLengthUpperLimit.Location = new System.Drawing.Point(29, 9);
            this.wordsLengthUpperLimit.Name = "wordsLengthUpperLimit";
            this.wordsLengthUpperLimit.Size = new System.Drawing.Size(66, 13);
            this.wordsLengthUpperLimit.TabIndex = 2;
            this.wordsLengthUpperLimit.Text = "(5000/5000)";
            this.toolTip.SetToolTip(this.wordsLengthUpperLimit, "字符上限");
            this.wordsLengthUpperLimit.Visible = false;
            // 
            // miniHide
            // 
            this.miniHide.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.miniHide.Location = new System.Drawing.Point(52, 5);
            this.miniHide.Name = "miniHide";
            this.miniHide.Size = new System.Drawing.Size(20, 20);
            this.miniHide.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.miniHide.TabIndex = 6;
            this.miniHide.TabStop = false;
            this.miniHide.Visible = false;
            this.miniHide.MouseClick += new System.Windows.Forms.MouseEventHandler(this.miniHide_MouseClick);
            this.miniHide.MouseEnter += new System.EventHandler(this.miniHide_MouseEnter);
            this.miniHide.MouseLeave += new System.EventHandler(this.miniHide_MouseLeave);
            this.miniHide.MouseHover += new System.EventHandler(this.miniHide_MouseHover);
            // 
            // miniEditNQuery
            // 
            this.miniEditNQuery.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.miniEditNQuery.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.miniEditNQuery.Image = global::pdfTranslater.Properties.Resources.mini_edit;
            this.miniEditNQuery.Location = new System.Drawing.Point(8, 8);
            this.miniEditNQuery.Name = "miniEditNQuery";
            this.miniEditNQuery.Size = new System.Drawing.Size(15, 15);
            this.miniEditNQuery.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.miniEditNQuery.TabIndex = 5;
            this.miniEditNQuery.TabStop = false;
            this.miniEditNQuery.MouseClick += new System.Windows.Forms.MouseEventHandler(this.miniEditNQuery_MouseClick);
            this.miniEditNQuery.MouseEnter += new System.EventHandler(this.miniEditNQuery_MouseEnter);
            this.miniEditNQuery.MouseLeave += new System.EventHandler(this.miniEditNQuery_MouseLeave);
            this.miniEditNQuery.MouseHover += new System.EventHandler(this.miniEditNQuery_MouseHover);
            // 
            // miniClose
            // 
            this.miniClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.miniClose.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.miniClose.Image = ((System.Drawing.Image)(resources.GetObject("miniClose.Image")));
            this.miniClose.Location = new System.Drawing.Point(105, 10);
            this.miniClose.Name = "miniClose";
            this.miniClose.Size = new System.Drawing.Size(11, 11);
            this.miniClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.miniClose.TabIndex = 4;
            this.miniClose.TabStop = false;
            this.miniClose.MouseClick += new System.Windows.Forms.MouseEventHandler(this.miniClose_MouseClick);
            this.miniClose.MouseDown += new System.Windows.Forms.MouseEventHandler(this.miniClose_MouseDown);
            this.miniClose.MouseEnter += new System.EventHandler(this.miniClose_MouseEnter);
            this.miniClose.MouseLeave += new System.EventHandler(this.miniClose_MouseLeave);
            this.miniClose.MouseHover += new System.EventHandler(this.miniClose_MouseHover);
            // 
            // miniPin
            // 
            this.miniPin.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.miniPin.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.miniPin.Image = global::pdfTranslater.Properties.Resources.mini_sprig;
            this.miniPin.Location = new System.Drawing.Point(80, 7);
            this.miniPin.Name = "miniPin";
            this.miniPin.Size = new System.Drawing.Size(15, 15);
            this.miniPin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.miniPin.TabIndex = 0;
            this.miniPin.TabStop = false;
            this.miniPin.MouseClick += new System.Windows.Forms.MouseEventHandler(this.miniPin_MouseClick);
            this.miniPin.MouseDown += new System.Windows.Forms.MouseEventHandler(this.miniPin_MouseDown);
            this.miniPin.MouseEnter += new System.EventHandler(this.miniPin_MouseEnter);
            this.miniPin.MouseLeave += new System.EventHandler(this.miniPin_MouseLeave);
            this.miniPin.MouseHover += new System.EventHandler(this.miniPin_MouseHover);
            this.miniPin.MouseUp += new System.Windows.Forms.MouseEventHandler(this.miniPin_MouseUp);
            // 
            // splitPanel
            // 
            this.splitPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(195)))), ((int)(((byte)(245)))));
            this.splitPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitPanel.Location = new System.Drawing.Point(0, 30);
            this.splitPanel.Margin = new System.Windows.Forms.Padding(0);
            this.splitPanel.Name = "splitPanel";
            this.splitPanel.Size = new System.Drawing.Size(124, 2);
            this.splitPanel.TabIndex = 1;
            // 
            // textBoxPanel
            // 
            this.textBoxPanel.AutoSize = true;
            this.textBoxPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(253)))), ((int)(((byte)(253)))));
            this.textBoxPanel.Controls.Add(this.tbClipboardText);
            this.textBoxPanel.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBoxPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPanel.Location = new System.Drawing.Point(0, 32);
            this.textBoxPanel.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxPanel.Name = "textBoxPanel";
            this.textBoxPanel.Padding = new System.Windows.Forms.Padding(8);
            this.textBoxPanel.Size = new System.Drawing.Size(124, 36);
            this.textBoxPanel.TabIndex = 0;
            // 
            // tbClipboardText
            // 
            this.tbClipboardText.BackColor = System.Drawing.Color.White;
            this.tbClipboardText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbClipboardText.ContextMenuStrip = this.tbContextMenu;
            this.tbClipboardText.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbClipboardText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbClipboardText.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbClipboardText.Location = new System.Drawing.Point(8, 8);
            this.tbClipboardText.Margin = new System.Windows.Forms.Padding(0);
            this.tbClipboardText.Name = "tbClipboardText";
            this.tbClipboardText.ReadOnly = true;
            this.tbClipboardText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.tbClipboardText.Size = new System.Drawing.Size(108, 20);
            this.tbClipboardText.TabIndex = 1;
            this.tbClipboardText.TabStop = false;
            this.tbClipboardText.Text = "Translit!";
            this.tbClipboardText.EnabledChanged += new System.EventHandler(this.tbClipboardText_EnabledChanged);
            this.tbClipboardText.SizeChanged += new System.EventHandler(this.tbClipboardText_SizeChanged);
            this.tbClipboardText.TextChanged += new System.EventHandler(this.tbClipboardText_TextChanged);
            this.tbClipboardText.VisibleChanged += new System.EventHandler(this.tbClipboardText_VisibleChanged);
            this.tbClipboardText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tbClipboardText_MouseDown);
            this.tbClipboardText.MouseEnter += new System.EventHandler(this.tbClipboardText_MouseEnter);
            this.tbClipboardText.MouseLeave += new System.EventHandler(this.tbClipboardText_MouseLeave);
            this.tbClipboardText.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tbClipboardText_MouseMove);
            this.tbClipboardText.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbClipboardText_MouseUp);
            // 
            // tbContextMenu
            // 
            this.tbContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.edExitToolStripMenuItem,
            this.edToolStripMenuItem,
            this.toolStripSeparator4,
            this.cpToolStripMenuItem,
            this.cpAllToolStripMenuItem,
            this.cpSrcToolStripMenuItem});
            this.tbContextMenu.Name = "tbContextMenu";
            this.tbContextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.tbContextMenu.Size = new System.Drawing.Size(147, 120);
            // 
            // edExitToolStripMenuItem
            // 
            this.edExitToolStripMenuItem.Name = "edExitToolStripMenuItem";
            this.edExitToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.edExitToolStripMenuItem.Text = "退出编辑";
            this.edExitToolStripMenuItem.Visible = false;
            this.edExitToolStripMenuItem.Click += new System.EventHandler(this.edExitToolStripMenuItem_Click);
            // 
            // edToolStripMenuItem
            // 
            this.edToolStripMenuItem.Name = "edToolStripMenuItem";
            this.edToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.edToolStripMenuItem.Text = "编辑原文";
            this.edToolStripMenuItem.Click += new System.EventHandler(this.edToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(143, 6);
            // 
            // cpToolStripMenuItem
            // 
            this.cpToolStripMenuItem.Name = "cpToolStripMenuItem";
            this.cpToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.cpToolStripMenuItem.Text = "复制";
            this.cpToolStripMenuItem.Visible = false;
            this.cpToolStripMenuItem.Click += new System.EventHandler(this.cpToolStripMenuItem_Click);
            // 
            // cpAllToolStripMenuItem
            // 
            this.cpAllToolStripMenuItem.Name = "cpAllToolStripMenuItem";
            this.cpAllToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.cpAllToolStripMenuItem.Text = "复制全部译文";
            this.cpAllToolStripMenuItem.Click += new System.EventHandler(this.cpAllToolStripMenuItem_Click);
            // 
            // cpSrcToolStripMenuItem
            // 
            this.cpSrcToolStripMenuItem.Name = "cpSrcToolStripMenuItem";
            this.cpSrcToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.cpSrcToolStripMenuItem.Text = "复制全部原文";
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 600;
            this.toolTip.ReshowDelay = 1000;
            // 
            // TransForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(124, 68);
            this.Controls.Add(this.tableLayoutPanel);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(75, 30);
            this.Name = "TransForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Translit!";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Deactivate += new System.EventHandler(this.TransForm_Deactivate);
            this.Load += new System.EventHandler(this.TransForm_Load);
            this.VisibleChanged += new System.EventHandler(this.TransForm_VisibleChanged);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.titlePanel.ResumeLayout(false);
            this.titlePanel.PerformLayout();
            this.iconContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.miniHide)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.miniEditNQuery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.miniClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.miniPin)).EndInit();
            this.textBoxPanel.ResumeLayout(false);
            this.tbContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.NotifyIcon hiddenNotifyIcon;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Panel titlePanel;
        private System.Windows.Forms.PictureBox miniPin;
        private System.Windows.Forms.Panel textBoxPanel;
        private System.Windows.Forms.RichTextBox tbClipboardText;
        private System.Windows.Forms.PictureBox miniClose;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Panel splitPanel;
        private System.Windows.Forms.ContextMenuStrip tbContextMenu;
        private System.Windows.Forms.ToolStripMenuItem cpAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cpToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip iconContextMenu;
        private System.Windows.Forms.ToolStripMenuItem iCbonToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem iPnonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iCeonToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem iShowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iAboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem iExitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem edToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem edExitToolStripMenuItem;
        private System.Windows.Forms.PictureBox miniEditNQuery;
        private System.Windows.Forms.ToolStripMenuItem cpSrcToolStripMenuItem;
        private System.Windows.Forms.PictureBox miniHide;
        private System.Windows.Forms.Label wordsLengthUpperLimit;
    }
}

