# Udon Data Reader
Udon Data Reader is an editor library providing an API to read Serialized Udon Programs to provide symbols, entry points and other metadata about the Udon program.

## Disclaimer

This project is in beta and early development, there will be a lot to add as I come up with ideas or receive feeback.

## Getting Started

1. Add my VCC Repository listing by visiting my [Listing Page](https://balphagorevr.github.io/balphagore-vcc-listings/).
2. Select your world project and click "Manage Project".
3. Add Udon Data Reader to your project.
4. You're ready to use the API.

## Examples

### Reading a Udon Program
```csharp
// Reference your UdonBehaviour
UdonBehaviour myUdonBehaviour;

// Create a new UdonProgramData object and populate with the UdonDataReader class.
UdonProgramData programData = UdonDataReader.ReadUdonProgram(myUdonBehaviour);

// Get a symbol from the program.
UdonSymbol playerNameSymbol = programData.GetVariable("PlayerName");
```

### API

#### Object UdonSymbol
##### Fields
|Data Type|Name|Description|
|---|---|---|
|string|Symbol Name|Name of the symbol|
|Enum|AccessModifier|Specifies the access modifier of the symbol such as public or private.|
|SymbolType|SymbolType|Specifies if the symbol represents a method, variable, or a variable that holds a method result.|
|string|DataType|Specifies the Type of the symbol.|
|bool|isExported|Specifies if the symbol is marked as exported in the assembly.|
|bool|synced|Specifies if the symbol variable is marked as Udon Sycned for Udon Networking.|
|uint|address|Specifies the symbol address.|
|UdonSymbol|returnSymbol|Specifies the method symbol's return symbol that holds data for the method result.|

##### Methods
|Returns|Method Name|Description|
|---|---|---|---|
|string|TryGetDataType()|Returns the data type of the symbol. This is important to compute the data type for the method's symbol.|

#### Object UdonProgramData

##### Fields

##### Methods
|Returns|Method Name|Description|
|---|---|---|---|
|IEnumerable\<UdonSymbol>|GetSymbols\<T>(bool includeExported)||

## Roadmap

### Next Release

#### 1.1.0
* Add support for getting source compiler names/versions.
* Separate symbols vs. exported symbols.
* Separate entry points vs. exported entry points.
* Get Sync metadata data.

#### 1.2.0
* Potential optimizations.
* Add Example Script and basic Editor window demonstrating use of Udon Data Reader.

## Changelog
### 1.0.0
* Public release