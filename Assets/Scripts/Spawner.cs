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

    private void Start()
    {
        //InvokeRepeating(nameof(Spawn), 0, spawnRate);// same as below but more expensive

        countdownTimer = new CountdownTimer(spawnRate);
        countdownTimer.OnTimerStart += Spawn;
        countdownTimer.OnTimerStop += () => countdownTimer.Start();

        countdownTimer.Start();
    }

    private void Update() => countdownTimer.Tick(Time.deltaTime);

    private void Spawn()
    {
        //add logic to spawn based on phase here

        WaterObject objToSpawn = flipSpawn ? GetWaveObject() : GetObstacleObject(); // change later based on phase
        flipSpawn = !flipSpawn;//remove made for debuging
        Vector3 spawnPos = GetSpawnPos();//change based on type 
        objToSpawn.transform.position = spawnPos;
        objToSpawn.gameObject.SetActive(true);
    }

    private Vector3 GetSpawnPos() // change based on phase
    {
        Vector3 spawnPos;

        switch (currentPosIndex)
        {
            case 0:
                spawnPos = topSpawnPos.position;
                break;
            case 1:
                spawnPos = middleSpawnPos.position;
                break;
            case 2:
                spawnPos = bottomSpawnPos.position;
                break;
            default:
                spawnPos = topSpawnPos.position;
                break;
        }

        currentPosIndex = (currentPosIndex + 1) % phasePosIndex;
        return spawnPos;
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

    private void OnEnable() => Event.OnReachDeadZone.AddListener(SortObject);
    private void OnDisable() => Event.OnReachDeadZone.RemoveListener(SortObject);
}
