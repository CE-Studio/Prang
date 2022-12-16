using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaveAlejandro : Enemy
{
    private string state = "idle"; // idle, point, orbit, target, carry
    private bool animState = false;
    private Vector2 lastPos = Vector2.zero;
    private Vector2 targetPos = Vector2.zero;
    private float speed;
    private float speedFraction = 0.65f;
    private float timer;
    private float posTimer;
    private Vector2 orbitPoint;
    private Vector2 orbitRadii;
    private readonly float idleMax;
    private readonly float orbitMax;
    private GameObject food;

    public override void Awake()
    {
        base.Awake();
        sprite.color = core.palette[3];
    }

    public override void Instance(Vector2 pos, string data)
    {
        base.Instance(pos, data);
        lastPos = transform.position;
    }

    public override void Update()
    {
        base.Update();
        if (!active)
            return;

        switch (state)
        {
            default:
            case "idle":
                break;
            case "point":
                break;
            case "orbit":
                break;
            case "target":
                break;
            case "carry":
                break;
        }
    }
}
