using System;
using System.Collections.Generic;
using System.Linq;

using Derivas.Expression;
using Derivas.Exception;

namespace Derivas.Expression
{
    internal abstract class OrderedOperator : Operator
    {
        #region base class functionality

        public override IEnumerable<CloneableExpr> Operands { get => Operands_; }
        
        protected internal List<CloneableExpr> Operands_;

        public OrderedOperator(params CloneableExpr[] lst) => Operands_ = new List<CloneableExpr>(lst);

        #endregion base class functionality
    }
}