using Derivas.Expression;
using System;
using System.Collections.Generic;

namespace Derivas.Expression
{
    public class DvNameVal : Dictionary<string, double>, IDictionary<string, double>
    {
    }

    /// <summary>Some common mathemtaical constants</summary>
    public static class DvConsts
    {
        public static IDvExpr E { get; } = new Constant(Math.E);
        public static IDvExpr PI { get; } = new Constant(Math.PI);
    }
}