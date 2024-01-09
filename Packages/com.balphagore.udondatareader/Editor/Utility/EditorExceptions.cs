using System;

namespace Balphagore.UdonDataReader.Utility
{
    public class UdonDataReaderException : Exception
    {
        public UdonDataReaderException()
        {
            UdonDataReaderLogging.WriteError(base.Message, base.StackTrace);
        }
    }
}
