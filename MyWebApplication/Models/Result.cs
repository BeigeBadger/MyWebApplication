namespace MyWebApplication.Models
{
	public class Result
	{
		public ResultTypeEnum ResultCode { get; private set; } = ResultTypeEnum.Success;

		public string ResultMessage { get; private set; } = "Success";

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