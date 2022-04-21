using System;
using System.Threading.Tasks;

namespace UserCreator.Implementations
{
	public class UserDataValidator
	{
		public async Task<bool> Validate(string paramName, string value)
		{
			if (value.Contains(",", StringComparison.CurrentCultureIgnoreCase))
			{
				await Console.Error.WriteLineAsync($"Param '{ paramName }' must not contain a comma.");
				return false;
			}

			if (value.Length > 255)
			{
				await Console.Error.WriteLineAsync($"Param '{ paramName }' must be less than 255 characters.");
				return false;
			}

			return true;
		}
	}
}
