using Derivas.Exception;
using System;
using System.Linq;

namespace Derivas.Expression
{
    using static DvOps;

    internal class Derivative
    {
        public IDvExpr Original { get; }
        public Symbol By { get; }

        public Derivative(IDvExpr expr, Symbol by)
            => (Original, By) = (expr, by);

        public IDvExpr Get() => Get(Original);

        private IDvExpr Get(IDvExpr expr)
            => expr switch
            {
                Constant con => Get(con),
                Symbol sym => Get(sym),
                Logarithm log => Get(log),
                UnaryOperator uop => Get(uop),
                BinaryOperator bop => Get(bop),
                CommutativeAssociativeOperator caop => Get(caop),
                _ => throw new DvDerivativeMismatchException(Original)
            };

        private IDvExpr Get(Constant con) => new Constant(0d);

        private IDvExpr Get(Symbol sym)
            => sym.Equals(By) ? new Constant(1d) : new Constant(0d);

        private IDvExpr Get(Logarithm log)
        {
            IDvExpr bas = log.Base, of = log.Of;
            return bas == DvConsts.E ? Natural(of) : Get(Div(Natural(of), Natural(bas)));

            IDvExpr Natural(IDvExpr of) => Mul(Div(1, of), Get(of));
        }

        private IDvExpr Get(UnaryOperator uop)
        {
            return uop.Sign switch
            {
                DvOpSigns.cos => DerCos(uop),
                DvOpSigns.sin => DerSin(uop),
                DvOpSigns.tan => DerTan(uop),
                DvOpSigns.cotan => DerCotan(uop),
                DvOpSigns.acos => DerAcos(uop),
                DvOpSigns.asin => DerAsin(uop),
                DvOpSigns.atan => DerAtan(uop),
                DvOpSigns.acotan => DerAcotan(uop),
                DvOpSigns.cosh => DerCosh(uop),
                DvOpSigns.sinh => DerSinh(uop),
                DvOpSigns.tanh => DerTanh(uop),
                DvOpSigns.cotanh => DerCotanh(uop),
                _ => throw new DvDerivativeMismatchException(uop, $"no such expr name: {uop.Sign}")
            };

            // -sin(x)
            IDvExpr DerCos(UnaryOperator op) => Mul(-1, Sin(op.Of), Get(op.Of));
            // cos(x)
            IDvExpr DerSin(UnaryOperator op) => Mul(Cos(op.Of), Get(op.Of));
            // 1/cos^2(x)
            IDvExpr DerTan(UnaryOperator op) => Mul(Div(1, Pow(Cos(op.Of), 2)), Get(op.Of));
            // -1/sin^2(x)
            IDvExpr DerCotan(UnaryOperator op) => Mul(-1, Div(1, Pow(Sin(op.Of), 2)), Get(op.Of));
            // -1/sqrt(1-x^2)
            IDvExpr DerAcos(UnaryOperator op) => Mul(-1, DerAsin(op));
            // 1/sqrt(1-x^2)
            IDvExpr DerAsin(UnaryOperator op) => Mul(Div(1, Pow(Sub(1, Pow(op.Of, 2)), 0.5)), Get(op.Of));
            // 1/(1+x^2)
            IDvExpr DerAtan(UnaryOperator op) => Mul(Div(1, Add(1, Pow(op.Of, 2))), Get(op.Of));
            // -1/(1+x^2)
            IDvExpr DerAcotan(UnaryOperator op) => Mul(-1, DerAtan(op));
            // sinh
            IDvExpr DerCosh(UnaryOperator op) => Mul(Sinh(op.Of), Get(op.Of));
            // -cosh
            IDvExpr DerSinh(UnaryOperator op) => Mul(-1, Cosh(op.Of), Get(op.Of));
            // 1/cosh^2(x)
            IDvExpr DerTanh(UnaryOperator op) => Mul(Div(1, Pow(Cosh(op.Of), 2)), Get(op.Of));
            // -1/sinh^2(x)
            IDvExpr DerCotanh(UnaryOperator op) => Mul(-1, Div(1, Pow(Sinh(op.Of), 2)), Get(op.Of));
        }

        #region binary operator

        private IDvExpr Get(BinaryOperator bop)
        {
            return bop.Sign switch
            {
                DvOpSigns.sub => Subtraction(bop),
                DvOpSigns.div => Division(bop),
                DvOpSigns.pow => Power(bop),
                _ => throw new DvDerivativeMismatchException(bop, $"no such sign: {bop.Sign}")
            };

            IDvExpr Subtraction(BinaryOperator op)
                => op.CreateInstance(Get(op.First), Get(op.Second));

            IDvExpr Division(BinaryOperator op)
            {
                IDvExpr up = op.First, low = op.Second;
                return Div(
                    Sub(
                        Mul(low, Get(up)),
                        Mul(Get(low), up)
                    ), Pow(low, 2));
            }

            IDvExpr Power(BinaryOperator op)
            {
                IDvExpr low = op.First, up = op.Second;

                return Mul(
                    Pow(low, Sub(up, 1)),
                    Add
                    (
                        Mul(Get(up), low, Log(low)),
                        Mul(up, Get(low))
                    )
                );
            }
        }

        #endregion binary operator

        #region commutative associative operator

        private IDvExpr Get(CommutativeAssociativeOperator caop)
        {
            return caop.Sign switch
            {
                DvOpSigns.add => Addition(caop),
                DvOpSigns.mul => Multiplication(caop),
                _ => throw new DvDerivativeMismatchException(caop, $"no such sign: {caop.Sign}")
            };

            IDvExpr Addition(CommutativeAssociativeOperator op)
                => op.CreateInstance(op.Operands.Select(Get).ToArray());

            IDvExpr Multiplication(CommutativeAssociativeOperator op)
            {
                var first = op.Operands.ElementAt(0);
                if (op.Operands.Count() == 2)
                {
                    var second = op.Operands.ElementAt(1);

                    return Add(Mul(Get(first), second), Mul(Get(second), first));
                }

                var other = op.CreateInstance(op.Operands.Skip(1).ToArray());
                return Add(Mul(Get(first), other), Mul(Get(other), first));
            }
        }

        #endregion commutative associative operator
    }
}