using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace UserCreator
{
    public class UserDataParser<T> : IUserDataEnterer
    {
        public async Task WriteDataToCsv(TextWriter textWriter, string fieldName, object data)
        {
            await textWriter.WriteLineAsync($"{ IdGenerator.GetNextId() },{fieldName},{data}");
        }

        public bool TryConvertData(string input, out T data)
        {
			if (input.Contains(",", StringComparison.CurrentCultureIgnoreCase))
			{
				Console.Error.WriteLine($"Param 'input' must not contain a comma.");
				data = default;
                return false;
			}

            if (input.Length > 255)
            {
                Console.Error.WriteLineAsync($"Param 'input' must be less than 255 characters");
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