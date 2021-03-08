using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float m_scrollSpeed = 0;
    public float m_borderRight;
    public float m_nextOffset;

    public bool ScrollEnabled { get; set; }

    private void Start()
    {
        ScrollEnabled = true;
    }

    private void Update()
    {
        if (!ScrollEnabled) return;

        transform.Translate(new Vector3(-m_scrollSpeed * Time.deltaTime, 0, 0));

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
}
