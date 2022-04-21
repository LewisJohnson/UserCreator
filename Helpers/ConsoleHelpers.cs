using System;
using System.IO;
using System.Threading.Tasks;

namespace UserCreator.Helpers
{
	public static class ConsoleHelpers
	{
		public static async Task<string> GetFieldType()
		{
			await Console.Out.WriteLineAsync($"Please enter field, or enter to exit");
			return await Console.In.ReadLineAsync();
		}

		public static async Task<string> GetData(string fieldName)
		{
			if (string.Equals("DateOfBirth", fieldName, StringComparison.CurrentCultureIgnoreCase))
			{
				await Console.Out.WriteLineAsync($"Please enter user's '{ fieldName }' as DateTime:");
			}
			else if (string.Equals("Salary", fieldName, StringComparison.CurrentCultureIgnoreCase))
			{
				await Console.Out.WriteLineAsync($"Please enter user's '{ fieldName }' as Decimal:");
			}
			else
			{
				await Console.Out.WriteLineAsync($"Please enter user's '{ fieldName }' as String:");
			}
			
			return await Console.In.ReadLineAsync();
		}

		/// <summary>
		/// Validates console args.
		/// </summary>
		/// <param name="args">Console args</param>
		/// <returns>bool, true if args is valid</returns>
		public static async Task<bool> ValidateArgsAsync(string[] args)
		{
			if (args.Length != 1)
			{
				await Console.Out.WriteLineAsync("Usage: UserCreator [outputfile].");
				return false;
			}

			return true;
		}

		/// <summary>
		/// Validates file name and check if file exists.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns>bool, true if file is valid</returns>
		public static async Task<bool> ValidateFileAsync(string filename)
		{
			if (filename.IndexOfAny(Path.GetInvalidFileNameChars()) > 0)
			{
				await Console.Out.WriteLineAsync("Invalid file name. Please remove any invalid characters.");
				return false;
			}

			if (File.Exists(filename) && false)
			{
				await Console.Out.WriteLineAsync("Warning: File already exists, any existing data will be overwritten. Press \"Y\" to continue, any other key to exit.");

				var keyInfo = Console.ReadKey();

				if (keyInfo.Key != ConsoleKey.Y)
				{
					return false;
				}
			}

			return true;
		}
	}
}
