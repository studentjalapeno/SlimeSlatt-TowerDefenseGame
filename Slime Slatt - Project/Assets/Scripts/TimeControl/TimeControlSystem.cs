using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControlSystem : MonoBehaviour
{
    [SerializeField]
    private float turnDuration = 1f;

    [SerializeField]
    private float fastForwardMultiplier = 5f;

    [SerializeField]
    public bool paused;

    [SerializeField]
    private bool fastForward;

    public delegate void OnTimeAdvanceHandler();
    public static event OnTimeAdvanceHandler OnTimeAdvance;

    private float advanceTimer;

    
    // Start is called before the first frame update
    void Start()
    {
        advanceTimer = turnDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            //if fast foward == true multiply by fastForwardMultipier
            advanceTimer -= Time.deltaTime * (fastForward ? fastForwardMultiplier : 1f);
            if (advanceTimer <= 0)
            {
                advanceTimer += turnDuration;

                //if onTimeAdvance is not null invoke OnTimeAdvance
                OnTimeAdvance?.Invoke();
            }
        }
    }

    public void Step()
    {
        OnTimeAdvance?.Invoke();
    }

    public void Pause()
    {
        paused = true;
        fastForward = false;
        Time.timeScale = 0;
    }

    public void Play()
    {
        paused = false;
        fastForward = false;
        Time.timeScale = 1;
    }

    public void FastFoward()
    {
        paused = false;
        fastForward = true;
        Time.timeScale = fastForwardMultiplier;
    }
}
