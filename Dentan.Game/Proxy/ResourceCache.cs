using Moen.KanColle.Dentan.Data;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks.Dataflow;

namespace Moen.KanColle.Dentan.Proxy
{
    public static class ResourceCache
    {
        static ActionBlock<ResourceSession> r_SaveFileBlock = new ActionBlock<ResourceSession>(new Action<ResourceSession>(SaveFileCore));

        static Regex r_Regex = new Regex(@"(?<Path>kcs.*)/(?<Filename>\w+)\.(?<Extension>\w+)(\?VERSION=(?<Version>.*))?$", RegexOptions.IgnoreCase);

        public static bool IsEnabled { get; set; } = false;

        public static string CacheFolder { get; set; } = "Cache";

        public static void SaveFile(ResourceSession rpSession)
        {
            r_SaveFileBlock.Post(rpSession);
        }
        static void SaveFileCore(ResourceSession rpSession)
        {
            var rDirectory = Path.GetDirectoryName(rpSession.CachePath);
            if (!Directory.Exists(rDirectory))
                Directory.CreateDirectory(rDirectory);

            try
            {
                File.WriteAllBytes(rpSession.CachePath, rpSession.Data);
                File.SetLastWriteTime(rpSession.CachePath, rpSession.LastModifiedTime);

                rpSession.Status = SessionStatus.Cached;
            }
            catch (IOException e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }
        public static string GetFilePath(ResourceSession rpSession)
        {
            var rMatch = r_Regex.Match(rpSession.Url);
            if (!rMatch.Success)
                throw new Exception();

            var rBuilder = new StringBuilder(256);
            var rPath = rMatch.Groups["Path"].Value;

            var rVoiceIndex = rPath.IndexOf("kcs/sound/kc");
            if (rVoiceIndex != 0)
                rBuilder.Append(rPath);
            else
            {
                var rShipFilename = rPath.Replace("kcs/sound/kc", string.Empty); rPath.Substring(rVoiceIndex + 12);
                var rShipGraph = KanColleGame.Current.Base.ShipGraphs.Values.SingleOrDefault(r => r.Filename == rShipFilename);
                if (rShipGraph == null)
                    rBuilder.Append(rPath);
                else
                {
                    rBuilder.Append(@"kcs\sound\").Append(rShipGraph.ID).Append('_').Append(rShipGraph.Name);
                    rpSession.Name = rShipGraph.Name;
                }
            }
            rBuilder.Append(@"\");
            rBuilder.Replace('/', '\\');

            var rFilename = rMatch.Groups["Filename"].Value;
            var rExtension = rMatch.Groups["Extension"].Value;
            if (rPath != "kcs/resources/swf/ships")
                rBuilder.Append(rFilename);
            else
            {
                var rShipInfo = KanColleGame.Current.Base.ShipGraphs.Values.SingleOrDefault(r => r.Filename == rFilename).ShipInfo;
                var rShipNameBuilder = new StringBuilder(rShipInfo.Name);
                rBuilder.Append(rShipInfo.ID).Append('_').Append(rShipInfo.Name);

                if (rShipInfo.YomiName == "elite" || rShipInfo.YomiName == "flagship")
                {
                    rBuilder.Append(rShipInfo.YomiName);
                    rShipNameBuilder.Append(rShipInfo.YomiName);
                }
                rpSession.Name = rShipNameBuilder.ToString();
            }

            rpSession.FileNameWithoutVersion = rBuilder.ToString();

            var rVersionGroup = rMatch.Groups["Version"];
            if (rVersionGroup.Success)
                rBuilder.Append('_').Append(rVersionGroup.Value);

            rBuilder.Append('.').Append(rExtension);

            return rBuilder.ToString();
        }

        public static void Normailize(BaseInfo rpData)
        {
            NormalizeShips(rpData);
            NormalizeVoices(rpData);
        }

        static void NormalizeShips(BaseInfo rpData)
        {
            var rShipsFolder = Path.Combine(CacheFolder, @"kcs\resources\swf\ships\");
            foreach (var rFile in Directory.EnumerateFiles(rShipsFolder, "*.swf").Select(r => new FileInfo(r)).Where(r => !r.Name.Contains('_')))
            {
                var rName = Path.GetFileNameWithoutExtension(rFile.Name);
                var rShipgraph = rpData.ShipGraphs.Values.SingleOrDefault(r => r.Filename == rName);
                if (rShipgraph != null)
                {
                    var rShipInfo = rpData.Ships[rShipgraph.ID];

                    var rNormalizedPath = string.Format("{0}{1}_{2}_{3}.swf", rShipsFolder, rShipInfo.ID, rShipInfo.Name, rShipgraph.Version);
                    if (!File.Exists(rNormalizedPath))
                        rFile.MoveTo(rNormalizedPath);
                    if (File.GetLastWriteTime(rNormalizedPath) < rFile.LastWriteTime)
                    {
                        File.Delete(rNormalizedPath);
                        rFile.MoveTo(rNormalizedPath);
                    }
                }
            }
        }
        static void NormalizeVoices(BaseInfo rpData)
        {
            var rSoundsFolder = Path.Combine(CacheFolder, @"kcs\sound\");
            foreach (var rFolder in Directory.EnumerateDirectories(rSoundsFolder).Select(r => new DirectoryInfo(r)).Where(r => r.Name.StartsWith("kc") && r.Name.Length > 2))
            {
                var rName = rFolder.Name.Substring(2);
                var rShipgraph = rpData.ShipGraphs.Values.SingleOrDefault(r => r.Filename == rName);
                if (rShipgraph != null)
                {
                    var rShipInfo = rpData.Ships[rShipgraph.ID];
                    var rNormalizedFolder = string.Format(@"{0}{1}_{2}\", rSoundsFolder, rShipInfo.ID, rShipInfo.Name);
                    if (!Directory.Exists(rNormalizedFolder))
                        rFolder.MoveTo(rNormalizedFolder);
                    else
                    {
                        foreach (var rSubFiles in rFolder.EnumerateFiles())
                            rSubFiles.CopyTo(rNormalizedFolder + rSubFiles.Name, true);
                        rFolder.Delete(true);
                    }

                    if (rShipgraph.Version > 1)
                        foreach (var rFile in Directory.EnumerateFiles(rNormalizedFolder, ".mp3").Select(r => new FileInfo(r)).Where(r=>!r.Name.Contains('_')))
                        {
                            var rNormalizedPath = string.Format("{0}{1}_{2}.mp3", rNormalizedFolder, Path.GetFileNameWithoutExtension(rFile.Name), rShipgraph.Version);
                            if (!File.Exists(rNormalizedPath))
                                rFile.MoveTo(rNormalizedPath);
                            if (File.GetLastWriteTime(rNormalizedPath) < rFile.LastWriteTime)
                            {
                                File.Delete(rNormalizedPath);
                                rFile.MoveTo(rNormalizedPath);
                            }
                        }
                }
            }
        }
    }
}
