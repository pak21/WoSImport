using System.Collections.Generic;

namespace WoSImport.DatabaseTypes
{
    public class SpectrumPublisher
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<SpectrumProgram> Programs { get; set; }
        public virtual ICollection<SpectrumProgramRelease> Releases { get; set; }
    }
}
