using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(SpriteRenderer))]
public class BreakableSprite : MonoBehaviour
{
    // 吹き飛びやすさ
    public float m_collisionImpulseScale = 1;

    // 破片の数
    public int m_polygonCount = 10;

    // 破片が属するレイヤー。カスタムエディタでフィールドを入力する
    [Layer]
    public int m_pieceLayer;

    SpriteRenderer m_spriteRenderer;
    List<GameObject> m_pieces = new List<GameObject>();

    private void Start()
    {
        // 事前に破壊されたスプライトを生成し、親子付けしておく
        m_spriteRenderer = GetComponent<SpriteRenderer>();

        // フリップは非対応
        Debug.Assert(m_spriteRenderer.flipX == false && m_spriteRenderer.flipY == false);

        // ボロノイ領域の生成
        var voronoi = ComputeRandomVoronoi(m_polygonCount, m_spriteRenderer.sprite.rect);

        // 破片スプライトの作成
        var regions = voronoi.Regions();

        for (int i = 0; i < regions.Count; i++)
        {
            var sprite = GenerateSpritePiece(m_spriteRenderer.sprite, regions[i]);

            // 子オブジェクトとして生成
            var go = new GameObject("piece");
            go.layer = m_pieceLayer;

            // コンポーネントの設定
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;

            // ピボット位置を原点として配置されるため、補正する
            float offsetX = sprite.pivot.x / sprite.pixelsPerUnit - sprite.bounds.extents.x;
            float offsetY = sprite.pivot.y / sprite.pixelsPerUnit - sprite.bounds.extents.y;
            go.transform.position = transform.position + new Vector3(offsetX, offsetY);
            go.transform.parent = transform;

            go.AddComponent<PolygonCollider2D>(); // Sprite の物理形状を元にコライダを生成する
            var rb = go.AddComponent<Rigidbody2D>();
            rb.useAutoMass = true;

            go.SetActive(false);

            m_pieces.Add(go);
        }
    }

    private void OnDestroy()
    {
        foreach (var obj in m_pieces)
        {
            Destroy(obj);
        }
    }

    private Sprite GenerateSpritePiece(Sprite base_sprite, IReadOnlyList<Vector2f> region)
    {
        // ボロノイ領域からスプライトのポリゴンを作成する
        var vertices = new Vector2[region.Count];
        var triangles = new ushort[3 * (region.Count - 2)];

        for (int i = 0; i < region.Count; i++)
        {
            vertices[i] = new Vector2(region[i].x, region[i].y);
        }

        int t = 0;
        for (int i = 1; i < region.Count - 1; i++)
        {
            triangles[t++] = 0;
            triangles[t++] = (ushort)i;
            triangles[t++] = (ushort)(i + 1);
        }

        // 三角形の重心と面積を計算し、ポリゴン全体の重心を求める
        Vector2 polygon_centroid = Vector2.zero;
        float polygon_area = 0;
        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector2 A = vertices[triangles[i]];
            Vector2 B = vertices[triangles[i + 1]];
            Vector2 C = vertices[triangles[i + 2]];

            var triangle_centroid = new Vector2((A.x + B.x + C.x) / 3, (A.y + B.y + C.y) / 3);

            Vector2 AB = B - A;
            Vector2 AC = C - A;

            var area = Mathf.Abs(Vector3.Cross(AB, AC).z) * 0.5f;

            polygon_area += area;
            polygon_centroid = Vector2.Lerp(polygon_centroid, triangle_centroid, area / polygon_area);
        }

        // スプライト生成
        var texture = base_sprite.texture;

        var pivot = new Vector2(polygon_centroid.x / texture.width, polygon_centroid.y / texture.height);

        Sprite sprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            pivot,
            base_sprite.pixelsPerUnit);

        sprite.OverrideGeometry(vertices, triangles);
        sprite.OverridePhysicsShape(new List<Vector2[]> { vertices });

        return sprite;
    }

    private csDelaunay.Voronoi ComputeRandomVoronoi(int num_points, Rect bounds)
    {
        var plotBounds = new Rectf(bounds.min.x, bounds.min.y, bounds.size.x, bounds.size.y);

        var sites = new List<Vector2f>();
        for (int i = 0; i < m_polygonCount; i++)
        {
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);

            sites.Add(new Vector2f(x, y));
        }

        return new csDelaunay.Voronoi(sites, plotBounds);
    }

    IEnumerator BreakSpriteCoroutine(Vector2 contactPoint)
    {
        foreach (var go in m_pieces)
        {
            go.transform.parent = null;
        }

        foreach (var go in m_pieces)
        {
            go.SetActive(true);

            Vector2 forceDirection = (Vector2)go.transform.position - contactPoint;
            Vector2 force = forceDirection.normalized * m_collisionImpulseScale;

            var rb = go.GetComponent<Rigidbody2D>();
            if (rb)
            {
                Debug.DrawLine(contactPoint, contactPoint + force, Color.cyan, 5);
                rb.AddForce(force, ForceMode2D.Impulse);
            }
        }

        yield return null;
    }

    public void BreakSprite(Vector2 contactPoint)
    {
        // 破片オブジェクトを吹き飛ばす
        StartCoroutine(BreakSpriteCoroutine(contactPoint));
    }

#if DEBUG
    [ContextMenu("BreakSprite")]
    public void BreakSpriteRandomPoint()
    {
        var min = m_spriteRenderer.sprite.bounds.min;
        var max = m_spriteRenderer.sprite.bounds.max;

        BreakSprite(new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y)));
    }
#endif
}
