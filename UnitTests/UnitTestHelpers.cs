using System;
using System.IO;

namespace UnitTests
{
	public static class UnitTestHelpers
	{
		public static string GetTempFile()
		{
			var path = Path.GetTempPath();
			var fileName = Path.ChangeExtension(Guid.NewGuid().ToString(), ".txt");
			return Path.Combine(path, fileName);
		}
	}
}
