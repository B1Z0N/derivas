using System;

namespace Derivas.Expression
{
    internal class UnaryOperator : OrderedOperator
    {
        public CloneableExpr Of { get => Operands_[0]; set => Operands_[0] = value; }
        protected Func<double, double> UnFunc { get; }

        public UnaryOperator(CloneableExpr of, string name, Func<double, double> op)
            : base(of) => (Sign, UnFunc) = (name, op);

        #region abstract class implementation

        public override string Represent() => $"{Sign}({Of.Represent()})";

        protected override CloneableExpr CreateFromClonable(params CloneableExpr[] operands)
        {
            var unOp = (operands.Length != 0 ? operands[0] : Of).CreateInstance();
            return new UnaryOperator(unOp, Sign, UnFunc);
        }

        public override string Sign { get; }
        public override int Priority { get; } = int.MaxValue;

        public override Func<double[], double> OpFunc => (args) => UnFunc(args[0]);

        #endregion abstract class implementation
    }
}