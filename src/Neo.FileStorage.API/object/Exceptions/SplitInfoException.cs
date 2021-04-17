using System;

namespace Neo.FileStorage.API.Object
{
    public class SplitInfoException : Exception
    {
        public SplitInfo SplitInfo { get; private set; }

        public SplitInfoException(SplitInfo si)
        {
            SplitInfo = si;
        }
    }
}
