﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool m_lookatPlayer;
    [Range(0, 90)]
    public float m_lookatRotationLimit = 30;
    public float m_lookatRotationSpeed = 10;

    GameObject gameController;
    GameObject m_player;

    void Start()
    {
        gameController = GameObject.FindWithTag("GameController");
        m_player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        // プレイヤーのほうを向く
        if (m_lookatPlayer)
        {
            Quaternion targetRotation = Quaternion.identity;

            Vector3 enemyDirection = Vector3.left;
            var toPlayer = m_player.transform.position - transform.position;

            var angle = Vector3.SignedAngle(enemyDirection, toPlayer, Vector3.forward);

            bool inSight = (angle < m_lookatRotationLimit && angle > -m_lookatRotationLimit);
            if (inSight)
            {
                targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

            var step = m_lookatRotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var contact = collision.contacts[0];
            gameObject.SendMessage("BreakSprite", contact.point, SendMessageOptions.DontRequireReceiver);

            gameObject.SetActive(false);

            return;
        }

        gameObject.SetActive(false);
    }

#if DEBUG
    private void OnDrawGizmos()
    {
        var handlePos = transform.position;

        UnityEditor.Handles.color = new Color(1, 0, 0, 0.2f);
        UnityEditor.Handles.DrawSolidArc(
            handlePos,
            Vector3.back,
            Vector3.left,
            m_lookatRotationLimit,
            UnityEditor.HandleUtility.GetHandleSize(handlePos));

        UnityEditor.Handles.DrawSolidArc(
            handlePos,
            Vector3.back,
            Vector3.left,
            -m_lookatRotationLimit,
            UnityEditor.HandleUtility.GetHandleSize(handlePos));
    }
#endif
}
