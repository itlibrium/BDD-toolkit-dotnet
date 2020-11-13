using System;
using System.Collections.Generic;

namespace ITLIBRIUM.BddToolkit.Examples
{
    public class PrePaidAccountHistory
    {
        public Guid AccountId { get; set; }
        public List<PrePaidAccountOperation> Operations { get; set; } = new List<PrePaidAccountOperation>();
    }
}