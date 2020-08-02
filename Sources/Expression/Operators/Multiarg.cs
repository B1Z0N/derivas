using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Derivas.Utils;

namespace Derivas.Expression.Operators
{
    internal interface IOperandReplacable<OperandT>
    {
        void Replace(OperandT fst, OperandT snd);
    }

    /// <summary>
    /// Commodity abstract class that conatins functionality common to all binary operators
    /// </summary>
    internal abstract class MultiArgOperator : IDvExpr
    {
        #region base class functionality

        public IEnumerable<IDvExpr> Operands { get; protected set; }

        public MultiArgOperator(params IDvExpr[] lst)
            => Operands = new List<IDvExpr>(lst);

        #endregion

        #region IDvExpr implementation

        public double Calculate(IDictionary<string, double> nameVal)
            => Operator(
                Operands.Select((IDvExpr el) => el.Calculate(nameVal))
                .ToArray()
            );

        public string Represent()
        {
            var withPars = new List<string>();
            foreach (var el in Operands)
            {
                withPars.Add(
                    el is MultiArgOperator op && Priority > op.Priority ?
                    $"({el.Represent()})" : el.Represent()
                );
            }

            return String.Join($" {Sign} ", withPars);
        }

        #endregion

        #region IInstaneInfo implementation

        public abstract DvMultiArgOperator CreateInstance(params object[] args);

        #endregion

        #region abstract members specific to any operator

        protected abstract Func<double[], double> Operator { get; }
        protected abstract int Priority { get; }
        protected abstract string Sign { get; }

        #endregion
    }

}
