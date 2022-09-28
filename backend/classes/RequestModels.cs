using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace awl_raumreservierung
{
	public class ReturnModel
	{
        public ReturnModel() : this (new StatusCodeResult(200)) {
		  }
        public ReturnModel(StatusCodeResult statusCode) {
            status = statusCode.StatusCode;
				if (status is < 300 and > 199) {
					statusMessage = "success";
				} else {
					statusMessage = ReasonPhrases.GetReasonPhrase(status);
				}

				message = string.Empty;
        }

		public int status { get; set; }
		public string statusMessage { get; set; }
		public string message { get; set; }
	}

    public class CreateUserModel {

        public string username {get; set;}
        public string firstname {get; set;}
        public string lastname {get; set;} 
        public UserRole rolle {get; set;}
        public string password {get; set;}
    }

}