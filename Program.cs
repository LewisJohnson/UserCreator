using System;
using System.IO;
using System.Threading.Tasks;

namespace UserCreator
{
	class Program
	{
		static async Task<int> Main(string[] args)
		{
			string fieldType;
			if (args.Length != 1)
			{
				await Console.Out.WriteLineAsync("Usage: UserCreator [outputfile].");
				return 1;
			}

			string filename = args[0];

			if (filename.IndexOfAny(Path.GetInvalidFileNameChars()) > 0)
			{
				await Console.Out.WriteLineAsync("Invalid file name. Please remove any invalid characters.");
				return 1;
			}

			if (File.Exists(filename) && false)
			{
				await Console.Out.WriteLineAsync("Warning: File already exists, any existing data will be overwritten. Press \"Y\" to continue, any other key to exit.");

				var keyInfo = Console.ReadKey();

				if (keyInfo.Key != ConsoleKey.Y)
				{
					return 1;
				}
			}

			using (var outputFile = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read, 4096, FileOptions.Asynchronous | FileOptions.WriteThrough))
			{
				// Empty file, as the IDs will be messed up
				outputFile.SetLength(0);

				using (var outputFileWriter = new StreamWriter(outputFile))
				{
					while (!string.IsNullOrEmpty(fieldType = await GetFieldType()))
					{
						if (fieldType.Contains(",", StringComparison.CurrentCultureIgnoreCase))
						{
							await Console.Error.WriteLineAsync($"Param 'fieldType' must not contain a comma");
							continue;
						}

						if (fieldType.Length > 255)
						{
							await Console.Error.WriteLineAsync($"Param 'fieldType' must be less than 255 characters");
							continue;
						}

						if (string.Equals("DateOfBirth", fieldType, StringComparison.CurrentCultureIgnoreCase))
						{
							await WriteUserDataToFile<DateTime>(fieldType, outputFileWriter);
						}
						else if (string.Equals("Salary", fieldType, StringComparison.CurrentCultureIgnoreCase))
						{
							await WriteUserDataToFile<decimal>(fieldType, outputFileWriter);
						}
						else
						{
							await WriteUserDataToFile<string>(fieldType, outputFileWriter);
						}

						await Console.Out.WriteLineAsync($"============");

						// Write data to file as we go
						await outputFileWriter.FlushAsync();
						await outputFile.FlushAsync();
					}
				}
			}
			return 0;
		}

		private static async Task WriteUserDataToFile<TDataType>(string fieldName, StreamWriter streamWriter)
		{
			var userDataParser = new UserDataParser<TDataType>();
			var dataAsString = await GetData<TDataType>(fieldName);

			if (userDataParser.TryConvertData(dataAsString, out var data))
			{
				await userDataParser.WriteDataToCsv(streamWriter, fieldName, data);
			}
		}

		private static async Task<string> GetFieldType()
		{
			await Console.Out.WriteLineAsync($"Please enter field, or enter to exit");
			return await Console.In.ReadLineAsync();
		}

		private static async Task<string> GetData<TDataType>(string fieldName)
		{
			await Console.Out.WriteLineAsync($"Please enter user's '{fieldName}' as a {typeof(TDataType).Name}:");
			return await Console.In.ReadLineAsync();
		}

	}

}
