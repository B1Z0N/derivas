using System;
using System.Collections.Generic;
using System.Linq;

namespace Derivas.Expression
{
    /// <summary>
    /// Abstract class to define functionality and interface(in broader sense)
    /// to all nongeneric MultiArgOperator functionality
    /// </summary>
    internal abstract class MultiArgOperator : IDvExpr
    {
        #region abstract members specific to any operator

        public abstract string Sign { get; }
        public abstract int Priority { get; }
        public abstract Func<double[], double> OpFunc { get; }
        public abstract MultiArgOperator CreateInstance(params IDvExpr[] operands);  

        #endregion abstract members specific to any operator

        #region base class functionality

        public IEnumerable<IDvExpr> Operands { get => Operands_; }
        
        protected internal List<IDvExpr> Operands_;

        public MultiArgOperator(params IDvExpr[] lst) => Operands_ = new List<IDvExpr>(lst);

        #endregion base class functionality

        #region IDvExpr interface implementation

        public virtual double Calculate(DvNameVal concrete)
            => OpFunc(
                Operands_.Select(
                    el => el.Calculate(concrete)
                ).ToArray()
            );

        public virtual string Represent()
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
        
        #endregion IDvExpr interface implementation

        #region equals related stuff

        public virtual bool Equals(IDvExpr other)
        {
            var op = other as MultiArgOperator;
            return op != null && GetType() == other.GetType() && Sign == op.Sign &&
                Operands_.SequenceEqual(op.Operands_);
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