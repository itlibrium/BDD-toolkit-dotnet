using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITLIBRIUM.BddToolkit.Scenarios
{
    public class AggregateAssertException : Exception
    {
        private readonly IReadOnlyList<Exception> _exceptions;

        public override string Message => GetAggregatedInfo(e => e.Message);
        
        public AggregateAssertException(IEnumerable<Exception> exceptions)
        {
            if (exceptions == null) throw new ArgumentNullException(nameof(exceptions));
            
            IReadOnlyList<Exception> exceptionsList = exceptions.ToList();
            if(exceptionsList.Count == 0)
                throw new ArgumentException("Exceptions collection can not be empty.", nameof(exceptions));
            
            _exceptions = exceptionsList;
        }

        public override string ToString()
        {
            return GetAggregatedInfo(e => e.ToString());
        }

        private string GetAggregatedInfo(Func<Exception, string> infoFactory)
        {
            var exceptionsCount = _exceptions.Count;
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