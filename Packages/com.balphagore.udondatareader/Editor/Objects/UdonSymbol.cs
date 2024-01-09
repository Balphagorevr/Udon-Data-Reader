namespace Balphagore.UdonDataReader
{
    /// <summary>
    /// An Udon Symbol from the Udon Program that represents a variable or method.
    /// </summary>
    public class UdonSymbol
    {
        /// <summary>
        /// Name of the Udon Symbol.
        /// </summary>
        public string symbolName { get; internal set; }
        /// <summary>
        /// The access modifier dictated by the compiler/IDE.
        /// </summary>
        public AccessModifier accessModifier
        {
            get => this.symbolName.StartsWith('_') ? AccessModifier.Private : AccessModifier.Public;
        }
        /// <summary>
        /// The type that the symbol represents.
        /// </summary>
        public SymbolType symbolType { get; internal set; }
        /// <summary>
        /// The data type of the symbol if it is a variable.
        /// </summary>
        public string dataType 
        { 
            get => TryGetDataType(); 
            internal set => _dataType = value; 
        }
        /// <summary>
        /// Whether or not the symbol as marked as exported.
        /// </summary>
        public bool isExported { get; internal set; }
        /// <summary>
        /// Whether or not the symbol has been marked synced.
        /// </summary>
        public bool? synced { get; internal set; }
        /// <summary>
        /// The address of the symbol from the Udon Assembly.
        /// </summary>
        public uint address { get; internal set; }
        /// <summary>
        /// The return symbol for method symbols that have a return type.
        /// </summary>
        public UdonSymbol? returnSymbol { get; private set; }

        private string _dataType;
        /// <summary>
        /// An Udon Symbol from the Udon Program that represents a variable or method.
        /// </summary>
        public UdonSymbol() { }
        /// <summary>
        /// An Udon Symbol from the Udon Program that represents a variable or method.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dataType"></param>
        /// <param name="symbolType"></param>
        /// <param name="address"></param>
        /// <param name="isExported"></param>
        /// <param name="synced"></param>
        /// <param name="returnSymbol"></param>
        public UdonSymbol(string name, string? dataType, SymbolType symbolType, uint address, bool isExported, bool? synced = null, UdonSymbol? returnSymbol = null)
        {

            this.symbolName = name;
            this.dataType = dataType == null ? string.Empty : dataType;
            this.symbolType = symbolType;
            this.isExported = isExported;
            this.synced = synced;
            this.address = address;
            this.returnSymbol = returnSymbol;
        }

        private string TryGetDataType()
        {
            if (this.returnSymbol != null)
            {
                return $"Returns {this.returnSymbol.dataType}";
            } else
            {
                return _dataType;
            }
        }
    }
    /// <summary>
    /// The access modifier dictated by the compiler/IDE.
    /// </summary>
    public enum AccessModifier
    {
        Public,
        Private
    }
    /// <summary>
    /// The type that the symbol represents.
    /// </summary>
    public enum SymbolType
    {
        Variable,
        MethodReturnVariable,
        Method
    }
}