using System;
using System.Collections.Generic;
using System.Text;

using exp = System.Linq.Expressions.Expression;
using System.Linq.Expressions;

using Derivas.Exception;
using Derivas.Constant;


namespace Derivas.Expression
{

    /// <summary>
    /// Commodity abstract class that conatins functionality 
    /// common to all binary operators
    /// </summary>
    /// <typeparam name="TNum">Any "numeric" type with operators overloaded(+, -, *, /, ...)</typeparam>
    public abstract class DvBinOp<TNum> : IDvExpr<TNum>
    {
        # region base class functionality

        public IDvExpr<TNum> first { get; protected set; }
        public IDvExpr<TNum> second { get; protected set; }
        private bool putParentheses { get; set; } = false;
        private char operatorSign { get; }

        public DvBinOp(IDvExpr<TNum> first, IDvExpr<TNum> second, char operatorSign)
        {
            enableParenthesesIfApplies(first);
            enableParenthesesIfApplies(second);

            this.first = first;
            this.second = second;
            this.operatorSign = operatorSign;
        }

        private void enableParenthesesIfApplies(IDvExpr<TNum> expr)
        {
            var op = expr as DvBinOp<TNum>;
            if (op != null) op.putParentheses = true;
        }

        /// <summary>
        /// Create function that will calculate opeartor for concrete type TNum
        /// </summary>
        /// <param name="calcLambda">exp.SomeOperation lambda for this operator</param>
        /// <returns>Compilde concrete function ready for invokation</returns>
        protected static Func<TNum, TNum, TNum> CreateCalcFunc(
            Func<ParameterExpression, ParameterExpression, BinaryExpression> calcLambda)
        {
            ParameterExpression first = exp.Parameter(typeof(TNum), "first");
            ParameterExpression second = exp.Parameter(typeof(TNum), "second");

            BinaryExpression body = calcLambda(first, second);
            return exp.Lambda<Func<TNum, TNum, TNum>>(body, first, second).Compile();
        }

        #endregion

        #region interface implementation

        public TNum Calculate(IDictionary<string, TNum> nameVal)
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

        protected abstract Func<TNum, TNum, TNum> Operator { get; }

        # endregion
    }

    public class DvAdd<TNum> : DvBinOp<TNum>
    {
        public DvAdd(IDvExpr<TNum> first, IDvExpr<TNum> second) : base(first, second, '+')
        {
            Operator = CreateCalcFunc((fst, snd) => exp.Add(fst, snd));
        }

        protected override Func<TNum, TNum, TNum> Operator { get; }
    }

    public class DvMultiply<TNum> : DvBinOp<TNum>
    {
        public DvMultiply(IDvExpr<TNum> first, IDvExpr<TNum> second) : base(first, second, '*')
        {
            Operator = CreateCalcFunc((fst, snd) => exp.Multiply(fst, snd));
        }

        protected override Func<TNum, TNum, TNum> Operator { get; }
    }

    public class DvZeroDivisionException<TNum> : DvBaseException
    {
        public DvZeroDivisionException(IDvExpr<TNum> quotient)
            : base($"You can't divide anything by zero, particularly {quotient.Represent()}")
        {
        }
    }

    public class DvDivide<TNum> : DvBinOp<TNum>
    {
        public DvDivide(IDvExpr<TNum> first, IDvExpr<TNum> second) : base(first, second, '/')
        {
            var inner = CreateCalcFunc((fst, snd) => exp.Divide(fst, snd));
            var zero = new DvDefaultConstantsProvider<TNum>().Zero.Val;
            Operator = (fst, snd) => EqualityComparer<TNum>.Default.Equals(snd, zero) ? 
                throw new DvZeroDivisionException<TNum>(first) : inner(fst, snd);
        }

        protected override Func<TNum, TNum, TNum> Operator { get; }
    }

    public class DvSubtract<TNum> : DvBinOp<TNum>
    {
        public DvSubtract(IDvExpr<TNum> first, IDvExpr<TNum> second) : base(first, second, '-')
        {
            Operator = CreateCalcFunc((fst, snd) => exp.Subtract(fst, snd));
        }

        protected override Func<TNum, TNum, TNum> Operator { get; }
    }

}
