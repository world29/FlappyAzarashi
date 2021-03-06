using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    ShotTypeChange,
    BulletSupply,
    StageClear,
}

public class Item : MonoBehaviour
{
    public ItemType m_itemType;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.SetActive(false);
    }
}
