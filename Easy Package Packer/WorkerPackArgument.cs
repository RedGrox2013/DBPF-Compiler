namespace Easy_Package_Packer
{
    internal class WorkerPackArgument(string unpackedPath, string packagePath)
    {
        public string UnpackedPath { get; set; } = unpackedPath;
        public string PackagePath { get; set; } = packagePath;
    }
}
