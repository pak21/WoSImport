using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using WoSImport.DatabaseTypes;

namespace WoSImport
{
    class Program
    {

        static void Main(string[] args)
        {
            Database.SetInitializer<WoSDbContext>(new WoSDbContextInitializer());

            var programCache = new Dictionary<int, SpectrumProgram>();
            var publisherCache = new Dictionary<string, SpectrumPublisher>();
            var programTypeCache = new Dictionary<string, SpectrumProgramType>();
            var modelCache = new Dictionary<string, SpectrumModel>();
            var protectionSchemeCache = new Dictionary<string, SpectrumProtectionScheme>();
            var authorCache = new Dictionary<string, SpectrumAuthor>();
            var languageCache = new Dictionary<string, SpectrumLanguage>();
            var noteCache = new Dictionary<string, SpectrumProgramNote>();

            using (var db = new WoSDbContext())
            {
                db.Configuration.AutoDetectChangesEnabled = false;

                var instructionsFlags = new Dictionary<string, SpectrumInstructionsFlag> {
                    { string.Empty, db.InstructionsFlags.Where(n => n.Value == "NotAvailable").Single() },
                    { "s", db.InstructionsFlags.Where(n => n.Value == "Scanned").Single() },
                    { "x", db.InstructionsFlags.Where(n => n.Value == "Text").Single() }
                };

                var loadingScreenFlags = new Dictionary<string, SpectrumLoadingScreenFlag> {
                    { string.Empty, db.LoadingScreenFlags.Where(n => n.Value == "NotAvailable").Single() },
                    { "-", db.LoadingScreenFlags.Where(n => n.Value == "DoesNotUse").Single() },
                    { "x", db.LoadingScreenFlags.Where(n => n.Value == "Available").Single() }
                };

                var joystickTypes = new Dictionary<char, SpectrumJoystickType> {
                    { 'K', db.JoystickTypes.Where(j => j.JoystickType == "Kempston").Single() },
                    { '1', db.JoystickTypes.Where(j => j.JoystickType == "Sinclair 1").Single() },
                    { '2', db.JoystickTypes.Where(j => j.JoystickType == "Sinclair 2").Single() },
                    { 'C', db.JoystickTypes.Where(j => j.JoystickType == "Cursor").Single() },
                    { 'R', db.JoystickTypes.Where(j => j.JoystickType == "Redefinable").Single() }
                };

                foreach (var line in File.ReadAllLines(@"C:\Users\Philip\Desktop\Spectrum\Infoseek\infoseek-out.dat"))
                {
                    var fields = line.Split(new[] { '\t' });
                    var id = int.Parse(fields[0]);

                    if (!programCache.ContainsKey(id))
                    {
                        var authorNames = fields[10].Split(new[] { ',', '(', ')' })
                            .Select(name => name.Trim()).Where(name => !string.IsNullOrEmpty(name));
                        var authors = authorNames.Select(authorName =>
                            FindDbObject(authorName.Trim(), authorCache, db.Authors, n => new SpectrumAuthor { Name = n })).ToList();

                        var noteString = fields[26];
                        var note = string.IsNullOrEmpty(noteString) ? null :
                            FindDbObject(fields[26], noteCache, db.Notes, n => new SpectrumProgramNote { Note = n });

                        var program = new SpectrumProgram
                        {
                            Id = id,
                            Name = fields[1],
                            Publishers = ParsePublishers(fields[2], publisherCache, db),
                            ProgramType = FindDbObject(fields[6], programTypeCache, db.ProgramTypes, n => new SpectrumProgramType { Type = n }),
                            Authors = authors,
                            Score = EmptySafeParseToFloat(fields[18]),
                            Votes = EmptySafeParseToFloat(fields[19]),
                            Instructions = instructionsFlags[fields[20]],
                            Inlay = ParseBool(fields[21]),
                            Advert = ParseBool(fields[22]),
                            Map = ParseBool(fields[23]),
                            LoadingScreen = loadingScreenFlags[fields[24]],
                            InGameScreen = ParseBool(fields[25]),
                            Note = note,
                            InstructionsPath = EmptyStringToNull(fields[27])
                        };

                        programCache.Add(id, program);
                        db.Programs.Add(program);
                    }

                    var protectionSchemeName = fields[8];

                    var protectionScheme = string.IsNullOrEmpty(protectionSchemeName) ? null :
                        FindDbObject(fields[8], protectionSchemeCache, db.ProtectionSchemes, n => new SpectrumProtectionScheme { Name = n });

                    var allLanguageNames = fields[11];

                    ICollection<string> languageNames;
                    if (string.IsNullOrEmpty(allLanguageNames))
                    {
                        languageNames = new List<string>();
                    }
                    else if (allLanguageNames == "<I>unknown</I>")
                    {
                        languageNames = new List<string> { "[unknown]" };
                    }
                    else
                    {
                        languageNames = allLanguageNames.Split(new[] { '/' });
                    }

                    var languages = languageNames.Select(lanugageName =>
                        FindDbObject(lanugageName, languageCache, db.Languages, n => new SpectrumLanguage { Language = n })).ToList();

                    var release = new SpectrumProgramRelease
                    {
                        Program = programCache[id],
                        ReleaseName = EmptyStringToNull(fields[3]),
                        ReleasePublishers = ParsePublishers(fields[4], publisherCache, db),
                        Year = EmptySafeParseToInt(fields[5]),
                        Model = FindDbObject(fields[7], modelCache, db.Models, n => new SpectrumModel { Model = n }),
                        ProtectionScheme = protectionScheme,
                        Joysticks = fields[9].Select(c => joystickTypes[c]).ToList(),
                        Languages = languages,
                        ProductCode = EmptyStringToNull(fields[12]),
                        Barcode = EmptyStringToNull(fields[13]),
                        Filesize = EmptySafeParseToInt(fields[14]),
                        CRC32 = fields[15],
                        Filename = EmptyStringToNull(fields[16]),
                        Path = EmptyStringToNull(fields[17]),
                    };

                    db.Releases.Add(release);
                }

                db.SaveChanges();
            }
        }

        private static IList<SpectrumPublisher> ParsePublishers(
            string allPublisherNames,
            IDictionary<string, SpectrumPublisher> publisherCache,
            WoSDbContext db)
        {
            ICollection<string> publisherNames;
            if (string.IsNullOrEmpty(allPublisherNames))
            {
                publisherNames = new List<string>();
            }
            else if (allPublisherNames == "16/48 Tape Magazine")
            {
                publisherNames = new List<string> { allPublisherNames };
            }
            else
            {
                publisherNames = allPublisherNames.Split(new[] { '/' });
            }

            return publisherNames.Select(publisherName =>
                FindDbObject(publisherName, publisherCache, db.Publishers, n => new SpectrumPublisher { Name = n })).ToList();
        }

        private static TObj FindDbObject<TKey, TObj>(TKey key, IDictionary<TKey, TObj> cache, DbSet<TObj> dbSet, Func<TKey, TObj> generator)
            where TObj : class
        {
            TObj result;
            if (!cache.TryGetValue(key, out result))
            {
                result = generator(key);
                cache.Add(key, result);
                dbSet.Add(result);
            }
            return result;
        }

        private static int? EmptySafeParseToInt(string input)
        {
            return string.IsNullOrEmpty(input) ? null : (int?)int.Parse(input);
        }

        private static float? EmptySafeParseToFloat(string input)
        {
            return string.IsNullOrEmpty(input) ? null : (float?)float.Parse(input);
        }

        private static bool ParseBool(string input)
        {
            return input == "x";
        }

        private static string EmptyStringToNull(string input)
        {
            return string.IsNullOrEmpty(input) ? null : input;
        }
    }
}
