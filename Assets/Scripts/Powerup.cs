using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : Food
{
    public AudioClip powerupAppear;
    public AudioClip powerupPickup;

    public override void Instance(int newType, Vector2 pos)
    {
        core.PlaySound(powerupAppear);
        originY = pos.y;
        transform.position = pos;
        type = 5;
        sprite.enabled = false;
        sprite.color = core.palette[6];
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Prang"))
        {
            core.IncrementScore(pointValues[type]);
            core.spawn.activePickups--;
            core.PlaySound(powerupPickup);
            core.powerupState = 1;
            Destroy(gameObject);
        }
    }
}
