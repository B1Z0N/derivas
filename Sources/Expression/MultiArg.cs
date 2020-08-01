using System;
using System.Collections.Generic;
using System.Linq;

using Derivas.Exception;
using Derivas.Expression;

namespace Derivas.Exception
{
    internal class DvZeroDivisionException : DvBaseException
    {
        public DvZeroDivisionException(IDvExpr quotient)
            : base($"You can't divide anything by zero, particularly {quotient.Represent()}")
        {
        }
    }
}

namespace Derivas.Expression
{
    /// <summary>
    /// Commodity abstract class that conatins functionality common to all binary operators
    /// </summary>
    internal abstract class DvMultiArgOperator : IDvExpr
    {
        #region base class functionality

        public IEnumerable<IDvExpr> Operands { get; protected set; }

        public DvMultiArgOperator(params IDvExpr[] lst)
        {
            Operands = lst;
        }

        #endregion

        #region interface implementation
        public double Calculate(IDictionary<string, double> nameVal)
        => Operator(Operands.Select((IDvExpr el) => el.Calculate(nameVal)).ToArray());

        public string Represent()
        {
            var withPars = new List<string>();
            foreach (var el in Operands)
            {
                withPars.Add(
                    el is DvMultiArgOperator op && Priority > op.Priority ?
                    $"({el.Represent()})" : el.Represent()
                );
            }

            return String.Join($" {Sign} ", withPars);
        }

        protected abstract DvMultiArgOperator CreateInstance(params IDvExpr[] operands);

        #endregion

        #region abstract members specific to any operator

        protected abstract Func<double[], double> Operator { get; }
        protected abstract int Priority { get; }
        protected abstract string Sign { get; }

        #endregion
    }

    internal class DvAddition : DvMultiArgOperator
    {
        public DvAddition(params IDvExpr[] operands) : base(operands)
        {
        }

        protected override Func<double[], double> Operator { get; }
            = (double[] args) => args.Aggregate(0d, (acc, el) => acc + el);
        protected override int Priority { get; } = 0;
        protected override string Sign { get; } = "+";

        protected override DvMultiArgOperator CreateInstance(params IDvExpr[] operands)
            => new DvAddition(operands.ToArray());
    }

    internal abstract class DvBinaryOperator : DvMultiArgOperator
    {
        public IDvExpr First { get; }
        public IDvExpr Second { get; }

        public DvBinaryOperator(IDvExpr first, IDvExpr second)
            : base(first, second)
        {
            (First, Second) = (first, second);
            Operator = (double[] args) => BinaryOperator(args[0], args[1]);
        }

        protected override Func<double[], double> Operator { get; }
        protected abstract Func<double, double, double> BinaryOperator { get; }
    }

    internal class DvMultiplication : DvBinaryOperator
    {
        public DvMultiplication(IDvExpr first, IDvExpr second)
            : base(first, second)
        {
        }

        protected override Func<double, double, double> BinaryOperator { get; }
            = (fst, snd) => fst * snd;
        protected override int Priority { get; } = 1;
        protected override string Sign { get; } = "*";

        protected override DvMultiArgOperator CreateInstance(params IDvExpr[] operands)
            => new DvMultiplication(operands[0], operands[1]);
    }

    internal class DvDivision : DvBinaryOperator
    {
        public DvDivision(IDvExpr first, IDvExpr second) : base(first, second)
        {
            BinaryOperator = (fst, snd) => snd == 0 ? throw new DvZeroDivisionException(first) : fst / snd;
        }

        protected override Func<double, double, double> BinaryOperator { get; }
        protected override string Sign { get; } = "/";
        protected override int Priority { get; } = 1;

        protected override DvMultiArgOperator CreateInstance(params IDvExpr[] operands)
            => new DvDivision(operands[0], operands[1]);
    }

    internal class DvSubtraction : DvBinaryOperator
    {
        public DvSubtraction(IDvExpr first, IDvExpr second) : base(first, second)
        {
        }

        protected override Func<double, double, double> BinaryOperator { get; } = (fst, snd) => fst - snd;
        protected override string Sign { get; } = "-";
        protected override int Priority { get; } = 0;

        protected override DvMultiArgOperator CreateInstance(params IDvExpr[] operands)
            => new DvSubtraction(operands[0], operands[1]);
    }

    internal class DvExponantiation : DvBinaryOperator
    {
        public DvExponantiation(IDvExpr first, IDvExpr second) : base(first, second)
        {
        }

        protected override Func<double, double, double> BinaryOperator { get; } = (fst, snd) => Math.Pow(fst, snd);
        protected override string Sign { get; } = "^";
        protected override int Priority { get; } = 2;

        protected override DvMultiArgOperator CreateInstance(params IDvExpr[] operands)
            => new DvExponantiation(operands[0], operands[1]);
    }

}