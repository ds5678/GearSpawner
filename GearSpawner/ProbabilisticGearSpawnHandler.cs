namespace GearSpawner;

public abstract class ProbabilisticGearSpawnHandler : GearSpawnHandler
{
	/// <summary>
	/// Get the probability that the item will be spawned.
	/// </summary>
	/// <param name="difficultyLevel"></param>
	/// <param name="firearmAvailability"></param>
	/// <param name="gearSpawnInfo"></param>
	/// <returns>The percent probability of the item being spawned.</returns>
	public abstract float GetProbability(DifficultyLevel difficultyLevel, FirearmAvailability firearmAvailability, GearSpawnInfo gearSpawnInfo);

	public sealed override bool ShouldSpawn(DifficultyLevel difficultyLevel, FirearmAvailability firearmAvailability, GearSpawnInfo gearSpawnInfo)
	{
		return RollChance(GetProbability(difficultyLevel, firearmAvailability, gearSpawnInfo));
	}
}