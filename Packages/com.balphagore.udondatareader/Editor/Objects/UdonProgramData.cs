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

using System;
using System.Collections.Generic;
using System.Linq;

namespace Balphagore.UdonDataReader
{
    /// <summary>
    /// Holds Information and Data representing the Serialized Udon Assembly File.
    /// </summary>
    public class UdonProgramData
    {
        public string programName { get; internal set; }
        public int updateOrder { get; internal set; }
        /// <summary>
        /// Consists of all exported methods defined in the Udon Program. These are also referred to as Events in the Udon Graph. The methods MUST have the public declaration.
        /// </summary>
        public UdonSymbol[] exportedMethods { get; internal set; }
        /// <summary>
        /// Consists of all exported variables defined in the Udon Program. 
        /// </summary>
        public UdonSymbol[] exportedVariables { get; internal set; }
        public UdonProgramData() { }

        /// <summary>
        /// Returns an enumerable object consisting of variables following the specified Type.
        /// </summary>
        /// <typeparam name="T">Type of object to search for.</typeparam>
        /// <returns>IEnumerable object consisting of variables matching the given type.</returns>
        public IEnumerable<UdonSymbol> GetVariablesByType<T>() => exportedVariables.Where(v => v.symbolType == typeof(T));
    }

    public class UdonSymbol
    {
        public string symbolName { get; internal set; }
        public Type symbolType { get; set; }
    }
}
