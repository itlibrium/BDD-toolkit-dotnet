using System;

namespace ITLIBRIUM.BddToolkit.Examples
{
    public class Recharged
    {
        public Guid PrePaidAccountId { get; }
        public DateTime OccuredOn { get; }
        public decimal Value { get; }
        public string CurrencyCode { get; }

        public Recharged(Guid prePaidAccountId, DateTime occuredOn, decimal value, string currencyCode)
        {
            PrePaidAccountId = prePaidAccountId;
            OccuredOn = occuredOn;
            Value = value;
            CurrencyCode = currencyCode;
        }
    }
}