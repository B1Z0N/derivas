using Derivas.Utils;
using System;

namespace Derivas.Expression
{
    internal class Logarithm : Expr
    {
        public Expr Of { get; }
        public Expr Base { get; }

        public Logarithm(Expr of, Expr bas = null)
            => (Of, Base) = (of, bas ?? DvConsts.E);

        #region abstract class implementation

        public override string Represent()
            => Base.Equals(DvConsts.E) ? $"log({Of.Represent()})" :
            $"log({Of.Represent()}, base={Base.Represent()})";

        public override double Calculate(NameVal concrete)
            => Math.Log(Of.Calculate(concrete), Base.Calculate(concrete));

        #endregion abstract class implementation

        #region equals related stuff

        public override bool Equals(Expr other)
        {
            var op = other as Logarithm;
            return op != null && Of == op.Of && Base == op.Base;
        }

        public override bool Equals(object obj) => Equals(obj as Expr);

        public override int GetHashCode() => HashCode.Combine(Of, Base);

        #endregion equals related stuff
    }

    internal static partial class OperatorCollection
    {
        public static Expr Logarithm(Expr of, Expr bas = null)
            => new Logarithm(of, bas);
    }
}