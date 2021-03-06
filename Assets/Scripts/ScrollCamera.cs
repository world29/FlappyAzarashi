using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScrollCamera : MonoBehaviour
{
    public GameObject m_target;
    public float m_offsetX;

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

        var targetPos = m_target.transform.position;
        targetPos.x += m_offsetX;
        var translation = targetPos - transform.position;

        // Y, Z座標は変更しない
        translation.y = translation.z = 0;
        transform.Translate(translation);
    }
}
