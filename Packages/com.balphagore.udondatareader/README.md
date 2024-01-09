# Udon Data Reader
Udon Data Reader is an editor library providing an API to read Serialized Udon Programs to provide symbols, entry points and other metadata about the Udon program.

## Getting Started
### Creator Companion
1. Add my VCC Repository listing by visiting my [Listing Page](https://balphagorevr.github.io/balphagore-vcc-listings/).
2. Select your world project and click "Manage Project".
3. Add Udon Data Reader to your project.
4. You're ready to use the API.

### Release Packages
![Dynamic JSON Badge](https://img.shields.io/badge/dynamic/json?url=https%3A%2F%2Fraw.githubusercontent.com%2FBalphagorevr%2FUdonDataReader%2Fmain%2FPackages%2Fcom.balphagore.udondatareader%2Fpackage.json&query=%24.version&label=VPM%20Release&color=green&link=https%3A%2F%2Fgithub.com%2FBalphagorevr%2FUdonDataReader%2Freleases)


## API Methods & Documentation

### UdonDataReader Class
|Static Method|Returns|Details|
|---|---|---|
|ReadUdonProgram(UdonBehaviour Object)|UdonProgramData|Returns a UdonProgramData Object consisting of valid data read from the serialized Udon Program asset referenced by the provided Udon Behaviour.|

### UdonProgramData Object
Holds information and metadata read from an Udon Program asset file.

### UdonSymbol Object
An Udon Symbol from the Udon Program that represents a variable or method.

## Roadmap
### Next Release
N/A - initial requirements have been met for a project. If there is feedback provided in the issues tracker, I will consider enhancements.

## Changelog
### 1.0.0
* Public release

### 1.1.0
1/8/2024
* Created editor window demonstrating use of the API and providing a basic display of the Udon program and its symbols.
* Add support for getting source compiler names/versions.
* Separate symbols vs. exported symbols.
* Separate entry points vs. exported entry points.
* Get Sync metadata data.
