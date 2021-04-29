using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour, IStageEvents
{
    private int m_savedScore = 0;

    private void OnEnable()
    {
        BroadcastReceivers.RegisterBroadcastReceiver<IStageEvents>(gameObject);

        GameDataAccessor.OnStageChanged.AddListener(OnStageChanged);
    }

    private void OnDisable()
    {
        GameDataAccessor.OnStageChanged.RemoveListener(OnStageChanged);

        BroadcastReceivers.UnregisterBroadcastReceiver<IStageEvents>(gameObject);
    }

    public void OnStageRetry(StageId id)
    {
        // リトライ時にステージ開始時点でのスコアに戻す

        GameDataAccessor.Score = m_savedScore;
    }

    public void OnStageChanged(StageId id)
    {
        m_savedScore = GameDataAccessor.Score;
    }
}
