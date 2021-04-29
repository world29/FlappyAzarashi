using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WarpPoint : MonoBehaviour
{
    public Transform m_warpDestination;
    public string m_warpSe;
    public UnityEvent m_onWarp;

    public PlayerController m_player;
    public Camera m_camera;

    private List<Background> m_props;

    // singleton
    public static WarpPoint Instance { get; private set; }

    bool InitializeAsSingleton()
    {
        if (Instance == null)
        {
            Instance = this;

            return true;
        }
        else
        {
            Debug.Assert(false, "WarpPoint is singleton");

            Destroy(this);

            return false;
        }
    }

    private void Awake()
    {
        if (!InitializeAsSingleton()) return;

        if (m_player == null)
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            m_player = go.GetComponent<PlayerController>();
        }

        if (m_camera == null)
        {
            m_camera = Camera.main;
        }

        {
            m_props = new List<Background>();

            var objects = GameObject.FindGameObjectsWithTag("Prop");
            foreach (var o in objects)
            {
                m_props.Add(o.GetComponent<Background>());
            }
        }
    }

    private void OnEnable()
    {
        GameDataAccessor.OnStageChanged.AddListener(OnStageChanged);
    }

    private void OnDisable()
    {
        GameDataAccessor.OnStageChanged.RemoveListener(OnStageChanged);
    }

    public void LateUpdate()
    {
        // プレイヤーがこのオブジェクトを通過したときにワープする
        if (m_player.transform.position.x > transform.position.x)
        {
            DoWarp();

            //TODO: 難易度を変更する
            // ステージ変更イベントによってブロックが生成されるため、難易度の変更はステージ変更よりも前に行う必要がある。
            DifficultyBehaviour.Instance.NextLevel();

            GameDataAccessor.CurrentStageId = (StageId)((int)(GameDataAccessor.CurrentStageId + 1) % (int)StageId.Num);
        }
    }

    private void DoWarp()
    {
        var offsetX = m_warpDestination.position.x - m_player.transform.position.x;

        {
            var newPos = m_player.transform.position;
            newPos.x += offsetX;
            m_player.transform.position = newPos;
        }

        {
            var newPos = m_camera.transform.position;
            newPos.x += offsetX;
            m_camera.transform.position = newPos;
        }

        foreach (var prop in m_props)
        {
#if false
            var distance = prop.transform.position - transform.position;

            prop.transform.position = m_warpDestination.position + distance;
#else
            var newPos = prop.transform.position;
            newPos.x += offsetX;
            prop.transform.position = newPos;
#endif
            prop.SkipNextUpdate();
        }

        Sound.GetInstance().PlaySe(m_warpSe);

        m_onWarp.Invoke();
    }

    public void OnStageChanged(StageId id)
    {
        // ステージ開始時に非アクティブ化する。
        // 各ステージの敵を倒すとアクティブ化される。

        gameObject.SetActive(false);
    }
}
