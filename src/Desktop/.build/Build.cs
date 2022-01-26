using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
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

    AbsolutePath ModuleDirectory => RootDirectory / "src" / "Modules";

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
            DotNet($"publish {projekt} --configuration \"Release\"  -r win-x64 -p:PublishSingleFile=true -p:PublishReadyToRun=true -o {ArtifactsDirectory} --self-contained true");
        });

    Target CopyModules => _ => _
    .DependsOn(Compile)
    .Executes(() =>
    {
        if(Directory.Exists(OutputDirectory))
            Directory.CreateDirectory(OutputDirectory);

        CopyDirectory(ArtifactsDirectory, Path.Combine(OutputDirectory, "Application"), new() { ".pdb"} );
        File.CreateSymbolicLink(Path.Combine(OutputDirectory, "ptsc.exe"), Path.Combine(OutputDirectory, "Application", "ptsc.exe"));


        //Copy Driver
        CopyDirectory(DriverDriectory, Path.Combine(OutputDirectory, "Driver"));

        //Copy Modules
        CopyWebcamModule();
        CopyTestModule();
        CopyMediaPipeModule();
        CopyKinectV1Module();
        CopyKinectV2Module();

    });

    private void CopyKinectV1Module()
    {
        var source = Path.Combine(ModuleDirectory, "Kinect", "KinectModule", "KinectV1", "bin", "Release");
        var target = Path.Combine(OutputDirectory, "Modules", "KinectV1");
        if(Directory.Exists(source) && Directory.GetFiles(source).Length > 0)
        {
            FileSystemTasks.CopyDirectoryRecursively(source, target);
            File.WriteAllText(Path.Combine(target, "kinectV1.ptsc"), @"
{
  'Name': 'Kinect V1',
  'Description': 'Uses a Kinect Camera to perform BodyTracking',
  'SupportsImage': true,
  'Process': 'KinectV1.exe',
  'Arguments': '-c false',
  'InstallationScript': '',
  'InstallationDirectory': ''
}
".Replace('\'','"'));
        }
    }


    private void CopyKinectV2Module()
    {
        var source = Path.Combine(ModuleDirectory, "Kinect", "KinectModule", "KinectV2", "bin", "Release");
        var target = Path.Combine(OutputDirectory, "Modules", "KinectV2");
        if (Directory.Exists(source) && Directory.GetFiles(source).Length > 0)
        {
            FileSystemTasks.CopyDirectoryRecursively(source, target);
            File.WriteAllText(Path.Combine(target, "kinectV2.ptsc"), @"
{
  'Name': 'Kinect V2',
  'Description': 'Uses a Kinect Camera to perform BodyTracking',
  'SupportsImage': true,
  'Process': 'KinectV2.exe',
  'Arguments': '-c false',
  'InstallationScript': '',
  'InstallationDirectory': ''
}
".Replace('\'', '"'));
        }
    }

    private void CopyDirectory(string source,string traget,List<string> ignoredTypes=null)
    {
        if (Directory.Exists(source))
        {
            if (!Directory.Exists(traget))
                Directory.CreateDirectory(traget);

            foreach (var file in Directory.GetFiles(source, "*.*", SearchOption.AllDirectories))
            {

                if(ignoredTypes != null)
                {
                    if (ignoredTypes.Any(x => x.Equals(Path.GetExtension(file))))
                        continue;
                }

                var newfile = Path.Combine(traget, Path.GetRelativePath(source, file));
                var root = Path.GetDirectoryName(newfile);
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
                File.Copy(file, newfile);
            }
        }
        
        
    }

    private void CopyMediaPipeModule()
    {
        var source = Path.Combine(ModuleDirectory, "PoseClassifier");
        var target = Path.Combine(OutputDirectory, "Modules", "PoseClassifier");
        FileSystemTasks.CopyDirectoryRecursively(source, target, excludeDirectory: ExcludePythonDirs);
    }

    private void CopyTestModule()
    {
        var source = Path.Combine(ModuleDirectory, "ModulePipe");
        var target = Path.Combine(OutputDirectory, "Modules", "PipeTest");
        FileSystemTasks.CopyDirectoryRecursively(source,target, excludeDirectory: ExcludePythonDirs);
    }

    private readonly List<string> PythonModuleIgnoredDirs = new() { ".pytest_cache", "__pycache__", "venv" };
    private void CopyWebcamModule()
    {
        var source = Path.Combine(ModuleDirectory, "SimpleWebcam");
        var target = Path.Combine(OutputDirectory, "Modules", "SimpleWebcam");
        FileSystemTasks.CopyDirectoryRecursively(source, target, excludeDirectory:ExcludePythonDirs);
    }

    private bool ExcludePythonDirs(DirectoryInfo arg)
    {
        return PythonModuleIgnoredDirs.Any(d => d.Equals(arg.Name));
    }
}
