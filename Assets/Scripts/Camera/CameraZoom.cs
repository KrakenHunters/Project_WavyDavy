using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{

    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;


    [Header("Zoom Values")]
    [SerializeField] float phase1Zoom = 4.0f;
    [SerializeField] float phase2Zoom = 7.0f;
    [SerializeField] float phase3Zoom = 10.0f;


    [SerializeField] private float zoomSpeed = 2.0f;

    [Header("Game Event")]
    public GameEvent Event;

    private void Awake()
    {
        if(virtualCamera == null)
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        SetPhase(GamePhase.Phase1);
    }

    private void OnEnable()
    {
        Event.OnChangeGameState.AddListener(SetPhase);
    }

    private void OnDisable()
    {
        Event.OnChangeGameState.RemoveListener(SetPhase);
    }

    private void SetPhase(GamePhase newPhase)
    {
        switch (newPhase)
        {
            case GamePhase.Phase1:
                StopZoom();
                StartCoroutine(SetZoom(phase1Zoom));    
                break;
            case GamePhase.Phase2:
                StopZoom();
                StartCoroutine(SetZoom(phase2Zoom));    
                break;
            case GamePhase.Phase3:
                StopZoom();
                StartCoroutine(SetZoom(phase3Zoom));   
                break;
            case GamePhase.Trick:

                break;
        }
    }


    private void StopZoom()
    {
        StopCoroutine(SetZoom(phase1Zoom));
        StopCoroutine(SetZoom(phase2Zoom));
        StopCoroutine(SetZoom(phase3Zoom));

    }


    private IEnumerator SetZoom(float zoomValue)
    {
        while (virtualCamera.m_Lens.OrthographicSize != zoomValue)
        {
            float currentZoom = virtualCamera.m_Lens.OrthographicSize;
            currentZoom = Mathf.MoveTowards(currentZoom, zoomValue, zoomSpeed * Time.deltaTime);
            virtualCamera.m_Lens.OrthographicSize = currentZoom;
            yield return null;
        }
    }
}
