using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITLibrium.Bdd.Scenarios
{
    public class AggregateAssertException : Exception
    {
        private readonly Exception[] _exceptions;

        public AggregateAssertException(IEnumerable<Exception> exceptions)
        {
            _exceptions = exceptions.ToArray();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (Exception exception in _exceptions)
                builder.AppendLine(exception.ToString());

            return builder.ToString();
        }
    }
}