using System;

namespace ITLIBRIUM.BddToolkit.Examples
{
    public interface Clock
    {
        DateTime Now { get; }
    }
}