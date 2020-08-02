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
    internal abstract class BaseOperator : Expr
    {

        #region abstract members specific to any operator

        protected abstract Func<double[], double> OpFunc { get; }
        public abstract int Priority { get; }
        public abstract string Sign { get; }

        protected abstract Type ConcreteType { get; }

        #endregion

        #region base class functionality

        public IEnumerable<Expr> Operands => Operands_;
        protected List<Expr> Operands_;

        public BaseOperator(params Expr[] lst)
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
                    el is BaseOperator op && Priority > op.Priority ?
                    $"({el.Represent()})" : el.Represent()
                );
            }

            return String.Join($" {Sign} ", withPars);
        }

        #endregion

        #region equals related stuff

        public override bool Equals(Expr other)
        {
            var op = other as BaseOperator;
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

            hash.Add(ConcreteType);
            return hash.ToHashCode();
        }

        public static bool operator ==(BaseOperator fst, BaseOperator snd) => fst.Equals(snd);
        public static bool operator !=(BaseOperator fst, BaseOperator snd) => !fst.Equals(snd);

        #endregion
    }

    /// <summary>Generic operator class with more info about suboperator</summary>
    internal abstract class Operator<SubOpT>
        : BaseOperator where SubOpT : Operator<SubOpT>
    {
        protected override Type ConcreteType => typeof(SubOpT);
    }

    internal abstract class CommutativeAssociativeOperator<SubOpT>
        : Operator<SubOpT> where SubOpT : CommutativeAssociativeOperator<SubOpT>
    {
    }

    internal abstract class BinaryOperator<SubOpT>
        : Operator<SubOpT> where SubOpT : BinaryOperator<SubOpT>
    {
    }

    internal abstract class UnaryOperator<SubOpT>
        : Operator<SubOpT> where SubOpT : UnaryOperator<SubOpT>
    {
    }
}
