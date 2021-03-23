using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeCounter : MonoBehaviour
{
    public Image[] m_lifeImages;

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
        for (int i= 0; i < m_lifeImages.Length; i++) {
            if (i < lifeCount) {
                m_lifeImages[i].enabled = true;
            }
            else {
                m_lifeImages[i].enabled = false;
            }
        }
    }
}
