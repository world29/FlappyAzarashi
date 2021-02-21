using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float minHeight;
    public float maxHeight;
    public GameObject enemyPrefab;

    private GameObject m_spawnedEnemy;

    void Start()
    {
        Respawn();
    }

    void Update()
    {

    }

    void Respawn()
    {
        if (m_spawnedEnemy)
        {
            Destroy(m_spawnedEnemy);
        }

        float height = Random.Range(minHeight, maxHeight);

        m_spawnedEnemy = GameObject.Instantiate(enemyPrefab, gameObject.transform);
        m_spawnedEnemy.transform.localPosition = new Vector3(0.0f, height, 0.0f);
    }

    void OnScrollEnd()
    {
        Respawn();
    }
}
