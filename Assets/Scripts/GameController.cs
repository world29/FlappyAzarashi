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
        Intro, // プレイヤーの登場
        Ready, // プレイ開始可能
        Play,
        Death, // プレイヤー死亡。コンティニュー可能。
        GameOver,
        GameClear,
    }

    State state;
    int score;

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
        if (m_checkPoint != null)
        {
            m_checkPoint.SavePropsPosition();
        }

        RespawnPlayer();
    }

    void LateUpdate()
    {
        switch (state)
        {
            case State.Ready:
                if (gameInput.GetButtonDown(GameInput.ButtonType.Main)) GameStart();
                break;
            case State.Play:
                if (azarashi.IsDead()) Death();
                break;
            case State.Death:
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

    void RespawnPlayer()
    {
        if (m_checkPoint != null)
        {
            azarashi.Respawn();
            azarashi.transform.position = m_checkPoint.transform.position;

            m_checkPoint.RestorePropsPosition();
            m_checkPoint.SpawnEnemies();

            m_checkPoint.SetSaved();
        }

        Intro();
    }

    void Intro()
    {
        state = State.Intro;

        StopScroll();

        azarashi.SetSteerActive(false);
        scoreText.gameObject.SetActive(false);

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
        state = State.Ready;

        scoreText.text = "Score : " + 0;

        StartDummyScroll();
    }

    void GameStart()
    {
        state = State.Play;

        m_readyText.gameObject.SetActive(false);
        azarashi.SetSteerActive(true);

        azarashi.Flap();

        StartParallaxScroll();
    }

    void Death()
    {
        state = State.Death;

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
