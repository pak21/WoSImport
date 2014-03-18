using System.Collections.Generic;

namespace WoSImport.DatabaseTypes
{
    public class SpectrumProgramRelease
    {
        public int Id { get; set; }

        public SpectrumProgram Program { get; set; }

        public string ReleaseName { get; set; }
        public virtual ICollection<SpectrumPublisher> ReleasePublishers { get; set; }
        public int? Year { get; set; }
        public SpectrumModel Model { get; set; }
        public SpectrumProtectionScheme ProtectionScheme { get; set; }
        public virtual ICollection<SpectrumJoystickType> Joysticks { get; set; }
        public virtual ICollection<SpectrumLanguage> Languages { get; set; }
        public string ProductCode { get; set; }
        public string Barcode { get; set; }
        public int? Filesize { get; set; }
        public string CRC32 { get; set; }
        public string Filename { get; set; }
        public string Path { get; set; }
    }
}
