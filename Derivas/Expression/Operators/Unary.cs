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
        public static IDvExpr Cos(object of)
            => new UnaryOperator(CheckExpr(of), "cos", Math.Cos);

        public static IDvExpr Sin(object of)
            => new UnaryOperator(CheckExpr(of), "sin", Math.Sin);

        public static IDvExpr Tan(object of)
            => new UnaryOperator(CheckExpr(of), "tan", Math.Tan);

        public static IDvExpr Cotan(object of)
            => new UnaryOperator(CheckExpr(of), "cotan", of => 1 / Math.Tan(of));

        public static IDvExpr Acos(object of)
            => new UnaryOperator(CheckExpr(of), "arccos", Math.Acos);

        public static IDvExpr Asin(object of)
            => new UnaryOperator(CheckExpr(of), "arcsin", Math.Asin);

        public static IDvExpr Atan(object of)
            => new UnaryOperator(CheckExpr(of), "arctan", Math.Atan);

        public static IDvExpr Acotan(object of)
            => new UnaryOperator(CheckExpr(of), "arccotan", of => Math.PI / 2 - Math.Atan(of));

        public static IDvExpr Cosh(object of)
            => new UnaryOperator(CheckExpr(of), "cosh", Math.Cosh);

        public static IDvExpr Sinh(object of)
            => new UnaryOperator(CheckExpr(of), "sinh", Math.Sinh);

        public static IDvExpr Tanh(object of)
            => new UnaryOperator(CheckExpr(of), "tanh", Math.Tanh);

        public static IDvExpr Cotanh(object of)
            => new UnaryOperator(CheckExpr(of), "cotanh", of => 1 / Math.Tanh(of));
    }
}