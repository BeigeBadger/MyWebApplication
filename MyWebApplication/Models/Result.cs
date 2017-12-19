namespace MyWebApplication.Models
{
	public class Result
	{
		private ResultTypeEnum ResultCode { get; set; } = ResultTypeEnum.Success;

		private string ResultMessage { get; set; } = "Success";

		/// <summary>
		/// Default success constructor
		/// </summary>
		public Result()
		{
		}

		public Result(ResultTypeEnum resultCode, string resultMessage)
		{
			ResultCode = resultCode;
			ResultMessage = resultMessage;
		}
	}
}