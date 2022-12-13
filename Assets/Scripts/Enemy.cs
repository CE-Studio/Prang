using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Core core;
    public Prang prang;
    public BoxCollider2D box;
    public SpriteRenderer sprite;

    public Sprite[] enemySprites;
    public Sprite[] explosionSprites;

    private int[] pointValues = new int[] { 100, 200, 500, 1000, 2000, 3000, 5000, 8000 };

    public AudioClip defeat;

    public bool active = true;
    private float resetTimer;
    private Vector2 resetDir;
    private float resetSpeed = 30f;
    
    public virtual void Awake()
    {
        core = GameObject.FindWithTag("Core").GetComponent<Core>();
        prang = GameObject.FindWithTag("Prang").GetComponent<Prang>();
        box = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        resetTimer = Random.Range(-1.5f, -1f);
        resetDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    public virtual void Instance(Vector2 pos, string data)
    {
        transform.position = pos;
    }

    public virtual void Update()
    {
        if (Mathf.Abs(transform.position.x) > core.bounds.x + 5 || Mathf.Abs(transform.position.y) > core.bounds.y + 5)
        {
            core.spawn.activeEnemies--;
            Destroy(gameObject);
        }
        if (core.deathState)
            active = false;
        if (!active)
        {
            resetTimer += Time.deltaTime;
            if (resetTimer >= 0)
                transform.position += Time.deltaTime * resetSpeed * (Vector3)resetDir;
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Prang") && core.powerupState == 2)
        {
            active = false;
            int pointValue = pointValues[core.powerupCombo >= pointValues.Length ? pointValues.Length - 1 : core.powerupCombo];
            core.powerupCombo++;
            core.IncrementScore(pointValue);
            core.CreatePointPopup(transform.position, pointValue);
            box.enabled = false;
            core.spawn.activeEnemies--;
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
