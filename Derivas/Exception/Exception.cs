﻿using Derivas.Expression;
using System;

namespace Derivas.Exception
{
    /// <summary>
    /// Base exception class for the project
    /// </summary>
    public class DvBaseException : System.Exception
    {
        public DvBaseException(string msg) : base(msg)
        {
        }
    }

    #region expression exceptions

    /// <summary>
    /// Symbol value not supplied during calculation
    /// </summary>
    public class DvExpressionMismatchException : DvBaseException
    {
        public Type WrongType;

        public DvExpressionMismatchException(Type t)
            : base($"You can't pass in {t} type, use int, string or IDvExpr")
        {
            WrongType = t;
        }
    }

    /// <summary>
    /// No derivative rule for this expression
    /// </summary>
    public class DvDerivativeMismatchException : DvBaseException
    {
        public Type WrongType;
        public string Other;

        public DvDerivativeMismatchException(IDvExpr op, string other = null) : base(
            $"There is no DvDerivative handler for type {op.GetType()}" +
            other == null ? "." : $" and this: '{other}'.")
        {
            (WrongType, Other) = (op.GetType(), other);
        }
    }

    /// <summary>
    /// Symbol value not supplied during calculation
    /// </summary>
    public class DvSymbolMismatchException : DvBaseException
    {
        public string ShouldBe;

        public DvSymbolMismatchException(string shouldBe)
            : base($"Value of '{shouldBe}' Symbol is not included in the dictionary")
            => ShouldBe = shouldBe;
    }

    /// <summary>
    /// Wrong number of arguments passed
    /// </summary>
    public class DvNotEnoughArgumentsException : DvBaseException
    {
        public int NOrMore;

        public DvNotEnoughArgumentsException(int nOrMore)
            : base($"Not enough arguments passed, accepts {nOrMore} or more")
            => NOrMore = nOrMore;
    }

    #endregion

    #region parser exceptions

    /// <summary>
    /// Base class for parser exceptions
    /// </summary>
    public class DvParserException : DvBaseException
    {
        public DvParserException(string msg) : base(msg)
        {
        }
    }

    #endregion
}