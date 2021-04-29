using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public interface IStageEvents : IEventSystemHandler
{
    void OnStageRetry(StageId id);
}
