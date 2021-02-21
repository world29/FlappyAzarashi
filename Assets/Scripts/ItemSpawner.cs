using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public float minHeight;
    public float maxHeight;
    public GameObject itemPrefab;

    private GameObject m_spawnedItem;

    void Start()
    {
        Respawn();
    }

    void Update()
    {

    }

    void Respawn()
    {
        if (m_spawnedItem)
        {
            Destroy(m_spawnedItem);
        }

        float height = Random.Range(minHeight, maxHeight);

        m_spawnedItem = GameObject.Instantiate(itemPrefab, gameObject.transform);
        m_spawnedItem.transform.localPosition = new Vector3(0.0f, height, 0.0f);
    }

    void OnScrollEnd()
    {
        Respawn();
    }
}
