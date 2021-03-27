using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    public TextMeshProUGUI m_scoreText;

    private void OnEnable()
    {
        GameDataAccessor.OnScoreChanged.AddListener(UpdateUI);

        UpdateUI(GameDataAccessor.Score);
    }

    private void OnDisable()
    {
        GameDataAccessor.OnScoreChanged.RemoveListener(UpdateUI);
    }

    public void UpdateUI(int score)
    {
        m_scoreText.text = "Score: " + score;
    }
}
