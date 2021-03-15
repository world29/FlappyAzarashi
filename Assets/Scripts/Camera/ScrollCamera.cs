using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScrollCamera : MonoBehaviour
{
    public GameObject m_target;
    public float m_offsetX;
    public float m_speed = 10;

    private void Update()
    {
        if (Application.isEditor)
        {
            UpdateCameraPosition();
        }
    }

    private void LateUpdate()
    {
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        if (m_target == null)
        {
            return;
        }

        var targetPos = new Vector2(m_target.transform.position.x + m_offsetX, transform.position.y);

        var step = m_speed * Time.deltaTime;
        var eyePos = Vector2.MoveTowards((Vector2)transform.position, targetPos, step);

        // Y, Z座標は変更しない
        transform.position = new Vector3(eyePos.x, transform.position.y, transform.position.z);
    }
}
