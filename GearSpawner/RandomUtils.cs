using System.Buffers.Binary;
using System.Security.Cryptography;

namespace GearSpawner;

internal static class RandomUtils
{
	/// <summary>
	/// Rolls the chance that an action is successful.
	/// </summary>
	/// <param name="percent">Chance of success between 0 and 100</param>
	/// <returns>True if successful and false otherwise.</returns>
	public static bool RollChance(float percent)
	{
		return percent switch
		{
			<= 0 => false,
			>= 100 => true,
			_ => RandomFloat() < percent / 100
		};
	}

	public static float RandomFloat()
	{
		Span<byte> span = stackalloc byte[4];
		RandomNumberGenerator.Fill(span);
		return BinaryPrimitives.ReadSingleLittleEndian(span);
	}
}
