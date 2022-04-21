using System;
using UserCreator.Interfaces;

namespace UserCreator.Implementations
{
	public class UserDataParser<T> : IUserDataParser<T>
	{
		public bool TryConvertData(string input, out T data)
		{
			var validator = new UserDataValidator();

			var inputValid = validator.Validate("FieldData", input).Result;

			if (!inputValid)
			{
				data = default;
				return false;
			}

			try
			{
				var parseMethod = typeof(T).GetMethod("Parse", 0, new[] { typeof(string) });
				if (parseMethod != null)
				{
					data = (T)parseMethod.Invoke(null, new[] { input });
					return true;
				}
				else
				{
					data = (T)Convert.ChangeType(input, typeof(T));
				}
				return true;
			}
			catch (Exception e)
			{
				Console.Error.WriteLine(e.GetBaseException().ToString());
				Console.Error.WriteLine($"Could not convert {input} to {typeof(T).Name}!");
				data = default;
				return false;
			}
		}
	}
}