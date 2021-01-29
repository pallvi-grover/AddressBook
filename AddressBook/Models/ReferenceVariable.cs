namespace AddressBook.Models
{
    public class ReferenceVariable
    {
        public string localhostURL = "https://localhost:44336";
        public string getAPIURL = "api/Contacts/0?userId=";
        public string[] allowedExtensions = new[] {
            ".Jpg", ".png", ".jpg", "jpeg", ".JPG"
        };
        public string imagePathURL = "~/ContactImages/";
        public string deleteContactAPIURL = "/api/Contacts/";
        public string getEmailIDURL = "/Home/getEmailId/";
        public string redirectToIndexPage = "/Home/Index?email=";
        public string defaultImage = "~/ContactImages/defaultImage.JPG";
        public string postContactNumberAPIURL = "/api/Contacts/";
        public string contentTypeHeader = "application/x-www-form-urlencoded";
        public string loginURLTokenCheck = "/api/values";
        public string registerAPIURL = "/api/Account/Register";
        public string redirectToLoginPage = "/Home/Login";
    }
}