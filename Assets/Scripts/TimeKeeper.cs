using System;
using System.Collections.Generic;
using UnityEngine;

public enum GamePhase
{
    IntroFastMove = -1, Moving, Grabbing
}

class TimeKeeper : MonoBehaviour
{
    public float IntroDuration = 5;
    public float MovementDuration = 4;
    public float GrabbingDuration = 2;
    public bool DebugMode = false;

    float PhaseTime;
    float PhaseDuration;
    float TimeFactor;

    public float GlobalTime { get; private set; }
    public float TimeRatio { get; private set; }
    public float DeltaTime { get; private set; }
    public GamePhase Phase { get; private set; }

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
        Phase = GamePhase.IntroFastMove;
        PhaseDuration = IntroDuration;

        PhaseChanged += () => Debug.Log("PHASE CHANGE! " + Phase);
    }

    void Update()
    {
        PhaseTime += Time.deltaTime;

        switch (Phase)
        {
            case GamePhase.IntroFastMove:
            case GamePhase.Grabbing:
                TimeFactor = 1;
                break;

            case GamePhase.Moving:
                TimeFactor = GrabbingDuration / MovementDuration;
                break;
        }

        DeltaTime = Time.deltaTime * TimeFactor;
        GlobalTime += DeltaTime;
        TimeRatio = PhaseTime / PhaseDuration;

        if (!DebugMode && PhaseTime >= PhaseDuration)
            ChangePhase();
    }

    void ChangePhase()
    {
        Phase = (GamePhase) (((int)Phase + 1) % 2);
        PhaseTime = 0;
        TimeRatio = 0;

        if (PhaseChanged != null)
            PhaseChanged();

        switch (Phase)
        {
            case GamePhase.Moving: PhaseDuration = MovementDuration; break;
            case GamePhase.Grabbing: PhaseDuration = GrabbingDuration; break;
        }
    }
}
