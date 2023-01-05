namespace GearSpawner;

public sealed class InvalidFormatException : Exception
{
	internal InvalidFormatException() : base() { }
	internal InvalidFormatException(string message) : base(message) { }
}
