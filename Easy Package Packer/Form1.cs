using DBPF_Compiler.DBPF;
using DBPF_Compiler.Types;
using System.ComponentModel;

namespace Easy_Package_Packer
{
    public partial class Form1 : Form
    {
        private readonly BackgroundWorker _packWorker;

        public Form1()
        {
            InitializeComponent();

            _packWorker = new();
            _packWorker.DoWork += Pack;
            _packWorker.RunWorkerCompleted += PackCompleted;
        }

        private void unpackedPathTextBox_TextChanged(object sender, EventArgs e)
            => packBtn.Enabled = unpackBtn.Enabled = !string.IsNullOrWhiteSpace(unpackedPathTextBox.Text) && !string.IsNullOrWhiteSpace(packedPathTextBox.Text);

        private void Pack(object? sender, DoWorkEventArgs e)
        {
            try
            {
                if (e.Argument is not WorkerPackArgument args)
                    throw new ArgumentException(null, nameof(e));

                DBPFPacker packer = new(args.UnpackedPath);
                packer.PackHandler += ProgressChanged;

                using FileStream stream = File.Create(args.PackagePath);
                using DatabasePackedFile dbpf = new(stream);
                packer.Pack(dbpf);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProgressChanged(object? sender, StringResourceKey key)
            => ++progressBar.Value;

        private void packBtn_Click(object sender, EventArgs e)
        {
            DisableElements();
            progressBar.Maximum = GetFilesCount(new(unpackedPathTextBox.Text), true);

            _packWorker.RunWorkerAsync(new WorkerPackArgument(unpackedPathTextBox.Text, packedPathTextBox.Text));
        }

        private void PackCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Файл запакован!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private static int GetFilesCount(DirectoryInfo dir, bool onlySubdirectories = false)
        {
            int count = onlySubdirectories ? 0 : dir.GetFiles().Length;

            foreach (var subdir in dir.GetDirectories())
                count += GetFilesCount(subdir);

            return count;
        }
    }
}
