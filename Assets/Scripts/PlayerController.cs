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
    SpriteRenderer m_spriteRenderer;
    Animator m_animator;
    PlayerAction m_input;

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
    public AudioClip m_breakSound;
    public ParticleSystem m_breakParticle;

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
        m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        m_animator = GetComponent<Animator>();

        m_input = new PlayerAction();
    }

    private void OnEnable()
    {
        m_input.Enable();
    }

    private void OnDisable()
    {
        m_input.Disable();
    }

    private void OnDestroy()
    {
        m_input.Disable();
    }

    void Start()
    {
        m_gameController = GameObject.FindWithTag("GameController");
    }

    void Update()
    {
        if (m_input.PlatformAction.Jump.triggered)
        {
            Flap();

            Debug.Log("Flap");
        }

        if (m_input.PlatformAction.Dash.triggered)
        {
            Dash();

            Debug.Log("Dash");
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

        m_animator.SetTrigger("dash");

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
        if (m_dashHitCameraShake)
        {
            Camera.main.SendMessage("Shake");
        }

        StartCoroutine(HitStopCoroutine());

        StartCoroutine(HitBackCoroutine(hit));
    }

    public void Clash(Collision2D collision)
    {
        if (isDead) return;

        Camera.main.SendMessage("Clash");

        StartCoroutine(PlayerBreakCoroutine(collision));

        isDead = true;
    }

    IEnumerator PlayerBreakCoroutine(Collision2D collision)
    {
        var contact = collision.contacts[0];
        m_spriteRenderer.gameObject.SendMessage("BreakSprite", contact.point, SendMessageOptions.DontRequireReceiver);

        GameObject.Instantiate(m_breakParticle, transform.position, transform.rotation);

        //AudioSource.PlayClipAtPoint(m_breakSound, transform.position);
        m_audioSource.clip = m_breakSound;
        m_audioSource.Play();

        m_spriteRenderer.color = new Color(0, 0, 0, 0);
        m_animator.enabled = false;
        var colliders = GetComponentsInChildren<Collider2D>();
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }
        GetComponent<Rigidbody2D>().simulated = false;

        yield return new WaitForSecondsRealtime(m_breakSound.length);

        //gameObject.SetActive(false);
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

        m_spriteRenderer.color = Color.white;

        m_animator.enabled = true;
        var colliders = GetComponentsInChildren<Collider2D>(true);
        foreach (var collider in colliders)
        {
            collider.enabled = true;
        }

        GetComponent<Rigidbody2D>().simulated = true;

        GetComponentInChildren<BreakableSprite>().InitializePieces();
    }
}
