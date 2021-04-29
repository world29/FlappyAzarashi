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
    public GameObject m_movingBlock;

    public float m_areaStartX = 20;
    public float m_areaEndX = 100;

    public float m_blockHeightMin = 2;
    public float m_blockHeightMax = 5;

    public float m_interval = 10f;
    public float m_movingBlockRate = 0.5f;

    private List<GameObject> m_blockPool;
    private Camera m_camera;

    private void Awake()
    {
        m_camera = Camera.main;
    }

    private GameObject SpawnNextBlock(Vector3 position)
    {
        bool spawnMovingBlock = (Random.value < m_movingBlockRate);

        GameObject blockInstance = null;

        if (spawnMovingBlock)
        {
            float y = (m_blockHeightMax - m_blockHeightMin) * 0.5f + m_blockHeightMin;

            var pos = new Vector3(position.x, y, position.z);

            blockInstance = GameObject.Instantiate(m_movingBlock, pos, Quaternion.identity, transform);

            var movingBlock = blockInstance.GetComponent<MovingBlock>();
            movingBlock.m_phaseTime = Random.value * movingBlock.m_time;
        }
        else
        {
            float y = Random.Range(m_blockHeightMin, m_blockHeightMax);

            var pos = new Vector3(position.x, y, position.z);

            blockInstance = GameObject.Instantiate(m_block, pos, Quaternion.identity, transform);
        }

        return blockInstance;
    }

    private void SpawnBlocks()
    {
        if (m_blockPool != null)
        {
            foreach (var go in m_blockPool)
            {
                Destroy(go);
            }
            m_blockPool.Clear();
        }

        m_blockPool = new List<GameObject>();

        var x = m_areaStartX;

        while (x <= m_areaEndX)
        {
            m_blockPool.Add(SpawnNextBlock(new Vector3(x, 0, 0)));

            x += m_interval;
        }
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

    public void OnStageRetry(StageId id)
    {
        SpawnBlocks();
    }

    public void OnStageChanged(StageId id)
    {
        SpawnBlocks();
    }

    public void OnChangeDifficulty(DifficultyLevel difficultyLevel)
    {
        m_interval = difficultyLevel.BlockSpawnInterval;
        m_movingBlockRate = difficultyLevel.MovingBlockSpawnRate;

        Debug.LogFormat("[BlockSpawner] Difficulty changed. BlockSpawnInterval={0}, MovingBlockRate={1}", m_interval, m_movingBlockRate);
    }

    public void OnGameStateChanged(IGameState prevState, IGameState nextState)
    {
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
