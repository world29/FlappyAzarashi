using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 画面内いる間、一定間隔でプレイヤーに向けて弾を撃ってくる敵
public class EnemyShot : MonoBehaviour
{
    public EnemyBullet m_bullet;

    public float m_speed = 10;

    GameObject m_player;

    void Start()
    {
        m_player = GameObject.FindWithTag("Player");
    }

    void Shot()
    {
        var shotDirection = Vector2.left;

        // シーンにプレイヤーがいるならプレイヤーの方向へ発射する
        if (m_player)
        {
            shotDirection = m_player.transform.position - transform.position;
            shotDirection.Normalize();
        }

        var bullet = GameObject.Instantiate(m_bullet, transform.position, transform.rotation);
        bullet.Velocity = shotDirection * m_speed;
    }
}
