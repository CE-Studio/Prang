using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Courier : Enemy
{
    private string direction = "right";
    private float timer;
    private float parkTime;
    private float speed;
    private float speedMax;

    public override void Awake()
    {
        base.Awake();
        timer = Random.Range(-2f, -1f);
        parkTime = Random.Range(5f, 30f);
        speedMax = Random.Range(2f, 5f);
        speed = speedMax;
        sprite.enabled = false;
    }

    public override void Instance(Vector2 pos, string data)
    {
        base.Instance(pos, data);
        direction = data;
        int length = Random.Range(1, 4);
        for (int i = 0; i < length; i++)
        {
            GameObject newSegment = new GameObject();
            newSegment.transform.parent = transform;
            newSegment.transform.localPosition = new Vector2(i - (length * 0.5f), 0);
            SpriteRenderer segmentSprite = newSegment.AddComponent<SpriteRenderer>();
            segmentSprite.color = core.palette[4];
            if (length == 1)
                segmentSprite.sprite = enemySprites[0];
            else
            {
                if (i == 0)
                    segmentSprite.sprite = enemySprites[1];
                else if (i == length - 1)
                    segmentSprite.sprite = enemySprites[3];
                else
                    segmentSprite.sprite = enemySprites[2];
            }
        }
        if (direction == "right" || direction == "down")
            transform.localScale = new Vector2(-1, 1);
        if (direction == "down" || direction == "up")
            transform.Rotate(new Vector3(0, 0, -90));
        box.size = new Vector2(0.85f + (1 * (length - 1)), 0.85f);
    }

    public override void Update()
    {
        base.Update();
        if (!active)
            return;

        Vector2 dir = direction switch { "up" => Vector2.up, "down" => Vector2.down, "left" => Vector2.left, _ => Vector2.right };
        timer += Time.deltaTime;
        if (timer >= 0 && timer < parkTime)
            speed = Mathf.Clamp(speed - Time.deltaTime, 0, Mathf.Infinity);
        else if (timer >= parkTime)
            speed = Mathf.Clamp(speed + Time.deltaTime, 0, speedMax);
        transform.position += speed * Time.deltaTime * (Vector3)dir;
    }

    public override void OnTriggerEnter2D(Collider2D collision) { }
}
