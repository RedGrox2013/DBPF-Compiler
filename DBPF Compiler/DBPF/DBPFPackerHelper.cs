﻿using DBPF_Compiler.FileTypes;
using DBPF_Compiler.FNV;
using DBPF_Compiler.Types;

namespace DBPF_Compiler.DBPF
{
    public class DBPFPackerHelper
    {
        public string? ProjectFolderPath { get; set; }
        public string? ProjectName { get; set; }

        private LocalizationTable? _localizationTable;
        private uint _lastTextID = 0;

        private string? _localeTableID;
        public string LocalizationTableID
        {
            get
            {
                if (_localizationTable == null)
                    return string.Empty;
                if (!string.IsNullOrEmpty(_localeTableID))
                    return _localeTableID;

                if (string.IsNullOrWhiteSpace(ProjectName))
                {
                    _random ??= new Random();
                    _localeTableID = FNVHash.ToString((uint)_random.NextInt64());
                }
                else
                    _localeTableID = ProjectName + "__autogenerated";

                return _localeTableID;
            }
            set => _localeTableID = value;
        }

        private Random? _random;

        public string? GetText(string textID) => _localizationTable?.GetText(textID);
        public string? GetText(uint textID) => _localizationTable?.GetText(textID);

        public StringLocalizedString AddText(string text)
        {
            (_localizationTable ??= new LocalizationTable()).AddText(++_lastTextID, text);

            var res = new StringLocalizedString(LocalizationTableID, FNVHash.ToString(_lastTextID));
            try
            {
                res.PlaceholderText = text;
            }
            catch
            {
                res.PlaceholderText = "PLACEHOLDER";
            }

            return res;
        }

        internal void WriteHelperData(DatabasePackedFile dbpf)
        {
            if (_localizationTable == null)
                return;

            if (!FNVHash.TryParse(LocalizationTableID, out uint tableID))
                tableID = FNVHash.Compute(LocalizationTableID);

            dbpf.WriteSporeFile(_localizationTable,
                new(tableID, (uint)TypeIDs.locale, (uint)GroupIDs.locale));
        }
    }
}
