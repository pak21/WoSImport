using System.Collections.Generic;

namespace WoSImport.DatabaseTypes
{
    public class SpectrumJoystickType
    {
        public int Id { get; set; }
        public string JoystickType { get; set; }

        public virtual ICollection<SpectrumProgramRelease> Releases { get; set; }
    }
}
