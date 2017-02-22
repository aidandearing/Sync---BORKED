using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class RawImageColourSequencer : MonoBehaviour
{
    public RawImage image;
    public Gradient gradient;
    public enum Format { Loop, PingPong, Random };
    public Format format = Format.Loop;
    public enum Animation { Lerp, FadeTo, FadeFrom };
    public Animation animation = Animation.FadeTo;
    public Color fade = Color.black;
    [Range(1, 360)]
    public int steps = 16;
    private int step;
    private Color start;
    private Color end;
    public Synchronism.Synchronisations synchronisation = Synchronism.Synchronisations.WHOLE_NOTE;

    // Use this for initialization
    void Start()
    {
        ((Synchronism)Blackboard.Global["Synchroniser"].Value).synchronisers[synchronisation].RegisterCallback(this, Callback);
        Callback();
    }

    // Update is called once per frame
    void Update()
    {
        image.color = Color.Lerp(start, end, ((Synchronism)Blackboard.Global["Synchroniser"].Value).synchronisers[synchronisation].Percent);
    }

    void Callback()
    {
        step++;
        
        start = end;

        float evaluate = 0;

        switch(format)
        {
            case Format.Loop:
                if (step >= steps)
                {
                    step -= steps;
                }
                evaluate = (float)step / (float)steps;
                break;
            case Format.PingPong:
                if (step >= steps * 2)
                {
                    step -= steps * 2;
                }
                if (step < steps)
                {
                    evaluate = (float)step / (float)steps;
                }
                else
                {
                    evaluate = (steps - ((float)step - steps)) / (float)steps;
                }
                break;
            case Format.Random:
                evaluate = Random.Range(0.0f, 1.0f);
                break;
        }

        end = gradient.Evaluate(evaluate);

        switch (animation)
        {
            case Animation.Lerp:
                break;
            case Animation.FadeTo:
                start = end;
                end = fade;
                break;
            case Animation.FadeFrom:
                start = fade;
                break;
        }
    }
}
