using System;
using System.Text;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Tests.Results.Exceptions
{
    internal class UncheckedExceptionInWhenActionFound : Exception
    {
        internal Exception ExceptionFromWhenAction { get; }

        public override string Message => GetInfo();

        public override string StackTrace =>
            $"Stack trace of {nameof(UncheckedExceptionInWhenActionFound)} is omitted.";

        public UncheckedExceptionInWhenActionFound([NotNull] Exception exceptionFromWhenAction)
        {
            ExceptionFromWhenAction = exceptionFromWhenAction ??
                                       throw new ArgumentNullException(nameof(exceptionFromWhenAction));
        }

        public override string ToString() => GetInfo();

        private string GetInfo()
        {
            var builder = new StringBuilder();
            builder.AppendLine();
            builder.AppendLine("Exception was thrown in When action and no exception check was made:");
            builder.AppendLine();
            builder.AppendLine(ExceptionFromWhenAction.Message);
            builder.AppendLine(ExceptionFromWhenAction.StackTrace);
            return builder.ToString();
        }
    }
}