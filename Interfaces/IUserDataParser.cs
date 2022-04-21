namespace UserCreator.Interfaces
{
	internal interface IUserDataParser<T>
	{
		public bool TryConvertData(string input, out T data);
	}
}