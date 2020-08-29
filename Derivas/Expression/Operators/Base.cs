using System;
using System.Collections.Generic;
using System.Linq;

using Derivas.Exception;
using Derivas.Expression;

namespace Derivas.Expression
{
    /// <summary>
    /// Abstract class to define functionality and interface(in broader sense)
    /// to all nongeneric Operator functionality
    /// </summary>
    internal abstract class Operator : CloneableExpr
    {
        #region abstract members specific to any operator

        public abstract string Sign { get; }
        public abstract int Priority { get; }
        public abstract Func<double[], double> OpFunc { get; }
        public abstract IEnumerable<CloneableExpr> Operands { get; }

        #endregion abstract members specific to any operator

        #region IDvExpr interface implementation

        public override double Calculate(IDictionary<string, double> concrete)
            => OpFunc(
                Operands.Select(
                    el => el.Calculate(concrete)
                ).ToArray()
            );

        public override string Represent()
        {
            var withPars = new List<string>();
            foreach (var el in Operands)
            {
                withPars.Add(
                    el is Operator op && Priority > op.Priority ?
                    $"({el.Represent()})" : el.Represent()
                );
            }

            return String.Join($" {Sign} ", withPars);
        }

        #endregion IDvExpr interface implementation

        #region equals related stuff

        public override bool Equals(IDvExpr other)
        {
            var op = other as Operator;
            return op != null && GetType() == other.GetType() && Sign == op.Sign &&
                Operands.SequenceEqual(op.Operands);
        }

        public override bool Equals(object obj) => Equals(obj as IDvExpr);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            foreach (var item in Operands)
            {
                hash.Add(item);
            }

            hash.Add(GetType());
            hash.Add(Sign);

            return hash.ToHashCode();
        }

        #endregion equals related stuff
    }
}
