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
            packedPathTextBox = new TextBox();
            unpackedPathBrowseBtn = new Button();
            packedPathBrowseBtn = new Button();
            unpackedPathTextBox = new TextBox();
            label1 = new Label();
            label2 = new Label();
            progressBar = new ProgressBar();
            unpackBtn = new Button();
            packBtn = new Button();
            SuspendLayout();
            // 
            // packedPathTextBox
            // 
            packedPathTextBox.Anchor = AnchorStyles.Top;
            packedPathTextBox.Location = new Point(175, 12);
            packedPathTextBox.Name = "packedPathTextBox";
            packedPathTextBox.Size = new Size(439, 27);
            packedPathTextBox.TabIndex = 0;
            // 
            // unpackedPathBrowseBtn
            // 
            unpackedPathBrowseBtn.Anchor = AnchorStyles.Top;
            unpackedPathBrowseBtn.Location = new Point(620, 12);
            unpackedPathBrowseBtn.Name = "unpackedPathBrowseBtn";
            unpackedPathBrowseBtn.Size = new Size(94, 29);
            unpackedPathBrowseBtn.TabIndex = 1;
            unpackedPathBrowseBtn.Text = "Обзор";
            unpackedPathBrowseBtn.UseVisualStyleBackColor = true;
            // 
            // packedPathBrowseBtn
            // 
            packedPathBrowseBtn.Anchor = AnchorStyles.Top;
            packedPathBrowseBtn.Location = new Point(620, 45);
            packedPathBrowseBtn.Name = "packedPathBrowseBtn";
            packedPathBrowseBtn.Size = new Size(94, 29);
            packedPathBrowseBtn.TabIndex = 3;
            packedPathBrowseBtn.Text = "Обзор";
            packedPathBrowseBtn.UseVisualStyleBackColor = true;
            // 
            // unpackedPathTextBox
            // 
            unpackedPathTextBox.Anchor = AnchorStyles.Top;
            unpackedPathTextBox.Location = new Point(175, 45);
            unpackedPathTextBox.Name = "unpackedPathTextBox";
            unpackedPathTextBox.Size = new Size(439, 27);
            unpackedPathTextBox.TabIndex = 2;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top;
            label1.AutoSize = true;
            label1.Location = new Point(1, 16);
            label1.Name = "label1";
            label1.Size = new Size(168, 20);
            label1.TabIndex = 4;
            label1.Text = "Распакованные файлы";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top;
            label2.AutoSize = true;
            label2.Location = new Point(65, 48);
            label2.Name = "label2";
            label2.Size = new Size(104, 20);
            label2.TabIndex = 5;
            label2.Text = "Package-файл";
            // 
            // progressBar
            // 
            progressBar.Dock = DockStyle.Bottom;
            progressBar.Location = new Point(0, 124);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(722, 29);
            progressBar.TabIndex = 6;
            progressBar.Click += progressBar1_Click;
            // 
            // unpackBtn
            // 
            unpackBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            unpackBtn.Location = new Point(476, 89);
            unpackBtn.Name = "unpackBtn";
            unpackBtn.Size = new Size(116, 29);
            unpackBtn.TabIndex = 7;
            unpackBtn.Text = "Распаковать";
            unpackBtn.UseVisualStyleBackColor = true;
            // 
            // packBtn
            // 
            packBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            packBtn.Location = new Point(598, 89);
            packBtn.Name = "packBtn";
            packBtn.Size = new Size(116, 29);
            packBtn.TabIndex = 8;
            packBtn.Text = "Запаковать";
            packBtn.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(722, 153);
            Controls.Add(packBtn);
            Controls.Add(unpackBtn);
            Controls.Add(progressBar);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(packedPathBrowseBtn);
            Controls.Add(unpackedPathTextBox);
            Controls.Add(unpackedPathBrowseBtn);
            Controls.Add(packedPathTextBox);
            MinimumSize = new Size(740, 150);
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
        private TextBox unpackedPathTextBox;
        private Label label1;
        private Label label2;
        private ProgressBar progressBar;
        private Button unpackBtn;
        private Button packBtn;
    }
}
