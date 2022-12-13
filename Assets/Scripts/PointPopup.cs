using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointPopup : TextAlign
{
    private float lifeTime = 0.75f;
    private readonly float speed = 100f;
    private bool flashState = false;

    public void Instance(Vector2 pos, int value)
    {
        truePos = pos;
        SetText(value.ToString());
    }

    public override void Update()
    {
        truePos += speed * Time.deltaTime * Vector2.up;
        base.Update();
        lifeTime -= Time.deltaTime;
        flashState = !flashState;
        SetColor(flashState ? 0 : 7);
        if (lifeTime <= 0f)
            Destroy(gameObject);
    }
}
