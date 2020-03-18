using System;
using System.Text;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Tests.Results.Exceptions
{
    internal class GivenActionFailed : Exception
    {
        internal Exception ExceptionFromGivenAction { get; }

        public override string Message => GetInfo();

        public override string StackTrace => $"Stack trace of {nameof(GivenActionFailed)} is omitted.";

        public GivenActionFailed([NotNull] Exception exceptionFromGivenAction)
        {
            ExceptionFromGivenAction = exceptionFromGivenAction ??
                                        throw new ArgumentNullException(nameof(exceptionFromGivenAction));
        }

        public override string ToString() => GetInfo();

        private string GetInfo()
        {
            var builder = new StringBuilder();
            builder.AppendLine();
            builder.AppendLine("Exception was thrown in Given action:");
            builder.AppendLine();
            builder.AppendLine(ExceptionFromGivenAction.Message);
            builder.AppendLine(ExceptionFromGivenAction.StackTrace);
            return builder.ToString();
        }
    }
}