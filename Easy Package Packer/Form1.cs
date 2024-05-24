using DBPF_Compiler.DBPF;
using System.ComponentModel;

namespace Easy_Package_Packer
{
    public partial class Form1 : Form
    {
        private readonly BackgroundWorker _bgWorker;

        public Form1()
        {
            InitializeComponent();

            _bgWorker = new()
            {
                WorkerReportsProgress = true,
            };
            _bgWorker.ProgressChanged += ProgressChanged;
        }

        private void unpackedPathTextBox_TextChanged(object sender, EventArgs e)
            => packBtn.Enabled = unpackBtn.Enabled = !string.IsNullOrWhiteSpace(unpackedPathTextBox.Text) && !string.IsNullOrWhiteSpace(packedPathTextBox.Text);

        private void Pack(object? sender, DoWorkEventArgs e)
        {
            using FileStream stream = File.Create(unpackedPathTextBox.Text);
            using DatabasePackedFile dbpf = new(stream);
            // ��������
        }

        private void ProgressChanged(object? sender, ProgressChangedEventArgs e)
            => progressBar.Value = e.ProgressPercentage;

        private void packBtn_Click(object sender, EventArgs e)
        {
            DisableElements();
            _bgWorker.DoWork += Pack;
            _bgWorker.RunWorkerCompleted += PackCompleted;
            _bgWorker.RunWorkerAsync();
        }

        private void PackCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("���� ���������!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            EnableElements();
        }

        private void DisableElements()
        {
            packedPathBrowseBtn.Enabled = false;
            unpackedPathBrowseBtn.Enabled = false;
            packedPathTextBox.Enabled = false;
            unpackBtn.Enabled = false;
            unpackedPathTextBox.Enabled = false;
            packBtn.Enabled = false;
        }

        private void EnableElements()
        {
            packedPathBrowseBtn.Enabled = true;
            unpackedPathBrowseBtn.Enabled = true;
            packedPathTextBox.Enabled = true;
            unpackBtn.Enabled = true;
            unpackedPathTextBox.Enabled = true;
            packBtn.Enabled = true;

            progressBar.Value = 0;
        }
    }
}
