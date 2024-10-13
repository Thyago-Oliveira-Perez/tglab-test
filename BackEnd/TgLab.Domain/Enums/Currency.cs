namespace TgLab.Domain.Enums
{
    public class Currency
    {
        private Currency(string value) { Value = value; }

        public string Value { get; private set; }

        public static Currency USD { get { return new("USD"); } }
        public static Currency BRL { get { return new("BRL"); } }
        public static Currency EUR { get { return new("EUR"); } }

        public override string ToString()
        {
            return Value;
        }
    }
}
