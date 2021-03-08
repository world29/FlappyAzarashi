using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    enum State
    {
        Ready,
        Play,
        GameOver,
        GameClear,
    }

    State state;
    int score;

    public PlayerController azarashi;
    public Text scoreText;
    public Text stateText;
    public Fade m_fade;
    public GameInput gameInput;

    public AudioClip m_gameOverBGM;

    private AudioSource m_audioSource;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        Ready();
    }

    void LateUpdate()
    {
        switch (state)
        {
            case State.Ready:
                if (gameInput.GetButtonDown(GameInput.ButtonType.Main)) GameStart();
                break;
            case State.Play:
                if (azarashi.IsDead()) GameOver();
                break;
            case State.GameOver:
                if (gameInput.GetButtonDown(GameInput.ButtonType.Main)) Reload();
                break;
            case State.GameClear:
                if (gameInput.GetButtonDown(GameInput.ButtonType.Main)) Reload();
                break;
        }
    }

    void Ready()
    {
        state = State.Ready;

        azarashi.SetSteerActive(false);

        scoreText.text = "Score : " + 0;

        stateText.gameObject.SetActive(true);
        stateText.text = "Ready";
    }

    void GameStart()
    {
        state = State.Play;

        azarashi.SetSteerActive(true);

        azarashi.Flap();

        stateText.gameObject.SetActive(false);
        stateText.text = "";
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

    public void StopScroll()
    {
        var scrollObjects = GameObject.FindObjectsOfType<Background>();
        foreach (var so in scrollObjects)
        {
            so.ScrollEnabled = false;
        }
    }
}
