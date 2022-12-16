using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanielGarry : Enemy
{
    private float speed;
    private float speedInc;
    private Vector2 direction;
    private float animTimer;

    public override void Awake()
    {
        base.Awake();
        speed = 2f;
        speedInc = Random.Range(0.75f, 1.15f);
        sprite.color = core.palette[6];
    }

    public override void Instance(Vector2 pos, string data)
    {
        base.Instance(pos, data);
        float randomDir = Random.Range(-2f, 2f);
        direction = data switch
        {
            "up" => new Vector2(randomDir, 1).normalized,
            "down" => new Vector2(randomDir, -1).normalized,
            "left" => new Vector2(-1, randomDir).normalized,
            _ => new Vector2(1, randomDir).normalized
        };
        if (direction.x > 0)
            sprite.flipX = true;
    }

    public override void Update()
    {
        base.Update();
        if (!active)
            return;

        transform.position += speed * Time.deltaTime * (Vector3)direction;
        speed += speedInc * Time.deltaTime;

        animTimer += Time.deltaTime;
        if (animTimer > 0.8f)
            animTimer -= 0.8f;
        if (animTimer < 0.2f)
            sprite.sprite = enemySprites[0];
        else if (animTimer < 0.4f)
            sprite.sprite = enemySprites[1];
        else if (animTimer < 0.6f)
            sprite.sprite = enemySprites[2];
        else
            sprite.sprite = enemySprites[1];
    }
}
