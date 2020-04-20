using System;
using System.Globalization;

namespace ITLIBRIUM.BddToolkit.Examples
{
    public readonly struct Money : IEquatable<Money>
    {
        public decimal Value { get; }
        public Currency Currency { get; }
        public bool IsNegative => Value < 0;

        public static Money Of(decimal value, Currency currency) => new Money(value, currency);

        private Money(decimal value, Currency currency)
        {
            Value = value;
            Currency = currency;
        }
        
        public static Money operator +(Money x, Money y) => Calculate(x, y, (a, b) => a + b);
        public static Money operator -(Money x, Money y) => Calculate(x, y, (a, b) => a - b);

        private static Money Calculate(Money x, Money y, Func<decimal, decimal, decimal> calculate)
        {
            CheckCurrencies(x, y);
            return new Money(calculate(x.Value, y.Value), x.Currency);
        }
        
        public static bool operator ==(Money x, Money y) => x.Equals(y);
        public static bool operator !=(Money x, Money y) => !x.Equals(y);
        public static bool operator >(Money x, Money y) => Compare(x, y, (a, b) => a > b);
        public static bool operator <(Money x, Money y) => Compare(x, y, (a, b) => a < b);
        public static bool operator >=(Money x, Money y) => Compare(x, y, (a, b) => a >= b);
        public static bool operator <=(Money x, Money y) => Compare(x, y, (a, b) => a <= b);
        
        private static bool Compare(Money x, Money y, Func<decimal, decimal, bool> compare)
        {
            CheckCurrencies(x, y);
            return compare(x.Value, y.Value);
        }
        
        private static void CheckCurrencies(Money x, Money y)
        {
            if (x.Currency != y.Currency) throw new DomainException();
        }

        //...
        
        public bool Equals(Money other) => (Value, Currency).Equals((other.Value, other.Currency));
        public override bool Equals(object obj) => obj is Money other && Equals(other);
        public override int GetHashCode() => (Value, Currency).GetHashCode();

        public override string ToString() => $"{Value.ToString(CultureInfo.CurrentCulture)} {Currency.ToString()}";
    }
}