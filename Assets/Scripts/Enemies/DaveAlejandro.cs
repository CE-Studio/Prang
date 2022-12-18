using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaveAlejandro : Enemy
{
    private string state = "point"; // idle, point, orbit, target, carry
    private bool animState = false;
    private Vector2 lastPos = Vector2.zero;
    private Vector2 targetPos = Vector2.zero;
    private float speed;
    private float speedFraction = 0.65f;
    private float timer;
    private float posTimer;
    private Vector2 orbitPoint;
    private Vector2 orbitRadii;
    private readonly float idleMax = 1.5f;
    private readonly float orbitMax = 2.5f;
    private Food food;

    public override void Awake()
    {
        base.Awake();
        sprite.color = core.palette[3];
    }

    public override void Instance(Vector2 pos, string data)
    {
        base.Instance(pos, data);
        lastPos = transform.position;
        speed = Random.Range(4.25f, 6f);
        float distance = Random.Range(1.5f, 4f);
        float spread = Random.Range(-4f, 4f);
        targetPos = (Vector2)transform.position + data switch
        {
            "up" => new Vector2(spread, distance),
            "down" => new Vector2(spread, -distance),
            "left" => new Vector2(-distance, spread),
            _ => new Vector2(distance, spread)
        };
        state = "point";
    }

    public override void Update()
    {
        base.Update();
        if (!active)
            return;

        float rand = Random.Range(0f, 1f);
        switch (state)
        {
            default:
            case "idle":
                box.enabled = true;
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    GameObject powerup = GameObject.FindWithTag("Powerup");
                    GameObject[] allFood = GameObject.FindGameObjectsWithTag("Food");
                    if (rand <= 0.25f && (powerup != null || allFood.Length > 0))
                    {
                        if (powerup != null)
                            food = powerup.GetComponent<Food>();
                        else
                            food = allFood[Random.Range(0, allFood.Length)].GetComponent<Food>();
                        targetPos = food.transform.position + Vector3.up * 2.5f;
                        orbitRadii = new Vector2(Random.Range(2f, 4f), Random.Range(1.5f, 3.75f));
                        state = "target";
                    }
                    else
                    {
                        targetPos = new Vector2(Random.Range(-core.bounds.x + 2, core.bounds.x - 2), Random.Range(-core.bounds.y + 3, core.bounds.y - 3));
                        state = "point";
                    }
                }
                break;
            case "point":
                box.enabled = false;
                transform.position += speed * Time.deltaTime * ((Vector3)targetPos - transform.position).normalized;
                if (Vector2.Distance(transform.position, targetPos) < 0.1f)
                {
                    transform.position = targetPos;
                    timer = Random.Range(idleMax * 0.5f, idleMax);
                    state = "idle";
                }
                break;
            case "orbit":
                box.enabled = false;
                if (food == null)
                {
                    targetPos = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * 20;
                    speed *= 3;
                    state = "point";
                }
                else
                {
                    posTimer += Time.deltaTime * 6;
                    if (posTimer > Core.TAU)
                        posTimer -= Core.TAU;
                    timer -= Time.deltaTime;
                    food.lifeTime = 2f;
                    if (timer <= 0)
                    {
                        targetPos = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * 20;
                        state = "carry";
                    }
                    else
                    {
                        transform.position = new Vector2(
                            orbitPoint.x + (Mathf.Sin(posTimer) * orbitRadii.x * (timer > 1 ? 1 : timer)),
                            orbitPoint.y + (Mathf.Cos(posTimer) * orbitRadii.y * (timer > 1 ? 1 : timer))
                            );
                    }
                }
                break;
            case "target":
                box.enabled = false;
                if (food == null)
                {
                    timer = Random.Range(idleMax * 0.5f, idleMax);
                    state = "idle";
                }
                else
                {
                    transform.position += speed * Time.deltaTime * ((Vector3)targetPos - transform.position).normalized;
                    if (Vector2.Distance(transform.position, targetPos) < 0.1f)
                    {
                        transform.position = targetPos;
                        orbitPoint = food.transform.position;
                        timer = orbitMax;
                        posTimer = 0;
                        state = "orbit";
                    }
                }
                break;
            case "carry":
                box.enabled = food != null;
                if (food == null)
                    transform.position += speed * 3 * Time.deltaTime * ((Vector3)targetPos - transform.position).normalized;
                else
                {
                    transform.position += speed * speedFraction * Time.deltaTime * ((Vector3)targetPos - transform.position).normalized;
                    food.transform.position = transform.position + Vector3.down * 0.85f;
                    food.lifeTime = 2f;
                }
                break;
        }

        if (box.enabled)
            sprite.enabled = true;
        else
            sprite.enabled = !sprite.enabled;
        if (state == "idle")
            sprite.sprite = enemySprites[0];
        else
        {
            if (sprite.enabled)
            {
                animState = !animState;
                sprite.sprite = enemySprites[animState ? 1 : 2];
            }
        }
        sprite.flipX = transform.position.x > lastPos.x;

        lastPos = transform.position;
    }
}
