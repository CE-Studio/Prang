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
    private readonly float enemyTimerHardMax = 4f;
    private readonly float enemyTimerRetractRate = 1.5f;
    private readonly float enemyTimerDecRate = 0.00625f;
    private float timeUntilPowerup = 15;
    private float powerupTimeMax = 15;
    private readonly float powerupTimeInc = 5;
    private readonly float powerupChance = 0.05f;

    public GameObject foodObj;
    public GameObject powerupObj;
    public GameObject johnBObj;
    public GameObject johnWObj;
    public GameObject daveObj;
    public GameObject loanObj;
    public GameObject danielObj;
    public GameObject susanObj;
    public GameObject courierObj;

    private float[] foodWeights = new float[] { 1f, 0.5f, 0.35f, 0.125f, 0.05f };
    // Apple, cherry, fish, purple drink, blue drink
    private float[] enemyWeights = new float[] { 1f, 0.8f, 0.75f, 0.5f, 0.35f, 0.2f, 0.1f};
    // John B, John W, Courier, Loan, Daniel, Dave, Susan
    
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
                if (!core.deathState)
                    SpawnFood();
            }
        }
        if (activeEnemies < MAX_ENEMIES)
        {
            enemyTimer -= Time.deltaTime;
            if (enemyTimer < 0)
            {
                enemyTimer = enemyTimerMax;
                if (!core.deathState)
                    SpawnEnemy();
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

    public void SpawnEnemy()
    {
        activeEnemies++;
        Vector2 spawnPos;
        string originDir;
        if (Random.Range(0, 2) == 1)
        {
            if (Random.Range(0, 2) == 1)
            {
                spawnPos.y = core.bounds.y + 1.5f;
                originDir = "down";
            }
            else
            {
                spawnPos.y = -core.bounds.y - 1.5f;
                originDir = "up";
            }
            spawnPos.x = Random.Range(-core.bounds.x + 1f, core.bounds.x - 1f);
        }
        else
        {
            if (Random.Range(0, 2) == 1)
            {
                spawnPos.x = core.bounds.x + 1.5f;
                originDir = "left";
            }
            else
            {
                spawnPos.x = -core.bounds.x - 1.5f;
                originDir = "right";
            }
            spawnPos.y = Random.Range(-core.bounds.y + 1f, core.bounds.y - 1f);
        }

        int newType;
        float spawnValue = Random.Range(0f, 1f);
        if (spawnValue <= enemyWeights[6])
            newType = 6;
        else if (spawnValue <= enemyWeights[5])
            newType = 5;
        else if (spawnValue <= enemyWeights[4])
            newType = 4;
        else if (spawnValue <= enemyWeights[3])
            newType = 3;
        else if (spawnValue <= enemyWeights[2])
            newType = 2;
        else if (spawnValue <= enemyWeights[1])
            newType = 1;
        else
            newType = 0;

        GameObject newEnemy = Instantiate(newType switch
        {
            1 => johnWObj,
            2 => courierObj,
            3 => loanObj,
            4 => danielObj,
            5 => daveObj,
            6 => susanObj,
            _ => johnBObj
        });
        newEnemy.GetComponent<Enemy>().Instance(spawnPos, originDir);
    }

    public void RetractEnemyRate()
    {
        enemyTimerMax = Mathf.Clamp(enemyTimerMax + enemyTimerRetractRate, 0, enemyTimerHardMax);
    }
}
