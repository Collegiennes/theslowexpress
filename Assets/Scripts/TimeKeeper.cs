using System;
using System.Collections.Generic;
using UnityEngine;

public enum GamePhase
{
    IntroFastMove = -1, Moving, Grabbing
}

class TimeKeeper : MonoBehaviour
{
    public float IntroDuration = 0;
    public float MovementDuration = 0;
    public float ShootingDuration = 0;

    float CurrentPhaseTime;
    float CurrentPhaseDuration;
    float DestinationTimeFactor;

    public float CurrentTimeRatio { get; private set; }
    public float CurrentTimeFactor { get; private set; }
    public GamePhase CurrentPhase { get; private set; }

    public event Action PhaseChanged;

    static TimeKeeper instance;
    public static TimeKeeper Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(TimeKeeper)) as TimeKeeper;
                if (instance == null)
                    throw new InvalidOperationException("No instance in scene!");
            }
            return instance;
        }
    }
    void OnApplicationQuit()
    {
        instance = null;
    }

    void Start()
    {
        CurrentPhase = GamePhase.IntroFastMove;
        CurrentPhaseDuration = IntroDuration;

        PhaseChanged += () => Debug.Log("PHASE CHANGE! " + CurrentPhase);
    }

    void Update()
    {
        CurrentPhaseTime += Time.deltaTime;

        switch (CurrentPhase)
        {
            case GamePhase.IntroFastMove:
            case GamePhase.Grabbing:
                CurrentTimeFactor = 1;
                break;

            case GamePhase.Moving:
                CurrentTimeFactor = ShootingDuration / MovementDuration;
                break;
        }

        CurrentTimeRatio = CurrentPhaseTime / CurrentPhaseDuration;

        if (CurrentPhaseTime >= CurrentPhaseDuration)
            ChangePhase();
    }

    void ChangePhase()
    {
        CurrentPhase = (GamePhase) (((int)CurrentPhase + 1) % 2);
        CurrentPhaseTime = 0;
        CurrentTimeRatio = 0;

        if (PhaseChanged != null)
            PhaseChanged();

        switch (CurrentPhase)
        {
            case GamePhase.Moving: CurrentPhaseDuration = MovementDuration; break;
            case GamePhase.Grabbing: CurrentPhaseDuration = ShootingDuration; break;
        }
    }
}