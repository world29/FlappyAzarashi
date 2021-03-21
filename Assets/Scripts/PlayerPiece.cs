using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPiece : MonoBehaviour
{
    public Vector3 m_respawnPosition;

    public AnimationCurve m_speed;

    Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    IEnumerator RespawnCoroutine(float delay)
    {
        gameObject.layer = LayerMask.NameToLayer("Default");

        yield return new WaitForSeconds(delay);
    }
}
