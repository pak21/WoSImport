using System.Data.Entity;

namespace WoSImport.DatabaseTypes
{
    class WoSDbContextInitializer : DropCreateDatabaseIfModelChanges<WoSDbContext>
    {
        protected override void Seed(WoSDbContext context)
        {
            context.InstructionsFlags.Add(new SpectrumInstructionsFlag { Value = "NotAvailable" });
            context.InstructionsFlags.Add(new SpectrumInstructionsFlag { Value = "Scanned" });
            context.InstructionsFlags.Add(new SpectrumInstructionsFlag { Value = "Text" });

            context.LoadingScreenFlags.Add(new SpectrumLoadingScreenFlag { Value = "NotAvailable" });
            context.LoadingScreenFlags.Add(new SpectrumLoadingScreenFlag { Value = "DoesNotUse" });
            context.LoadingScreenFlags.Add(new SpectrumLoadingScreenFlag { Value = "Available" });

            context.JoystickTypes.Add(new SpectrumJoystickType { JoystickType = "Kempston" });
            context.JoystickTypes.Add(new SpectrumJoystickType { JoystickType = "Sinclair 1" });
            context.JoystickTypes.Add(new SpectrumJoystickType { JoystickType = "Sinclair 2" });
            context.JoystickTypes.Add(new SpectrumJoystickType { JoystickType = "Cursor" });
            context.JoystickTypes.Add(new SpectrumJoystickType { JoystickType = "Redefinable" });
        }
    }
}
