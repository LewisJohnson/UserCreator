using System;

namespace UserCreator.Interfaces
{
	internal interface IIdGenerator
	{
		static int nextId = 0;

		static int GetNextId()
		{
			throw new NotImplementedException();
		}

		static void ResetId()
		{
			throw new NotImplementedException();
		}
	}
}
