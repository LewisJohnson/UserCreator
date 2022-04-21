using System;
using System.IO;
using System.Threading.Tasks;
using UserCreator.Helpers;
using UserCreator.Implementations;

namespace UserCreator
{
	class Program
	{
		static async Task<int> Main(string[] args)
		{
			string fieldName;
			bool isArgsValid = await ConsoleHelpers.ValidateArgsAsync(args);

			if (!isArgsValid)
			{
				return 1;
			}

			string filename = args[0];
			bool isFileValid = await ConsoleHelpers.ValidateFileAsync(filename);

			if (!isFileValid)
			{
				return 1;
			}

			using (var outputFile = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read, 4096, FileOptions.Asynchronous | FileOptions.WriteThrough))
			{
				// Empty the file, as the IDs will be messed up
				outputFile.SetLength(0);

				using (var outputFileWriter = new StreamWriter(outputFile))
				{
					UserDataValidator validator = new UserDataValidator();

					while (!string.IsNullOrEmpty(fieldName = await ConsoleHelpers.GetFieldType()))
					{
						bool isFieldTypeValid = await validator.Validate("FieldName", fieldName);

						if (!isFieldTypeValid)
						{
							continue;
						}

						string dataAsString = await ConsoleHelpers.GetData(fieldName);
						await UserDataWriter.WriteUserData(outputFileWriter, fieldName, dataAsString);

						await Console.Out.WriteLineAsync($"============");

						// Write data to file as we go
						await outputFileWriter.FlushAsync();
						await outputFile.FlushAsync();
					}
				}
			}

			// Program successfully complete
			return 0;
		}
	}
}
