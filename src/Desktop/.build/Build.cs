using System;
using System.IO;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.CopyModules);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = Configuration.Release;

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;

    AbsolutePath TestsDirectory => RootDirectory / "tests";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";


    AbsolutePath DriverDriectory => RootDirectory / "src" / "PTSCDriver" / "ptsc" / "driver" / "ptsc";

    AbsolutePath OutputDirectory => RootDirectory / "release";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(ArtifactsDirectory);
            EnsureCleanDirectory(OutputDirectory);
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            var projekt = Solution.Projects.First(p => p.Name.Equals("PTSC"));
            DotNet($"publish {projekt} --configuration \"Release\"  -r win-x64 -p:PublishSingleFile=true  -p:PublishReadyToRun=true -o {ArtifactsDirectory} --self-contained true");
        });

    Target CopyModules => _ => _
    .DependsOn(Compile)
    .Executes(() =>
    {
        if(Directory.Exists(OutputDirectory))
            Directory.CreateDirectory(OutputDirectory);

        File.Copy(Path.Combine(ArtifactsDirectory,"ptsc.exe"), Path.Combine(OutputDirectory, "ptsc.exe"));

        if (Directory.Exists(DriverDriectory))
        {
            var drivertarget = Path.Combine(OutputDirectory, "Driver");
            Directory.CreateDirectory(drivertarget);
            foreach(var file in Directory.GetFiles(DriverDriectory, "*.*", SearchOption.AllDirectories))
            {
                var newfile = Path.Combine(drivertarget, Path.GetRelativePath(DriverDriectory, file));
                var root = Path.GetDirectoryName(newfile);
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
                File.Copy(file, newfile);
            }
        }


            
    });

}
