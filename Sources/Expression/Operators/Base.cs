using System;
using System.Collections.Generic;
using System.Linq;

using Derivas.Utils;

namespace Derivas.Expression
{
    /// <summary>
    /// Abstract class to define functionality and interface(in broader sense)
    /// to all nongeneric Operator functionality
    /// </summary>
    internal abstract class Operator : Expr
    {
        #region abstract members specific to any operator

        public abstract string Sign { get; }
        public abstract int Priority { get; }
        protected abstract Func<double[], double> OpFunc { get; }

        #endregion

        #region base class functionality

        public IEnumerable<Expr> Operands => Operands_;
        protected List<Expr> Operands_;

        public Operator(params Expr[] lst)
            => Operands_ = new List<Expr>(lst);

        #endregion

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
                    el is Operator op && Priority > op.Priority ?
                    $"({el.Represent()})" : el.Represent()
                );
            }

            return String.Join($" {Sign} ", withPars);
        }

        #endregion

        #region equals related stuff

        public override bool Equals(Expr other)
        {
            var op = other as Operator;
            return op != null && ConcreteType == op.ConcreteType &&
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

        public static bool operator ==(Operator fst, Operator snd) => fst.Equals(snd);
        public static bool operator !=(Operator fst, Operator snd) => !fst.Equals(snd);

        #endregion
    }
}
