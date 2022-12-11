using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public Core core;
    public SpriteRenderer sprite;
    public Transform shine;
    public SpriteRenderer shineSprite;
    public BoxCollider2D box;
    public Rigidbody2D rb;

    public int type = 0;
    public int[] pointValues = new int[] { 100, 200, 300, 500, 800, 1000 };
    public Sprite[] foodSprites = new Sprite[] { };

    private float lifeTime = 0;
    private const float LIFETIME_MAX = 15f;

    public float originY = 0;

    public AudioClip sfxAppear;
    public AudioClip sfxPickup;

    private bool flashBuffer = false;

    public void Awake()
    {
        core = GameObject.FindWithTag("Core").GetComponent<Core>();
        sprite = GetComponent<SpriteRenderer>();
        if (transform.childCount > 0)
        {
            shine = transform.GetChild(0);
            shineSprite = shine.GetComponent<SpriteRenderer>();
        }
        box = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        box.enabled = false;
    }

    public virtual void Instance(int newType, Vector2 pos)
    {
        core.PlaySound(sfxAppear, 0.5f);
        originY = pos.y;
        transform.position = pos;
        type = newType;
        sprite.sprite = foodSprites[newType];
        sprite.enabled = false;
        shineSprite.enabled = false;
        switch (newType)
        {
            default:
            case 0:
                sprite.color = core.palette[4];
                break;
            case 1:
                sprite.color = core.palette[1];
                shineSprite.color = core.palette[7];
                break;
            case 2:
                sprite.color = core.palette[2];
                break;
            case 3:
                sprite.color = core.palette[6];
                shine.localPosition = new Vector2(-0.114f, -0.222f);
                break;
            case 4:
                sprite.color = core.palette[5];
                shine.localPosition = new Vector2(-0.111f, -0.333f);
                break;
        }
    }

    void Update()
    {
        if (lifeTime < Mathf.PI * 0.25f)
        {
            sprite.enabled = !sprite.enabled;
            if (shineSprite != null)
                shineSprite.enabled = sprite.enabled;
            transform.position = new Vector2(transform.position.x, originY + Mathf.Cos((lifeTime - 0.4f) * 4));
        }
        else if (lifeTime > Mathf.PI * 0.25f && lifeTime < LIFETIME_MAX - 4)
        {
            sprite.enabled = true;
            if (shineSprite != null)
                shineSprite.enabled = true;
            transform.position = new Vector2(transform.position.x, originY);
            box.enabled = true;
        }
        else if (lifeTime > LIFETIME_MAX - 4 && lifeTime < LIFETIME_MAX)
        {
            flashBuffer = !flashBuffer;
            if (flashBuffer)
            {
                sprite.enabled = !sprite.enabled;
                if (shineSprite != null)
                    shineSprite.enabled = sprite.enabled;
            }
        }
        else
        {
            core.spawn.activePickups--;
            Destroy(gameObject);
        }
        lifeTime += Time.deltaTime;
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Prang"))
        {
            core.IncrementScore(pointValues[type]);
            core.spawn.activePickups--;
            core.PlaySound(sfxPickup);
            Destroy(gameObject);
        }
    }
}
