using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 正面に扇状に弾を発射する
public class EnemyDanmaku : MonoBehaviour
{
    public EnemyBullet m_bullet;

    public int m_bulletCount = 7;

    public float m_bulletSpeed = 1;

    [ContextMenu("Shot")]
    public void Shot()
    {
        StartCoroutine(ShotCoroutine());
    }

    IEnumerator ShotCoroutine()
    {
        Vector2 baseDirection = Vector2.up;

        const float interval = 0.2f;

        float angle_step = 180.0f / (m_bulletCount - 1);

        for (int i = 0; i < m_bulletCount; i++)
        {
            var rot = Quaternion.AngleAxis(i * angle_step, Vector3.forward);
            var dir = rot * baseDirection;

            var bullet = GameObject.Instantiate(m_bullet, transform.position, Quaternion.identity);
            bullet.Velocity = dir * m_bulletSpeed;

            var constraint = bullet.gameObject.GetComponent<PositionConstraintObject>();

            yield return new WaitForSeconds(interval);
        }
    }
}
