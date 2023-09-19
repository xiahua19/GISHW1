using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMapObjects
{
    public abstract class moSymbol
    {
        public moSymbolTypeConstant _SymbolType;
        public abstract moSymbolTypeConstant SymbolType { get; set; }
        public abstract moSymbol Clone();
    }
}
