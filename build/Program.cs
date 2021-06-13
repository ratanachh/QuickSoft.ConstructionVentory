using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static Bullseye.Targets;
using static SimpleExec.Command;
using GlobExpressions;

namespace build
{
    public static class Program
    {
        private const string Clean = "clean";
        private const string Build = "build";
        private const string Test = "test";
        private const string Publish = "publish";

        private static async Task Main(string[] args)
        {
            Target(Clean,
                ForEach("publish", "**/bin", "**/obj"),
                dir =>
                {
                    static IEnumerable<string> GetDirectories(string d)
                    {
                        return Glob.Directories(".", d);
                    }

                    static void RemoveDirectory(string d)
                    {
                        if (!Directory.Exists(d)) return;
                        Console.WriteLine($@"Cleaning {d}");
                        Directory.Delete(d, true);
                    }

                    foreach (var d in GetDirectories(dir))
                    {
                        RemoveDirectory(d);
                    }
                });
            Target(Build, () => Run("dotnet", "build src/QuickSoft.ConstructionVentory -c Release"));
            
            Target(Test, DependsOn(Build),
                () =>
                {
                    static IEnumerable<string> GetFiles(string d)
                    {
                        return Glob.Files(".", d);
                    }

                    foreach (var file in GetFiles("tests/**/*.csproj"))
                    {
                        Run("dotnet", $"test {file} -c Release --no-restore --no-build --verbosity=normal");
                    }
                });
            
            Target(Publish, DependsOn(Test),
                ForEach("src/QuickSoft.ConstructionVentory"),
                project =>
                {
                    Console.WriteLine("build: " + project);
                    Run("dotnet",
                        $"publish {project} -c Release -f net5.0 -o ./publish --no-restore --no-build --verbosity=normal");
                });

            Target("default", DependsOn(Publish), () => Console.WriteLine(@"Done!"));
            await RunTargetsAndExitAsync(args);
        }
    }
}