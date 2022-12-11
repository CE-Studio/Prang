using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Core : MonoBehaviour
{
    public const float TAU = Mathf.PI * 2;

    public Vector2 bounds = new Vector2(9.45f, 11.45f);
    public Color32[] palette = new Color32[]
    {
        new Color32(255, 255, 255, 255),
        new Color32(255, 0, 0, 255),
        new Color32(255, 200, 40, 255),
        new Color32(255, 255, 0, 255),
        new Color32(0, 255, 0, 255),
        new Color32(0, 255, 255, 255),
        new Color32(255, 0, 255, 255),
        new Color32(0, 0, 0, 0)
    };
    public Vector2 lastMousePoint = new Vector2(99, 99);

    public List<TextAlign> textAssets;

    public Camera cam;
    public SpawnManager spawn;
    public AudioSource sfx;

    public AudioClip extraLife;

    public int lives = 2;
    public const int MAX_LIVES = 5;
    public List<Image> livesImages = new List<Image>();

    public int score = 0;
    private int scoreTo1up = 50000;
    private readonly int score1upInc = 50000;
    public int powerupState = 0;

    void Start()
    {
        Application.targetFrameRate = 60;
        spawn = GetComponent<SpawnManager>();
        sfx = GetComponent<AudioSource>();

        for (int i = 0; i < livesImages.Count; i++)
            livesImages[i].color = palette[i < lives ? 3 : 7];
        textAssets[0].SetText("000000");
    }

    void Update()
    {
        
    }

    public void IncrementLives()
    {
        if (lives < MAX_LIVES)
        {
            livesImages[lives].color = palette[3];
            lives++;
        }
    }

    public void DecrementLives()
    {
        if (lives > 0)
        {
            lives--;
            livesImages[lives].color = palette[7];
        }
    }

    public void IncrementScore(int points)
    {
        score += points;
        string scoreString = score.ToString();
        string scoreFinalString = "";
        for (int i = 5 - scoreString.Length; i >= 0; i--)
            scoreFinalString += "0";
        scoreFinalString += scoreString;
        textAssets[0].SetText(scoreFinalString);

        if (score >= scoreTo1up)
        {
            PlaySound(extraLife);
            scoreTo1up += score1upInc;
            IncrementLives();
        }
    }

    public void PlaySound(AudioClip clip, float volume = 1)
    {
        sfx.PlayOneShot(clip, volume);
    }
}
