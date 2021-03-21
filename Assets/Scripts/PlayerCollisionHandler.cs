using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 衝突時の処理をまとめて記述し、インスペクターで切り替えられるにする
[RequireComponent(typeof(Collider2D))]
public class PlayerCollisionHandler : MonoBehaviour
{
    public enum CollisionHandlerType
    {
        Obstacle,
        Damage,
        DashAttack,
        Item,
        ProjectileReflect,
    }

    public CollisionHandlerType m_handlerType;

    PlayerController player;

    private void Start()
    {
        var go = GameObject.FindGameObjectWithTag("Player");
        player = go.GetComponent<PlayerController>();

        Debug.Assert(player != null);
    }

    void ReflectEnemyProjectile(Collision2D collision)
    {
        var go = collision.gameObject;

        go.layer = LayerMask.NameToLayer("PlayerShot");
        var ps = go.GetComponentInChildren<ParticleSystem>();
        var main = ps.main;
        main.startColor = new ParticleSystem.MinMaxGradient(Color.blue);

#if DEBUG
        var rb = go.GetComponent<Rigidbody2D>();
        Debug.DrawLine(go.transform.position, go.transform.position + (Vector3)rb.velocity, Color.blue);
#endif
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(m_handlerType);

        switch (m_handlerType)
        {
            case CollisionHandlerType.Obstacle:
            case CollisionHandlerType.Damage:
                player.Clash(collision);
                break;
            case CollisionHandlerType.DashAttack:
                player.HitBack(collision);
                break;
            case CollisionHandlerType.Item:
                var item = collision.gameObject.GetComponent<Item>();
                player.PickupItem(item.m_itemType);
                break;
            case CollisionHandlerType.ProjectileReflect:
                ReflectEnemyProjectile(collision);
                break;
        }
    }
}
