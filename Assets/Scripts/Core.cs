using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    public Vector2 bounds = new Vector2(9.45f, 11.45f);
    public Color32[] palette = new Color32[]
    {
        new Color32(255, 255, 255, 255),
        new Color32(255, 0, 0, 255),
        new Color32(255, 200, 40, 255),
        new Color32(255, 255, 0, 255),
        new Color32(0, 255, 0, 255),
        new Color32(0, 255, 255, 255),
        new Color32(255, 0, 255, 255)
    };
    public Vector2 lastMousePoint = new Vector2(99, 99);

    public List<TextAlign> textAssets;

    public enum InputMethods { keyboard, mouse };
    public InputMethods currentInput = InputMethods.keyboard;

    public Camera cam;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
