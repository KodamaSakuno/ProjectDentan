using System;
namespace Moen.KanColle.Dentan
{
    public static class AppInformation
    {
        public const string VersionString = "0.0.1.6";
        
        public static string Name { get; } = "Project Dentan";
        public static Version Version { get; } = Version.Parse(VersionString);
    }
}
