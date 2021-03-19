using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Playables;
using System.Linq;

public class GameController : MonoBehaviour
{
    enum State
    {
        Title, // タイトル
        Intro, // タイトルとゲームプレイをつなぐデモ
        Gameplay_Spawn, // プレイヤースポーン
        Gameplay_Ready, // プレイ開始可能
        Gameplay_Play,
        Gameplay_Death, // プレイヤー死亡。コンティニュー可能。
        GameOver,
        GameClear,
    }

    State state;
    int score = 0;

    public PlayerController azarashi;
    public Text scoreText;
    public Text m_readyText;
    public Fade m_fade;
    public GameInput gameInput;
    public CheckPoint m_checkPoint;
    public PlayableDirector m_respawnTimeline;
    public PlayableDirector m_deathTimeline;

    void Start()
    {
        azarashi.SetSteerActive(false);

        if (m_checkPoint != null)
        {
            m_checkPoint.SavePropsPosition();
        }

        state = State.Title;
    }

    void LateUpdate()
    {
        switch (state)
        {
            case State.Title:
                break;
            case State.Gameplay_Ready:
                if (gameInput.GetButtonDown(GameInput.ButtonType.Main)) GameStart();
                break;
            case State.Gameplay_Play:
                if (azarashi.IsDead()) Death();
                break;
            case State.Gameplay_Death:
                break;
            case State.GameOver:
                if (gameInput.GetButtonDown(GameInput.ButtonType.Main)) Reload();
                break;
            case State.GameClear:
                if (gameInput.GetButtonDown(GameInput.ButtonType.Main)) Reload();
                break;
            default:
                break;
        }
    }

    //todo: Gameplay_Start
    void RespawnPlayer()
    {
        state = State.Gameplay_Spawn;

        if (m_checkPoint != null)
        {
            m_checkPoint.DestroyEnemies();
            m_checkPoint.RestorePropsPosition();
        }

        azarashi.Respawn();
        azarashi.SetSteerActive(false);
        azarashi.transform.position = m_checkPoint.transform.position;

        StopScroll();

        m_respawnTimeline.stopped += OnStoppedRespawnTimeline;
        m_respawnTimeline.Play();
    }

    void OnStoppedRespawnTimeline(PlayableDirector playableDirector)
    {
        playableDirector.stopped -= OnStoppedRespawnTimeline;

        Ready();
    }

    void Ready()
    {
        state = State.Gameplay_Ready;

        scoreText.text = "Score : " + 0;

        if (m_checkPoint != null)
        {
            m_checkPoint.SpawnEnemies();

            m_checkPoint.SetSaved();
        }

        StartDummyScroll();
    }

    void GameStart()
    {
        state = State.Gameplay_Play;

        m_readyText.gameObject.SetActive(false);
        azarashi.SetSteerActive(true);

        azarashi.Flap();

        StartParallaxScroll();
    }

    void Death()
    {
        state = State.Gameplay_Death;

        StopScroll();
        m_deathTimeline.Play();
        m_deathTimeline.stopped += OnStoppedDeathTimeline;
    }

    void OnStoppedDeathTimeline(PlayableDirector playableDirector)
    {
        playableDirector.stopped -= OnStoppedDeathTimeline;
    }

    void GameOver()
    {
        state = State.GameOver;

        StopScroll();
    }

    void Reload()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void StartGameplay()
    {
        RespawnPlayer();
    }

    public void GameClear()
    {
        state = State.GameClear;

        azarashi.SetSteerActive(false);

        m_fade.FadeIn(3, () => {
            Reload();
        });
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = "Score : " + score;
    }

    public void StartDummyScroll()
    {
        var scrollObjects = GameObject.FindObjectsOfType<Background>();
        foreach (var so in scrollObjects)
        {
            so.StartDummyScroll();
        }
    }

    public void StartParallaxScroll()
    {
        var scrollObjects = GameObject.FindObjectsOfType<Background>();
        foreach (var so in scrollObjects)
        {
            so.StartParallaxScroll();
        }
    }

    public void StopScroll()
    {
        var scrollObjects = GameObject.FindObjectsOfType<Background>();
        foreach (var so in scrollObjects)
        {
            so.StopScroll();
        }
    }

    public void SetRespawnPoint(CheckPoint checkPoint)
    {
        m_checkPoint = checkPoint;

        checkPoint.SavePropsPosition();
    }
}
