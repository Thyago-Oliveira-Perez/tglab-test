namespace TgLab.Domain.Enums
{
    public class TransactionType
    {
        private TransactionType(string value) { Value = value; }

        public string Value { get; private set; }

        public static TransactionType WIN { get { return new("Win"); } }
        public static TransactionType LOSS { get { return new("Loss"); } }

        public override string ToString()
        {
            return Value;
        }
    }
}
