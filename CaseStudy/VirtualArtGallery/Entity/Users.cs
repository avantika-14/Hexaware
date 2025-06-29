using System;

namespace Entity
{
    public class Users
    {
        private int uUserID;
        private string uUserName;
        private string uPassword;
        private string uEmail;
        private string uFirstName;
        private string uLastName;
        private DateTime uDateOfBirth;
        private byte[] uProfilePicture;

        // default constructors
        public Users() { }

        // parameterised constructor
        public Users(int userID, string username, string password, string email, string firstName, string lastName, DateTime dateOfBirth, byte[] profilePicture)
        {
            this.uUserID = userID;
            this.uUserName = username;
            this.uPassword = password;
            this.uEmail = email;
            this.uFirstName = firstName;
            this.uLastName = lastName;
            this.uDateOfBirth = dateOfBirth;
            this.uProfilePicture = profilePicture;
        }

        public int UserID { get { return uUserID; } set { uUserID = value; } }

        public string UserName { get { return uUserName; } set { this.uUserName = value; } }

        public string Password { get { return uPassword; } set { uPassword = value; } }

        public string Email { get { return uEmail; } set { uEmail = value; } }

        public string FirstName { get {return uFirstName; } set {uFirstName = value; } }

        public string LastName { get { return uLastName; } set { uLastName = value; } }

        public DateTime DateOfBirth { get { return uDateOfBirth; } set {uDateOfBirth= value; } }

        public byte[] ProfilePicture { get { return uProfilePicture; } set {uProfilePicture = value; } }
    }
}
