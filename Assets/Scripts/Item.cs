using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        ShotTypeChange,
        BulletSupply,
    }

    public ItemType m_itemType;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        var playerObject = collision.gameObject;

        switch (m_itemType)
        {
            case ItemType.ShotTypeChange:
                playerObject.SendMessage("ToggleShotType");
                break;
            case ItemType.BulletSupply:
                playerObject.SendMessage("SupplyBullet");
                break;
        }

        gameObject.SetActive(false);
    }
}
