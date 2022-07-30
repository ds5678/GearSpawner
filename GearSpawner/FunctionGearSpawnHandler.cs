using System;

namespace GearSpawner;

internal sealed class FunctionGearSpawnHandler : IGearSpawnHandler
{
	private readonly Func<DifficultyLevel, FirearmAvailability, GearSpawnInfo, float> function;

	public FunctionGearSpawnHandler(Func<DifficultyLevel, FirearmAvailability, GearSpawnInfo, float> function)
	{
		this.function = function ?? throw new ArgumentNullException(nameof(function));
	}

	public float GetProbability(DifficultyLevel difficultyLevel, FirearmAvailability firearmAvailability, GearSpawnInfo gearSpawnInfo)
	{
		return function.Invoke(difficultyLevel, firearmAvailability, gearSpawnInfo);
	}
}
