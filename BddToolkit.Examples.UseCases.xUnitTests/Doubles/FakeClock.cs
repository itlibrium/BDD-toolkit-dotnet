using System;

namespace ITLIBRIUM.BddToolkit.Examples.Doubles
{
    public class FakeClock : Clock
    {
        public DateTime Now { get; set; }
    }
}