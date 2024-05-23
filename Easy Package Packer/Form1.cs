namespace Easy_Package_Packer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void unpackedPathTextBox_TextChanged(object sender, EventArgs e)
            => packBtn.Enabled = !string.IsNullOrWhiteSpace(unpackedPathTextBox.Text);

        private void packedPathTextBox_TextChanged(object sender, EventArgs e)
            => unpackBtn.Enabled = !string.IsNullOrWhiteSpace(packedPathTextBox.Text);
    }
}
