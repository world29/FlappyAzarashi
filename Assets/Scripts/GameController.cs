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
    public Text stateText;
    public Fade m_fade;
    public GameObject m_menuOnDeath;
    public GameInput gameInput;
    public AudioClip m_gameplayBGM;
    public AudioClip m_gameOverBGM;
    public PlayableAsset m_introDemo;
    public CheckPoint m_checkPoint;

    private AudioSource m_audioSource;
    private PlayableDirector m_playableDirector;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_playableDirector = GetComponent<PlayableDirector>();
    }

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
        m_menuOnDeath.SetActive(false);
        m_audioSource.Stop();

        if (m_checkPoint != null)
        {
            azarashi.Respawn();
            azarashi.transform.position = m_checkPoint.transform.position;

            m_checkPoint.RestorePropsPosition();

            m_checkPoint.enabled = false;
        }

        Intro();
    }

    void Intro()
    {
        state = State.Intro;

        StopScroll();

        azarashi.SetSteerActive(false);
        scoreText.gameObject.SetActive(false);
        stateText.gameObject.SetActive(false);

        // オブジェクトのバインド情報を設定してからデモを再生
        var targetAnimator = azarashi.GetComponent<Animator>();
        var binding = m_introDemo.outputs.First(c => c.streamName == "Animation Track");
        m_playableDirector.SetGenericBinding(binding.sourceObject, targetAnimator);

        m_playableDirector.stopped += OnStoppedIntroDemo;
        m_playableDirector.Play(m_introDemo);
    }

    void OnStoppedIntroDemo(PlayableDirector playableDirector)
    {
        playableDirector.stopped -= OnStoppedIntroDemo;

        Ready();
    }

    void Ready()
    {
        state = State.Ready;

        scoreText.text = "Score : " + 0;

        stateText.gameObject.SetActive(true);
        stateText.text = "Ready";

        StartDummyScroll();
    }

    void GameStart()
    {
        state = State.Play;

        azarashi.SetSteerActive(true);

        azarashi.Flap();

        stateText.gameObject.SetActive(false);
        stateText.text = "";

        m_audioSource.clip = m_gameplayBGM;
        m_audioSource.loop = true;
        m_audioSource.Play();

        StartParallaxScroll();
    }

    void Death()
    {
        state = State.Death;

        m_menuOnDeath.SetActive(true);

        m_audioSource.clip = m_gameOverBGM;
        m_audioSource.loop = false;
        m_audioSource.Play();

        StopScroll();
    }

    void GameOver()
    {
        state = State.GameOver;

        stateText.gameObject.SetActive(true);
        stateText.text = "GameOver";

        m_audioSource.clip = m_gameOverBGM;
        m_audioSource.loop = false;
        m_audioSource.Play();

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

        stateText.gameObject.SetActive(true);
        stateText.text = "Stage Clear";

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
