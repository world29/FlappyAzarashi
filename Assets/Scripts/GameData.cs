using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameData {
    public int PlayerLifeCount;
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

    public static void Initialize()
    {
        PlayerLifeCount = 3;
    }
}
