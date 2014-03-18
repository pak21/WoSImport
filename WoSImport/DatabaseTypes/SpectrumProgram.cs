using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WoSImport.DatabaseTypes
{
    public class SpectrumProgram
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Name { get; set; }
        public virtual ICollection<SpectrumPublisher> Publishers { get; set; }
        public virtual SpectrumProgramType ProgramType { get; set; }
        public virtual ICollection<SpectrumAuthor> Authors { get; set; }
        public float? Score { get; set; }
        public float? Votes { get; set; }
        public SpectrumInstructionsFlag Instructions { get; set; }
        public bool Inlay { get; set; }
        public bool Advert { get; set; }
        public bool Map { get; set; }
        public SpectrumLoadingScreenFlag LoadingScreen { get; set; }
        public bool InGameScreen { get; set; }
        public SpectrumProgramNote Note { get; set; }
        public string InstructionsPath { get; set; }
    }
}
