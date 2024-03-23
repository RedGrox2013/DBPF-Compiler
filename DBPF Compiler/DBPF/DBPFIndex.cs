namespace DBPF_Compiler.DBPF
{
    internal struct DBPFIndex
    {
        public uint ValuesFlag { get; set; } = 4;
        public uint? TypeID
        {
            readonly get => GetFlagAt(0) ? _typeID : null;
            set => _typeID = value ?? 0;
        }
        public uint? GroupID
        {
            readonly get => GetFlagAt(1) ? _groupID : null;
            set => _groupID = value ?? 0;
        }
        public uint? UnknownID
        {
            readonly get => GetFlagAt(2) ? _unknownID : null;
            set => _unknownID = value ?? 0;
        }

        private uint _typeID, _groupID, _unknownID;

        public readonly int SizeWithoutEntries
        {
            get
            {
                int size = sizeof(uint);
                if (GetFlagAt(0))
                    size += sizeof(uint);
                if (GetFlagAt(1))
                    size += sizeof(uint);
                if (GetFlagAt(2))
                    size += sizeof(uint);

                return size;
            }
        }

        public readonly bool GetFlagAt(int bitIndex)
            => ((ValuesFlag >> bitIndex) & 0b0001) == 1;

        public List<IndexEntry> Entries { get; private set; }

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

        public DBPFIndex() => Entries = [];
    }
}
