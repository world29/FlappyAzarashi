using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollObject : MonoBehaviour
{
    public float speed = 1.0f;
    public float startPosition;
    public float endPosition;

    private Vector2 m_spriteSize;
    private Vector3 m_basePosition;

    private void Start()
    {
        var sr = GetComponent<SpriteRenderer>();
        m_spriteSize = sr.sprite.bounds.size;

        m_basePosition = transform.position;
    }

    void LateUpdate()
    {
        transform.Translate(-1 * speed * Time.deltaTime, 0, 0);

        if (transform.position.x < (Camera.main.transform.position.x + endPosition)) ScrollEnd();
    }

    void ScrollEnd()
    {
        var offsetX = Mathf.Abs(endPosition - startPosition);
        var newPos = new Vector3(m_basePosition.x + offsetX, m_basePosition.y, m_basePosition.z);

        transform.position = newPos;

        SendMessage("OnScrollEnd", SendMessageOptions.DontRequireReceiver);

        m_basePosition = newPos;
    }
}
