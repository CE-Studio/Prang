using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Core core;
    public Prang prang;
    public SpawnManager spawn;
    public BoxCollider2D box;
    public SpriteRenderer sprite;

    public Sprite[] enemySprites;
    public Sprite[] explosionSprites;

    public int pointValue = 0;

    public AudioClip defeat;

    public bool active = true;
    
    public virtual void Start()
    {
        core = GameObject.FindWithTag("Core").GetComponent<Core>();
        prang = GameObject.FindWithTag("Prang").GetComponent<Prang>();
        spawn = GameObject.FindWithTag("Core").GetComponent<SpawnManager>();
        box = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public virtual void Instance(Vector2 pos, string data)
    {
        transform.position = pos;
    }

    public virtual void Update()
    {
        if (Mathf.Abs(transform.position.x) > core.bounds.x + 5 || Mathf.Abs(transform.position.y) > core.bounds.y + 5)
        {
            spawn.activeEnemies--;
            Destroy(gameObject);
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Prang") && core.powerupState == 2)
        {
            active = false;
            core.IncrementScore(pointValue);
            box.enabled = false;
            spawn.activeEnemies--;
            core.PlaySound(defeat);
            StartCoroutine(DefeatAnim());
        }
    }

    public IEnumerator DefeatAnim()
    {
        Color32 color1;
        Color32 color2;
        bool colorState = false;
        bool colorChange = false;
        float timerMax = 0.125f;
        float timer;
        for (int i = 0; i < explosionSprites.Length; i++)
        {
            color1 = core.palette[i switch { 0 => 3, 1 => 2, 2 => 1, _ => 0 }];
            color2 = core.palette[i switch { 0 => 1, 1 => 1, 2 => 0, _ => 7 }];
            sprite.sprite = explosionSprites[i];
            timer = timerMax;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                sprite.color = colorChange ? color2 : color1;
                colorState = !colorState;
                if (colorState)
                    colorChange = !colorChange;
                yield return new WaitForEndOfFrame();
            }
        }
        Destroy(gameObject);
    }
}
