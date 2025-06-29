using System;


namespace Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base("User not found") { }

        public UserNotFoundException(string message) : base($"user not found: {message}") { }

        public UserNotFoundException(int UserID) : base ($"user with ID {UserID} does not exist") { }

        public UserNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
