using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageActor : MonoBehaviour, IStageEvents
{
    private void OnEnable()
    {
        BroadcastReceivers.RegisterBroadcastReceiver<IStageEvents>(gameObject);
    }

    private void OnDisable()
    {
        BroadcastReceivers.UnregisterBroadcastReceiver<IStageEvents>(gameObject);
    }

    public void OnStageRetry(StageId id)
    {
        StartCoroutine(DestroyNextFrameCoroutine());
    }

    private IEnumerator DestroyNextFrameCoroutine()
    {
        yield return null;

        GameObject.Destroy(gameObject);
    }
}
