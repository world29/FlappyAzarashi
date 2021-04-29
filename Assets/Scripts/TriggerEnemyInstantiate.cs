using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnemyInstantiate : MonoBehaviour
{
    public GameObject m_enemyPrefab;

    public Transform m_instantiatePosition;

    private bool m_triggered = false;

    private void LateUpdate()
    {
        if (m_triggered) return;

        var cameraPosX = Camera.main.transform.position.x;

        if (cameraPosX >= transform.position.x)
        {
            GameObject.Instantiate(m_enemyPrefab, m_instantiatePosition.position, Quaternion.identity);

            m_triggered = true;
        }
    }
}
