/*
MIT License

Copyright (c) 2023 Balphagore

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

#pragma warning disable IDE1006 // Skip the underscore naming rule violation since we may have to use this convention to prevent network callable events.


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
