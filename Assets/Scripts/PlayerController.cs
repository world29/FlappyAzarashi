using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb2d;
    float angle;
    bool isDead;
    SpriteTrailRenderer m_trailRenderer;
    AudioSource m_audioSource;
    GameObject m_gameController;

    Coroutine m_runningDashCoroutine;

    public GameInput gameInput;
    public float maxHeight;
    public float m_speed = 1;
    public float m_dashSpeed = 2;
    public float flapVelocity;
    public float dashVelocity;
    [Range(-180, 0)]
    public float m_dashAngle = -90;
    public float m_hitBackVelocity;
    public float relativeVelocityX;

    public GameObject dashAttackCollision;
    public GameObject damageCollision;
    public GameObject projectileReflectCollision;
    public float m_hitStopTime = 0.1f;
    public bool m_dashHitCameraShake = false;
    public bool m_jumpTrail = false;
    public Color m_flapTrailColor = Color.blue;
    public Color m_dashTrailColor = Color.red;

    public GameObject sprite;
    public AudioClip m_dashSound;
    public AudioClip m_dashHitSound;

    public bool IsDead()
    {
        return isDead;
    }

    public bool IsDash()
    {
        return m_runningDashCoroutine != null;
    }

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        m_trailRenderer = GetComponent<SpriteTrailRenderer>();
        m_audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        m_gameController = GameObject.FindWithTag("GameController");
    }

    void Update()
    {
        if (gameInput.GetButtonDown(GameInput.ButtonType.Main) && transform.position.y < maxHeight)
        {
            Flap();
        }

        if (gameInput.GetButtonDown(GameInput.ButtonType.Sub))
        {
            Dash();
        }

        float offsetAngle = IsDash() ? m_dashAngle : 0;
        ApplyAngle(offsetAngle);
    }

    public void Flap()
    {
        if (isDead) return;

        if (rb2d.isKinematic) return;

        StartCoroutine(FlapCoroutine());
    }

    public void Dash()
    {
        if (isDead) return;

        if (rb2d.isKinematic) return;

        m_runningDashCoroutine = StartCoroutine(DashCoroutine());
    }

    IEnumerator FlapCoroutine()
    {
        rb2d.velocity = new Vector2(m_speed, flapVelocity);

        const float trailTime = 0.3f;

        if (m_jumpTrail)
        {
            ChangeTrailColor(m_flapTrailColor);
            m_trailRenderer.m_TrailTime = trailTime;
            m_trailRenderer.SetEnabled(true);
        }

        yield return new WaitForSeconds(trailTime);

        if (m_jumpTrail)
        {
            m_trailRenderer.SetEnabled(false);
        }
    }

    IEnumerator DashCoroutine()
    {
        //
        OnBeginDash();

        yield return new WaitForSeconds(0.5f); // SpriteTrailRenderer の残像持続時間と合わせる

        OnEndDash(false);
    }

    IEnumerator HitStopCoroutine()
    {
        if (m_hitStopTime == 0)
        {
            yield break;
        }

        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(m_hitStopTime);

        Time.timeScale = 1;
    }

    IEnumerator HitBackCoroutine(Collision2D hit)
    {
        if (hit.contactCount == 0)
        {
            yield break;
        }

        var contactPoint = hit.contacts[0];

        var hitBackVelocity = Vector3.Reflect(rb2d.velocity, contactPoint.normal);
        Debug.DrawLine(transform.position, transform.position + hitBackVelocity, Color.red, 3);

        rb2d.velocity = hitBackVelocity;

        const float trailTime = 0.3f;

        yield return new WaitForSeconds(trailTime);

        m_trailRenderer.SetEnabled(false);
    }

    void OnBeginDash()
    {
        rb2d.velocity = new Vector2(m_dashSpeed, dashVelocity);

        dashAttackCollision.SetActive(true);
        damageCollision.SetActive(false);
        projectileReflectCollision.SetActive(true);

        ChangeTrailColor(m_dashTrailColor);
        m_trailRenderer.m_TrailTime = 0.5f;
        m_trailRenderer.SetEnabled(true);

        m_audioSource.PlayOneShot(m_dashSound);
    }

    void OnEndDash(bool canceled)
    {
        if (!canceled)
        {
            m_trailRenderer.SetEnabled(false);
        }
        projectileReflectCollision.SetActive(false);
        damageCollision.SetActive(true);
        dashAttackCollision.SetActive(false);

        m_runningDashCoroutine = null;
    }

    void ApplyAngle(float offsetAngleInDegree)
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

        targetAngle += offsetAngleInDegree;

        angle = Mathf.Lerp(angle, targetAngle, Time.deltaTime * 10.0f);

        sprite.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
    }

    void ChangeTrailColor(Color c)
    {
        m_trailRenderer.SetNextColor(c);
    }

    public void HitBack(Collision2D hit)
    {
        if (m_runningDashCoroutine == null)
        {
            return;
        }

        // ダッシュをキャンセルする
        StopCoroutine(m_runningDashCoroutine);
        OnEndDash(true);

        // ヒット時の演出
        m_audioSource.PlayOneShot(m_dashHitSound);
        if (m_dashHitCameraShake)
        {
            Camera.main.SendMessage("Shake");
        }

        StartCoroutine(HitStopCoroutine());

        StartCoroutine(HitBackCoroutine(hit));
    }

    public void Clash()
    {
        if (isDead) return;

        Camera.main.SendMessage("Clash");

        isDead = true;
    }

    public void PickupItem(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.StageClear:
                m_gameController.SendMessage("GameClear");
                break;
        }
    }

    public void SetSteerActive(bool active)
    {
        rb2d.velocity = Vector2.zero;
        rb2d.isKinematic = !active;
    }

    public void Respawn()
    {
        isDead = false;
    }
}
