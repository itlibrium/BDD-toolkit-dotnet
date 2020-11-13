using System;

namespace ITLIBRIUM.BddToolkit.Examples
{
    public class PrePaidAccountOperation
    {
        public DateTime OccuredOn { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
    }
}