using System;
using System.Collections.Generic;
using System.Text;

using Derivas.Exception;

namespace Derivas.Expression
{
    internal class DvLogarithm : IDvExpr
    {
        public IDvExpr Of { get; protected set; }
        public IDvExpr Base { get; protected set; }

        public DvLogarithm(IDvExpr _of, IDvExpr _base = null)
        {
            (Of, Base) = (_of, _base ?? DvConsts.E);
        }

        public double Calculate(IDictionary<string, double> nameVal)
            => Math.Log(Of.Calculate(nameVal), Base.Calculate(nameVal));

        public string Represent() 
            => Base == DvConsts.E ? 
            $"log({Of.Represent()})": 
            $"log({Of.Represent()}, base={Base.Represent()})";
    }


    internal class DvSingleArgFunc : IDvExpr
    {
        public IDvExpr Of { get; protected set; }
        private Func<double, double> CalcFunc { get; }
        private string FuncName { get; }

        public DvSingleArgFunc(IDvExpr of, Func<double, double> calcF, string funName)
        {
            (Of, CalcFunc, FuncName) = (of, calcF, funName);
        }

        public double Calculate(IDictionary<string, double> nameVal) => CalcFunc(Of.Calculate(nameVal));

        public string Represent() => $"{FuncName}({Of.Represent()})";
    }

    internal class DvCosine : DvSingleArgFunc 
    { 
        public DvCosine(IDvExpr of) : base(of, Math.Cos, "cos") { }
    }

    internal class DvSine : DvSingleArgFunc
    {
        public DvSine(IDvExpr of) : base(of, Math.Sin, "sin") { }
    }

    internal class DvTangens : DvSingleArgFunc
    {
        public DvTangens(IDvExpr of) : base(of, Math.Tan, "tg") { }
    }

    internal class DvCotangens : DvSingleArgFunc
    {
        private static Func<double, double> Cotan = (of) => 1 / Math.Tan(of);
        public DvCotangens(IDvExpr of) : base(of, Cotan, "ctg") { }
    }

    internal class DvArccosine : DvSingleArgFunc
    {
        public DvArccosine(IDvExpr of) : base(of, Math.Acos, "arccos") { }
    }

    internal class DvArcsine : DvSingleArgFunc
    {
        public DvArcsine(IDvExpr of) : base(of, Math.Asin, "arcsin") { }
    }

    internal class DvArctangens : DvSingleArgFunc
    {
        public DvArctangens(IDvExpr of) : base(of, Math.Atan, "arctg") { }
    }

    internal class DvArccotangens : DvSingleArgFunc
    {
        private static Func<double, double> Arccotan = (of) => Math.PI / 2 - Math.Atan(of);
        public DvArccotangens(IDvExpr of) : base(of, Arccotan, "arcctg") { }
    }


}
