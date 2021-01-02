using System;
using System.Collections.Immutable;
using System.Text;

namespace ITLIBRIUM.BddToolkit.Tests.Results.Exceptions
{
    internal class ExceptionChecksFailed : Exception
    {
        internal Exception ExceptionFromWhenAction { get; }

        internal ImmutableArray<Exception> FailedExceptionChecks { get; }

        public override string Message => GetAggregatedInfo();
        
        public override string StackTrace => $"Stack trace of {nameof(ExceptionChecksFailed)} is omitted.";

        internal ExceptionChecksFailed(ImmutableArray<Exception> failedExceptionChecks)
        {
            if (failedExceptionChecks.Length == 0)
                throw new ArgumentException("Exceptions collection can not be empty.", nameof(failedExceptionChecks));
            FailedExceptionChecks = failedExceptionChecks;
        }
        
        internal ExceptionChecksFailed(Exception exceptionFromWhenAction,
            ImmutableArray<Exception> failedExceptionChecks)
        {
            ExceptionFromWhenAction = exceptionFromWhenAction ??
                throw new ArgumentNullException(nameof(exceptionFromWhenAction));
            if (failedExceptionChecks.Length == 0)
                throw new ArgumentException("Exceptions collection can not be empty.", nameof(failedExceptionChecks));
            FailedExceptionChecks = failedExceptionChecks;
        }

        public override string ToString() => GetAggregatedInfo();

        private string GetAggregatedInfo()
        {
            var builder = new StringBuilder();
            builder.AppendLine();
            if (ExceptionFromWhenAction is null)
            {
                builder.AppendLine("No exception was thrown in When action.");
            }
            else
            {
                builder.AppendLine("Unexpected exception was thrown in When action:");
                builder.AppendLine();
                builder.AppendLine(ExceptionFromWhenAction.Message);
                builder.AppendLine(ExceptionFromWhenAction.StackTrace);
            }
            builder.AppendLine();
            builder.AppendLine();
            builder.AppendLine("Failed exception checks:");
            builder.AppendLine();
            for (var i = 0; i < FailedExceptionChecks.Length; i++)
            {
                var failedCheck = FailedExceptionChecks[i];
                builder.Append($"{(i + 1).ToString()}) ");
                builder.AppendLine(failedCheck.Message);
                if (!failedCheck.Message.EndsWith(Environment.NewLine) && i != FailedExceptionChecks.Length - 1)
                    builder.AppendLine();
            }
            return builder.ToString();
        }
    }
}