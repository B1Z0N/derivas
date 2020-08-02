using System;
using System.Collections.Generic;
using System.Linq;

namespace Derivas.Expressions
{
    /// <summary>
    /// Abstract class to define functionality and interface(in broader sense)
    /// to all nongeneric Operator functionality
    /// </summary>
    internal abstract class BaseOperator : IDvExpr, IEquatable<BaseOperator>
    {

        #region abstract members specific to any operator

        protected abstract Func<double[], double> OpFunc { get; }
        public abstract int Priority { get; }
        public abstract string Sign { get; }

        protected abstract Type ConcreteType { get; }

        #endregion

        #region base class functionality

        public IEnumerable<IDvExpr> Operands => Operands_;
        protected IList<IDvExpr> Operands_;

        public BaseOperator(params IDvExpr[] lst)
        {
            Operands_ = new List<IDvExpr>(lst);
        }

        #endregion

        #region IDvExpr interface implementation

        public double Calculate(IDictionary<string, double> nameVal)
            => OpFunc(
                Operands.Select(
                    el => el.Calculate(nameVal)
                ).ToArray()
            );

        public string Represent()
        {
            var withPars = new List<string>();
            foreach (var el in Operands)
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

        public override bool Equals(object obj)
            => Equals(obj as BaseOperator);

        public bool Equals(BaseOperator other)
            => other != null && 
            ConcreteType == other.ConcreteType && 
            Operands.SequenceEqual(other.Operands);

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

        public static bool operator ==(BaseOperator left, BaseOperator right)
        {
            return EqualityComparer<BaseOperator>.Default.Equals(left, right);
        }

        public static bool operator !=(BaseOperator left, BaseOperator right)
        {
            return !(left == right);
        }

        #endregion
    }

    /// <summary>Generic operator class with more info about suboperator</summary>
    internal abstract class Operator<SubOpT> 
        : BaseOperator where SubOpT : Operator<SubOpT>
    {
        protected override Type ConcreteType => typeof(SubOpT);

        public virtual Operator<SubOpT> ReplaceOperand(IDvExpr it, IDvExpr with)
        {
            if (it is Operator<SubOpT> thisop)
            {

            }
            else if (it is BaseOperator thatop)
            {
                foreach (var operand in Operands_)
                {
                }
            }
        }
    }

    internal abstract class CommutativeAssociativeOperator<SubT> : Operator<SubT>
    {
        public override Operator<SubT> ReplaceOperand(IDvExpr fst, IDvExpr snd)
        {

        }
    }

    internal abstract class BinaryOperator<SubT> : Operator<SubT>
    {
    }

    internal abstract class UnaryOperator<SubT> : Operator<SubT>
    {
        public override Operator<SubT> ReplaceOperand(IDvExpr fst, IDvExpr snd)
        {

        }
    }
}
