using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnemyActivation : MonoBehaviour
{
    public GameObject m_tareget;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_tareget.SetActive(true);
    }
}
