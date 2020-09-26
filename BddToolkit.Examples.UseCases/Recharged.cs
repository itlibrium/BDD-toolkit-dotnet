using System;

namespace ITLIBRIUM.BddToolkit.Examples
{
    public class Recharged
    {
        public Guid PrePaidAccountId { get; }
        public decimal Value { get; }
        public string CurrencyCode { get; }

        public Recharged(Guid prePaidAccountId, decimal value, string currencyCode)
        {
            PrePaidAccountId = prePaidAccountId;
            Value = value;
            CurrencyCode = currencyCode;
        }
    }
}