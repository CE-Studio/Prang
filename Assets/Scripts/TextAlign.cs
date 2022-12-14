using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextAlign : MonoBehaviour
{
    public Core core;
    public RectTransform rect;
    public Text text;

    public Vector2 truePos;
    public Vector2 displayPos;
    public Vector2 posOffset;

    void Awake()
    {
        core = GameObject.FindWithTag("Core").GetComponent<Core>();
        rect = GetComponent<RectTransform>();
        text = GetComponent<Text>();

        truePos = rect.position;
        posOffset.x += 2;
        if (rect.anchorMin.y == 0)
            posOffset.y -= 2;
        else if (rect.anchorMin.y == 1)
            posOffset.y += 2;
    }

    public virtual void Update()
    {
        displayPos = new Vector2(
            Mathf.RoundToInt(truePos.x * 0.25f) * 4f + posOffset.x,
            Mathf.RoundToInt(truePos.y * 0.25f) * 4f + posOffset.y
            );
        rect.position = new Vector3(displayPos.x, displayPos.y, 0);
    }

    public void SetColor(int color)
    {
        if (text == null)
            text = GetComponent<Text>();
        text.color = core.palette[color];
    }

    public void SetText(string newText)
    {
        if (text == null)
            text = GetComponent<Text>();
        text.text = newText;
    }
}
