using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float m_scrollSpeed = 0;
    [Range(0, 1)]
    public float m_parallax = 1;
    public float m_borderRight;
    public float m_nextOffset;

    private bool m_dummyScroll = false;
    private bool m_parallaxScroll = false;

    private Vector3 m_lastCamerPosition;

    private void Start()
    {
        m_lastCamerPosition = Camera.main.transform.position;
    }

    private void Update()
    {
        if (!m_dummyScroll) return;

        Scroll(-m_scrollSpeed * Time.deltaTime);
    }

    private void LateUpdate()
    {
        if (m_parallaxScroll)
        {
            var camPos = Camera.main.transform.position;

            var diff = camPos.x - m_lastCamerPosition.x;
            var offsetX = diff * m_parallax;

            Scroll(offsetX);
        }
        m_lastCamerPosition = Camera.main.transform.position;
    }

    void Scroll(float x)
    {
        transform.Translate(new Vector3(x, 0, 0));

        var cameraLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z));

        if ((transform.position.x + m_borderRight) < cameraLeft.x)
        {
            OnScrollEnd();
        }
    }

    void OnScrollEnd()
    {
        transform.Translate(new Vector3(m_nextOffset, 0, 0));
    }

    public void StartDummyScroll()
    {
        m_dummyScroll = true;
        m_parallaxScroll = false;
    }

    public void StartParallaxScroll()
    {
        m_parallaxScroll = true;
        m_dummyScroll = false;
    }

    public void StopScroll()
    {
        m_dummyScroll = m_parallaxScroll = false;
    }
}
