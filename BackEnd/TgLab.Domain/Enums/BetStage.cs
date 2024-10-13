namespace TgLab.Domain.Enums
{
    public class BetStage
    {
        private BetStage(string value) { Value = value; }

        public string Value { get; private set; }

        public static BetStage SENT { get { return new("Sent"); } }
        public static BetStage EXECUTED { get { return new("Executed"); } }
        public static BetStage CANCELLED { get { return new("Cancelled"); } }

        public override string ToString()
        {
            return Value;
        }
    }
}
