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
            => Base == DvConsts.E ? $"log({Of.Represent()})" :
            $"log({Of.Represent()}, base={Base.Represent()})";
    }

    internal abstract class DvSingleArgOperator : IDvExpr
    {
        public IDvExpr Of { get; protected set; }
        private Func<double, double> CalcFunc { get; }
        private string FuncName { get; }

        public DvSingleArgOperator(IDvExpr of, Func<double, double> calcF, string funName)
        {
            (Of, CalcFunc, FuncName) = (of, calcF, funName);
        }

        public double Calculate(IDictionary<string, double> nameVal) => CalcFunc(Of.Calculate(nameVal));

        public string Represent() => $"{FuncName}({Of.Represent()})";

        protected abstract DvSingleArgOperator CreateInstance(IDvExpr of);
    }

    internal class DvCosine : DvSingleArgOperator
    {
        public DvCosine(IDvExpr of) : base(of, Math.Cos, "cos") { }

        protected override DvSingleArgOperator CreateInstance(IDvExpr of) => new DvCosine(of);
    }

    internal class DvSine : DvSingleArgOperator
    {
        public DvSine(IDvExpr of) : base(of, Math.Sin, "sin") { }

        protected override DvSingleArgOperator CreateInstance(IDvExpr of) => new DvSine(of);

    }

    internal class DvTangens : DvSingleArgOperator
    {
        public DvTangens(IDvExpr of) : base(of, Math.Tan, "tg") { }

        protected override DvSingleArgOperator CreateInstance(IDvExpr of) => new DvTangens(of);
    }

    internal class DvCotangens : DvSingleArgOperator
    {
        private static Func<double, double> Cotan = (of) => 1 / Math.Tan(of);
        public DvCotangens(IDvExpr of) : base(of, Cotan, "ctg") { }

        protected override DvSingleArgOperator CreateInstance(IDvExpr of) => new DvCotangens(of);

    }

    internal class DvArccosine : DvSingleArgOperator
    {
        public DvArccosine(IDvExpr of) : base(of, Math.Acos, "arccos") { }

        protected override DvSingleArgOperator CreateInstance(IDvExpr of) => new DvArccosine(of);

    }

    internal class DvArcsine : DvSingleArgOperator
    {
        public DvArcsine(IDvExpr of) : base(of, Math.Asin, "arcsin") { }

        protected override DvSingleArgOperator CreateInstance(IDvExpr of) => new DvArcsine(of);

    }

    internal class DvArctangens : DvSingleArgOperator
    {
        public DvArctangens(IDvExpr of) : base(of, Math.Atan, "arctg") { }

        protected override DvSingleArgOperator CreateInstance(IDvExpr of) => new DvArctangens(of);
    }

    internal class DvArccotangens : DvSingleArgOperator
    {
        private static Func<double, double> Arccotan = (of) => Math.PI / 2 - Math.Atan(of);
        public DvArccotangens(IDvExpr of) : base(of, Arccotan, "arcctg") { }

        protected override DvSingleArgOperator CreateInstance(IDvExpr of) => new DvArccotangens(of);

    }
}
