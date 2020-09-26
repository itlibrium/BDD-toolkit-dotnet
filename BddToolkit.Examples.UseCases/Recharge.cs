using System;

namespace ITLIBRIUM.BddToolkit.Examples
{
    public class Recharge : Command
    {
        public Guid PrePaidAccountId { get; }
        public decimal Value { get; }
        public string CurrencyCode { get; }

        public Recharge(Guid prePaidAccountId, decimal value, string currencyCode)
        {
            PrePaidAccountId = prePaidAccountId;
            Value = value;
            CurrencyCode = currencyCode;
        }
    }
}