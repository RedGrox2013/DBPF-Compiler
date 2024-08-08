﻿using DBPF_Compiler.DBPF;
using DBPF_Compiler.Types;
using System.Text.Json.Serialization;

namespace DBPF_Compiler.ModsTemplates
{
    internal class MusicModTemplate : IModTemplate
    {
        //////// soundprop ////////
        public string? FileName { get; set; }
        public StringResourceKey MusicTemplate { get; set; } = new("0x869DB904");
        public bool IsLooped { get; set; } = true;
        ///////////////////////////

        public string? PlannerThumbnail { get; set; }

        public string? SporepediaName { get; set; }

        public void BuildMod(DatabasePackedFile dbpf, ModTemplateHelper helper)
        {
            if (string.IsNullOrEmpty(FileName))
                throw new NullReferenceException();

            string name = Path.GetFileNameWithoutExtension(FileName);
            // доделать
        }
    }
}
