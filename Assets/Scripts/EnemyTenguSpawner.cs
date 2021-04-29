using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTenguSpawner : MonoBehaviour, IActorEvents
{
    public GameObject[] m_enemies;

    public float m_interval = 1;

    public bool m_playOnAwake = false;

    private int m_aliveEnemiesCount;
    private GameObject m_player;

    private void Awake()
    {
        m_player = GameObject.FindWithTag("Player");
    }

    private void OnEnable()
    {
        BroadcastReceivers.RegisterBroadcastReceiver<IActorEvents>(gameObject);
    }

    private void OnDisable()
    {
        BroadcastReceivers.UnregisterBroadcastReceiver<IActorEvents>(gameObject);
    }

    private void Start()
    {
        if (m_playOnAwake)
        {
            Play();
        }
    }

    public void Play()
    {
        m_aliveEnemiesCount = m_enemies.Length;

        StartCoroutine(SpawnEnemyCoroutine());
    }

    IEnumerator SpawnEnemyCoroutine()
    {
        int index = 0;

        while (index < m_enemies.Length)
        {
            yield return new WaitForSeconds(m_interval);

            var prefab = m_enemies[index];

            var go = GameObject.Instantiate(prefab, transform.position, Quaternion.identity);

            go.GetComponent<EnemyLookAt>().m_lookAtTarget = m_player;
            go.GetComponent<EnemyMovementTrackingTarget>().m_target = m_player.transform;
            go.GetComponent<EnemyKnockback>().m_cameraTransform = Camera.main.transform;

            index++;
        }
    }

    public void OnActorDied(ActorId id)
    {
        if (id == ActorId.Tengu)
        {
            m_aliveEnemiesCount--;

            if (m_aliveEnemiesCount == 0)
            {
                OnAllEnemiesDied();
            }
        }
    }

    private void OnAllEnemiesDied()
    {
        var warpPoint = WarpPoint.Instance;

        var newPos = warpPoint.transform.position;
        newPos.x = transform.position.x + 10;
        warpPoint.transform.position = newPos;

        warpPoint.gameObject.SetActive(true);
    }
}
