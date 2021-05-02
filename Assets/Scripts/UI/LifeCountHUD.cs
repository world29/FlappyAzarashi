using UnityEngine;
using System.Collections;
using TMPro;

public class LifeCountHUD : MonoBehaviour
{
    public TextMeshProUGUI m_lifeText;

    private void OnEnable()
    {
        GameDataAccessor.OnPlayerLifeCountChanged.AddListener(UpdateUI);

        UpdateUI(GameDataAccessor.PlayerLifeCount);
    }

    private void OnDisable()
    {
        GameDataAccessor.OnPlayerLifeCountChanged.RemoveListener(UpdateUI);
    }

    public void UpdateUI(int lifeCount)
    {
        m_lifeText.text = "x " + lifeCount;
    }
}
