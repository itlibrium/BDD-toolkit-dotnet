using System;
using System.Collections.Immutable;
using System.Text;

namespace ITLIBRIUM.BddToolkit.Execution
{
    public class AggregateAssertException : Exception
    {
        private readonly ImmutableArray<Exception> _exceptions;

        public override string Message => GetAggregatedInfo(e => e.Message);

        internal AggregateAssertException(ImmutableArray<Exception> exceptions)
        {
            if (exceptions.Length == 0)
                throw new ArgumentException("Exceptions collection can not be empty.", nameof(exceptions));
            _exceptions = exceptions;
        }

        public override string ToString() => GetAggregatedInfo(e => e.ToString());

        private string GetAggregatedInfo(Func<Exception, string> infoFactory)
        {
            var exceptionsCount = _exceptions.Length;
            if (exceptionsCount == 1)
                return infoFactory(_exceptions[0]);

            var builder = new StringBuilder();
            builder.AppendLine("More than one assert failed.");
            builder.AppendLine();
            for (var i = 0; i < exceptionsCount; i++)
            {
                var exception = _exceptions[i];

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