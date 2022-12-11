using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JohnBarosa : Enemy
{
    private string direction = "right";
    private float timer = 0;
    private readonly float jumpTimerMax = 0.45f;
    private readonly float jumpDuration = 1f;
    private readonly float jumpCharge = 0.65f;
    private Vector2 jumpOrigin = Vector2.zero;
    private Vector2 jumpTarget = Vector2.zero;
    private readonly float jumpDistance = 3.5f;
    private readonly float jumpHeight = 1.25f;
    private readonly float detectDistance = 4f;
    private string state = "idle";

    public AudioClip lunge;

    public override void Start()
    {
        base.Start();
        pointValue = 1000;
        sprite.color = core.palette[5];
    }

    public override void Instance(Vector2 pos, string data)
    {
        base.Instance(pos, data);
        direction = data;
        if (data == "right")
            sprite.flipX = true;
        if (data == "up" || data == "down")
            if (Random.Range(0f, 1f) >= 0.5f)
                sprite.flipX = true;
    }

    public override void Update()
    {
        base.Update();
        if (active)
        {
            timer -= Time.deltaTime;
            switch (state)
            {
                default:
                case "idle":
                    sprite.sprite = enemySprites[0];
                    if (Vector2.Distance(transform.position, prang.transform.position) < detectDistance)
                    {
                        state = "lunge";
                        timer = jumpCharge;
                        core.PlaySound(lunge);
                    }
                    else if (timer <= 0)
                    {
                        state = "jump";
                        timer = jumpDuration;
                        jumpOrigin = transform.position;
                        jumpTarget = jumpOrigin + direction switch { "left" => Vector2.left, "down" => Vector2.down, "up" => Vector2.up, _ => Vector2.right } * jumpDistance;
                        if (jumpOrigin.x - jumpTarget.x != 0)
                            sprite.flipX = jumpTarget.x > jumpOrigin.x;
                    }
                    break;
                case "jump":
                    sprite.sprite = enemySprites[timer < jumpDuration * 0.5f ? 2 : 1];
                    float jumpTime = timer == 0 ? 0 : Mathf.Abs(timer - jumpDuration) / jumpDuration;
                    transform.position = Vector2.Lerp(jumpOrigin, jumpTarget, jumpTime) + jumpHeight * Mathf.Sin(jumpTime * Mathf.PI) * Vector2.up;
                    if (timer <= 0)
                    {
                        state = "idle";
                        timer = jumpTimerMax;
                    }
                    break;
                case "lunge":
                    sprite.sprite = enemySprites[3];
                    sprite.flipX = prang.transform.position.x > transform.position.x;
                    if (timer <= 0)
                    {
                        state = "jump";
                        timer = jumpDuration;
                        jumpOrigin = transform.position;
                        jumpTarget = prang.transform.position;
                    }
                    break;
            }
        }
    }
}
