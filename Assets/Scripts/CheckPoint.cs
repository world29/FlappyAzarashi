using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(AudioSource))]
public class CheckPoint : MonoBehaviour
{
    public AudioClip m_sound;
    public GameObject[] m_enemies;

    AudioSource m_audioSource;
    GameController m_gameController;
    bool m_saved = false;

    Dictionary<GameObject, Vector3> m_propsPosition = new Dictionary<GameObject, Vector3>();
    Dictionary<Vector3, GameObject> m_enemyPrefabAndPosition = new Dictionary<Vector3, GameObject>();

    private void Start()
    {
        m_gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        m_audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (m_saved) return;

        FlashEffect.Play();

        m_audioSource.PlayOneShot(m_sound);

        m_gameController.SetRespawnPoint(this);

        SpawnEnemies();
    }

    public void SetSaved()
    {
        m_saved = true;
    }

    public void SavePropsPosition()
    {
        var props = GameObject.FindGameObjectsWithTag("Prop");
        foreach (var obj in props)
        {
            m_propsPosition.Add(obj, obj.transform.position);
        }
    }

    public void RestorePropsPosition()
    {
        foreach (var entry in m_propsPosition)
        {
            var obj = entry.Key;
            var pos = entry.Value;

            obj.transform.position = pos;
        }
    }

    public void SpawnEnemies()
    {
        // 初回呼び出しの際に登録されたオブジェクトをオリジナルとして登録する
        if (m_enemyPrefabAndPosition.Count < m_enemies.Length)
        {
            foreach (var e in m_enemies)
            {
                m_enemyPrefabAndPosition.Add(e.transform.position, e);
            }
        }

        foreach (var entry in m_enemyPrefabAndPosition)
        {
            var pos = entry.Key;
            var obj = entry.Value;

            var cloneObject = GameObject.Instantiate(obj, pos, Quaternion.identity, obj.transform.parent);
            cloneObject.GetComponent<Enemy>().Activate();
        }
    }
}
