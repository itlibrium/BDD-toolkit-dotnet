using System;
using System.Collections.Immutable;
using System.Text;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Execution
{
    public class AggregateAssertException : Exception
    {
        [PublicAPI]
        public ImmutableArray<Exception> Exceptions { get; }

        public override string Message => GetAggregatedInfo(e => e.Message);

        internal AggregateAssertException(ImmutableArray<Exception> exceptions)
        {
            if (exceptions.Length == 0)
                throw new ArgumentException("Exceptions collection can not be empty.", nameof(exceptions));
            Exceptions = exceptions;
        }

        public override string ToString() => GetAggregatedInfo(e => e.ToString());

        private string GetAggregatedInfo(Func<Exception, string> infoFactory)
        {
            var exceptionsCount = Exceptions.Length;
            if (exceptionsCount == 1)
                return infoFactory(Exceptions[0]);

            var builder = new StringBuilder();
            builder.AppendLine("More than one assert failed.");
            builder.AppendLine();
            for (var i = 0; i < exceptionsCount; i++)
            {
                var exception = Exceptions[i];

                builder.Append($"{(i + 1).ToString()}) ");

                var info = infoFactory(exception);
                builder.AppendLine(info);
                if (!info.EndsWith(Environment.NewLine) && i != exceptionsCount - 1)
                    builder.AppendLine();
            }
            return builder.ToString();
        }
    }
}