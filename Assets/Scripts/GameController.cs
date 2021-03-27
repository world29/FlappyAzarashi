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
        Gameplay_GameOver, // 残機ゼロでプレイヤー死亡。
        GameClear,
    }

    State state;
    PlayerAction m_input;

    public PlayerController azarashi;
    public Text m_readyText;
    public Fade m_fade;
    public CheckPoint m_checkPoint;
    public PlayableDirector m_respawnTimeline;
    public PlayableDirector m_deathTimeline;
    public PlayableDirector m_gameoverTimeline;

    private void Awake()
    {
        m_input = new PlayerAction();
    }

    private void OnEnable()
    {
        m_input.Enable();
    }

    private void OnDisable()
    {
        m_input.Disable();
    }

    private void OnDestroy()
    {
        m_input.Disable();
    }

    void Start()
    {
        GameDataAccessor.Initialize();

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
                if (m_input.PlatformAction.Jump.triggered) GameStart();
                break;
            case State.Gameplay_Play:
                if (azarashi.IsDead()) Death();
                break;
            case State.Gameplay_Death:
                break;
            case State.Gameplay_GameOver:
                break;
            case State.GameClear:
                if (m_input.PlatformAction.Jump.triggered) Reload();
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

    public void ResetLifeAndRespawnPlayer()
    {
        GameDataAccessor.PlayerLifeCount = GameDataAccessor.InitialPlayerLifeCount;

        RespawnPlayer();
    }

    void OnStoppedRespawnTimeline(PlayableDirector playableDirector)
    {
        playableDirector.stopped -= OnStoppedRespawnTimeline;

        Ready();
    }

    void Ready()
    {
        state = State.Gameplay_Ready;

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
        StopScroll();

        int lifeCount = GameDataAccessor.PlayerLifeCount;

        if (lifeCount > 0) {
            state = State.Gameplay_Death;

            GameDataAccessor.PlayerLifeCount = Mathf.Max(0, lifeCount - 1);

            m_deathTimeline.Play();
        }
        else {
            state = State.Gameplay_GameOver;

            m_gameoverTimeline.Play();
        }
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
        if (state != State.Gameplay_Play) {
            return;
        }

        GameDataAccessor.Score++;
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
