using System;

namespace Derivas.Expression
{
    internal class UnaryOperator : OrderedOperator
    {
        public IDvExpr Of { get => Operands_[0]; set => Operands_[0] = value; }
        protected Func<double, double> UnFunc { get; }

        public UnaryOperator(IDvExpr of, string name, Func<double, double> op)
            : base(of) => (Sign, UnFunc) = (name, op);

        #region abstract class implementation

        public override string Represent() => $"{Sign}({Of.Represent()})";

        public override Operator CreateInstance(params IDvExpr[] operands)
            => new UnaryOperator(operands.Length != 0 ? operands[0] : Of, Sign, UnFunc); 

        public override string Sign { get; }
        public override int Priority { get; } = int.MaxValue;

        public override Func<double[], double> OpFunc => (args) => UnFunc(args[0]); 

        #endregion abstract class implementation
    }

    public static partial class DvOps
    {
        public static IDvExpr Cos(IDvExpr of)
            => new UnaryOperator(of, "cos", Math.Cos);

        public static IDvExpr Sin(IDvExpr of)
            => new UnaryOperator(of, "sin", Math.Sin);

        public static IDvExpr Tan(IDvExpr of)
            => new UnaryOperator(of, "tan", Math.Tan);

        public static IDvExpr Cotan(IDvExpr of)
            => new UnaryOperator(of, "cotan", of => 1 / Math.Tan(of));

        public static IDvExpr Acos(IDvExpr of)
            => new UnaryOperator(of, "arccos", Math.Acos);

        public static IDvExpr Asin(IDvExpr of)
            => new UnaryOperator(of, "arcsin", Math.Asin);

        public static IDvExpr Atan(IDvExpr of)
            => new UnaryOperator(of, "arctan", Math.Atan);

        public static IDvExpr Acotan(IDvExpr of)
            => new UnaryOperator(of, "arccotan", of => Math.PI / 2 - Math.Atan(of));

        public static IDvExpr Cosh(IDvExpr of)
            => new UnaryOperator(of, "cosh", Math.Cosh);

        public static IDvExpr Sinh(IDvExpr of)
            => new UnaryOperator(of, "sinh", Math.Sinh);

        public static IDvExpr Tanh(IDvExpr of)
            => new UnaryOperator(of, "tanh", Math.Tanh);

        public static IDvExpr Cotanh(IDvExpr of)
            => new UnaryOperator(of, "cotanh", of => 1 / Math.Tanh(of));
    }
}