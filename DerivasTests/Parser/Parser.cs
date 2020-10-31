using Derivas.Expression;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

using static Derivas.DvOps;

namespace DerivasTests.Parser
{
    class Parser
    {

        private static Dictionary<string, double> ConcreteValues { get; } = new Dictionary<string, double>
        {
            ["x"] = 5.56,
            ["y"] = 1.0202010,
            ["multiword"] = 10,
        };

        protected static string ReplaceWithConcreteValues(string expr)
        {
            foreach (var pair in ConcreteValues)
            {
                expr = expr.Replace(pair.Key, pair.Value.ToString());
            }

            return expr;
        }

        protected static void BackendCompare(string input, bool compareRepresentations = true)
        {
            if (compareRepresentations)
                Assert.AreEqual(input, Parse(input).Represent());

            Assert.AreEqual(
                Parse(ReplaceWithConcreteValues(input)).Calculate(Dict.Get()),
                Parse(input).Calculate(ConcreteValues),
                1e-10
            );
        }

        [Test, Sequential]
        public void SingleTrivialTest([Values("31", "x", "y", "5.45")] string input)
        {
            BackendCompare(input);
        }

        [Test, Sequential]
        public void SingleOperatorTest(
            [Values("cos(3)", "sin(x)", "y + x", "3 * x", "y - 632.31", "x ^ y")] string input)
        {
            BackendCompare(input);
        }

        [Test, Sequential]
        public void MultiLevelTest(
            [Values(
                "5 + 4 * (3 - cos(x)) + sin(1 ^ y)",
                "5 + 4 * (3 - cos(x)) + sin(log(2, y))",
                "3 * (3 - cos(x))",
                "3 * (4 * (5))")
            ] string input,
            [Values(true, false, true, false)] bool compareRepresentations
            )
        {
            BackendCompare(input, compareRepresentations);
        }
    }
}
