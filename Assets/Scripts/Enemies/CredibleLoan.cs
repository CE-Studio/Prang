using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CredibleLoan : Enemy
{
    private int wallReflections;
    private float speed;
    private Vector2 direction = Vector2.zero;

    public override void Awake()
    {
        base.Awake();
        wallReflections = Random.Range(2, 9);
        speed = Random.Range(1.5f, 3.75f);
        sprite.color = core.palette[0];
    }

    public override void Instance(Vector2 pos, string data)
    {
        base.Instance(pos, data);
        int randomDir = Random.Range(0, 2) == 1 ? 1 : -1;
        direction = data switch
        {
            "up" => new Vector2(randomDir, 1),
            "down" => new Vector2(randomDir, -1),
            "left" => new Vector2(-1, randomDir),
            _ => new Vector2(1, randomDir)
        };
        if (direction.x == 1)
            sprite.flipX = true;
        if (direction.y == -1)
            sprite.flipY = true;
    }

    public override void Update()
    {
        base.Update();
        if (!active)
            return;

        transform.position += speed * Time.deltaTime * (Vector3)direction;
        if (((transform.position.x < -core.bounds.x && direction.x < 0) || (transform.position.x > core.bounds.x && direction.x > 0))
            && wallReflections > 0)
        {
            Reflect(false);
            wallReflections--;
        }
        if (((transform.position.y < -core.bounds.y && direction.x < 0) || (transform.position.y > core.bounds.y && direction.x > 0))
            && wallReflections > 0)
        {
            Reflect(true);
            wallReflections--;
        }
    }

    private void Reflect(bool axis)
    {
        if (axis)
        {
            sprite.flipY = !sprite.flipY;
            direction.y = -direction.y;
        }
        else
        {
            sprite.flipX = !sprite.flipX;
            direction.x = -direction.x;
        }
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.name.Contains("Courier"))
        {
            Vector2 collisionPoint = collision.ClosestPoint(transform.position);
            Vector2 collisionNormal = (Vector2)transform.position - collisionPoint;

            if (Mathf.Abs(collisionNormal.x) > Mathf.Abs(collisionNormal.y))
                collisionNormal = Vector2.right;
            else
                collisionNormal = Vector2.up;

            Reflect(collisionNormal == Vector2.up);
        }
    }
}
