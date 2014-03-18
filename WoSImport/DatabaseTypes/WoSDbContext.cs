using System.Data.Entity;

namespace WoSImport.DatabaseTypes
{
    class WoSDbContext : DbContext
    {
        public DbSet<SpectrumProgram> Programs { get; set; }
        public DbSet<SpectrumProgramRelease> Releases { get; set; }

        public DbSet<SpectrumPublisher> Publishers { get; set; }
        public DbSet<SpectrumProgramType> ProgramTypes { get; set; }
        public DbSet<SpectrumAuthor> Authors { get; set; }
        public DbSet<SpectrumInstructionsFlag> InstructionsFlags { get; set; }
        public DbSet<SpectrumLoadingScreenFlag> LoadingScreenFlags { get; set; }
        public DbSet<SpectrumProgramNote> Notes { get; set; }

        public DbSet<SpectrumModel> Models { get; set; }
        public DbSet<SpectrumProtectionScheme> ProtectionSchemes { get; set; }
        public DbSet<SpectrumLanguage> Languages { get; set; }
        public DbSet<SpectrumJoystickType> JoystickTypes { get; set; }
    }
}
