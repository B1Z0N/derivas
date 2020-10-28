using System;
using System.Linq;

using Derivas.Expression;
using Derivas.Exception;
using Derivas.Simplifier;

namespace Derivas
{
    public static class DvOps
    {
        # region binary operators
        public static IDvExpr Div(object fst, object snd)
            => new BinaryOperator(CheckExpr(fst), CheckExpr(snd), "/", 1, (fst, snd) => fst / snd);

        public static IDvExpr Sub(object fst, object snd)
            => new BinaryOperator(CheckExpr(fst), CheckExpr(snd), "-", 0, (fst, snd) => fst - snd);

        public static IDvExpr Pow(object bas, object pow) => new BinaryOperator(
            CheckExpr(bas), CheckExpr(pow), "^", 2, (bas, pow) => Math.Pow(bas, pow)
        );

        private static Func<double[], double> Addition =
            args => args.Aggregate(0d, (fst, snd) => fst + snd);

        private static Func<double[], double> Multiplication =
            args => args.Aggregate(1d, (fst, snd) => fst * snd);

        #endregion

        #region commutative associative multiarg operator

        private static IDvExpr CheckForLessThanTwo(Func<CloneableExpr[], CloneableExpr> createF, params CloneableExpr[] args)
            => args.Count() < 2 ? throw new DvNotEnoughArgumentsException(2) : createF(args);

        public static IDvExpr Add(params object[] args)
            => CheckForLessThanTwo(
                ops => new CommutativeAssociativeOperator("+", 0, Addition, ops),
                CheckExpr(args)
            );

        public static IDvExpr Mul(params object[] args)
            => CheckForLessThanTwo(
                ops => new CommutativeAssociativeOperator("*", 1, Multiplication, ops),
                CheckExpr(args)
            );

        #endregion

        #region unary operator

        public static IDvExpr Log(object of, object bas = null)
            => new Logarithm(CheckExpr(of), CheckExpr(bas ?? DvConsts.E));

        public static IDvExpr Cos(object of)
            => new UnaryOperator(CheckExpr(of), "cos", Math.Cos);

        public static IDvExpr Sin(object of)
            => new UnaryOperator(CheckExpr(of), "sin", Math.Sin);

        public static IDvExpr Tan(object of)
            => new UnaryOperator(CheckExpr(of), "tan", Math.Tan);

        public static IDvExpr Cotan(object of)
            => new UnaryOperator(CheckExpr(of), "cotan", of => 1 / Math.Tan(of));

        public static IDvExpr Acos(object of)
            => new UnaryOperator(CheckExpr(of), "arccos", Math.Acos);

        public static IDvExpr Asin(object of)
            => new UnaryOperator(CheckExpr(of), "arcsin", Math.Asin);

        public static IDvExpr Atan(object of)
            => new UnaryOperator(CheckExpr(of), "arctan", Math.Atan);

        public static IDvExpr Acotan(object of)
            => new UnaryOperator(CheckExpr(of), "arccotan", of => Math.PI / 2 - Math.Atan(of));

        public static IDvExpr Cosh(object of)
            => new UnaryOperator(CheckExpr(of), "cosh", Math.Cosh);

        public static IDvExpr Sinh(object of)
            => new UnaryOperator(CheckExpr(of), "sinh", Math.Sinh);

        public static IDvExpr Tanh(object of)
            => new UnaryOperator(CheckExpr(of), "tanh", Math.Tanh);

        public static IDvExpr Cotanh(object of)
            => new UnaryOperator(CheckExpr(of), "cotanh", of => 1 / Math.Tanh(of));

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
        
        #endregion
    }
}