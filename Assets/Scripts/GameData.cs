using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameData
{
    public int PlayerLifeCount;

    public int Score;
}

[System.Serializable]
public class UnityEventInt : UnityEvent<int> {}

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

    public static void Initialize()
    {
        PlayerLifeCount = InitialPlayerLifeCount;
        Score = 0;
    }

    public static int InitialPlayerLifeCount = 3;
}
