using Derivas.Utils;
using System;

namespace Derivas.Expression
{
    internal class UnaryOperator : Expr
    {
        public Expr Of { get; }
        public string Name { get; }

        protected Func<double, double> OpFunc { get; }

        public UnaryOperator(Expr of, string name, Func<double, double> op)
            => (Of, Name, OpFunc) = (of, name, op);

        #region abstract class implementation

        public override string Represent() => $"{Name}({Of.Represent()})";

        public override double Calculate(NameVal concrete)
            => OpFunc(Of.Calculate(concrete));

        #endregion abstract class implementation

        #region equals related stuff

        public override bool Equals(Expr other)
        {
            var op = other as UnaryOperator;
            return op != null && Of == op.Of && Name == op.Name;
        }

        public override bool Equals(object obj) => Equals(obj as Expr);

        public override int GetHashCode() => HashCode.Combine(Of, Name);

        #endregion equals related stuff
    }

    internal static partial class OperatorCollection
    {
        public static Expr Cosine(Expr of)
            => new UnaryOperator(of, "cos", Math.Cos);

        public static Expr Sine(Expr of)
            => new UnaryOperator(of, "sin", Math.Sin);

        public static Expr Tangent(Expr of)
            => new UnaryOperator(of, "tan", Math.Tan);

        public static Expr Cotangent(Expr of)
            => new UnaryOperator(of, "cotan", of => 1 / Math.Tan(of));

        public static Expr Arccosine(Expr of)
            => new UnaryOperator(of, "arccos", Math.Acos);

        public static Expr Arcsine(Expr of)
            => new UnaryOperator(of, "arcsin", Math.Asin);

        public static Expr Arctangent(Expr of)
            => new UnaryOperator(of, "arctan", Math.Atan);

        public static Expr Arccotangent(Expr of)
            => new UnaryOperator(of, "arccotan", of => Math.PI / 2 - Math.Atan(of));

        public static Expr HyperbolicCosine(Expr of)
            => new UnaryOperator(of, "cosh", Math.Cosh);

        public static Expr HyperbolicSine(Expr of)
            => new UnaryOperator(of, "sinh", Math.Sinh);

        public static Expr HyperbolicTangent(Expr of)
            => new UnaryOperator(of, "tanh", Math.Tanh);

        public static Expr HyperbolicCotangent(Expr of)
            => new UnaryOperator(of, "cotanh", of => 1 / Math.Tanh(of));
    }
}