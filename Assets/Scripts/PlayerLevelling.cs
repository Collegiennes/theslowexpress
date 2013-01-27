using System;
using System.Collections.Generic;
using UnityEngine;

class PlayerLevelling : MonoBehaviour
{
    public List<Texture2D> BodyTextures = new List<Texture2D>();
    public List<Texture2D> WingedTextures = new List<Texture2D>();

    public event Action LevelChanged;

    int LevelAtMoveStart;
    int CurrentLevel;

    public Texture2D CurrentBodyTexture
    {
        get { return BodyTextures[CurrentLevel]; }
    }
    public Texture2D CurrentWingedTexture
    {
        get { return WingedTextures[CurrentLevel]; }
    }

    public void Downgrade()
    {
        CurrentLevel--;
        if (CurrentLevel < 0)
        {
            // TODO : death screen, restart
            CurrentLevel = 0;
        }

        if (LevelChanged != null) LevelChanged();

        // todo : Particle effects and shit
    }

    public void Upgrade()
    {
        CurrentLevel = Mathf.Min(CurrentLevel + 1, BodyTextures.Count - 1);
        if (LevelChanged != null) LevelChanged();

        // todo : Particle effects and shit
    }

    static PlayerLevelling instance;
    public static PlayerLevelling Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(PlayerLevelling)) as PlayerLevelling;
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
        CurrentLevel = 0;
        if (LevelChanged != null) LevelChanged();

        TimeKeeper.Instance.PhaseChanged += () =>
        {
            if (TimeKeeper.Instance.Phase == GamePhase.Moving)
                LevelAtMoveStart = CurrentLevel;
            else if (TimeKeeper.Instance.Phase == GamePhase.Grabbing)
            {
                CurrentLevel = LevelAtMoveStart;
                LevelChanged();
            }
        };
    }

    void Update()
    {
    }

    void ChangePhase()
    {
    }
}
