using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTrailObject : MonoBehaviour
{
    public SpriteRenderer m_Renderer;
    public Color m_StartColor;
    public Color m_EndColor;

    public bool m_ScrollTrail;
    public float m_ScrollSpeed = 1.0f;

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

            if (m_ScrollTrail)
            {
                float offsetX = -1 * m_ScrollSpeed * m_TimeDisplayed;

                var scrolledPosition = transform.position;
                scrolledPosition.x += offsetX;

                transform.position = scrolledPosition;
                //transform.Translate(offsetX, 0, 0);
            }

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
