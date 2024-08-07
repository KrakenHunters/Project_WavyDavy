using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleScaler : MonoBehaviour
{
    [SerializeField]
    private float maxScale = 1f;

    [SerializeField] private float timeScale = 10f;

    private float _timeIncrement;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _timeIncrement += Time.deltaTime * timeScale;
        var currentScale = Mathf.Sin(_timeIncrement) * maxScale;
        this.transform.localScale = new Vector3(currentScale, currentScale, 1);
    }
}
