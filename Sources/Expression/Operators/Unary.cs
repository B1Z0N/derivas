using System;

namespace Derivas.Expression
{
    internal class UnaryOperator : IDvExpr
    {
        public IDvExpr Of { get; }
        public string Name { get; }

        protected Func<double, double> OpFunc { get; }

        public UnaryOperator(IDvExpr of, string name, Func<double, double> op)
            => (Of, Name, OpFunc) = (of, name, op);

        #region interface implementation

        public  string Represent() => $"{Name}({Of.Represent()})";

        public  double Calculate(DvNameVal concrete)
            => OpFunc(Of.Calculate(concrete));

        public IDvExpr Clone() => new UnaryOperator(Of, Name, OpFunc);

        #endregion interface implementation

        #region equals related stuff

        public  bool Equals(IDvExpr other)
        {
            var op = other as UnaryOperator;
            return op != null && Of == op.Of && Name == op.Name;
        }

        public override bool Equals(object obj) => Equals(obj as IDvExpr);

        public override int GetHashCode() => HashCode.Combine(Of, Name);


        #endregion equals related stuff
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