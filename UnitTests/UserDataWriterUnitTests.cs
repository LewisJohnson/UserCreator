using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;
using UserCreator;
using UserCreator.Implementations;

namespace UnitTests
{
	[TestClass]
	public class UserDataWriterUnitTests
	{
		[TestInitialize]
		public void Startup()
		{
			IdGenerator.ResetId();
		}

		[TestMethod]
		public async Task Should_Write_Standard_Field()
		{
			//Arrange
			string fieldName = "ExampleField";
			string fieldValue = "ExampleValue";
			var tempFile = UnitTestHelpers.GetTempFile();

			//Act
			using (var outputFileWriter = new StreamWriter(tempFile))
			{


				await UserDataWriter.WriteUserData(outputFileWriter, fieldName, fieldValue);
			}

			//Assert
			var fileToValidate = File.ReadAllText(tempFile);
			Assert.AreEqual($"1,{fieldName},{fieldValue}{Environment.NewLine}", fileToValidate);
		}

		[TestMethod]
		public async Task Should_Write_DateOfBirth_ddMMyyyy_Field()
		{
			//Arrange
			string fieldName = "DateOfBirth";
			string fieldValue = "17/07/1997";
			var tempFile = UnitTestHelpers.GetTempFile();

			//Act
			using (var outputFileWriter = new StreamWriter(tempFile))
			{


				await UserDataWriter.WriteUserData(outputFileWriter, fieldName, fieldValue);
			}

			//Assert
			var fileToValidate = File.ReadAllText(tempFile);
			Assert.AreEqual($"1,{fieldName},{fieldValue} 00:00:00{Environment.NewLine}", fileToValidate);
		}

		[TestMethod]
		public async Task Should_Write_DateOfBirth_enGb_Field()
		{
			//Arrange
			string fieldName = "DateOfBirth";
			string fieldValue = "17/07/1997 05:40:12";
			var tempFile = UnitTestHelpers.GetTempFile();

			//Act
			using (var outputFileWriter = new StreamWriter(tempFile))
			{


				await UserDataWriter.WriteUserData(outputFileWriter, fieldName, fieldValue);
			}

			//Assert
			var fileToValidate = File.ReadAllText(tempFile);
			Assert.AreEqual($"1,{fieldName},{fieldValue}{Environment.NewLine}", fileToValidate);
		}

		[TestMethod]
		public async Task Should_Write_Salary()
		{
			//Arrange
			string fieldName = "Salary";
			string fieldValue = "20500";
			var tempFile = UnitTestHelpers.GetTempFile();

			//Act
			using (var outputFileWriter = new StreamWriter(tempFile))
			{
				await UserDataWriter.WriteUserData(outputFileWriter, fieldName, fieldValue);
			}

			//Assert
			var fileToValidate = File.ReadAllText(tempFile);
			Assert.AreEqual($"1,{fieldName},{fieldValue}{Environment.NewLine}", fileToValidate);
		}

		[DataTestMethod]
		[DataRow(2)]
		[DataRow(20)]
		[DataRow(2000)]
		[DataRow(2000000)]
		public void Should_Increment_Id_With_Same_Type(int amountOfFields)
		{
			//Arrange
			var fieldNames = new string[amountOfFields];
			var fieldValues = new string[amountOfFields];

			for (int i = 0; i < amountOfFields; i++)
			{
				fieldNames[i] = $"Field{i}";
				fieldValues[i] = $"Value{i}";
			}
			var tempFile = UnitTestHelpers.GetTempFile();

			//Act
			using (var outputFileWriter = new StreamWriter(tempFile))
			{
				for (int i = 0; i < amountOfFields; i++)
				{
					UserDataWriter.WriteUserData(outputFileWriter, fieldNames[i], fieldValues[i]).Wait();
				}
			}

			//Assert
			int counter = 1;
			foreach (string line in File.ReadLines(tempFile))
			{
				string fieldName = fieldNames[counter - 1];
				string fieldValue = fieldValues[counter - 1];

				Assert.AreEqual($"{counter},{fieldName},{fieldValue}", line);

				counter++;
			}
		}

		[TestMethod]
		public async Task Should_Increment_Id_With_Many_Types()
		{
			//Arrange
			var tempFile = UnitTestHelpers.GetTempFile();

			//Act
			using (var outputFileWriter = new StreamWriter(tempFile))
			{
				await UserDataWriter.WriteUserData(outputFileWriter, "Field1", "val1");
				await UserDataWriter.WriteUserData(outputFileWriter, "Salary", "20000");
				await UserDataWriter.WriteUserData(outputFileWriter, "Field3", "val3");
				await UserDataWriter.WriteUserData(outputFileWriter, "Salary", "20000");
				await UserDataWriter.WriteUserData(outputFileWriter, "DateOfBirth", "17/07/1997");
				await UserDataWriter.WriteUserData(outputFileWriter, "Field6", "val6");
			}

			//Assert
			var fileToValidate = File.ReadAllText(tempFile);
			string expectedString = @"1,Field1,val1
2,Salary,20000
3,Field3,val3
4,Salary,20000
5,DateOfBirth,17/07/1997 00:00:00
6,Field6,val6
";

			Assert.AreEqual(expectedString, fileToValidate);
		}
	}
}
