using System;

namespace Derivas.Expression
{
    internal class Logarithm : BinaryOperator
    {
        public IDvExpr Of { get => First; set => First = value; } 
        public IDvExpr Base { get => Second; set => Second = value; }

        public Logarithm(IDvExpr of, IDvExpr bas = null)
            : base(of, bas ?? DvConsts.E, "log", int.MaxValue, Math.Log)
        { 
        }
        
        #region abstract class implementation

        public override string Represent()
            => Base.Equals(DvConsts.E) ? $"log({Of.Represent()})" :
            $"log({Of.Represent()}, base={Base.Represent()})";

        public override MultiArgOperator CreateInstance(params IDvExpr[] operands)
        {
            if (operands.Length < 2)
            {
                return new Logarithm(Of, Base);
            }
            else
            {
                return new Logarithm(operands[0], operands[1]);
            }
        }
       
        #endregion abstract class implementation

    }

    public static partial class DvOps
    {
        public static IDvExpr Log(IDvExpr of, IDvExpr bas = null)
            => new Logarithm(of, bas);
    }
}