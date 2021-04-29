using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public interface IDifficultyEvents : IEventSystemHandler
{
    void OnChangeDifficulty(DifficultyLevel difficultyLevel);
}
