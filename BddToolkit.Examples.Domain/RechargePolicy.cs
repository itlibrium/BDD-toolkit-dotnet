namespace ITLIBRIUM.BddToolkit.Examples
{
    public interface RechargePolicy
    {
        Money CalculateAmount(Money value);
    }
}