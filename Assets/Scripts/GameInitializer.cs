using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public int m_playerLifeCount = 3;

    public StageId m_initialStageId = StageId.Stage1;

    private PlayerController m_player;

    private void Awake()
    {
        var go = GameObject.FindWithTag("Player");
        m_player = go.GetComponent<PlayerController>();
    }

    void Start()
    {
        DifficultyBehaviour.Instance.InitializeDifficultyLevel();

        InitializeGameData();

        m_player.SetSteerActive(false);

        StartCoroutine(InitializeCotoutine());
    }

    void InitializeGameData()
    {
        GameDataAccessor.Initialize(m_playerLifeCount, m_initialStageId);
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
