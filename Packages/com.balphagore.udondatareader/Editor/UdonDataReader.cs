using VRC.Udon.Common.Interfaces;
using Balphagore.UdonDataReader.Utility;
using VRC.Udon;
using System.Collections.Generic;
using System.Linq;

namespace Balphagore.UdonDataReader
{
    public static class UdonDataReader
    {
        public static UdonProgramData ReadUdonProgram(UdonBehaviour udonBehaviour)
        {
            UdonProgramData programData = new UdonProgramData();
            List<UdonSymbol> symbols = new List<UdonSymbol>();

            IUdonProgram program = udonBehaviour.GetProgram();

            UdonSymbolTableReader symbolMethodsReader = new UdonSymbolTableReader(program.EntryPoints);

            UdonSymbolTableReader symbolVariablesReader = new UdonSymbolTableReader(program.SymbolTable);

            IEnumerable<IUdonSyncMetadata> programSyncData = program.SyncMetadataTable.GetAllSyncMetadata();

            AbstractUdonProgramSource programSource = udonBehaviour.programSource;

            foreach (var variable in symbolVariablesReader.GetExportedSymbols())
            {
                string? dataType = symbolVariablesReader.GetSymbolTypeName(variable);
                uint address = symbolVariablesReader.GetAddressFromSymbol(variable);

                symbols.Add(new UdonSymbol(variable, dataType, SymbolType.Variable, address, true, programSyncData.Any(s => s.Name == variable)));
            }

            foreach (var variable in symbolVariablesReader.GetSymbols())
            {
                string? dataType = symbolVariablesReader.GetSymbolTypeName(variable);
                uint address = symbolVariablesReader.GetAddressFromSymbol(variable);

                symbols.Add(new UdonSymbol(variable, dataType, SymbolType.Variable, address, false, programSyncData.Any(s => s.Name == variable)));
            }

            // For exported
            foreach (var method in symbolMethodsReader.GetExportedSymbols())
            {
                string? dataType = symbolMethodsReader.GetSymbolTypeName(method);
                uint address = symbolMethodsReader.GetAddressFromSymbol(method);

                UdonSymbol methodReturnSymbol = symbols.Where(s => s.symbolName.Contains(method) && s.symbolType == SymbolType.Variable).FirstOrDefault();

                symbols.Add(new UdonSymbol(method, dataType, SymbolType.Method, address, true, returnSymbol: methodReturnSymbol));
            }

            foreach (var method in symbolMethodsReader.GetSymbols())
            {
                string? dataType = symbolMethodsReader.GetSymbolTypeName(method);
                uint address = symbolMethodsReader.GetAddressFromSymbol(method);

                UdonSymbol methodReturnSymbol = symbols.Where(s => s.symbolName.Contains(method) && s.symbolType == SymbolType.Variable).FirstOrDefault();

                symbols.Add(new UdonSymbol(method, dataType, SymbolType.Method, address, false, returnSymbol: methodReturnSymbol));
            }

            programData.programName = programSource.name;
            programData.symbols = symbols.ToArray();
            programData.updateOrder = program.UpdateOrder;
            programData.serializedAssetName = programSource.SerializedProgramAsset.name;
            programData.sourceCompiler = udonBehaviour.programSource.GetType().Name;

            return programData;
        }
    }
}