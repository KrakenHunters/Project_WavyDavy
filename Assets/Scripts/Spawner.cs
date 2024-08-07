using DG.Tweening;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Utilities;

public class Spawner : MonoBehaviour
{

    [Header("Prefabs")]
    [SerializeField] private WaterObject wavePrefab;
    [SerializeField] private WaterObject obstaclePrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnRate;
    [SerializeField] private float maxSpawnInterval;
    [SerializeField] private float minSpawnInterval;

    [Header("Spawn positions")]
    [SerializeField] private float randomRadius ;
    [SerializeField] private Transform topSpawnPos;
    [SerializeField] private Transform middleSpawnPos;
    [SerializeField] private Transform bottomSpawnPos;

    private Transform topPos;
    private Transform bottomPos;

    public GameEvent Event;

    private List<WaterObject> availableWaves = new();
    private List<WaterObject> availableObstacles = new();
    //private List<WaterObject> activeObjects = new();

    private GamePhase currentPhase;
    private bool flipSpawn;
    private CountdownTimer countdownTimer;

    private void OnEnable()
    {
        Event.OnChangeGameState.AddListener(SetGamePhase);
        Event.OnFinishTransition.AddListener(RestartSpawning);
        Event.OnReachDeadZone.AddListener(SortObject);
        Event.OnFlowChange.AddListener(AdjustSpawnRateBasedOnFlow);
        Event.OnGameEnd += StopSpawner;
    }

    private void OnDisable()
    {
        Event.OnChangeGameState.RemoveListener(SetGamePhase);
        Event.OnFinishTransition.RemoveListener(RestartSpawning);
        Event.OnReachDeadZone.RemoveListener(SortObject);
        Event.OnFlowChange.RemoveListener(AdjustSpawnRateBasedOnFlow);
        Event.OnGameEnd -= StopSpawner;

    }


    private void Start()
    {
        countdownTimer = new CountdownTimer(spawnRate);
        countdownTimer.Start();
    }

    private void Update()
    {
        countdownTimer.Tick(Time.deltaTime);

        if (countdownTimer.IsFinished)
        {
            Spawn();
            countdownTimer = new CountdownTimer(spawnRate);
            countdownTimer.Start();

        }
    }

    private void AdjustSpawnRateBasedOnFlow(float currentFlow)
    {
        spawnRate = Mathf.Lerp(maxSpawnInterval, minSpawnInterval, currentFlow / 2.5f); 
    }

    private void SetGamePhase(GamePhase newPhase)
    {
        currentPhase = newPhase;

        topPos = null;
        bottomPos = null;
        countdownTimer?.Stop();

        switch (currentPhase)
        {
            case GamePhase.Phase2:
                topPos = middleSpawnPos;
                bottomPos = bottomSpawnPos;
                break;
            case GamePhase.Phase3:
                topPos = topSpawnPos;
                bottomPos = bottomSpawnPos;
                break;
        }
    }

    private void Spawn()
    {
        //add logic to spawn based on phase here
        if (currentPhase == GamePhase.Phase1 || currentPhase == GamePhase.Trick)
        {
            // no spawning in Phase 1& trick
            return;
        }

        WaterObject objToSpawn = /*flipSpawn ? GetWaveObject() : */GetObstacleObject(); // change later based on phase
        flipSpawn = !flipSpawn;//remove made for debuging
        Vector3 spawnPos = GetSpawnPos();//change based on type 
        objToSpawn.transform.position = spawnPos;
        objToSpawn.gameObject.SetActive(true);
    }

    private void RestartSpawning()
    {
        switch (currentPhase)
        {
            case GamePhase.Phase2:
                countdownTimer = new CountdownTimer(spawnRate);
                countdownTimer.Start();
                break;
            case GamePhase.Phase3:
                countdownTimer = new CountdownTimer(spawnRate);
                countdownTimer.Start();
                break;
        }
    }

    private Vector3 GetSpawnPos()
    {
        float topY = topPos.position.y;
        float bottomY = bottomPos.position.y;

        float randomY = Random.Range(bottomY - randomRadius, topY + randomRadius);

        float xPos = topPos.position.x;

        float zPos = topPos.position.z;

        return new Vector3(xPos, randomY, zPos);

        //return new Vector3 (topPos.position.x, Random.Range(bottomPos.position.y, topPos.position.y),topPos.position.z);
    }

    private WaterObject GetWaveObject()
    {
        WaterObject waveObject;
        if (availableWaves.Count > 0)
        {
            waveObject = availableWaves[0];
            availableWaves.RemoveAt(0);
        }
        else
        {
            waveObject = Instantiate(wavePrefab);
            waveObject.gameObject.SetActive(false);
        }
        return waveObject;
    }
    private WaterObject GetObstacleObject()
    {
        WaterObject obstacleObject;
        if (availableObstacles.Count > 0)
        {
            obstacleObject = availableObstacles[0];
            availableObstacles.RemoveAt(0);
        }
        else
        {
            obstacleObject = Instantiate(obstaclePrefab);
            obstacleObject.gameObject.SetActive(false);
        }
        return obstacleObject;
    }

    private void ReturnWaveObject(WaterObject waveObject)
    {
        waveObject.gameObject.SetActive(false);
        availableWaves.Add(waveObject);
    }
    private void ReturnObstacleObject(WaterObject obstacleObject)
    {
        obstacleObject.gameObject.SetActive(false);
        availableObstacles.Add(obstacleObject);
    }

    private void StopSpawner()
    {
        countdownTimer.Stop();
    }

    private void SortObject(WaterObject waterObject)
    {
        if (waterObject.Flow > 0)
        {
            ReturnWaveObject(waterObject);
        }
        else if (waterObject.Flow < 0)
        {
            ReturnObstacleObject(waterObject);
        }
    }
}
