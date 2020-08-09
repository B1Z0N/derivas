using Derivas.Exception;
using System;
using System.Linq;
using static Derivas.Expression.DvOps;

namespace Derivas.Expression
{
    public class DvDerivativeMismatch : DvBaseException
    {
        public DvDerivativeMismatch(Type t, string other = null) : base(
            $"There is no DvDerivative handler for type {t}" +
            other == null ? "." : $" and this: '{other}'.")
        {
        }
    }
}

namespace Derivas.Expression
{
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
                _ => throw new DvDerivativeMismatch(Original.GetType())
            };

        private IDvExpr Get(Constant con) => new Constant(0d);

        private IDvExpr Get(Symbol sym)
            => sym.Equals(By) ? new Constant(1d) : new Constant(0d);

        private IDvExpr Get(Logarithm log)
        {
            IDvExpr bas = log.Base, pow = log.Of;
            return Mul(
                log, Add(
                    Mul(Get(pow), Log(bas)),
                    Div(Mul(pow, Get(bas)), bas)
                )
            );
        }

        private IDvExpr Get(UnaryOperator uop)
        {
            return uop.Sign switch
            {

                _ => throw new DvDerivativeMismatch(uop.GetType(), $"no such expr name: {uop.Sign}")
            };

            // TODO
        }

        #region binary operator

        private IDvExpr Get(BinaryOperator bop)
        {
            return bop.Sign switch
            {
                "-" => Subtraction(bop),
                "/" => Division(bop),
                _ => throw new DvDerivativeMismatch(bop.GetType(), $"no such sign: {bop.Sign}")
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
        }

        #endregion binary operator

        #region commutative associative operator

        private IDvExpr Get(CommutativeAssociativeOperator caop)
        {
            return caop.Sign switch
            {
                "+" => Addition(caop),
                "*" => Multiplication(caop),
                _ => throw new DvDerivativeMismatch(caop.GetType(), $"no such sign: {caop.Sign}")
            };

            IDvExpr Addition(CommutativeAssociativeOperator op)
                => op.CreateInstance(op.Operands.Select(Get).ToArray());

            IDvExpr Multiplication(CommutativeAssociativeOperator op)
            {
                // TODO
                return null;
            }
        }

        #endregion commutative associative operator
    }

    public static partial class DvOps
    {
        public static IDvExpr Der(object expr, object by)
        {
            var symBy = CheckExpr(by) as Symbol ??
                throw new ArgumentOutOfRangeException(
                    $"Should be one of 'string' or 'Symbol', got {by.GetType()}"
                );
            return new Derivative(CheckExpr(expr), symBy).Get();
        }
    }
}