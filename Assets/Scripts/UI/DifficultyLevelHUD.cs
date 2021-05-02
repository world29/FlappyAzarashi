using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DifficultyLevelHUD : MonoBehaviour, IDifficultyEvents
{
    public TextMeshProUGUI m_levelText;

    private void OnEnable()
    {
        BroadcastReceivers.RegisterBroadcastReceiver<IDifficultyEvents>(gameObject);

        UpdateUI(DifficultyBehaviour.Instance.CurrentLevelIndex + 1);
    }

    private void OnDisable()
    {
        BroadcastReceivers.UnregisterBroadcastReceiver<IDifficultyEvents>(gameObject);
    }

    public void OnChangeDifficulty(DifficultyLevel difficultyLevel)
    {
        var levels = DifficultyBehaviour.Instance.m_difficultyLevels;

        int i = 0;
        for (; i < levels.Length; i++) {
            if (levels[i] == difficultyLevel) {
                break;
            }
        }

        UpdateUI(i + 1);
    }


    public void UpdateUI(int level)
    {
        m_levelText.text = "Level " + level;
    }
}
