namespace awl_raumreservierung
{

	public class ReturnModel
	{
		public int status { get; set; } = 200;
		public string statusMessage { get; set; } = "success";
		public string message { get; set; }
	}

}