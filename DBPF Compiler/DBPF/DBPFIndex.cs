namespace DBPF_Compiler.DBPF
{
    internal struct DBPFIndex
    {
        public const uint ValuesFlag = 4;
        public readonly uint? TypeID = null;
        public readonly uint? GroupID = null;
        public const uint UnknownID = 0;

        /*/// <summary>
        /// A bitmask representing what values are in the index header that will apply to all index entries.
        /// Only a value between 4 and 7 is valid.
        /// </summary>
        public uint ValuesFlag
        {
            readonly get => _valuesFlag;
            set
            {
                if (value >= 4 && value <= 7)
                    _valuesFlag = value;
                else
                    _valuesFlag = 4;
            }
        }
        private uint _valuesFlag = 4;
        public uint? TypeID
        {
            readonly get
            {
                if ((ValuesFlag & 0b001) == 1)
                    return _typeID;
                return null;
            }
            set => _typeID = value ?? 0;
        }
        private uint _typeID;
        public uint? GroupID
        {
            readonly get
            {
                if ((ValuesFlag & 0b010) == 1)
                    return _groupID;
                return null;
            }
            set => _groupID = value ?? 0;
        }
        private uint _groupID;*/

        public DBPFIndex() { }
    }
}
