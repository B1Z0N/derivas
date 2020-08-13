using System;

namespace Derivas.Expression
{
    internal class Logarithm : BinaryOperator
    {
        public IDvExpr Of { get => First; set => First = value; } 
        public IDvExpr Base { get => Second; set => Second = value; }

        public Logarithm(IDvExpr of, IDvExpr bas = null)
            : base(of, bas ?? DvOps.E, "log", int.MaxValue, Math.Log)
        { 
        }
        
        #region abstract class implementation

        public override string Represent()
            => Base.Equals(DvOps.E) ? $"log({Of.Represent()})" :
            $"log({Of.Represent()}, base={Base.Represent()})";

        public override Operator CreateInstance(params IDvExpr[] operands)
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
        public static IDvExpr Log(object of, object bas = null)
            => new Logarithm(CheckExpr(of), CheckExpr(bas ?? E));
    }
}