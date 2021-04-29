using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum StageId
{
    Stage1,
    Stage2,
    Stage3,

    Num,
}

public class GameData
{
    public int PlayerLifeCount;

    public int Score;

    public StageId CurrentStageId;
}

[System.Serializable]
public class UnityEventInt : UnityEvent<int> {}

[System.Serializable]
public class UnityEventStageId : UnityEvent<StageId> { }

public static class GameDataAccessor
{
    static GameData s_gameData = new GameData();

    public static int PlayerLifeCount {
        get { return s_gameData.PlayerLifeCount; }
        set {
            s_gameData.PlayerLifeCount = value;
            OnPlayerLifeCountChanged.Invoke(value);
        }
    }

    public static UnityEventInt OnPlayerLifeCountChanged = new UnityEventInt();

    public static int Score {
        get { return s_gameData.Score; }
        set {
            s_gameData.Score = value;
            OnScoreChanged.Invoke(value);
        }
    }

    public static UnityEventInt OnScoreChanged = new UnityEventInt();

    public static StageId CurrentStageId
    {
        get { return s_gameData.CurrentStageId; }
        set
        {
            s_gameData.CurrentStageId = value;
            OnStageChanged.Invoke(value);
        }
    }

    public static UnityEventStageId OnStageChanged = new UnityEventStageId();

    public static void Initialize()
    {
        Initialize(InitialPlayerLifeCount, StageId.Stage1);
    }

    public static void Initialize(int playerLifeCount, StageId initialStageId)
    {
        PlayerLifeCount = playerLifeCount;
        Score = 0;
        CurrentStageId = initialStageId;
    }

    public static int InitialPlayerLifeCount = 3;
}
