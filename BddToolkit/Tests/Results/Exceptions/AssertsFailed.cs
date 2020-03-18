using System;
using System.Collections.Immutable;
using System.Text;

namespace ITLIBRIUM.BddToolkit.Tests.Results.Exceptions
{
    internal class AssertsFailed : Exception
    {
        internal ImmutableArray<Exception> FailedAssertions { get; }

        public override string Message => GetAggregatedInfo();

        public override string StackTrace => $"Stack trace of {nameof(AssertsFailed)} is omitted.";

        internal AssertsFailed(ImmutableArray<Exception> failedAssertions)
        {
            if (failedAssertions.Length == 0)
                throw new ArgumentException("Exceptions collection can not be empty.", nameof(failedAssertions));
            FailedAssertions = failedAssertions;
        }

        public override string ToString() => GetAggregatedInfo();

        private string GetAggregatedInfo()
        {
            var builder = new StringBuilder();
            builder.AppendLine();
            builder.AppendLine("One or more assert failed:");
            builder.AppendLine();
            for (var i = 0; i < FailedAssertions.Length; i++)
            {
                var failedAssertion = FailedAssertions[i];
                builder.Append($"{(i + 1).ToString()}) ");
                builder.AppendLine(failedAssertion.Message);
                if (!failedAssertion.Message.EndsWith(Environment.NewLine) && i != FailedAssertions.Length - 1)
                    builder.AppendLine();
            }
            return builder.ToString();
        }
    }
}