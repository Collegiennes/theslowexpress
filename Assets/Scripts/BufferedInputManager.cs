using System;
using System.Collections.Generic;
using UnityEngine;

public enum TimelineEventTypes
{
    Record, Playback
}
public static class TimelineEventTypeExtensions
{
    public static TimelineEventTypes GetNextEvent(this TimelineEventTypes current)
    {
        switch (current)
        {
            case TimelineEventTypes.Playback: return TimelineEventTypes.Record;
            case TimelineEventTypes.Record: return TimelineEventTypes.Playback;
        }
        throw new NotImplementedException();
    }
}

class BufferedInputManager : MonoBehaviour
{
    public float StopTimelineEvery;
    public float StopTimelineFor;
    public bool DebugBypass;

    float CurrentEventTime;
    float CurrentEventDuration;
    TimelineEventTypes CurrentEvent;
    float LastGameTime;
    InputFrame LastFrame;

    LinkedList<InputFrame> QueuedInput;

    public bool FrameIsNew { get; private set; }
    public InputFrame CurrentFrame { get; private set; }
    public float DeltaGameTime { get; private set; }

    public event Action BufferedUpdate;

    static BufferedInputManager instance;
    public static BufferedInputManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(BufferedInputManager)) as BufferedInputManager;
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
        QueuedInput = new LinkedList<InputFrame>();

        CurrentEventDuration = StopTimelineEvery;
        CurrentEvent = TimelineEventTypes.Playback;

        BufferedUpdate += () =>
        {
            Debug.Log("[UPDATE] dt = " + DeltaGameTime);
            if (FrameIsNew)
                Debug.Log("[FRAME] f = " + CurrentFrame);
        };
    }

    void OnBufferedUpdate(float timeRatio)
    {
        var thisGameTime = timeRatio * StopTimelineEvery;
        DeltaGameTime = thisGameTime - LastGameTime;
        LastGameTime = thisGameTime;

        if (BufferedUpdate != null)
            BufferedUpdate();
    }

    void Update()
    {
        if (DebugBypass)
        {
            CurrentFrame = new InputFrame
            {
                Horizontal = Input.GetAxis("Horizontal"),
                Vertical = Input.GetAxis("Vertical"),
                Fire1 = Input.GetButton("Fire1"),
                Fire2 = Input.GetButton("Fire2")
            };
            FrameIsNew = LastFrame != CurrentFrame;
            LastFrame = CurrentFrame;
            DeltaGameTime = Time.deltaTime;
            if (BufferedUpdate != null) BufferedUpdate();
            return;
        }

        CurrentEventTime += Time.deltaTime;

        switch (CurrentEvent)
        {
            case TimelineEventTypes.Playback:
            {
                var timeRatio = CurrentEventTime / CurrentEventDuration;

                InputFrame frame;
                bool updatePerformed = false;
                FrameIsNew = false;
                while (QueuedInput.Count > 0 && (frame = QueuedInput.First.Value).TimeRatio <= timeRatio)
                {
                    FrameIsNew = true;
                    QueuedInput.RemoveFirst();
                    CurrentFrame = frame;
                    OnBufferedUpdate(frame.TimeRatio);
                    updatePerformed = true;
                }
                // at least one update per frame anyway
                if (!updatePerformed)
                    OnBufferedUpdate(timeRatio);
                break;
            }

            case TimelineEventTypes.Record:
            {
                var frame = new InputFrame
                {
                    Horizontal = Input.GetAxis("Horizontal"),
                    Vertical = Input.GetAxis("Vertical"),
                    Fire1 = Input.GetButton("Fire1"),
                    Fire2 = Input.GetButton("Fire2")
                };

                if (QueuedInput.Count == 0 ? frame != default(InputFrame) : frame != QueuedInput.Last.Value)
                {
                    frame.TimeRatio = CurrentEventTime / CurrentEventDuration;
                    QueuedInput.AddLast(frame);
                }
                break;
            }
        }

        if (CurrentEventTime >= CurrentEventDuration)
        {
            CurrentEvent = CurrentEvent.GetNextEvent();
            CurrentEventTime = 0;

            Debug.Log("EVENT CHANGE! " + CurrentEvent);

            switch (CurrentEvent)
            {
                case TimelineEventTypes.Playback:
                    LastGameTime = 0;
                    CurrentEventDuration = StopTimelineEvery;
                    break;

                case TimelineEventTypes.Record:
                    if (QueuedInput.Count > 0)
                        Debug.LogError("Some input frames are still queued but we need to start recording again...?");
                    CurrentEventDuration = StopTimelineFor;
                    break;
            }
        }
    }
}