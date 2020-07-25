using System;
using System.Collections.Generic;
using System.Text;

using exp = System.Linq.Expressions.Expression;
using System.Linq.Expressions;

using Derivas.Exception;


namespace Derivas.Expression
{

    /// <summary>
    /// Commodity abstract class that conatins functionality 
    /// common to all binary operators
    /// </summary>
    /// <typeparam name="double">Any "numeric" type with operators overloaded(+, -, *, /, ...)</typeparam>
    internal abstract class DvBinaryOperator : IDvExpr
    {
        # region base class functionality

        public IDvExpr first { get; protected set; }
        public IDvExpr second { get; protected set; }
        private bool putParentheses { get; set; } = false;
        private char operatorSign { get; }

        public DvBinaryOperator(IDvExpr first, IDvExpr second, char operatorSign)
        {
            putParsOnBin(first);
            putParsOnBin(second);

            this.first = first;
            this.second = second;
            this.operatorSign = operatorSign;
        }

        public static bool putParsOnBin(IDvExpr expr)
            => expr is DvBinaryOperator op ? op.putParentheses = true : false;

        #endregion

        #region interface implementation

        public double Calculate(IDictionary<string, double> nameVal)
        {
            return Operator(first.Calculate(nameVal), second.Calculate(nameVal));
        }

        public string Represent()
        {
            var res = $"{first.Represent()} {operatorSign} {second.Represent()}";
            return putParentheses ? $"({res})" : res;
        }

        #endregion

        # region abstract members specific to binary operator

        protected abstract Func<double, double, double> Operator { get; }

        # endregion
    }

    internal class DvAddition : DvBinaryOperator
    {
        public DvAddition(IDvExpr first, IDvExpr second) : base(first, second, '+')
        {
            Operator = (fst, snd) => fst + snd;
        }

        protected override Func<double, double, double> Operator { get; }
    }

    internal class DvMultiplication : DvBinaryOperator
    {
        public DvMultiplication(IDvExpr first, IDvExpr second) : base(first, second, '*')
        {
            Operator = (fst, snd) => fst * snd;
        }

        protected override Func<double, double, double> Operator { get; }
    }

    internal class DvZeroDivisionException : DvBaseException
    {
        public DvZeroDivisionException(IDvExpr quotient)
            : base($"You can't divide anything by zero, particularly {quotient.Represent()}")
        {
        }
    }

    internal class DvDivision : DvBinaryOperator
    {
        public DvDivision(IDvExpr first, IDvExpr second) : base(first, second, '/')
        {
            Operator = (fst, snd) => snd == 0.0 ? throw new DvZeroDivisionException(first) : fst / snd;
        }

        protected override Func<double, double, double> Operator { get; }
    }

    internal class DvSubtraction : DvBinaryOperator
    {
        public DvSubtraction(IDvExpr first, IDvExpr second) : base(first, second, '-')
        {
            Operator = (fst, snd) => fst - snd;
        }

        protected override Func<double, double, double> Operator { get; }
    }

    internal class DvExponantiation : DvBinaryOperator
    {
        public DvExponantiation(IDvExpr first, IDvExpr second) : base(first, second, '^')
        {
            Operator = (fst, snd) => Math.Pow(fst, snd);
        }

        protected override Func<double, double, double> Operator { get; }
    }
}
