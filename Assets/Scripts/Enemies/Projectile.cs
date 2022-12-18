using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Enemy
{
    private Vector2 dir;
    private int spriteCounter;
    private int colorCounter;
    private readonly float speed = 5f;

    public override void Awake()
    {
        base.Awake();
        sprite.color = core.palette[0];
    }

    public override void Instance(Vector2 pos, string data)
    {
        base.Instance(pos, data);
        string[] dirParts = data.Split(',');
        dir = (new Vector2(float.Parse(dirParts[0]), float.Parse(dirParts[1])) - (Vector2)transform.position).normalized;
    }

    public override void Update()
    {
        base.Update();
        if (!active)
            return;

        transform.position += speed * Time.deltaTime * (Vector3)dir;
        spriteCounter++;
        if (spriteCounter >= enemySprites.Length)
            spriteCounter -= enemySprites.Length;
        sprite.sprite = enemySprites[spriteCounter];
        colorCounter++;
        if (colorCounter >= 8)
            colorCounter -= 8;
        sprite.color = core.palette[colorCounter switch
        {
            0 => 0,
            1 => 0,
            2 => 3,
            3 => 3,
            4 => 1,
            5 => 1,
            6 => 2,
            _ => 2
        }];
    }

    public override void OnTriggerEnter2D(Collider2D collision) { }
}
