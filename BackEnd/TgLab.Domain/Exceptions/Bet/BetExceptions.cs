namespace TgLab.Domain.Exceptions.Bet
{
    public class AllReadyCancelled : ArgumentException
    {
        public AllReadyCancelled(string message = "Bet already cancelled") : base(message) { }
    }
}
