using System.Collections.Generic;
using UnityEngine;

class StateRecorder : MonoBehaviour
{
    TimeKeeper TimeKeeper;

    int LastFrameIndex;
    List<StateFrame> RecordedFrames;

    void Start()
    {
        RecordedFrames = new List<StateFrame>();

        TimeKeeper = TimeKeeper.Instance;
        TimeKeeper.PhaseChanged += OnPhaseChanged;
    }

    void Update()
    {
        var timeRatio = TimeKeeper.CurrentTimeRatio;

        if (TimeKeeper.CurrentPhase == GamePhase.IntroFastMove)
            return;

        if (TimeKeeper.CurrentPhase == GamePhase.Moving)
        {
            RecordedFrames.Add(new StateFrame
            {
                TimeRatio = timeRatio,
                Position = transform.position
            });
        }
        else
        {
            int fromFrameIndex = LastFrameIndex;

            while (fromFrameIndex < RecordedFrames.Count && 
                   timeRatio >= RecordedFrames[fromFrameIndex].TimeRatio)
            {
                fromFrameIndex++;
            }
            fromFrameIndex--;

            var isFirstFrame = fromFrameIndex == -1;
            var isLastFrame = fromFrameIndex == RecordedFrames.Count - 1;

            var fromFrame = isFirstFrame ? RecordedFrames[0] : RecordedFrames[fromFrameIndex];
            var toFrame = isLastFrame ? RecordedFrames[RecordedFrames.Count - 1] : RecordedFrames[fromFrameIndex + 1];

            var gradient = isFirstFrame || isLastFrame ? 0 : Mathf.Clamp01((timeRatio - fromFrame.TimeRatio) / (toFrame.TimeRatio - fromFrame.TimeRatio));
            transform.position = Vector3.Lerp(fromFrame.Position, toFrame.Position, gradient);

            LastFrameIndex = fromFrameIndex;
            //Debug.Log("last frame is " + fromFrameIndex + " for time ratio = " + timeRatio);
        }
    }

    void OnPhaseChanged()
    {
        if (TimeKeeper.CurrentPhase == GamePhase.Moving)
            RecordedFrames.Clear();
        else
            LastFrameIndex = 0;
    }
}

struct StateFrame
{
    public float TimeRatio;
    public Vector3 Position;
}