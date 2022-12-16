using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prang : MonoBehaviour
{
    public Core core;
    public BoxCollider2D box;
    public Rigidbody2D rb;
    public SpriteRenderer sprite;

    public float speed = 6f;
    public float speedMod = 2f;

    public Transform powerupObj;
    public SpriteRenderer powerupSprite;
    private float powerupPosTimer = 0;
    private float powerupFlashTimer = 0;

    private float powerupTimer = 0;
    private readonly float powerupTimerMax = 15f;
    private bool powerupFlashState = false;

    public Sprite[] sprites;
    private bool flashState = false;

    private float animTimer;

    public AudioClip death;
    public AudioClip respawn;

    void Start()
    {
        core = GameObject.FindWithTag("Core").GetComponent<Core>();
        box = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        sprite.color = core.palette[3];

        powerupObj = transform.GetChild(0);
        powerupSprite = powerupObj.GetComponent<SpriteRenderer>();
        powerupSprite.color = core.palette[7];
    }

    void Update()
    {
        if (!core.deathState)
        {
            if (Input.GetKey(KeyCode.Mouse0))
                core.lastMousePoint = core.cam.ScreenToWorldPoint(Input.mousePosition);
            if (core.lastMousePoint != new Vector2(99, 99))
                rb.position += speed * (core.powerupState == 2 ? speedMod : 1) * Time.deltaTime * (core.lastMousePoint - rb.position).normalized;
            if (Vector2.Distance(rb.position, core.lastMousePoint) < 0.125f)
                core.lastMousePoint = new Vector2(99, 99);

            if (!Input.GetKey(KeyCode.Mouse0) && !(Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0))
            {
                core.lastMousePoint = new Vector2(99, 99);
                rb.position += speed * (core.powerupState == 2 ? speedMod : 1) * Time.deltaTime *
                    new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            }

            if (rb.position.x > core.bounds.x || rb.position.x < -core.bounds.x)
                rb.position = new Vector2(core.bounds.x * Mathf.Sign(rb.position.x), rb.position.y);
            if (rb.position.y > core.bounds.y || rb.position.y < -core.bounds.y)
                rb.position = new Vector2(rb.position.x, core.bounds.y * Mathf.Sign(rb.position.y));

            if (core.powerupState == 1)
            {
                core.powerupState = 2;
                powerupTimer = powerupTimerMax;
                powerupSprite.enabled = true;
            }
            powerupTimer = Mathf.Clamp(powerupTimer - Time.deltaTime, 0, Mathf.Infinity);
            if (core.powerupState == 2)
            {
                if (powerupTimer <= 4)
                {
                    powerupFlashState = !powerupFlashState;
                    if (powerupFlashState)
                        powerupSprite.enabled = !powerupSprite.enabled;
                }
                if (powerupTimer <= 0)
                {
                    core.powerupState = 0;
                    sprite.color = core.palette[3];
                }
            }

            if (core.deathTimer > 0)
            {
                core.deathTimer -= 2.5f * Time.deltaTime;
                sprite.enabled = !sprite.enabled;
                sprite.color = core.palette[3];
                if (core.deathTimer <= 0)
                {
                    core.deathTimer = 0;
                    sprite.enabled = true;
                }
            }

            if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0 && core.lastMousePoint == new Vector2(99, 99))
            {
                animTimer = 0;
                sprite.sprite = sprites[0];
            }
            else
            {
                animTimer += Time.deltaTime;
                if (animTimer > 1f)
                    animTimer--;
                if (animTimer <= 0.25f)
                    sprite.sprite = sprites[1];
                else if (animTimer <= 0.5f)
                    sprite.sprite = sprites[2];
                else if (animTimer <= 0.75f)
                    sprite.sprite = sprites[1];
                else if (animTimer <= 1f)
                    sprite.sprite = sprites[0];
            }
        }
        else
        {
            core.deathTimer += Time.deltaTime;
            flashState = !flashState;
            if (flashState)
                sprite.enabled = !sprite.enabled;
            if (core.deathTimer < 0.6f)
                sprite.sprite = sprites[3];
            else if (core.deathTimer >= 0.45f && core.deathTimer < 0.9f)
            {
                sprite.color = core.palette[0];
                sprite.sprite = sprites[4];
            }
            else
                sprite.color = core.palette[7];
            if (core.deathTimer > 2f)
            {
                if (core.lives > 0)
                {
                    box.enabled = true;
                    core.deathState = false;
                    transform.position = Vector2.zero;
                    core.PlaySound(respawn, 0.5f);
                    core.DecrementLives();
                }
                else
                {

                }
            }
        }

        powerupPosTimer += Time.deltaTime * 4;
        if (powerupPosTimer >= Core.TAU)
            powerupPosTimer -= Core.TAU;
        powerupObj.transform.localPosition = new Vector2(0.667f, 0.667f + Mathf.Sin(powerupPosTimer) * 0.125f);
        if (core.powerupState != 0)
        {
            powerupFlashTimer += Time.deltaTime;
            if (powerupFlashTimer > 0.5f)
                powerupFlashTimer -= 0.5f;
            powerupSprite.color = core.palette[powerupFlashTimer >= 0.25f ? 0 : 6];
            sprite.color = core.palette[powerupFlashTimer >= 0.25f ? 0 : 3];
        }
        else
            powerupSprite.color = core.palette[7];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hazard") && (core.powerupState == 0 || collision.name.Contains("Courier")))
        {
            box.enabled = false;
            core.powerupState = 0;
            powerupTimer = 0;
            core.deathState = true;
            core.lastMousePoint = new Vector2(99, 99);
            core.spawn.RetractEnemyRate();
            core.PlaySound(death);
        }
    }
}
