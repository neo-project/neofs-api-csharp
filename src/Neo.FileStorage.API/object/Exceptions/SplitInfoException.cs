using System;

namespace Neo.FileStorage.API.Object.Exceptions
{
    public class SplitInfoException : Exception
    {
        private readonly SplitInfo splitInfo;

        public SplitInfoException(SplitInfo si)
        {
            splitInfo = si;
        }

        public SplitInfo SplitInfo()
        {
            return splitInfo;
        }
    }
}
