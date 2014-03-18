using System.Collections.Generic;

namespace WoSImport.DatabaseTypes
{
    public class SpectrumAuthor
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<SpectrumProgram> Programs { get; set; }
    }
}
