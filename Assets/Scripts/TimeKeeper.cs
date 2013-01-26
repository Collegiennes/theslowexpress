using System;
using System.Collections.Generic;
using UnityEngine;

public enum GamePhase
{
    Movement, Shooting
}
public static class GamePhasesExtensions
{
    public static GamePhase GetNextEvent(this GamePhase current)
    {
        switch (current)
        {
            case GamePhase.Movement: return GamePhase.Shooting;
            case GamePhase.Shooting: return GamePhase.Movement;
        }
        throw new NotImplementedException();
    }
}

class TimeKeeper : MonoBehaviour
{
    public float MovementPhaseDuration;
    public float ShootingPhaseDuration;

    float CurrentPhaseTime;
    float CurrentPhaseDuration;

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
        CurrentPhase = GamePhase.Shooting;
        ChangePhase();

        PhaseChanged += () => Debug.Log("PHASE CHANGE! " + CurrentPhase);
    }

    void Update()
    {
        CurrentPhaseTime += Time.deltaTime;
        CurrentTimeRatio = CurrentPhaseTime / CurrentPhaseDuration;

        if (CurrentPhaseTime >= CurrentPhaseDuration)
            ChangePhase();
    }

    void ChangePhase()
    {
        CurrentPhase = CurrentPhase.GetNextEvent();
        CurrentPhaseTime = 0;

        if (PhaseChanged != null)
            PhaseChanged();

        switch (CurrentPhase)
        {
            case GamePhase.Movement:
                CurrentPhaseDuration = MovementPhaseDuration;
                break;

            case GamePhase.Shooting:
                CurrentPhaseDuration = ShootingPhaseDuration;
                break;
        }
    }
}