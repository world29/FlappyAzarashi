using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// コリジョンを Scene ビューで可視化するクラス
public class DebugCollision : MonoBehaviour
{
    public Color m_color = Color.white;

    enum CollisionShape
    {
        Box,
        Circle,
    }

    static readonly Dictionary<System.Type, CollisionShape> kTypeToCollisionShape = new Dictionary<System.Type, CollisionShape> {
            { typeof(BoxCollider2D), CollisionShape.Box},
            { typeof(CircleCollider2D), CollisionShape.Circle},
        };

    private void OnDrawGizmos()
    {
        var collider2d = GetComponent<Collider2D>();

        CollisionShape shapeType;
        if (!kTypeToCollisionShape.TryGetValue(collider2d.GetType(), out shapeType))
        {
            return;
        }

        var savedColor = Gizmos.color;

        Gizmos.color = m_color;

        switch (shapeType)
        {
            case CollisionShape.Box:
                {
                    var box = collider2d as BoxCollider2D;
                    Gizmos.DrawCube(box.bounds.center, box.bounds.size);
                }
                break;
            case CollisionShape.Circle:
                {
                    var circle = collider2d as CircleCollider2D;
                    Gizmos.DrawSphere(circle.bounds.center, circle.bounds.extents.x);
                }
                break;
        }

        Gizmos.color = savedColor;
    }
}
