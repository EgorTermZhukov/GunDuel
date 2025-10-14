using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.Source;
using UnityEngine;


public class Tickable
{
    public float Duration;
    public bool Finished = false;
    
    public Tickable(float duration) => Duration = duration;
}
public class Ticker : MonoBehaviour
{
    public float TimeScale = 1f;
    public List<Tickable> Tickables;

    private void Awake()
    {
        DOTween.Init(false);
        DOTween.useSmoothDeltaTime = false;
        DOTween.defaultUpdateType = UpdateType.Manual;
    }

    void Start()
    {
        Tickables = new List<Tickable>();
        G.Ticker = this;
    }
    void Update()
    {
        var dt = TimeScale * Time.deltaTime;
        var finished = new List<Tickable>();
        foreach (var tickable in Tickables)
        {
            tickable.Duration -= dt;
            if (tickable.Duration <= 0)
                finished.Add(tickable);
        }
        foreach (var tickable in finished)
        {
            Tickables.Remove(tickable);
            tickable.Finished = true;
        }
        DOTween.ManualUpdate(dt, dt);
    }
    public Tickable Create(float duration)
    {
        var tickable = new Tickable(duration);
        Tickables.Add(tickable);
        return tickable;
    }
    public Func<bool> CreatePr(float duration)
    {
        var tickable = new Tickable(duration);
        Tickables.Add(tickable);
        return () => tickable.Finished;
    }
}
