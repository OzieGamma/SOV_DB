namespace DB
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    internal class Program
    {
        private const string DirPath = "C:\\Users\\Oswald\\Downloads\\Movies";

        private static void Main()
        {
            var task = Task.WhenAll(
                Task.Run(() => HandleCsv("ALTERNATIVE_TITLE.csv", ImportAlternativeTitle)),
                Task.Run(() => HandleCsv("CHARACTER.csv", ImportCharacter)),
                Task.Run(() => HandleCsv("PRODUCTION_CAST.csv", ImportProductionCast)));

            task.Wait();
            foreach (var errors in task.Result)
            {
                Console.WriteLine(string.Join(Environment.NewLine, errors.Take(10)));
            }

            Console.WriteLine(
                "Unique roles: {0} {1}",
                Environment.NewLine,
                string.Join(Environment.NewLine + "Role :", UsedRoles));

            Console.Read();
        }

        private static async Task<List<string>>  HandleCsv(string file, Func<string[], bool> handleLine)
        {
            var errors = new List<string>();

            int i = 0;
            using (TextReader tr = File.OpenText(Path.Combine(DirPath, file)))
            {
                string line = await tr.ReadLineAsync();
                while (line != null)
                {
                    try
                    {
                        if (!handleLine(line.Split('\t')))
                        {
                            string msg = string.Format("[{0}] Can't parse line {1}, {2}", file, i, line);
                            errors.Add(msg);
                        }
                    }
                    catch (Exception)
                    {
                        string msg = string.Format("[{0}] Exception while parsing {1}, {2}", file, i, line);
                        errors.Add(msg);
                    }

                    line = await tr.ReadLineAsync();

                    if (i % 10000 == 0)
                    {
                        Console.WriteLine("[{0}] Done with {1}", file, i);
                    }
                    i += 1;
                }
            }

            return errors;
        }

        private static bool ImportAlternativeTitle(string[] inp)
        {
            long id = long.Parse(inp[0]);
            long productionId = long.Parse(inp[1]);
            string title = inp[2];

            return title != "\\N" && !string.IsNullOrWhiteSpace(title);
        }

        private static bool ImportCharacter(string[] inp)
        {
            long id = long.Parse(inp[0]);
            string name = inp[1];

            return name != "\\N" && !string.IsNullOrWhiteSpace(name);
        }

        private static readonly ISet<string> UsedRoles = new HashSet<string>();

        private static bool ImportProductionCast(string[] inp)
        {
            long productionId = long.Parse(inp[0]);
            long personId = long.Parse(inp[1]);
            long? characterId;
            if (inp[2] == "\\N")
            {
                characterId = null;
            }
            else
            {
                characterId = long.Parse(inp[2]);
            }

            string role = inp[3];

            UsedRoles.Add(role);

            return role != "\\N" && !string.IsNullOrWhiteSpace(role);
        }
    }
}
