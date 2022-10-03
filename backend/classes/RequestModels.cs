using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace awl_raumreservierung
{
    public class ReturnModel
    {
        public ReturnModel() : this(new StatusCodeResult(200)) { }
        public ReturnModel(StatusCodeResult statusCode)
        {
            Status = statusCode.StatusCode;
            if (Status is < 300 and > 199)
            {
                StatusMessage = "success";
            }
            else
            {
                StatusMessage = ReasonPhrases.GetReasonPhrase(Status);
            }

            Message = string.Empty;
        }

        public int Status { get; set; }
        public string StatusMessage { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }
    }

    public class CreateUserModel
    {
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public UserRole Role { get; set; }
        public string? Password { get; set; }
    }

    public class LoginUserModel
    {
        public string Username {get; set;}
        public string Password {get; set;}

        public LoginUserModel(string username, string password){
            Username = username;
            Password = password;
        }
    }

    public class UpdateUserModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public UserRole Role { get; set; }
    }


    public class CreateBookingModel
    {
        public int RoomID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

	public class CreateRoomModel
	{
		public long Id { get; set; }
		public string? Number { get; set; }
		public string? Name { get; set; }
		public bool Active { get; set; }
	}


}