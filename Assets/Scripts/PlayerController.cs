using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb2d;
    float angle;
    bool isDead;
    SpriteTrailRenderer m_trailRenderer;
    AudioSource m_audioSource;

    public GameInput gameInput;
    public float maxHeight;
    public float flapVelocity;
    public float dashVelocity;
    public float relativeVelocityX;
    public GameObject sprite;
    public AudioClip m_dashSound;

    public bool IsDead()
    {
        return isDead;
    }

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        m_trailRenderer = GetComponent<SpriteTrailRenderer>();
        m_audioSource = GetComponent<AudioSource>();
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
            Dash();
        }

        ApplyAngle();
    }

    public void Flap()
    {
        if (isDead) return;

        if (rb2d.isKinematic) return;

        rb2d.velocity = new Vector2(rb2d.velocity.x, flapVelocity);
    }

    public void Dash()
    {
        if (isDead) return;

        if (rb2d.isKinematic) return;

        StartCoroutine(DashCoroutine());
    }

    IEnumerator DashCoroutine()
    {
        //
        rb2d.velocity = new Vector2(rb2d.velocity.x, dashVelocity);

        m_trailRenderer.SetEnabled(true);

        m_audioSource.PlayOneShot(m_dashSound);

        Dictionary<ScrollObject, float> scrollObjects = new Dictionary<ScrollObject, float>();

        ScrollObject[] objects = FindObjectsOfType<ScrollObject>();
        foreach (var obj in objects)
        {
            scrollObjects.Add(obj, obj.speed);
        }

        foreach (var so in scrollObjects) so.Key.speed *= 3;

        yield return new WaitForSeconds(0.5f); // SpriteTrailRenderer の残像持続時間と合わせる

        // 速度を戻す
        foreach (var so in scrollObjects) so.Key.speed = so.Value;

        yield return new WaitForSeconds(0.2f);

        m_trailRenderer.SetEnabled(false);
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
}
