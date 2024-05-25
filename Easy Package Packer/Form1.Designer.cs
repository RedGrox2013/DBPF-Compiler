namespace Easy_Package_Packer
{
    partial class Form1
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
            unpackedPathBrowseBtn = new Button();
            packedPathBrowseBtn = new Button();
            packedPathTextBox = new TextBox();
            label1 = new Label();
            label2 = new Label();
            progressBar = new ProgressBar();
            unpackBtn = new Button();
            packBtn = new Button();
            unpackedPathTextBox = new TextBox();
            SuspendLayout();
            // 
            // unpackedPathBrowseBtn
            // 
            unpackedPathBrowseBtn.Anchor = AnchorStyles.Top;
            unpackedPathBrowseBtn.Location = new Point(542, 9);
            unpackedPathBrowseBtn.Margin = new Padding(3, 2, 3, 2);
            unpackedPathBrowseBtn.Name = "unpackedPathBrowseBtn";
            unpackedPathBrowseBtn.Size = new Size(82, 22);
            unpackedPathBrowseBtn.TabIndex = 1;
            unpackedPathBrowseBtn.Text = "Обзор";
            unpackedPathBrowseBtn.UseVisualStyleBackColor = true;
            unpackedPathBrowseBtn.Click += unpackedPathBrowseBtn_Click;
            // 
            // packedPathBrowseBtn
            // 
            packedPathBrowseBtn.Anchor = AnchorStyles.Top;
            packedPathBrowseBtn.Location = new Point(542, 34);
            packedPathBrowseBtn.Margin = new Padding(3, 2, 3, 2);
            packedPathBrowseBtn.Name = "packedPathBrowseBtn";
            packedPathBrowseBtn.Size = new Size(82, 22);
            packedPathBrowseBtn.TabIndex = 3;
            packedPathBrowseBtn.Text = "Обзор";
            packedPathBrowseBtn.UseVisualStyleBackColor = true;
            packedPathBrowseBtn.Click += packedPathBrowseBtn_Click;
            // 
            // packedPathTextBox
            // 
            packedPathTextBox.Anchor = AnchorStyles.Top;
            packedPathTextBox.Location = new Point(153, 34);
            packedPathTextBox.Margin = new Padding(3, 2, 3, 2);
            packedPathTextBox.Name = "packedPathTextBox";
            packedPathTextBox.Size = new Size(385, 23);
            packedPathTextBox.TabIndex = 2;
            packedPathTextBox.TextChanged += unpackedPathTextBox_TextChanged;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top;
            label1.AutoSize = true;
            label1.Location = new Point(1, 12);
            label1.Name = "label1";
            label1.Size = new Size(134, 15);
            label1.TabIndex = 4;
            label1.Text = "Распакованные файлы";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top;
            label2.AutoSize = true;
            label2.Location = new Point(57, 36);
            label2.Name = "label2";
            label2.Size = new Size(85, 15);
            label2.TabIndex = 5;
            label2.Text = "Package-файл";
            // 
            // progressBar
            // 
            progressBar.Dock = DockStyle.Bottom;
            progressBar.Location = new Point(0, 94);
            progressBar.Margin = new Padding(3, 2, 3, 2);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(634, 22);
            progressBar.TabIndex = 6;
            // 
            // unpackBtn
            // 
            unpackBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            unpackBtn.Enabled = false;
            unpackBtn.Location = new Point(416, 68);
            unpackBtn.Margin = new Padding(3, 2, 3, 2);
            unpackBtn.Name = "unpackBtn";
            unpackBtn.Size = new Size(102, 22);
            unpackBtn.TabIndex = 7;
            unpackBtn.Text = "Распаковать";
            unpackBtn.UseVisualStyleBackColor = true;
            unpackBtn.Click += unpackBtn_Click;
            // 
            // packBtn
            // 
            packBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            packBtn.Enabled = false;
            packBtn.Location = new Point(523, 68);
            packBtn.Margin = new Padding(3, 2, 3, 2);
            packBtn.Name = "packBtn";
            packBtn.Size = new Size(102, 22);
            packBtn.TabIndex = 8;
            packBtn.Text = "Запаковать";
            packBtn.UseVisualStyleBackColor = true;
            packBtn.Click += packBtn_Click;
            // 
            // unpackedPathTextBox
            // 
            unpackedPathTextBox.Anchor = AnchorStyles.Top;
            unpackedPathTextBox.Location = new Point(153, 9);
            unpackedPathTextBox.Margin = new Padding(3, 2, 3, 2);
            unpackedPathTextBox.Name = "unpackedPathTextBox";
            unpackedPathTextBox.Size = new Size(385, 23);
            unpackedPathTextBox.TabIndex = 0;
            unpackedPathTextBox.TextChanged += unpackedPathTextBox_TextChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(634, 116);
            Controls.Add(packBtn);
            Controls.Add(unpackBtn);
            Controls.Add(progressBar);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(packedPathBrowseBtn);
            Controls.Add(packedPathTextBox);
            Controls.Add(unpackedPathBrowseBtn);
            Controls.Add(unpackedPathTextBox);
            Margin = new Padding(3, 2, 3, 2);
            MinimumSize = new Size(650, 155);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Database Packed File Compiler";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox packedPathTextBox;
        private Button unpackedPathBrowseBtn;
        private Button packedPathBrowseBtn;
        private Label label1;
        private Label label2;
        private ProgressBar progressBar;
        private Button unpackBtn;
        private Button packBtn;
        private TextBox unpackedPathTextBox;
    }
}
