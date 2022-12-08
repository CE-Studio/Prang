using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prang : MonoBehaviour
{
    public Core core;
    public BoxCollider2D box;
    public Rigidbody2D rb;
    public SpriteRenderer sprite;

    public float speed = 6f;
    public float speedMod = 1f;

    void Start()
    {
        core = GameObject.FindWithTag("Core").GetComponent<Core>();
        box = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        sprite.color = core.palette[3];
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
            core.lastMousePoint = core.cam.ScreenToWorldPoint(Input.mousePosition);
        if (core.lastMousePoint != new Vector2(99, 99))
            rb.position += speed * speedMod * Time.deltaTime * (core.lastMousePoint - rb.position).normalized;
        if (Vector2.Distance(rb.position, core.lastMousePoint) < 0.125f)
            core.lastMousePoint = new Vector2(99, 99);

        if (!Input.GetKey(KeyCode.Mouse0) && !(Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0))
        {
            core.lastMousePoint = new Vector2(99, 99);
            rb.position += speed * speedMod * Time.deltaTime * new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        }

        if (rb.position.x > core.bounds.x || rb.position.x < -core.bounds.x)
            rb.position = new Vector2(core.bounds.x * Mathf.Sign(rb.position.x), rb.position.y);
        if (rb.position.y > core.bounds.y || rb.position.y < -core.bounds.y)
            rb.position = new Vector2(rb.position.x, core.bounds.y * Mathf.Sign(rb.position.y));
    }
}
