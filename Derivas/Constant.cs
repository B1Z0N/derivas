using System;
using System.Collections.Generic;
using System.Text;

using ExpressionTree = System.Linq.Expressions.Expression;
using Derivas.Expression;


namespace Derivas.Constant
{
    public interface IDvConstantsProvider<TNum>
    {
        /// <summary>Addition neutral element</summary>
        DvConstant<TNum> Zero { get; }
        /// <summary>Multiplicative neutral element</summary>
        DvConstant<TNum> One { get; }
    }

    public class DvDefaultConstantsProvider<TNum> : IDvConstantsProvider<TNum>
    {
        public DvConstant<TNum> Zero => GetConstant(0);
        public DvConstant<TNum> One => GetConstant(1);

        private DvConstant<TNum> GetConstant(int constant)
        {
            var integerConstant = ExpressionTree.Constant(constant, typeof(int));
            var genericTypeConstant = ExpressionTree.Convert(integerConstant, typeof(TNum));
            var lambda = ExpressionTree.Lambda<Func<TNum>>(genericTypeConstant);
            return new DvConstant<TNum>(lambda.Compile().Invoke());
        }
    }
}
