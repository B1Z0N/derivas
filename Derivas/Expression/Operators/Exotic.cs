using System;

namespace Derivas.Expression
{
    internal class Logarithm : BinaryOperator
    {
        public CloneableExpr Of { get => First; set => First = value; } 
        public CloneableExpr Base { get => Second; set => Second = value; }

        public Logarithm(CloneableExpr of, CloneableExpr bas = null)
            : base(of, bas ?? DvOps.DvConsts.CL_E, "log", int.MaxValue, Math.Log)
        { 
        }
        
        #region abstract class implementation

        public override string Represent()
            => Base.Equals(DvOps.DvConsts.E) ? $"log({Of.Represent()})" :
            $"log({Of.Represent()}, base={Base.Represent()})";

        protected override CloneableExpr CreateFromClonable(params CloneableExpr[] operands)
        {
            if (operands.Length < 2)
            {
                return new Logarithm(Of.CreateInstance(), Base.CreateInstance());
            }
            else
            {
                return new Logarithm(operands[0].CreateInstance(), operands[1].CreateInstance());
            }
        }
       
        #endregion abstract class implementation

    }

    public static partial class DvOps
    {
        public static IDvExpr Log(object of, object bas = null)
            => new Logarithm(CheckExpr(of), CheckExpr(bas ?? DvConsts.E));
    }
}