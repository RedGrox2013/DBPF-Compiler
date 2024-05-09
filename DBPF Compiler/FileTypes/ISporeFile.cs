namespace DBPF_Compiler.FileTypes
{
    public interface ISporeFile
    {
        TypeIDs TypeID { get; }

        /// <summary>
        /// Декодирует данные
        /// </summary>
        /// <param name="input">Поток данных</param>
        /// <returns>
        /// <c>true</c>, если удалось декодировать, в противном случае <c>false</c>
        /// </returns>
        bool Decode(Stream input);

        /// <summary>
        /// Кодирует файл и записывает в поток
        /// </summary>
        /// <param name="output">Куда будет записан файл</param>
        /// <returns>Размер записанных данных</returns>
        uint Encode(Stream output);
    }
}
