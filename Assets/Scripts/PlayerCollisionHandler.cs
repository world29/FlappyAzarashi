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
    }

    public CollisionHandlerType m_handlerType;

    PlayerController player;

    private void Start()
    {
        var go = GameObject.FindGameObjectWithTag("Player");
        player = go.GetComponent<PlayerController>();

        Debug.Assert(player != null);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (m_handlerType)
        {
            case CollisionHandlerType.Obstacle:
            case CollisionHandlerType.Damage:
                player.Clash();
                break;
            case CollisionHandlerType.DashAttack:
                player.HitBack();
                break;
            case CollisionHandlerType.Item:
                var item = collision.gameObject.GetComponent<Item>();
                player.PickupItem(item.m_itemType);
                break;
        }
    }
}
