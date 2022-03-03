using System.Collections.Generic;

public static class Difficulties
{
    public static readonly Dictionary<DifficultyType, Difficulty> List = new Dictionary<DifficultyType, Difficulty>
    {
        { 
            DifficultyType.NORMAL, 
            new Difficulty
            {
                SpawnTime = 1.45f,
                BaseEnemies = 25,
                MediumRate = 9,
                MediumWave = 2,
                HardRate = 40,
                HardWave = 5,
            }
        },
        { 
            DifficultyType.HARD, 
            new Difficulty
            {
                SpawnTime = 0.94f,
                BaseEnemies = 40,
                MediumRate = 6,
                MediumWave = 1,
                HardRate = 28,
                HardWave = 3,
            }
        },
        { 
            DifficultyType.HELL, 
            new Difficulty
            {
                SpawnTime = 0.65f,
                BaseEnemies = 55,
                MediumRate = 3,
                MediumWave = 1,
                HardRate = 16,
                HardWave = 2,
            }
        },
    };
}

public enum DifficultyType
{
    NORMAL,
    HARD,
    HELL
}