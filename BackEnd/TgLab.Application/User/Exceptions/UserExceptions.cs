namespace TgLab.Application.User.Exceptions
{
    public class Under18Exception : ArgumentException
    {
        public Under18Exception(string message = "User is under 18."): base(message) { }
    }

    public class DuplicatedEmail : ArgumentException
    {
        public DuplicatedEmail(string message = "Already exists a user with this email address.") : base(message) { }
    }
}
