using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public int m_playerLifeCount = 3;

    public StageId m_initialStageId = StageId.Stage1;

    public bool m_resetHighScore = false;

    void Start()
    {
        DifficultyBehaviour.Instance.InitializeDifficultyLevel();

        InitializeGameData();

        StartCoroutine(InitializeCotoutine());
    }

    void InitializeGameData()
    {
        int highScore = 0;

        var saveData = SaveLoadGame.LoadGame();

        if (saveData != null)
        {
            highScore = saveData.HighScore;
        }

        if (m_resetHighScore)
        {
            highScore = 0;
        }

        GameDataAccessor.Initialize(m_playerLifeCount, highScore, m_initialStageId);
    }

    IEnumerator InitializeCotoutine()
    {
        var sound = Sound.GetInstance();

        yield return sound.InitializeCoroutine();

        sound.LoadBgm("Stage", "Stage01");

        sound.LoadSe("break_1", "break_1");
        sound.LoadSe("break_2", "break_2");
        sound.LoadSe("break_3", "break_3");
        sound.LoadSe("dash_1", "dash_1");
        sound.LoadSe("hurt_1", "hurt_1");
        sound.LoadSe("wind_1", "wind_1");
        sound.LoadSe("wind_2", "wind_2");
        sound.LoadSe("bell_1", "bell_1");
        sound.LoadSe("bell_2", "bell_2");
    }
}
