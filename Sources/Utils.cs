using System;
using System.Collections.Generic;
using System.Text;

using Derivas.Expression;

namespace Derivas.Utils
{
    public class NameVal : Dictionary<string, double>, IDictionary<string, double> 
    {
    }

    /// <summary>Some common mathemtaical constants</summary>
    internal readonly struct DvConsts
    {
        public static Expr E { get; } = new Constant(Math.E);
        public static Expr PI { get; } = new Constant(Math.PI);
    }
}
