using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Susaaaaan : Enemy
{
    private string state = "point"; // point, fire
    private float timer;
    private float speed;
    private float recoil;
    private readonly float recoilMax = 1.25f;
    private readonly float aimTime = 1f;
    private float animTimer;
    private bool animState = false;
    private bool flashState = false;
    private Vector2 targetPos;
    private readonly float maxRange = 8f;
    private int cycles;
    private bool firedThisCycle;

    public GameObject projectile;
    public AudioClip charge;
    public AudioClip fire;

    public override void Awake()
    {
        base.Awake();
        sprite.color = core.palette[1];
    }

    public override void Instance(Vector2 pos, string data)
    {
        base.Instance(pos, data);
        speed = Random.Range(2f, 2.5f);
        cycles = Random.Range(2, 7);
        float distance = Random.Range(maxRange * 0.5f, maxRange);
        float spread = Random.Range(-maxRange, maxRange);
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

        switch (state)
        {
            default:
            case "point":
                sprite.flipX = transform.position.x < targetPos.x;
                transform.position += speed * Time.deltaTime * ((Vector3)targetPos - transform.position).normalized;
                if (Vector2.Distance(transform.position, targetPos) < 0.1f)
                {
                    transform.position = targetPos;
                    timer = -aimTime;
                    recoil = recoilMax;
                    firedThisCycle = false;
                    state = "fire";
                    core.PlaySound(charge);
                }
                animTimer += Time.deltaTime;
                if (animTimer > 0.6f)
                    animTimer -= 0.6f;
                sprite.sprite = enemySprites[animTimer >= 0.3f ? 1 : 0];
                break;
            case "fire":
                timer += Time.deltaTime;
                if (timer > 0)
                {
                    sprite.color = core.palette[1];
                    sprite.sprite = enemySprites[timer < 0.4f ? 2 : 0];
                    transform.position -= recoil * Time.deltaTime * ((Vector3)targetPos - transform.position).normalized;
                    recoil = Mathf.Clamp(recoil -= Time.deltaTime, 0, Mathf.Infinity);
                    if (!firedThisCycle)
                    {
                        firedThisCycle = true;
                        GameObject newProjectile = Instantiate(projectile);
                        newProjectile.GetComponent<Enemy>().Instance(transform.position + Vector3.up * 0.22f, "" + targetPos.x + "," + targetPos.y);
                        core.PlaySound(fire);
                    }
                    if (timer > 1.5f)
                    {
                        cycles--;
                        targetPos = new Vector2(
                            Mathf.Clamp(Random.Range(transform.position.x - maxRange, transform.position.x + maxRange), -core.bounds.x + 2, core.bounds.x - 2),
                            Mathf.Clamp(Random.Range(transform.position.y - maxRange, transform.position.y + maxRange), -core.bounds.y + 3, core.bounds.y - 3)
                            );
                        if (cycles <= 0)
                            targetPos *= 20;
                        state = "point";
                    }
                }
                else
                {
                    sprite.sprite = enemySprites[0];
                    targetPos = prang.transform.position;
                    sprite.flipX = transform.position.x < targetPos.x;
                    animState = !animState;
                    if (animState)
                    {
                        flashState = !flashState;
                        sprite.color = core.palette[flashState ? 0 : 1];
                    }
                }
                break;
        }
    }
}
