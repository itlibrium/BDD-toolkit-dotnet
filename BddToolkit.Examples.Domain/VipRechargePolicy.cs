namespace ITLIBRIUM.BddToolkit.Examples
{
    public class VipRechargePolicy : RechargePolicy
    {
        public Money CalculateAmount(Money value) => value * 1.2m;
    }
}