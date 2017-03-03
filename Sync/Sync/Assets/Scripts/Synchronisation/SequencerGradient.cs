using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[System.Serializable]
public class SequencerGradient
{
    public delegate void OnCallback();

    public enum Format { Linear, Loop, PingPong, Random };
    [Header("Format")]
    public Format format = Format.Linear;

    [Header("Timing")]
    public int delay = 0;
    private int delayCurrent = 0;
    public int duration = 4;
    private int durationCurrent = 0;
    private float evaluate = 0;

    [Header("Synchroniser")]
    public Synchronism.Synchronisations synchronisation = Synchronism.Synchronisations.BAR_8;
    public Synchroniser synchroniser;

    public OnCallback callback;

    [NonSerialized]
    public bool isInitialised = false;

    public void Initialise()
    {
        if (!isInitialised)
        {
            if ((Synchronism)Blackboard.Global["Synchroniser"].Value != null)
            {
                isInitialised = true;

                synchroniser = ((Synchronism)Blackboard.Global["Synchroniser"].Value).synchronisers[synchronisation];
                synchroniser.RegisterCallback(this, Callback);
            }
        }
    }

    public float Evaluate()
    {
        if (delayCurrent < delay)
            return 0;

        switch (format)
        {
            case Format.Linear:
                evaluate = ((float)durationCurrent + synchroniser.Percent) / (float)duration;
                break;
            case Format.Loop:
                evaluate = ((float)durationCurrent + synchroniser.Percent) / (float)duration;
                break;
            case Format.PingPong:
                if (durationCurrent < duration)
                {
                    evaluate = ((float)durationCurrent + synchroniser.Percent) / (float)duration;
                }
                else
                {
                    evaluate = (duration - ((float)durationCurrent + synchroniser.Percent - duration)) / (float)duration;
                }
                break;
            case Format.Random:
                evaluate = ((float)durationCurrent + synchroniser.Percent) / (float)duration;
                break;
        }

        return evaluate;
    }

    void Callback()
    {
        if (delayCurrent < delay)
        {
            delayCurrent++;
            return;
        }

        durationCurrent++;

        switch (format)
        {
            case Format.Linear:
                durationCurrent = Math.Min(durationCurrent, duration);
                break;
            case Format.Loop:
                if (durationCurrent >= duration)
                {
                    durationCurrent -= duration;
                }
                break;
            case Format.PingPong:
                if (durationCurrent >= duration * 2)
                {
                    durationCurrent -= duration * 2;
                }
                break;
            case Format.Random:
                durationCurrent = UnityEngine.Random.Range(0, duration);
                break;
        }

        if (callback != null)
            callback.Invoke();
    }
}
