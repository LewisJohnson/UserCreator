using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using UserCreator.Implementations;

namespace UnitTests
{
	[TestClass]
	public class UserDataValidatorUnitTests
	{
		[TestMethod]
		public async Task Should_Error_When_Value_Length_Over_Max_Length()
		{
			// Arrange
			UserDataValidator validator = new UserDataValidator();
			string stringOver255Chars = new string('a', 256);

			// Act
			bool res = await validator.Validate("testParam", stringOver255Chars);

			// Assert
			Assert.IsFalse(res);
		}

		[TestMethod]
		public async Task Should_Not_Error_When_Value_Length_Below_Max_Length()
		{
			// Arrange
			UserDataValidator validator = new UserDataValidator();
			string stringOver255Chars = new string('a', 254);

			// Act
			bool res = await validator.Validate("testParam", stringOver255Chars);

			// Assert
			Assert.IsTrue(res);
		}

		[TestMethod]
		public async Task Should_Error_When_Value_Contains_Comma()
		{
			// Arrange
			UserDataValidator validator = new UserDataValidator();
			string stringOver255Chars = new string('a', 256);

			// Act
			bool res = await validator.Validate("testParam", "hel,lo");

			// Assert
			Assert.IsFalse(res);
		}

		[TestMethod]
		public async Task Should_Not_Error_When_Value_Does_Not_Contain_Comma()
		{
			// Arrange
			UserDataValidator validator = new UserDataValidator();
			string stringOver255Chars = new string('a', 256);

			// Act
			bool res = await validator.Validate("testParam", "hello");

			// Assert
			Assert.IsTrue(res);
		}
	}
}
