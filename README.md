# Udon Data Reader
Udon Data Reader is an editor library providing an API to read Serialized Udon Programs to provide symbols, entry points and other metadata about the Udon program.

## Disclaimer

This project is in beta and early development, there will be a lot to add as I come up with ideas or receive feeback.

## Getting Started

1. Add my VCC Repository listing by visiting my [Listing Page](https://balphagorevr.github.io/balphagore-vcc-listings/).
2. Select your world project and click "Manage Project".
3. Add Udon Data Reader to your project.
4. You're ready to use the API.

## API Methods

|Method Name|Returns|Details|
|---|---|---|
|GetProgramData()|UdonProgramData|Returns a UdonProgramData Object consisting of valid data read from the serialized Udon Program asset.|
|LoadUdonBehaviour(UdonBehaviour behaviour)|Void|Loads the given UdonBehaviour object onto the Data Reader for reading.|

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