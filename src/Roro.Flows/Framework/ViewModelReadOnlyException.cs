using System;

namespace Roro.Flows.Framework
{
    public sealed class ViewModelReadOnlyException : Exception
    {
        public static void ThowIfReadOnly(bool isReadOnly)
        {
            if (isReadOnly)
                throw new ViewModelReadOnlyException();
        }
    }
}
