﻿using DBPF_Compiler.DBPF;
using DBPF_Compiler.FNV;
using DBPF_Compiler.Types;
using DBPF_Compiler.FileTypes.Prop;

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

        public StringResourceKey TemplateFixedObjectAudioMusic { get; set; } = new("TemplateFixedObjectAudioMusic", "prop", "PaletteItems");
        public StringResourceKey ModelMeshLOD0 { get; set; } = new("ep1_audioicon_default2", "rw4", "EP1_TerrainIcons");

        public void BuildMod(DatabasePackedFile dbpf, DBPFPackerHelper helper)
        {
            if (string.IsNullOrEmpty(FileName))
                throw new NullReferenceException();

            string name = Path.GetFileNameWithoutExtension(FileName);
            string templatePath = Path.Combine(helper.ProjectFolderPath ?? string.Empty,
                ".dbpfc_ignore", "templates");
            string filePath = Path.Combine(templatePath, name);
            if (Path.GetExtension(FileName).Equals(".mp3", StringComparison.InvariantCultureIgnoreCase))
            {
                // доделать конвертацию в snr
            }

            uint musicID = FNVHash.Compute(name);
            using FileStream audio = File.OpenRead(filePath + ".snr");
            dbpf.CopyFromStream(audio, new(musicID, (uint)TypeIDs.snr, (uint)GroupIDs.audio));
            PropertyList soundProp = new([
                new Property("gain") {PropertyType = PropertyType.@float, Value = .8f},
                new Property("islooped") {PropertyType = PropertyType.@bool, Value = IsLooped},
                new Property("musicTemplate") {PropertyType = PropertyType.key, Value = MusicTemplate},
                new Property("samples") {PropertyType = PropertyType.keys, Value = new ResourceKey[] {new(musicID)} },
                new Property("codec") {PropertyType = PropertyType.uint32, Value = 5}
                ]);
            dbpf.WriteSporeFile(soundProp, new(musicID, (uint)TypeIDs.soundProp, (uint)GroupIDs.audio));

            if (!string.IsNullOrWhiteSpace(PlannerThumbnail))
            {
                using FileStream thumb = File.OpenRead(Path.Combine(templatePath, PlannerThumbnail));
                dbpf.CopyFromStream(thumb, new(musicID, (uint)TypeIDs.png, (uint)GroupIDs.PlannerThumbnails));
            }

            PropertyList paletteItem = new([
                new Property("parent") {PropertyType = PropertyType.key, Value = TemplateFixedObjectAudioMusic},
                new Property("adventureMusicId") {PropertyType = PropertyType.uint32, Value = musicID},
                new Property("modelMeshLOD0") {PropertyType = PropertyType.key, Value = ModelMeshLOD0},
                new Property("paletteItemPlacedAsset") {PropertyType = PropertyType.key, Value = new ResourceKey(0xe5855b05, 0x074e0069, (uint)GroupIDs.civicobjects)}, // civicobjects!ep1_audioobject_music.cPlaceableSound
                // доделать текст
                new Property("sporepediaShow") {PropertyType = PropertyType.@bool, Value = true}
                ]);
            dbpf.WriteSporeFile(paletteItem, new(musicID, (uint)TypeIDs.prop, (uint)GroupIDs.PaletteItems));
        }
    }
}
