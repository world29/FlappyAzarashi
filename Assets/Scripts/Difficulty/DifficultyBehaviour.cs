using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyBehaviour : MonoBehaviour
{
    public DifficultyLevel[] m_difficultyLevels;

    public int m_initialLevelIndex = 0;

    private int m_levelIndex = 0;

    public DifficultyLevel CurrentLevel
    {
        get { return m_difficultyLevels[m_levelIndex]; }
    }

    public int CurrentLevelIndex
    {
        get { return m_levelIndex; }
    }

    // singleton
    public static DifficultyBehaviour Instance { get; private set; }
    bool InitializeAsSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            return true;
        }
        else
        {
            Debug.Assert(false, "DifficultyBehaviour is singleton");

            Destroy(this);

            return false;
        }
    }

    private void Awake()
    {
        if (!InitializeAsSingleton()) return;
    }

    public void InitializeDifficultyLevel()
    {
        m_levelIndex = m_initialLevelIndex;

        EmitChangeDifficultyLevel(m_levelIndex);
    }

    private void EmitChangeDifficultyLevel(int level)
    {
        BroadcastExecuteEvents.Execute<IDifficultyEvents>(null /* eventData */,
            (handler, eventData) => handler.OnChangeDifficulty(m_difficultyLevels[level]));
    }

    public void NextLevel()
    {
        var nextLevelIndex = Mathf.Min(m_levelIndex + 1, m_difficultyLevels.Length - 1);

        if (nextLevelIndex != m_levelIndex)
        {
            m_levelIndex = nextLevelIndex;

            EmitChangeDifficultyLevel(m_levelIndex);
        }
    }
}
