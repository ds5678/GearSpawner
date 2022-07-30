namespace GearSpawner;

public interface IGearSpawnHandler
{
	float GetProbability(DifficultyLevel difficultyLevel, FirearmAvailability firearmAvailability, GearSpawnInfo gearSpawnInfo);
}