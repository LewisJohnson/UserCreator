using System;
using System.IO;
using System.Threading.Tasks;
using UserCreator.Interfaces;

namespace UserCreator.Implementations
{
	public class UserDataWriter : IUserDataWriter
	{
		private static async Task WriteDataToCsv(TextWriter textWriter, string fieldName, object fieldData)
		{
			await textWriter.WriteLineAsync($"{ IdGenerator.GetNextId() },{ fieldName },{ fieldData }");
		}

		private static async Task WriteUserDataToFile<TDataType>(StreamWriter streamWriter, string fieldName, string dataAsString)
		{
			UserDataParser<TDataType> userDataParser = new UserDataParser<TDataType>();


			if (userDataParser.TryConvertData(dataAsString, out var data))
			{
				await WriteDataToCsv(streamWriter, fieldName, data);
			}
		}

		public async static Task WriteUserData(StreamWriter outputFileWriter, string fieldName, string dataAsString)
		{
			if (string.Equals("DateOfBirth", fieldName, StringComparison.CurrentCultureIgnoreCase))
			{
				await WriteUserDataToFile<DateTime>(outputFileWriter, fieldName, dataAsString);
			}
			else if (string.Equals("Salary", fieldName, StringComparison.CurrentCultureIgnoreCase))
			{
				await WriteUserDataToFile<decimal>(outputFileWriter, fieldName, dataAsString);
			}
			else
			{
				await WriteUserDataToFile<string>(outputFileWriter, fieldName, dataAsString);
			}
		}
	}
}
