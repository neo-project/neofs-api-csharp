namespace Neo.FileStorage.API.Refs
{
    public partial class Version
    {
        public const uint SDKMajor = 2;
        public const uint SDKMinor = 11;

        public static Version SDKVersion()
        {
            return new Version
            {
                Major = SDKMajor,
                Minor = SDKMinor,
            };
        }

        public static bool IsSupportedVersion(Version ver)
        {
            if (ver is null) return false;
            if (ver.Major == SDKMajor && ver.Minor == SDKMinor)
                return true;
            return false;
        }

        public string String()
        {
            return $"v{Major}.{Minor}";
        }
    }
}
