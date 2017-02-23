using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SequencerGradient
{
    public enum Format { Linear, Loop, PingPong };
    public Format format = Format.Linear;

    public int duration = 4;
    private int durationCurrent = 0;
    private float evaluate = 0;
    public Synchronism.Synchronisations synchronisation = Synchronism.Synchronisations.BAR_8;
    private Synchroniser synchroniser;

    private bool isInitialised = false;

    void Initialise()
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

    void Callback()
    {
        durationCurrent++;

        switch (format)
        {
            case Format.Linear:
                durationCurrent = Math.Min(durationCurrent, duration);
                evaluate = ((float)durationCurrent + synchroniser.Percent) / (float)duration;
                break;
            case Format.Loop:
                if (durationCurrent >= duration)
                {
                    durationCurrent -= duration;
                }
                evaluate = ((float)durationCurrent + synchroniser.Percent) / (float)duration;
                break;
            case Format.PingPong:
                if (durationCurrent >= duration * 2)
                {
                    durationCurrent -= duration * 2;
                }
                if (durationCurrent < duration)
                {
                    evaluate = ((float)durationCurrent + synchroniser.Percent) / (float)duration;
                }
                else
                {
                    evaluate = (duration - ((float)durationCurrent + synchroniser.Percent - duration)) / (float)duration;
                }
                break;
        }
    }
}
