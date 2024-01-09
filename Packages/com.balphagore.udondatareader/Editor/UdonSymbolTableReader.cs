using System;
using System.Collections.Immutable;
using VRC.Udon.Common;
using VRC.Udon.Common.Interfaces;

namespace Balphagore.UdonDataReader
{
    public class UdonSymbolTableReader : IUdonSymbolTable
    {
        UdonSymbolTable _table;

        public UdonSymbolTableReader(IUdonSymbolTable table)
        {
            this._table = table as UdonSymbolTable;
        }

        #region Default implementation 
        public uint GetAddressFromSymbol(string symbolName) => _table.GetAddressFromSymbol(symbolName);

        public ImmutableArray<string> GetExportedSymbols() => _table.GetExportedSymbols();

        public string GetSymbolFromAddress(uint address) => _table.GetSymbolFromAddress(address);

        public ImmutableArray<string> GetSymbols() => _table.GetSymbols();

        public string? GetSymbolTypeName(string symbolName)
        {
            Type type = this.GetSymbolType(symbolName);

            if (type != null) return type.Name;

            return null;
        }
        public Type GetSymbolType(string symbolName)
        {
            try
            {
                return _table.GetSymbolType(symbolName);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogWarning(ex.Message);
                return null;
            }
        }

        public bool HasAddressForSymbol(string symbolName) => _table.HasAddressForSymbol(symbolName);

        public bool HasExportedSymbol(string symbolName) => _table.HasExportedSymbol(symbolName);

        public bool HasSymbolForAddress(uint address) => _table.HasSymbolForAddress(address);

        public bool TryGetAddressFromSymbol(string symbolName, out uint address) => _table.TryGetAddressFromSymbol(symbolName, out address);
        public bool TryGetSymbolFromAddress(uint address, out string symbolName) => _table.TryGetSymbolFromAddress(address, out symbolName);

        #endregion
    }
}