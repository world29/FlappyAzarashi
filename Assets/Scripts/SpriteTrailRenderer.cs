﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTrailRenderer : MonoBehaviour
{
    public SpriteRenderer m_LeadingSprite;

    public int m_TrailSegments;
    public float m_TrailTime;
    public GameObject m_TrailObject;

    private float m_SpawnInterval;
    private float m_SpawnTimer;
    private bool m_Enabled;

    private List<GameObject> m_TrailObjectsInUse;
    private Queue<GameObject> m_TrailObjectsNotInUse;

    private void Start()
    {
        m_SpawnInterval = m_TrailTime / m_TrailSegments;
        m_TrailObjectsInUse = new List<GameObject>();
        m_TrailObjectsNotInUse = new Queue<GameObject>();

        for (int i = 0; i < m_TrailSegments; i++)
        {
            GameObject trail = GameObject.Instantiate(m_TrailObject);
            trail.transform.SetParent(transform);
            m_TrailObjectsNotInUse.Enqueue(trail);
        }

        m_Enabled = false;
    }

    private void Update()
    {
        if (m_Enabled)
        {
            m_SpawnTimer += Time.deltaTime;

            if (m_SpawnTimer >= m_SpawnInterval)
            {
                GameObject trail = m_TrailObjectsNotInUse.Dequeue();

                if (trail != null)
                {
                    SpriteTrailObject trailObject = trail.GetComponent<SpriteTrailObject>();

                    trailObject.Initialize(m_TrailTime, m_LeadingSprite.sprite, transform.position, m_LeadingSprite.transform.rotation, this);
                    m_TrailObjectsInUse.Add(trail);

                    m_SpawnTimer = 0;
                }
            }
        }
    }

    public void RemoveTrailObject(GameObject obj)
    {
        m_TrailObjectsInUse.Remove(obj);
        m_TrailObjectsNotInUse.Enqueue(obj);
    }

    public void SetEnabled(bool enabled)
    {
        if (m_Enabled == enabled) return;

        m_Enabled = enabled;

        if (enabled)
        {
            m_SpawnTimer = m_SpawnInterval;
        }
    }
}