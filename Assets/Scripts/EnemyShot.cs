using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 画面内いる間、一定間隔でプレイヤーに向けて弾を撃ってくる敵
public class EnemyShot : MonoBehaviour
{
    public float m_shotInterval = 1f;
    public float m_shotSpeed = 1f;
    public Rigidbody2D m_bulletObject;

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

        var toPlayer = m_player.transform.position - transform.position;

        if (toPlayer.x >= 0) return;

        var rb = GameObject.Instantiate(m_bulletObject, transform.position, Quaternion.identity);
        rb.velocity = toPlayer.normalized * m_shotSpeed;
    }

    IEnumerator ShotCoroutine()
    {
        float t = 0;

        while (true)
        {
            t += Time.deltaTime;

            if (t > m_shotInterval)
            {
                Shot();
                t = 0;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private void OnDrawGizmos()
    {
        var toPlayer = GameObject.FindWithTag("Player").transform.position - transform.position;

        var line = toPlayer.normalized * 3;

        Debug.DrawLine(transform.position, transform.position + line, Color.red);
    }
}
