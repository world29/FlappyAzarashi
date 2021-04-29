using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner
    : MonoBehaviour
    , IStageEvents
    , IGameStateEventHandler
    , IDifficultyEvents
{
    public GameObject m_block;

    public float m_areaStartX = 20;
    public float m_areaEndX = 100;

    public float m_blockHeightMin = 2;
    public float m_blockHeightMax = 5;

    public float m_interval = 10f;

    private int m_blockPoolCount = 4;
    private List<GameObject> m_blockPool;
    private float m_nextSpawnPositionX = 0;
    private Camera m_camera;
    private bool m_enableSpawning = false;
    private bool m_waitingForGameStateReady = false;

    private void Awake()
    {
        m_blockPool = new List<GameObject>(m_blockPoolCount);

        for (int i = 0; i < m_blockPoolCount; i++)
        {
            var blockInstance = GameObject.Instantiate(m_block, transform);
            blockInstance.SetActive(false);

            m_blockPool.Add(blockInstance);
        }

        m_camera = Camera.main;
    }

    private void SpawnBlocksToInitialPosition()
    {
        var x = m_areaStartX;

        for (int i = 0; i < m_blockPoolCount; i++)
        {
            float y = Random.Range(m_blockHeightMin, m_blockHeightMax);

            m_blockPool[i].transform.position = new Vector3(x, y, 0);
            m_blockPool[i].SetActive(true);

            x += m_interval;
        }

        m_nextSpawnPositionX = x;
    }

    private void OnEnable()
    {
        BroadcastReceivers.RegisterBroadcastReceiver<IStageEvents>(gameObject);
        BroadcastReceivers.RegisterBroadcastReceiver<IGameStateEventHandler>(gameObject);
        BroadcastReceivers.RegisterBroadcastReceiver<IDifficultyEvents>(gameObject);

        GameDataAccessor.OnStageChanged.AddListener(OnStageChanged);
    }

    private void OnDisable()
    {
        GameDataAccessor.OnStageChanged.RemoveListener(OnStageChanged);

        BroadcastReceivers.UnregisterBroadcastReceiver<IDifficultyEvents>(gameObject);
        BroadcastReceivers.UnregisterBroadcastReceiver<IGameStateEventHandler>(gameObject);
        BroadcastReceivers.UnregisterBroadcastReceiver<IStageEvents>(gameObject);
    }

    private void Update()
    {
        if (!m_enableSpawning) return;

        if (m_waitingForGameStateReady) return;

        // 画面外に出たブロックを次の場所に再登場させる

        for (int i = 0; i < m_blockPoolCount; i++)
        {
            var viewportPoint = m_camera.WorldToViewportPoint(m_blockPool[i].transform.position);

            if (viewportPoint.x < -1)
            {
                MoveToNextPosition(m_blockPool[i]);

                break;
            }
        }
    }

    private void MoveToNextPosition(GameObject block)
    {
        if (m_nextSpawnPositionX > m_areaEndX)
        {
            m_enableSpawning = false;

            return;
        }

        float y = Random.Range(m_blockHeightMin, m_blockHeightMax);

        block.transform.position = new Vector3(m_nextSpawnPositionX, y, 0);

        m_nextSpawnPositionX += m_interval;
    }

    public void OnStageRetry(StageId id)
    {
        m_enableSpawning = true;

        SpawnBlocksToInitialPosition();

        m_waitingForGameStateReady = true;
    }

    public void OnStageChanged(StageId id)
    {
        m_enableSpawning = true;

        SpawnBlocksToInitialPosition();
    }

    public void OnChangeDifficulty(DifficultyLevel difficultyLevel)
    {
        m_interval = difficultyLevel.BlockSpawnInterval;

        Debug.LogFormat("[BlockSpawner] Difficulty changed. BlockSpawnInterval={0}", m_interval);
    }

    public void OnGameStateChanged(IGameState prevState, IGameState nextState)
    {
        if (nextState is GameState_GameplayReady)
        {
            m_waitingForGameStateReady = false;
        }
    }

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        var center = new Vector3(
            (m_areaEndX - m_areaStartX) / 2f + m_areaStartX,
            (m_blockHeightMax - m_blockHeightMin) / 2f + m_blockHeightMin,
            0);

        var size = new Vector3(m_areaEndX - m_areaStartX, m_blockHeightMax - m_blockHeightMin, 1);

        Gizmos.color = new Color(1, 1, 0, 0.5f);
        Gizmos.DrawCube(center, size);
    }
#endif
}
