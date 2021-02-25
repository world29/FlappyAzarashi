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

    public AzarashiController azarashi;
    public GameObject blocks;
    public Text scoreText;
    public Text stateText;
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
        blocks.SetActive(false);

        scoreText.text = "Score : " + 0;

        stateText.gameObject.SetActive(true);
        stateText.text = "Ready";
    }

    void GameStart()
    {
        state = State.Play;

        azarashi.SetSteerActive(true);
        blocks.SetActive(true);

        azarashi.Flap();

        stateText.gameObject.SetActive(false);
        stateText.text = "";
    }

    void GameOver()
    {
        state = State.GameOver;

        ScrollObject[] scrollObjects = FindObjectsOfType<ScrollObject>();

        foreach (ScrollObject so in scrollObjects) so.enabled = false;

        stateText.gameObject.SetActive(true);
        stateText.text = "GameOver";

        m_audioSource.clip = m_gameOverBGM;
        m_audioSource.loop = false;
        m_audioSource.Play();
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
        blocks.SetActive(false);

        stateText.gameObject.SetActive(true);
        stateText.text = "Stage Clear";
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = "Score : " + score;
    }
}
