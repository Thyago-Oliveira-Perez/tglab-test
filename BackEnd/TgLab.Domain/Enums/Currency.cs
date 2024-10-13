namespace TgLab.Domain.Enums
{
    public class Currency
    {
        private Currency(string value) { Value = value; }

        public string Value { get; private set; }

        public static Currency USD { get { return new("Dolar"); } }
        public static Currency BRL { get { return new("Real"); } }
        public static Currency EUR { get { return new("Euro"); } }

        public override string ToString()
        {
            return Value;
        }
    }
}
