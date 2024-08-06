using Cinemachine;
using UnityEngine;

public class CinemachineShake : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;

    private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float shakeIntensity = 1.5f; 
    [SerializeField] private float defaultShakeTime = 0.5f;  
    private float shakeTimer;           

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

    }

    private void OnEnable()
    {
        gameEvent.OnHitObject.AddListener(ShakeCamera); 
    }

    private void OnDisable()
    {
        gameEvent.OnHitObject.RemoveListener(ShakeCamera); 
    }

    public void ShakeCamera(float intensity)
    {
        CinemachineBasicMultiChannelPerlin perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (perlin != null)
        {
            perlin.m_AmplitudeGain = shakeIntensity; 
            shakeTimer = defaultShakeTime;
        }
    }


    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
           

            if (shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                if (perlin != null)
                {
                    perlin.m_AmplitudeGain = 0f;
                   
                }
            }
        }
    }
}
