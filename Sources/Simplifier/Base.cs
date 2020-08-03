using System;
using System.Collections.Generic;
using System.Text;

using Derivas.Exception;
using Derivas.Expression;

namespace Derivas.Simplifier
{
    public interface IDvSimplifier
    {
        IDvExpr Simplify(IDvExpr expr);
    }
}
