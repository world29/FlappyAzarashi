using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 画面外に出たら次の位置に移動する
[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundInifinite : MonoBehaviour
{
    private SpriteRenderer m_spriteRenderer;

    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        var cameraLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z));

        if ((transform.position.x + m_spriteRenderer.size.x / 2) < cameraLeft.x)
        {
            float offsetX = m_spriteRenderer.size.x * 2;

            transform.Translate(new Vector3(offsetX, 0, 0));
        }
    }
}
