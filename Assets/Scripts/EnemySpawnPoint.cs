using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour, IStageEvents
{
    public GameObject m_enemyStage1;
    public GameObject m_enemyStage2;
    public GameObject m_enemyStage3;

    public Transform m_spawnPosition;

    private PlayerController m_player;
    private bool m_spawned = false;

    private GameObject m_currentStageEnemy;

    private void Awake()
    {
        if (m_player == null)
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            m_player = go.GetComponent<PlayerController>();
        }
    }

    private void OnEnable()
    {
        BroadcastReceivers.RegisterBroadcastReceiver<IStageEvents>(gameObject);

        GameDataAccessor.OnStageChanged.AddListener(OnStageChanged);
    }

    private void OnDisable()
    {
        GameDataAccessor.OnStageChanged.RemoveListener(OnStageChanged);

        BroadcastReceivers.UnregisterBroadcastReceiver<IStageEvents>(gameObject);
    }

    public void LateUpdate()
    {
        if (m_spawned) return;

        // プレイヤーがこのオブジェクトを通過したときにスポーンする
        if (m_player.transform.position.x > transform.position.x)
        {
            DoSpawn();

            m_spawned = true;
        }
    }

    private void DoSpawn()
    {
        if (m_currentStageEnemy)
        {
            GameObject.Instantiate(m_currentStageEnemy, m_spawnPosition.position, Quaternion.identity);
        }
    }

    public void OnStageRetry(StageId id)
    {
        m_spawned = false;
    }

    public void OnStageChanged(StageId id)
    {
        switch (id)
        {
            case StageId.Stage1:
                m_currentStageEnemy = m_enemyStage1;
                break;
            case StageId.Stage2:
                m_currentStageEnemy = m_enemyStage2;
                break;
            case StageId.Stage3:
                m_currentStageEnemy = m_enemyStage3;
                break;
        }

        m_spawned = false;
    }
}
