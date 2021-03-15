using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 画面内いる間、一定間隔でプレイヤーに向けて弾を撃ってくる敵
public class EnemyShot : MonoBehaviour
{
    public float m_shotInterval = 2f;
    public float m_shotForce = 100f;
    public Rigidbody2D m_bulletObject;
    public Animator m_animator;

    GameObject m_player;

    void Start()
    {
        m_player = GameObject.FindWithTag("Player");

        StartCoroutine(ShotCoroutine());
    }

    bool IsInCamera(Camera camera)
    {
        Rect rect = new Rect(0, 0, 1, 1);

        var viewportPos = camera.WorldToViewportPoint(transform.position);

        return rect.Contains(viewportPos);
    }

    void Shot()
    {
        if (!IsInCamera(Camera.main)) return;

        var shotDirection = Vector2.left;

        // シーンにプレイヤーがいるならプレイヤーの方向へ発射する
        if (m_player)
        {
            shotDirection = m_player.transform.position - transform.position;
            shotDirection.Normalize();
        }
        Debug.DrawLine(transform.position, transform.position + (Vector3)shotDirection);

        var rb = GameObject.Instantiate(m_bulletObject, transform.position, transform.rotation);
        rb.AddForce(shotDirection * m_shotForce, ForceMode2D.Impulse);
    }

    IEnumerator ShotCoroutine()
    {
        float t = 0;

        while (true)
        {
            t += Time.deltaTime;

            if (t > m_shotInterval)
            {
                m_animator.SetTrigger("shot");
                t = 0;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private void OnDrawGizmos()
    {
        var player = GameObject.FindWithTag("Player");
        if (player)
        {
            var toPlayer = player.transform.position - transform.position;

            var line = toPlayer.normalized * 3;

            Debug.DrawLine(transform.position, transform.position + line, Color.red);
        }
    }
}
