using Derivas.Exception;
using Derivas.Expression;
using Derivas.Simplifier;
using System;
using System.Linq;
using Derivas.Parser;

namespace Derivas
{
    using static Utils;

    public static class DvOps
    {
        # region binary operators

        public static IDvExpr Div(object first, object second)
            => new BinaryOperator(CheckExpr(first), CheckExpr(second), DvOpSigns.div, 1, (first, second) => first / second);

        public static IDvExpr Sub(object first, object second)
            => new BinaryOperator(CheckExpr(first), CheckExpr(second), DvOpSigns.sub, 0, (first, second) => first - second);

        public static IDvExpr Pow(object bas, object pow) => new BinaryOperator(
            CheckExpr(bas), CheckExpr(pow), DvOpSigns.pow, 2, (bas, pow) => Math.Pow(bas, pow)
        );

        private static Func<double[], double> Addition =
            args => args.Aggregate(0d, (first, second) => first + second);

        private static Func<double[], double> Multiplication =
            args => args.Aggregate(1d, (first, second) => first * second);

        #endregion

        #region commutative associative multiarg operator

        private static IDvExpr CheckForLessThanTwo(Func<CloneableExpr[], CloneableExpr> createF, params CloneableExpr[] args)
            => args.Count() < 2 ? throw new DvNotEnoughArgumentsException(2) : createF(args);

        public static IDvExpr Add(params object[] args)
            => CheckForLessThanTwo(
                ops => new CommutativeAssociativeOperator(DvOpSigns.add, 0, Addition, ops),
                CheckExpr(args)
            );

        public static IDvExpr Mul(params object[] args)
            => CheckForLessThanTwo(
                ops => new CommutativeAssociativeOperator(DvOpSigns.mul, 1, Multiplication, ops),
                CheckExpr(args)
            );

        #endregion

        #region unary operator

        public static IDvExpr Log(object of, object bas = null)
            => new Logarithm(CheckExpr(of), CheckExpr(bas ?? DvConsts.E));

        public static IDvExpr Cos(object of)
            => new UnaryOperator(CheckExpr(of), DvOpSigns.cos, Math.Cos);

        public static IDvExpr Sin(object of)
            => new UnaryOperator(CheckExpr(of), DvOpSigns.sin, Math.Sin);

        public static IDvExpr Tan(object of)
            => new UnaryOperator(CheckExpr(of), DvOpSigns.tan, Math.Tan);

        public static IDvExpr Cotan(object of)
            => new UnaryOperator(CheckExpr(of), DvOpSigns.cotan, of => 1 / Math.Tan(of));

        public static IDvExpr Acos(object of)
            => new UnaryOperator(CheckExpr(of), DvOpSigns.acos, Math.Acos);

        public static IDvExpr Asin(object of)
            => new UnaryOperator(CheckExpr(of), DvOpSigns.asin, Math.Asin);

        public static IDvExpr Atan(object of)
            => new UnaryOperator(CheckExpr(of), DvOpSigns.atan, Math.Atan);

        public static IDvExpr Acotan(object of)
            => new UnaryOperator(CheckExpr(of), DvOpSigns.acotan, of => Math.PI / 2 - Math.Atan(of));

        public static IDvExpr Cosh(object of)
            => new UnaryOperator(CheckExpr(of), DvOpSigns.cosh, Math.Cosh);

        public static IDvExpr Sinh(object of)
            => new UnaryOperator(CheckExpr(of), DvOpSigns.sinh, Math.Sinh);

        public static IDvExpr Tanh(object of)
            => new UnaryOperator(CheckExpr(of), DvOpSigns.tanh, Math.Tanh);

        public static IDvExpr Cotanh(object of)
            => new UnaryOperator(CheckExpr(of), DvOpSigns.cotanh, of => 1 / Math.Tanh(of));

        #endregion

        #region other

        /// <summary>
        /// Shortcut handlers, transforms numeric -> Const, string -> Symbol, IDvExpr -> IDvExpr
        /// and throws on others
        /// </summary>
        private static CloneableExpr CheckExpr(object arg) => Utils.CheckExpr(arg);

        /// <summary>Same as single arg function</summary>
        private static CloneableExpr[] CheckExpr(object[] args) => Utils.CheckExpr(args);

        /// <summary>Derivative</summary>
        public static IDvExpr Der(object expr, object by)
        {
            var symBy = CheckExpr(by) as Symbol ??
                throw new ArgumentOutOfRangeException(
                    $"Should be one of 'string' or 'Symbol', got {by.GetType()}"
                );
            return new Derivative(CheckExpr(expr), symBy).Get();
        }

        public static IDvExpr Const(double val) => new Constant(val);

        public static IDvExpr Sym(string name) => new Symbol(name);

        /// <summary>
        /// Common mathematical constants.
        /// Prepended with CL are Cloneable - only for internal use.
        /// </summary>
        public static class DvConsts
        {
            public static IDvExpr E { get; } = new Constant(Math.E);
            public static IDvExpr PI { get; } = new Constant(Math.PI);

            internal static CloneableExpr CL_E { get; } = new Constant(Math.E);
            internal static CloneableExpr CL_PI { get; } = new Constant(Math.PI);
        }

        public static DvDict Dict => new DvDict();

        public static DvSimplifier Simpl(IDvExpr expr) => DvSimplifier.Create(expr);

        public static IDvExpr Parse(string expr) => Parser.Parser.Parse(expr);

        #endregion
    }
}