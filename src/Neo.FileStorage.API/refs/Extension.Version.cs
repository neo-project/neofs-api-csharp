// Copyright (C) 2015-2025 The Neo Project.
//
// Extension.Version.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

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
