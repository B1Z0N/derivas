using System;

namespace Derivas.Expression
{
    internal class Logarithm : IDvExpr
    {
        public IDvExpr Of { get; }
        public IDvExpr Base { get; }

        public Logarithm(IDvExpr of, IDvExpr bas = null)
            => (Of, Base) = (of, bas ?? DvConsts.E);

        #region interface implementation

        public  string Represent()
            => Base.Equals(DvConsts.E) ? $"log({Of.Represent()})" :
            $"log({Of.Represent()}, base={Base.Represent()})";

        public  double Calculate(DvNameVal concrete)
            => Math.Log(Of.Calculate(concrete), Base.Calculate(concrete));

        public IDvExpr Clone() => new Logarithm(Of, Base);
        
        #endregion interface implementation

        #region equals related stuff

        public  bool Equals(IDvExpr other)
        {
            var op = other as Logarithm;
            return op != null && Of == op.Of && Base == op.Base;
        }

        public override bool Equals(object obj) => Equals(obj as IDvExpr);

        public override int GetHashCode() => HashCode.Combine(Of, Base);

        #endregion equals related stuff
    }

    public static partial class DvOps
    {
        public static IDvExpr Log(IDvExpr of, IDvExpr bas = null)
            => new Logarithm(of, bas);
    }
}