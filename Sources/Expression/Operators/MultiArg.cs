using Derivas.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Derivas.Expression
{
    /// <summary>
    /// Abstract class to define functionality and interface(in broader sense)
    /// to all nongeneric MultiArgOperator functionality
    /// </summary>
    internal abstract class MultiArgOperator : Expr
    {
        #region abstract members specific to any operator

        protected abstract string Sign { get; }
        protected abstract int Priority { get; }
        protected abstract Func<double[], double> OpFunc { get; }

        #endregion abstract members specific to any operator

        #region base class functionality

        public IEnumerable<Expr> Operands => Operands_;
        protected List<Expr> Operands_;

        public MultiArgOperator(params Expr[] lst)
            => Operands_ = new List<Expr>(lst);

        #endregion base class functionality

        #region Expr interface implementation

        public override double Calculate(NameVal concrete)
            => OpFunc(
                Operands_.Select(
                    el => el.Calculate(concrete)
                ).ToArray()
            );

        public override string Represent()
        {
            var withPars = new List<string>();
            foreach (var el in Operands_)
            {
                withPars.Add(
                    el is MultiArgOperator op && Priority > op.Priority ?
                    $"({el.Represent()})" : el.Represent()
                );
            }

            return String.Join($" {Sign} ", withPars);
        }

        #endregion Expr interface implementation

        #region equals related stuff

        public override bool Equals(Expr other)
        {
            var op = other as MultiArgOperator;
            return op != null && GetType() == other.GetType() &&
                Operands_.SequenceEqual(op.Operands_);
        }

        public override bool Equals(object obj) => Equals(obj as Expr);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            foreach (var item in Operands)
            {
                hash.Add(item);
            }

            hash.Add(this.GetType());
            return hash.ToHashCode();
        }

        #endregion equals related stuff
    }
}