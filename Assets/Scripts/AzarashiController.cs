using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AzarashiController : MonoBehaviour
{
    public enum ShotType
    {
        Normal,
        ThreeWay,
    };

    Rigidbody2D rb2d;
    Animator animator;
    float angle;
    bool isDead;
    int m_bulletCount;

    public GameInput gameInput;
    public float maxHeight;
    public float flapVelocity;
    public float relativeVelocityX;
    public GameObject sprite;
    public ShotType m_shotType;
    public int m_initialBulletCount = 3;
    public Text m_shotButtonText;

    public BulletController bullet;

    int BulletCount
    {
        get { return m_bulletCount; }
        set
        {
            m_bulletCount = value;
            UpdateShotButtonText(m_shotType, value);
        }
    }

    void UpdateShotButtonText(ShotType shotType, int bulletCount)
    {
        string label = shotType == ShotType.Normal ? "NORMAL" : "3-Way";
        m_shotButtonText.text = label + "\n" + bulletCount;
    }

    public bool IsDead()
    {
        return isDead;
    }

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = sprite.GetComponent<Animator>();

        BulletCount = m_initialBulletCount;
    }

    void Start()
    {
    }

    void Update()
    {
        if (gameInput.GetButtonDown(GameInput.ButtonType.Main) && transform.position.y < maxHeight)
        {
            Flap();
        }

        if (gameInput.GetButtonDown(GameInput.ButtonType.Sub))
        {
            Shot();
        }

        ApplyAngle();

        animator.SetBool("flap", angle >= 0.0f && !isDead);
    }

    public void Flap()
    {
        if (isDead) return;

        if (rb2d.isKinematic) return;

        rb2d.velocity = new Vector2(0.0f, flapVelocity);
    }

    public void Shot()
    {
        if (isDead) return;

        if (rb2d.isKinematic) return;

        if (m_bulletCount == 0) return;

        if (m_shotType == ShotType.Normal)
        {
            var clone = GameObject.Instantiate<BulletController>(bullet);
            clone.transform.position = transform.position;
        }
        else if (m_shotType == ShotType.ThreeWay)
        {
            var clone = GameObject.Instantiate<BulletController>(bullet);
            clone.transform.position = transform.position;

            {
                var dir = Quaternion.AngleAxis(10, Vector3.forward) * Vector3.right;
                var cloned = GameObject.Instantiate<BulletController>(bullet, transform.position, Quaternion.identity);
                cloned.Direction = dir;
            }
            {
                var dir = Quaternion.AngleAxis(-10, Vector3.forward) * Vector3.right;
                var cloned = GameObject.Instantiate<BulletController>(bullet, transform.position, Quaternion.identity);
                cloned.Direction = dir;
            }
        }

        int consumed = m_shotType == ShotType.ThreeWay ? 3 : 1;

        BulletCount = Mathf.Max(0, m_bulletCount - consumed);
    }

    void ApplyAngle()
    {
        float targetAngle;

        if (isDead)
        {
            targetAngle = 180.0f;
        }
        else
        {
            targetAngle = Mathf.Atan2(rb2d.velocity.y, relativeVelocityX) * Mathf.Rad2Deg;
        }

        angle = Mathf.Lerp(angle, targetAngle, Time.deltaTime * 10.0f);

        sprite.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Item"))
        {
            return;
        }

        Camera.main.SendMessage("Clash");

        isDead = true;
    }

    public void SetSteerActive(bool active)
    {
        rb2d.isKinematic = !active;
    }

    public void ToggleShotType()
    {
        if (m_shotType == ShotType.Normal)
        {
            m_shotType = ShotType.ThreeWay;
        }
        else
        {
            m_shotType = ShotType.Normal;
        }

        UpdateShotButtonText(m_shotType, m_bulletCount);
    }

    public void SupplyBullet()
    {
        BulletCount += 6;
    }
}
