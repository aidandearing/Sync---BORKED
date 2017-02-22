using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Synchronism : MonoBehaviour
{
    public enum Synchronisations { WHOLE_NOTE, HALF_NOTE, QUARTER_NOTE, EIGHTH_NOTE, SIXTEENTH_NOTE, THIRTYSECOND_NOTE, BAR, BAR_2, BAR_4, BAR_8 };
    public Dictionary<Synchronisations, Synchroniser> synchronisers = new Dictionary<Synchronisations, Synchroniser>();

    /// <summary>
    /// This establishes the 'heart rate' of the note waves, this should be set to the BPM of the current music playing
    /// </summary>
    public float BPM = 120;
    public float TimeSignature = 4 / 4;

    protected virtual void Initialise()
    {
        synchronisers.Add(Synchronisations.BAR_8, new Synchroniser(60 / (BPM / TimeSignature) * 8));
        synchronisers.Add(Synchronisations.BAR_4, new Synchroniser(60 / (BPM / TimeSignature) * 4));
        synchronisers.Add(Synchronisations.BAR_2, new Synchroniser(60 / (BPM / TimeSignature) * 2));
        synchronisers.Add(Synchronisations.BAR, new Synchroniser(60 / (BPM / TimeSignature)));
        synchronisers.Add(Synchronisations.WHOLE_NOTE, new Synchroniser(60 / BPM));
        synchronisers.Add(Synchronisations.HALF_NOTE, new Synchroniser(60 / BPM * 0.5));
        synchronisers.Add(Synchronisations.QUARTER_NOTE, new Synchroniser(60 / BPM * 0.25));
        synchronisers.Add(Synchronisations.EIGHTH_NOTE, new Synchroniser(60 / BPM * 0.125));
        synchronisers.Add(Synchronisations.SIXTEENTH_NOTE, new Synchroniser(60 / BPM * 0.0625));
        synchronisers.Add(Synchronisations.THIRTYSECOND_NOTE, new Synchroniser(60 / BPM * 0.03125));

        Blackboard.Global.Add("Synchroniser", new BlackboardValue() { Value = this });
    }

    protected virtual void Start()
    {
        Initialise();

        synchronisers[Synchronisations.BAR_8].RegisterCallback(this, OnTimeBar8);
        synchronisers[Synchronisations.BAR_4].RegisterCallback(this, OnTimeBar4);
        synchronisers[Synchronisations.BAR_2].RegisterCallback(this, OnTimeBar2);
        synchronisers[Synchronisations.BAR].RegisterCallback(this, OnTimeBar);
        synchronisers[Synchronisations.WHOLE_NOTE].RegisterCallback(this, OnTimeWhole);
        synchronisers[Synchronisations.HALF_NOTE].RegisterCallback(this, OnTimeHalf);
        synchronisers[Synchronisations.QUARTER_NOTE].RegisterCallback(this, OnTimeQuarter);
        synchronisers[Synchronisations.EIGHTH_NOTE].RegisterCallback(this, OnTimeEighth);
        synchronisers[Synchronisations.SIXTEENTH_NOTE].RegisterCallback(this, OnTimeSixteenth);
        synchronisers[Synchronisations.THIRTYSECOND_NOTE].RegisterCallback(this, OnTimeThirtySecond);
    }

    protected virtual void Update()
    {
        // Updating all the note waves
        foreach (KeyValuePair<Synchronisations, Synchroniser> synchs in synchronisers)
        {
            synchs.Value.Update();
        }
    }

    protected virtual void OnTimeBar8()
    {

    }

    protected virtual void OnTimeBar4()
    {

    }

    protected virtual void OnTimeBar2()
    {

    }

    protected virtual void OnTimeBar()
    {

    }

    protected virtual void OnTimeWhole()
    {

    }

    protected virtual void OnTimeHalf()
    {

    }

    protected virtual void OnTimeQuarter()
    {

    }

    protected virtual void OnTimeEighth()
    {

    }

    protected virtual void OnTimeSixteenth()
    {

    }

    protected virtual void OnTimeThirtySecond()
    {

    }
}

