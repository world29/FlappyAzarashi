using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 画面内いる間、一定間隔でプレイヤーに向けて弾を撃ってくる敵
public class EnemyShot : MonoBehaviour
{
    public float shotInterval = 0.5f;
    public BulletController bullet;

    bool isActive;
    float timeSinceActivated;
    float position_y;

    float shotTimer;

    GameObject gameController;
    GameObject player;

    void Start()
    {
        gameController = GameObject.FindWithTag("GameController");
        player = GameObject.FindWithTag("Player");

        isActive = false;
        position_y = transform.position.y;
        shotTimer = shotInterval;
    }

    private void Update()
    {
        if (!isActive) return;

        // 移動
        const float amplitude = 1;
        const float frequency = 0.3f;

        float elapsed = Time.timeSinceLevelLoad - timeSinceActivated;

        float offset = Mathf.Sin(2 * Mathf.PI * elapsed * frequency) * amplitude;

        var newPosition = transform.localPosition;
        newPosition.y = position_y + offset;
        transform.localPosition = newPosition;

        // ショット
        shotTimer -= Time.deltaTime;
        if (shotTimer < 0)
        {
            Shot();
            shotTimer = shotInterval;
        }
    }

    void Shot()
    {
        var vec = player.transform.position - transform.position;

        if (vec.x >= 0) return;

        var cloned = GameObject.Instantiate(bullet, transform.position, Quaternion.identity, transform);
        cloned.Direction = vec.normalized;
    }

    private void OnBecameVisible()
    {
        isActive = true;
        timeSinceActivated = Time.timeSinceLevelLoad;
    }

    private void OnBecameInvisible()
    {
        isActive = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        gameObject.SetActive(false);
    }
}
