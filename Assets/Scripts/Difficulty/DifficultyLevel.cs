using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DifficultyLevel : ScriptableObject
{
    public float BlockSpawnInterval = 10;

    [Range(0, 1)]
    public float MovingBlockSpawnRate = 0;
}
