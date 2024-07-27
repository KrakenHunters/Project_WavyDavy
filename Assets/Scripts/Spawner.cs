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
    [SerializeField] private float minSpawnRate;
    [SerializeField] private float maxSpawnRate;
    [SerializeField] private float difficultyRampDuration;

    [Header("Spawn positions")]
    [SerializeField] private Transform topSpawnPos;
    [SerializeField] private Transform middleSpawnPos;
    [SerializeField] private Transform bottomSpawnPos;

    public GameEvent Event;

    private List<WaterObject> availableWaves = new();
    private List<WaterObject> availableObstacles = new();
    private GamePhase currentPhase;
    private int phasePosIndex = 3;// change by currentPhase
    private int currentPosIndex;
    private bool flipSpawn;
    private CountdownTimer countdownTimer;

    private float nextSpawnTime;

    private void OnEnable()
    {
        Event.OnChangeGameState.AddListener(SetGamePhase);
        Event.OnReachDeadZone.AddListener(SortObject); 
    }

    private void OnDisable()
    {
        Event.OnChangeGameState.RemoveListener(SetGamePhase);
        Event.OnReachDeadZone.RemoveListener(SortObject); 
    }


    private void Start()
    {
        //InvokeRepeating(nameof(Spawn), 0, spawnRate);// same as below but more expensive

        countdownTimer = new CountdownTimer(spawnRate);
        countdownTimer.OnTimerStart += Spawn;
        countdownTimer.OnTimerStop += () => countdownTimer.Start();
        countdownTimer.Start();
    }

    private void Update()
    {
        countdownTimer.Tick(Time.deltaTime);

        if (Time.time >= nextSpawnTime)
        {
            Spawn();
            AdjustSpawnRateBasedOnFlow();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    private void AdjustSpawnRateBasedOnFlow()
    {
        float gameProgress = Mathf.Clamp01(Time.timeSinceLevelLoad / difficultyRampDuration);
        spawnRate = Mathf.Lerp(maxSpawnRate, minSpawnRate, gameProgress);
    }

    private void SetGamePhase(GamePhase newPhase)
    {
        currentPhase = newPhase;

        switch (currentPhase)
        {
            case GamePhase.Trick:
                phasePosIndex = 0;  // Trick
                break;
            case GamePhase.Phase1:
                phasePosIndex = 0;  // Phase 1
                break;
            case GamePhase.Phase2:
                phasePosIndex = 2;  // Phase 2
                break;
            case GamePhase.Phase3:
                phasePosIndex = 3;  // Phase 3
                break;
            default:
                phasePosIndex = 0; 
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

        WaterObject objToSpawn = flipSpawn ? GetWaveObject() : GetObstacleObject(); // change later based on phase
        flipSpawn = !flipSpawn;//remove made for debuging
        Vector3 spawnPos = GetSpawnPos();//change based on type 
        objToSpawn.transform.position = spawnPos;
        objToSpawn.gameObject.SetActive(true);
    }

    private Vector3 GetSpawnPos() // change based on phase
    {
        List<Vector3> possiblePositions = new List<Vector3>();

        if (phasePosIndex > 0) possiblePositions.Add(bottomSpawnPos.position);
        if (phasePosIndex > 1) possiblePositions.Add(middleSpawnPos.position);
        if (phasePosIndex > 2) possiblePositions.Add(topSpawnPos.position);

        if (possiblePositions.Count == 0)
            return Vector3.zero;

        return possiblePositions[Random.Range(0, possiblePositions.Count)];
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
