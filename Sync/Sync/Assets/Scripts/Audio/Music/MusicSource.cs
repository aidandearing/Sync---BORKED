using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicSource : MonoBehaviour
{
    public MusicTrack.Format format = MusicTrack.Format.Linear;
    public GameObject parent;
    public AnimationCurve volume;
    private float volumeTrack = 1;

    public AudioClip[] clips;
    private int index = 0;
    private float evaluate = 0;
    public AudioSource instance;

    public Synchronism.Synchronisations synchronisation = Synchronism.Synchronisations.BAR_8;
    private Synchroniser synchroniser;

    private bool isInitialised = false;

    void Initialise()
    {
        if (!isInitialised)
        {
            if (((Synchronism)Blackboard.Global["Synchroniser"].Value) != null)
            {
                synchroniser = ((Synchronism)Blackboard.Global["Synchroniser"].Value).synchronisers[synchronisation];
                synchroniser.RegisterCallback(this, Callback);
                instance.clip = clips[(int)Math.Floor(evaluate * clips.Length)];
                instance.time = synchroniser.Percent * (float)synchroniser.Duration;
                instance.Play();
                isInitialised = true;
            }
        }
    }

    public void Update()
    {
        if (!isInitialised)
            Initialise();
        else
        {
            instance.volume = volumeTrack * volume.Evaluate(evaluate);
        }
    }

    void Callback()
    {
        index++;

        switch (format)
        {
            case MusicTrack.Format.Linear:
                index = Math.Min(index, clips.Length - 1);
                evaluate = ((float)index + synchroniser.Percent) / (float)clips.Length;
                break;
            case MusicTrack.Format.Loop:
                if (index >= clips.Length)
                {
                    index -= clips.Length;
                }
                evaluate = ((float)index + synchroniser.Percent) / (float)clips.Length;
                break;
            case MusicTrack.Format.PingPong:
                if (index >= clips.Length * 2)
                {
                    index -= clips.Length * 2;
                }
                if (index < clips.Length)
                {
                    evaluate = ((float)index + synchroniser.Percent) / (float)clips.Length;
                }
                else
                {
                    evaluate = (clips.Length - ((float)index + synchroniser.Percent - clips.Length)) / (float)clips.Length;
                }
                break;
        }

        if (!(format == MusicTrack.Format.Linear && index == clips.Length - 1))
            instance.Stop();

        instance.clip = clips[(int)Math.Floor(evaluate * clips.Length)];
        instance.time = synchroniser.Percent * (float)synchroniser.Duration;
        instance.Play();
    }

    public void SetVolume(float v)
    {
        volumeTrack = v;
    }
}
