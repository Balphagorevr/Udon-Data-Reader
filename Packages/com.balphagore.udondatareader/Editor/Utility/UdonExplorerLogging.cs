using UnityEngine;

namespace Balphagore.UdonDataReader.Utility
{
    public static class UdonDataReaderLogging
    {
        public static void WriteMessage(string message)
        {
            Debug.Log($"[UdonDataReader][INFO]: {message}".ColorizeText(Color.cyan));
        }

        internal static void WriteError(string message, string stackTrace)
        {
            Debug.Log($"[UdonDataReader][ERROR]: {message}".ColorizeText(Color.red) + $"\n ********** \nSTACKTRACE: : {stackTrace}");
        }
    }
}