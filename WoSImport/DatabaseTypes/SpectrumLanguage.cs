using System.Collections.Generic;

namespace WoSImport.DatabaseTypes
{
    public class SpectrumLanguage
    {
        public int Id { get; set; }
        public string Language { get; set; }

        public virtual ICollection<SpectrumProgramRelease> Releases { get; set; }
    }
}
