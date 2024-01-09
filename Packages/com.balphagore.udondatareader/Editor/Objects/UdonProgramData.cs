using System.Collections.Generic;
using System.Linq;
using UdonSharp;

namespace Balphagore.UdonDataReader
{
    /// <summary>
    /// Holds information and metadata read from an Udon Program asset file.
    /// </summary>
    public class UdonProgramData
    {
        /// <summary>
        /// Name of the Udon Program.
        /// </summary>
        public string programName { get; internal set; }
        /// <summary>
        /// The order that this program would be updated in the behaviour queue.
        /// </summary>
        public int updateOrder { get; internal set; }
        /// <summary>
        /// All symbols provided in the Udon Program.
        /// </summary>
        public UdonSymbol[] symbols { get; internal set; }
        /// <summary>
        /// The compiler of the program to Udon Assembly.
        /// </summary>
        public string sourceCompiler { get; internal set; }
        /// <summary>
        /// The name of the file consisting of serialized Udon Assembly.
        /// </summary>
        public string serializedAssetName { get; internal set; }
        /// <summary>
        /// The sync behavior of the Udon Program in Udon Networking.
        /// </summary>
        public BehaviourSyncMode syncMode { get; internal set; }
        public UdonProgramData() { }
        /// <summary>
        /// Gets all symbols from the Udon Program by type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="includeExported">Whether or not to include exported symbols.</param>
        /// <returns></returns>
        public IEnumerable<UdonSymbol> GetSymbols<T>(bool includeExported)
        {
            if (includeExported)
            {
                return symbols.Where(v => v.dataType == typeof(T).Name);
            }
            else
            {
                return symbols.Where(v => v.dataType == typeof(T).Name && v.isExported == false);
            }
        }
        /// <summary>
        /// Gets all symbols for the Udon Program.
        /// </summary>
        /// <param name="includeExported">Whether or not to include exported symbols.</param>
        /// <returns></returns>
        public IEnumerable<UdonSymbol> GetSymbols(bool includeExported)
        {
            if (includeExported)
            {
                return symbols;
            } 
            else
            {
                return symbols.Where(v => v.isExported == false);
            }
        }
        /// <summary>
        /// Gets a symbol by name from the Udon Program.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public UdonSymbol GetSymbol(string name) => symbols.Where(s => s.symbolName == name).FirstOrDefault();
        /// <summary>
        /// Gets a symbol by name and type from the Udon Program.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public UdonSymbol GetSymbol<T>(string name) => symbols.Where(s => s.symbolName == name && s.dataType == typeof(T).Name).FirstOrDefault();
        /// <summary>
        /// Gets all symbols from the Udon Program.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UdonSymbol> GetSymbols() => symbols;
        /// <summary>
        /// Gets a method or entry point from the Udon Program by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public UdonSymbol GetMethod(string name) => symbols.Where(s => s.symbolName == name && s.symbolType == SymbolType.Method && s.isExported == false).FirstOrDefault();
        /// <summary>
        /// Gets all methods or entry points from the Udon Program.
        /// </summary>
        /// <param name="includeExported">Whether or not to include exported symbols.</param>
        /// <returns></returns>
        public IEnumerable<UdonSymbol> GetMethods(bool includeExported)
        {
            if (includeExported)
            {
                return symbols.Where(v => v.symbolType == SymbolType.Method);
            } else
            {
                return symbols.Where(v => v.symbolType == SymbolType.Method && v.isExported == false);
            }
        }
        /// <summary>
        /// Gets a variable by name from the Udon Program.
        /// </summary>
        /// <param name="variableName"></param>
        /// <returns></returns>
        public UdonSymbol GetVariable(string variableName) => symbols.Where(s => s.symbolName == variableName && s.symbolType == (SymbolType.MethodReturnVariable | SymbolType.Variable) && s.isExported == false).FirstOrDefault();
        /// <summary>
        /// Gets all variables from the Udon Program by name.
        /// </summary>
        /// <param name="includeExported">Whether or not to include exported symbols.</param>
        /// <returns></returns>
        public IEnumerable<UdonSymbol> GetVariables(bool includeExported)
        {
            if (includeExported)
            {
                return symbols.Where(v => v.symbolType == SymbolType.Variable);
            } 
            else
            {
                return symbols.Where(v => v.symbolType == SymbolType.Variable && v.isExported == false);
            }
        }
    }
}
