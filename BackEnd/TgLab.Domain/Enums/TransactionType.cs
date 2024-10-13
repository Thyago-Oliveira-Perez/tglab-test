namespace TgLab.Domain.Enums
{
    public class TransactionType
    {
        private TransactionType(string value) { Value = value; }

        public string Value { get; private set; }

        // Credits
        public static TransactionType WIN_BET { get { return new("Win"); } }
        public static TransactionType DEPOSIT { get { return new("Deposit"); } }
        public static TransactionType RECHARGE_BONUS { get { return new("RechargeBonus"); } }
        public static TransactionType REFUND { get { return new("Refund"); } }

        // Debts
        public static TransactionType BET { get { return new("Bet"); } }
        public static TransactionType WITHDRAWAL { get { return new("Withdrawal"); } }
        public static TransactionType SERVICE_FEE { get { return new("ServiceFee"); } }

        public override string ToString()
        {
            return Value;
        }
    }
}
