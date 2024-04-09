namespace DBPF_Compiler.FileTypes
{
    public interface ISporeFile
    {
        uint TypeID { get; }

        /// <summary>
        /// Декодирует массив байтов
        /// </summary>
        /// <param name="data">Исходные данные</param>
        /// <returns><c>true</c>, если удалось декодировать, в противном случае <c>false</c></returns>
        bool Decode(byte[]? data);

        /// <summary>
        /// Кодирует файл и записывает в поток
        /// </summary>
        /// <param name="stream">Куда будет записан файл</param>
        /// <returns>Размер записанных данных</returns>
        uint WriteToStream(Stream stream);
    }
}
