using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTrailObject : MonoBehaviour
{
    public SpriteRenderer m_Renderer;
    public Color m_StartColor;
    public Color m_EndColor;

    private bool m_InUse;
    private Vector2 m_Position;
    private Quaternion m_Rotation;
    private float m_DisplayTime;
    private float m_TimeDisplayed;
    private SpriteTrailRenderer m_Spawner;

    private void Start()
    {
        m_Renderer.enabled = false;
    }

    private void Update()
    {
        if (m_InUse)
        {
            transform.position = m_Position;
            transform.rotation = m_Rotation;

            m_TimeDisplayed += Time.deltaTime;

            m_Renderer.color = Color.Lerp(m_StartColor, m_EndColor, m_TimeDisplayed / m_DisplayTime);

            if (m_TimeDisplayed >= m_DisplayTime)
            {
                m_Spawner.RemoveTrailObject(gameObject);
                m_InUse = false;
                m_Renderer.enabled = false;
            }
        }
    }

    public void Initialize(float displayTime, Sprite sprite, Vector2 position, Quaternion rotation, SpriteTrailRenderer trailRenderer)
    {
        m_DisplayTime = displayTime;
        m_Renderer.sprite = sprite;
        m_Renderer.enabled = true;
        m_Position = position;
        m_Rotation = rotation;
        m_TimeDisplayed = 0;
        m_Spawner = trailRenderer;
        m_InUse = true;
    }
}
