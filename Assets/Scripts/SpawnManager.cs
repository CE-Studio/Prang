using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Core core;
    
    public int activePickups = 0;
    public const int MAX_PICKUPS = 5;
    public int activeEnemies = 0;
    public const int MAX_ENEMIES = 8;

    private float pickupTimer = 0;
    private readonly float pickupTimerMin = 0.85f;
    private readonly float pickupTimerMax = 1.25f;
    private float enemyTimer = 0;
    private float enemyTimerMax = 4f;
    private readonly float enemyTimerDecRate = 0.00625f;
    private float timeUntilPowerup = 15;
    private float powerupTimeMax = 15;
    private readonly float powerupTimeInc = 5;
    private readonly float powerupChance = 0.05f;

    public GameObject foodObj;
    public GameObject powerupObj;

    private float[] foodWeights = new float[] { 1f, 0.5f, 0.35f, 0.125f, 0.05f };
    
    void Start()
    {
        core = GetComponent<Core>();
        pickupTimer = pickupTimerMax;
        enemyTimer = enemyTimerMax;
    }

    void Update()
    {
        if (activePickups < MAX_PICKUPS)
        {
            pickupTimer -= Time.deltaTime;
            if (pickupTimer < 0)
            {
                pickupTimer = Random.Range(pickupTimerMin, pickupTimerMax);
                SpawnFood();
            }
        }
        if (activeEnemies < MAX_ENEMIES)
        {
            enemyTimer -= Time.deltaTime;
            if (enemyTimer < 0)
            {
                enemyTimer = enemyTimerMax;
            }
        }
        enemyTimerMax -= enemyTimerDecRate * Time.deltaTime;
        if (core.powerupState == 0)
            timeUntilPowerup = Mathf.Clamp(timeUntilPowerup - Time.deltaTime, 0, Mathf.Infinity);
    }

    public void SpawnFood()
    {
        activePickups++;
        Vector2 spawnPos = new Vector2(Random.Range(-core.bounds.x + 1, core.bounds.x - 1), Random.Range(-core.bounds.y + 1, core.bounds.y - 1));
        float spawnValue = Random.Range(0f, 1f);
        
        if (timeUntilPowerup == 0)
        {
            if (spawnValue <= powerupChance)
            {
                GameObject newPowerup = Instantiate(powerupObj);
                newPowerup.GetComponent<Powerup>().Instance(5, spawnPos);
                timeUntilPowerup = powerupTimeMax;
                powerupTimeMax += powerupTimeInc;
                return;
            }
            else
                spawnValue = Random.Range(0f, 1f);
        }

        int newType;
        if (spawnValue <= foodWeights[4])
            newType = 4;
        else if (spawnValue <= foodWeights[3])
            newType = 3;
        else if (spawnValue <= foodWeights[2])
            newType = 2;
        else if (spawnValue <= foodWeights[1])
            newType = 1;
        else
            newType = 0;

        GameObject newFood = Instantiate(foodObj);
        newFood.GetComponent<Food>().Instance(newType, spawnPos);
    }
}
