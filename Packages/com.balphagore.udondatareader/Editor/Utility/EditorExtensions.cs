using System;
using UnityEngine;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace Balphagore.UdonDataReader.Utility
{
    public static class EditorExtensions
    {
        /// <summary>
        /// Retrieves the Udon Program from the serialized asset.
        /// </summary>
        /// <param name="behaviour">Udon Behaviour used to locate the Udon Program asset.</param>
        /// <returns>IUdonProgram with data</returns>
        public static IUdonProgram GetProgram(this UdonBehaviour behaviour) => behaviour.programSource.SerializedProgramAsset.RetrieveProgram();
        public static string ColorizeText(this string text, string hexColor) => $"<color={hexColor}>{text}</color>";

        public static string ColorizeText(this string text, Color32 color) => $"<color={string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", color.r, color.g, color.b, color.a)}>{text}</color>";

    }
}
