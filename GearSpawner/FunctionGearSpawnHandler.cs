namespace GearSpawner;

internal sealed class FunctionGearSpawnHandler : ProbabilisticGearSpawnHandler
{
	private readonly Func<DifficultyLevel, FirearmAvailability, GearSpawnInfo, float> function;

	public FunctionGearSpawnHandler(Func<DifficultyLevel, FirearmAvailability, GearSpawnInfo, float> function)
	{
		this.function = function ?? throw new ArgumentNullException(nameof(function));
	}

	public override float GetProbability(DifficultyLevel difficultyLevel, FirearmAvailability firearmAvailability, GearSpawnInfo gearSpawnInfo)
	{
		return function.Invoke(difficultyLevel, firearmAvailability, gearSpawnInfo);
	}
}
