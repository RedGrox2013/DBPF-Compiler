using DBPF_Compiler.DBPF;
using DBPF_Compiler.Types;
using System.ComponentModel;

namespace Easy_Package_Packer
{
    public partial class Form1 : Form
    {
        private readonly BackgroundWorker _packWorker, _unpackWorker;

        public Form1()
        {
            InitializeComponent();

            _packWorker = new();
            _packWorker.DoWork += Pack;
            _packWorker.RunWorkerCompleted += PackCompleted;

            _unpackWorker = new();
            _unpackWorker.DoWork += Unpack;
            _unpackWorker.RunWorkerCompleted += UnpackCompleted;
        }

        private void unpackedPathTextBox_TextChanged(object sender, EventArgs e)
            => packBtn.Enabled = unpackBtn.Enabled = !string.IsNullOrWhiteSpace(unpackedPathTextBox.Text) && !string.IsNullOrWhiteSpace(packedPathTextBox.Text);

        private void Pack(object? sender, DoWorkEventArgs e)
        {
            if (e.Argument is not WorkerPackArgument args)
                throw new ArgumentException(null, nameof(e));

            DBPFPacker packer = new(args.UnpackedPath);
            packer.PackHandler += ProgressChanged;

            using FileStream stream = File.Create(args.PackagePath);
            using DatabasePackedFile dbpf = new(stream);
            packer.Pack(dbpf);
        }

        private void ProgressChanged(object? sender, StringResourceKey key)
            => ++progressBar.Value;

        private void packBtn_Click(object sender, EventArgs e)
        {
            try
            {
                progressBar.Maximum = GetFilesCount(new(unpackedPathTextBox.Text), true);
            }
            catch
            {
                MessageBox.Show("Некорректный путь: " + unpackedPathTextBox.Text, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            DisableElements();
            _packWorker.RunWorkerAsync(new WorkerPackArgument(unpackedPathTextBox.Text, packedPathTextBox.Text));
        }

        private void PackCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                MessageBox.Show(e.Error.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
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

        private void unpackedPathBrowseBtn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new();
            if (folderBrowser.ShowDialog() == DialogResult.OK)
                unpackedPathTextBox.Text = folderBrowser.SelectedPath;
        }

        private void packedPathBrowseBtn_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new()
            {
                Filter = "Database Packed File (*.package)|*.package|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                packedPathTextBox.Text = openFileDialog.FileName;
        }

        private void unpackBtn_Click(object sender, EventArgs e)
        {
            string outputPath;
            try
            {
                outputPath = unpackedPathTextBox.Text + "\\" + Path.GetFileNameWithoutExtension(packedPathTextBox.Text);
                if (!Directory.Exists(outputPath))
                    Directory.CreateDirectory(outputPath);

                progressBar.Maximum = GetEntriesCount(packedPathTextBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DisableElements();
            _unpackWorker.RunWorkerAsync(new WorkerPackArgument(outputPath, packedPathTextBox.Text));
        }

        private static int GetEntriesCount(string dbpfPath)
        {
            using FileStream file = File.OpenRead(dbpfPath);
            using DatabasePackedFile dbpf = new(file);

            return dbpf.ReadDBPFInfo().Length;
        }

        private void Unpack(object? sender, DoWorkEventArgs e)
        {
            if (e.Argument is not WorkerPackArgument args)
                throw new ArgumentException(null, nameof(e));

            DBPFPacker packer = new(args.UnpackedPath);
            packer.UnpackHandler += ProgressChanged;

            using FileStream file = File.OpenRead(args.PackagePath);
            using DatabasePackedFile dbpf = new(file);
            packer.Unpack(dbpf);
        }

        private void UnpackCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                MessageBox.Show(e.Error.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show("Файл распакован!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

            EnableElements();
        }
    }
}
