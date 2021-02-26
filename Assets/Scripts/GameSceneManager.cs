using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    public GameObject m_gameSceneTitle;
    public GameObject m_gameSceneGameplay;

    public int m_sceneIndex = 0;

    List<GameObject> m_gameSceneArray = new List<GameObject>();

    private void Awake()
    {
        m_gameSceneArray.Add(m_gameSceneTitle);
        m_gameSceneArray.Add(m_gameSceneGameplay);

        // 開始シーンのみ有効化する
        for (int i = 0; i < m_gameSceneArray.Count; i++)
        {
            if (i == m_sceneIndex)
            {
                m_gameSceneArray[i].SetActive(true);
            }
            else
            {
                m_gameSceneArray[i].SetActive(false);
            }
        }
    }

    public int GetActiveSceneIndex()
    {
        return m_sceneIndex;
    }

    public void NextScene()
    {
        var prevScene = m_gameSceneArray[m_sceneIndex];

        m_sceneIndex = (m_sceneIndex + 1) % m_gameSceneArray.Count;

        var nextScene = m_gameSceneArray[m_sceneIndex];

        prevScene.SetActive(false);
        nextScene.SetActive(true);
    }

}
