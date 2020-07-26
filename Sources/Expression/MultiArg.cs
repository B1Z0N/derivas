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
    /// Commodity abstract class that conatins functionality 
    /// common to all binary operators
    /// </summary>
    /// <typeparam name="double">Any "numeric" type with operators overloaded(+, -, *, /, ...)</typeparam>
    internal abstract class DvMultiArgOperator : IDvExpr
    {
        # region base class functionality

        public IDvExpr[] Operands { get; protected set; }
        private string OperatorSign { get; }
        protected static IDictionary<Type, int> Priorities { get; } = new Dictionary<Type, int>();

        public DvMultiArgOperator(string operatorSign, params IDvExpr[] lst)
        {
            this.Operands = lst;
            this.OperatorSign = operatorSign;
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
                if (el is DvMultiArgOperator op && Priorities[el.GetType()] < Priorities[GetType()])
                {
                    withPars.Add($"({el.Represent()})");
                }
                else
                {
                    withPars.Add(el.Represent());
                }
            }

            return String.Join($" {OperatorSign} ", withPars);
        }

        #endregion

        # region abstract members specific to any operator

        protected abstract Func<double[], double> Operator { get; }

        # endregion
    }

    internal class DvAddition : DvMultiArgOperator
    {
        public DvAddition(params IDvExpr[] operands) : base("+", operands)
        {
            Operator = (double[] args) => args.Aggregate(0d, (acc, el) => acc + el);
            Priorities[GetType()] = 0;
        }

        protected override Func<double[], double> Operator { get; }
    }

    internal class DvMultiplication : DvMultiArgOperator
    {
        public DvMultiplication(params IDvExpr[] operands) : base("*", operands)
        {
            Operator = (double[] args) => args.Aggregate(1d, (acc, el) => acc * el);
            Priorities[GetType()] = 1;
        }

        protected override Func<double[], double> Operator { get; }
    }

    internal abstract class DvBinaryOperator : DvMultiArgOperator
    {
        public DvBinaryOperator(IDvExpr first, IDvExpr second, string opSign) 
            : base (opSign, first, second)
        {
            Operator = (double[] args) => BinaryOperator(args[0], args[1]);
        }


        protected override Func<double[], double> Operator { get; }
        protected abstract Func<double, double, double> BinaryOperator { get; }
    }

    internal class DvDivision : DvBinaryOperator
    {
        public DvDivision(IDvExpr first, IDvExpr second) : base(first, second, "/")
        {

            BinaryOperator = (fst, snd) => snd == 0 ? throw new DvZeroDivisionException(first) : fst / snd;
            Priorities[GetType()] = 1;
        }

        protected override Func<double, double, double> BinaryOperator { get; }
    }

    internal class DvSubtraction : DvBinaryOperator
    {
        public DvSubtraction(IDvExpr first, IDvExpr second) : base(first, second, "-")
        {

            BinaryOperator = (fst, snd) => fst - snd;
            Priorities[GetType()] = 0;
        }

        protected override Func<double, double, double> BinaryOperator { get; }
    }

    internal class DvExponantiation : DvBinaryOperator
    {
        public DvExponantiation(IDvExpr first, IDvExpr second) : base(first, second, "^")
        {
            BinaryOperator = (fst, snd) => Math.Pow(fst, snd);
            Priorities[GetType()] = 2;
        }

        protected override Func<double, double, double> BinaryOperator { get; }
    }
}
