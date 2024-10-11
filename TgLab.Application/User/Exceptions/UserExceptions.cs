namespace TgLab.Application.User.Exceptions
{
    public class Under18Exception : ArgumentException
    {
        public Under18Exception(string message = "User is under 18."): base(message) { }
    }
}
