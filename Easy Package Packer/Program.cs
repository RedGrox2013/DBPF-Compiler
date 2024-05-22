using DBPF_Compiler.FNV;

namespace Easy_Package_Packer
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            LoadRegistries();

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }

        private static void LoadRegistries()
        {
            DirectoryInfo regDir = new("Registries");
            if (!regDir.Exists)
                return;

            foreach (var file in regDir.GetFiles())
                if (file.Name.StartsWith("reg_"))
                    NameRegistryManager.Instance.AddRegistryFromFileAsync(file.FullName).Wait();
        }
    }
}