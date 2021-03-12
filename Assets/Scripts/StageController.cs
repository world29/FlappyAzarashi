using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    public Rect m_stageArea;
    public Rect m_playArea;

    // 待機中の敵リスト
    List<Enemy> m_standbyEnemies = new List<Enemy>();
    List<Enemy> m_activeEnemies = new List<Enemy>();

    GameObject m_player;

    private void Start()
    {
        var enemies = gameObject.GetComponentsInChildren<Enemy>();
        foreach (var e in enemies)
        {
            m_standbyEnemies.Add(e);
        }

        foreach (var e in m_standbyEnemies)
        {
            e.gameObject.SetActive(false);
        }

        m_player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        DestroyEnemiesOutsideStage();

        Rect playAreaInWorld = m_playArea;
        playAreaInWorld.center += (Vector2)m_player.transform.position;

        ActivateEnemiesInPlayArea(playAreaInWorld);
    }

    void DestroyEnemiesOutsideStage()
    {
        var enemiesToDestroy = m_activeEnemies.FindAll(e => {
            return !m_stageArea.Contains((Vector2)e.gameObject.transform.position);
        });

        foreach (var e in enemiesToDestroy)
        {
            GameObject.Destroy(e.gameObject);

            m_activeEnemies.Remove(e);
        }
    }

    void ActivateEnemiesInPlayArea(Rect playArea)
    {
        var enemiesReady = m_standbyEnemies.FindAll(e => {
            return playArea.Contains((Vector2)e.gameObject.transform.position);
        });

        foreach (var e in enemiesReady)
        {
            e.gameObject.SetActive(true);

            m_activeEnemies.Add(e);
            m_standbyEnemies.Remove(e);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawCube(GameObject.FindWithTag("Player").transform.position, m_playArea.size);

        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawCube(m_stageArea.center, m_stageArea.size);
    }
}
