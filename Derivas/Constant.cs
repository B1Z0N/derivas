using System;
using System.Collections.Generic;
using System.Text;

using exp = System.Linq.Expressions.Expression;
using Derivas.Expression;


namespace Derivas.Constant
{
    /// <summary>Interface to common numeric constants</summary>
    public interface IDvConstantsProvider<TNum>
    {
        /// <summary>Addition neutral element</summary>
        DvConstant<TNum> Zero { get; }
        /// <summary>Multiplicative neutral element</summary>
        DvConstant<TNum> One { get; }
    }

    /// <summary>
    /// Implementation of <see cref="IDvConstantsProvider{TNum}">constants</see> with any custom type
    /// </summary>
    public class DvDefaultConstantsProvider<TNum> : IDvConstantsProvider<TNum>
    {
        public DvConstant<TNum> Zero => GetConstant(0);
        public DvConstant<TNum> One => GetConstant(1);

        private DvConstant<TNum> GetConstant(int constant)
        {
            var integerConstant = exp.Constant(constant, typeof(int));
            var genericTypeConstant = exp.Convert(integerConstant, typeof(TNum));
            var lambda = exp.Lambda<Func<TNum>>(genericTypeConstant);
            return new DvConstant<TNum>(lambda.Compile().Invoke());
        }
    }
}
