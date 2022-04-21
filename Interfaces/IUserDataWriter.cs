using System;
using System.IO;
using System.Threading.Tasks;

namespace UserCreator.Interfaces
{
	internal interface IUserDataWriter
	{
		static Task WriteDataToCsv(TextWriter streamWriter, string fieldName, object data)
		{
			throw new NotImplementedException();
		}

		static Task WriteUserDataToFile<TDataType>(StreamWriter streamWriter, string fieldName)
		{
			throw new NotImplementedException();
		}

		static Task WriteUserData(StreamWriter outputFileWriter, string fieldType)
		{
			throw new NotImplementedException();
		}
		
	}
}
