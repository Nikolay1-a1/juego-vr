using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GazeManager : MonoBehaviour
{
    public event Action OnGazeSelection;

    public static GazeManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    [SerializeField] private GameObject gazeBarCanvas;
    [SerializeField] private Image fillIndicator;
    [Tooltip("Time in seg")]
    [SerializeField] private float timeForSelection = 2.5f;

    private float timeCounter;
    private float timeProggres;
    private bool runTimer;

    void Start()
    {
        if (gazeBarCanvas != null)
        {
            gazeBarCanvas.SetActive(false);
        }
        if (fillIndicator != null)
        {
            fillIndicator.fillAmount = Normalise();
        }
    }

    public void Update()
    {
        if (runTimer)
        {
            timeProggres += Time.deltaTime;
            AddValue(timeProggres);
        }
    }

    public void SetUpGaze(float timeForSelection) 
    {
        this.timeForSelection = timeForSelection;
    }

    public void StartGazeSelection()
    {
        if (gazeBarCanvas != null)
        {
            gazeBarCanvas.SetActive(true);
        }
        runTimer = true;
        timeProggres = 0;
        timeCounter = 0;
    }

    public void CancelGazeSelection()
    {
        if (gazeBarCanvas != null)
        {
            gazeBarCanvas.SetActive(false);
        }
        runTimer = false;
        timeProggres = 0;
        timeCounter = 0;
        if (fillIndicator != null)
        {
            fillIndicator.fillAmount = 0;
        }
    }

    private void AddValue(float val) 
    {
        timeCounter = val;
        if (timeCounter >= timeForSelection)
        {
            timeCounter = 0;
            runTimer = false;
            if (gazeBarCanvas != null)
            {
                gazeBarCanvas.SetActive(false);
            }
            OnGazeSelection?.Invoke();
        }

        if (fillIndicator != null)
        {
            fillIndicator.fillAmount = Normalise();
        }
    }

    private float Normalise() 
    {
        if (timeForSelection <= 0) return 0;
        return Mathf.Clamp01(timeCounter / timeForSelection);
    }
}
