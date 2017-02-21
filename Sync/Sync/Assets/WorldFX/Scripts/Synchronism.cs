using UnityEngine;
using System.Collections.Generic;

public class Synchronism : MonoBehaviour
{
    private static Synchronism instance;
    public static Synchronism Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Synchronism>();
                instance.Initialise();
            }

            return instance;
        }
        set
        {
            instance = value;
        }
    }

    public enum Synchronisations { WHOLE_NOTE, HALF_NOTE, QUARTER_NOTE, EIGHTH_NOTE, SIXTEENTH_NOTE, THIRTYSECOND_NOTE, BAR, BAR_2, BAR_4, BAR_8 };
    public Dictionary<Synchronisations, Synchroniser> synchronisers = new Dictionary<Synchronisations, Synchroniser>();

    /// <summary>
    /// This establishes the 'heart rate' of the note waves, this should be set to the BPM of the current music playing
    /// </summary>
    //public static int BPM = 120;

    public float BPM = 120;
    public float TimeSignature = 4 / 4;

    public AudioSource Metronome;
    public Light GlobalLight;
    public ParticleSystem CameraFlashes;
    public GameObject Shockwave;
    public GameObject StartChargeEffect;
    public GameObject StartEffect;

    //public GameObject[] Effects;
    public SequencerGameObjects[] effects;

    public Vector3 PositionStart;
    public Vector3 PositionEnd;

    public float startDelay = 10;

    public ColourTransition colours;
    private Color colour;

    public static float startPercent;

    /// <summary>
    /// Intensity is the static value that is changed by Synchronism and players as the game progresses and actions are taken, that dictates how intense both the synchronism effects are
    /// and in special cases how often certain other actions occur (as can be attached to the closest 'intensity' beat by using the nearest delegates)
    /// </summary>
    public static float Intensity;
    public static float Intensity_Magnitude = 0;
    public float intensity_min = 0;
    public float intensity_max = 1000000;
    public float intensity_time = 60;
    public float intensity_exponent = 10;
    public float intensity_goal_threshold = 0.001f;
    public float intensity_goal_lerp = 0.01f;

    public static bool isStarting = true;

    private ParticleSystem.EmissionModule cameraFlashesEmitter;

    void Initialise()
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
    }

    // Use this for initialization
    void Start()
    {
        Instance = this;

        colours.SetColour();
        Intensity = 0;

        GetComponent<Rigidbody>().useGravity = false;

        transform.position = PositionStart;
        BeamToOrb.Orb = transform;

        cameraFlashesEmitter = CameraFlashes.emission;

        Instantiate(StartChargeEffect, transform.position, new Quaternion());

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

    // Update is called once per frame
    void Update()
    {
        // Updating all the note waves
        foreach (KeyValuePair<Synchronisations, Synchroniser> synchs in synchronisers)
        {
            synchs.Value.Update();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        // Start vs Game behaviour
        if (Time.time >= startDelay)
        {
            // The game has started and the orb should be dropped
            GetComponent<Rigidbody>().useGravity = true;

            // The orb has reached its goal position
            if (transform.position.y <= PositionEnd.y)
            {
                // One time trigger on starting being over
                if (isStarting)
                {
                    // Fixing all the values
                    isStarting = false;
                    GetComponent<Rigidbody>().useGravity = false;
                    GetComponent<Rigidbody>().velocity = Vector3.zero;
                    GetComponent<Rigidbody>().isKinematic = true;
                    transform.position = PositionEnd;
                    // Displaying the start effect
                    Instantiate(StartEffect, transform.position, new Quaternion());

                    cameraFlashesEmitter.rateOverTime = new ParticleSystem.MinMaxCurve(1000, 1000);
                }
            }

            // Calculating the baseline intensity that the intensity wants to be at, a function of time elapsed since start, clamped to a maximum value
            float intensity_baseLine = Mathf.Clamp(Mathf.Pow(((Time.time - startDelay) / intensity_time), intensity_exponent), intensity_min, intensity_max);

            // Ensures the current intensity actually achieves the baseline intensity, the problem with interpolation
            if (Mathf.Abs(Intensity - intensity_baseLine) <= intensity_baseLine * intensity_goal_threshold)
            {
                Intensity = intensity_baseLine;
                Intensity_Magnitude = Mathf.Log10(Intensity);
            }
            else
            {
                Intensity = Mathf.Lerp(Intensity, intensity_baseLine, intensity_goal_lerp);
                Intensity_Magnitude = Mathf.Log10(Intensity);
            }
        }
        else
        {
            // The orb is starting, and it should grow in brightness
            float percent = Time.time / startDelay;
            percent *= percent * percent * percent * percent;
            GetComponent<Light>().intensity = percent;
        }

        // Lerp the colour of the light and the emitter layer of the shader
        colour = colours.Evaluate(synchronisers[Synchronisations.WHOLE_NOTE].Percent);

        Renderer rend = GetComponent<Renderer>();
        rend.material.shader = Shader.Find("Standard");
        rend.material.SetColor("_EmissionColor", colour);

        GetComponent<Light>().color = colour;

        GlobalLight.color = colour;

        float cameraRate = Mathf.Clamp(100 * Mathf.Log10(Intensity), 0, 1000);

        if (Mathf.Abs(cameraFlashesEmitter.rateOverTime.constantMin - cameraRate) <= cameraRate * 0.001f)
        {

            cameraFlashesEmitter.rateOverTime = new ParticleSystem.MinMaxCurve(cameraRate, cameraRate);
        }
        else
        {
            float cfer = Mathf.Lerp(cameraFlashesEmitter.rateOverTime.constantMin, cameraRate, 0.01f);
            cameraFlashesEmitter.rateOverTime = new ParticleSystem.MinMaxCurve(cfer, cfer);
        }
    }

    void OnTimeBar8()
    {

    }

    void OnTimeBar4()
    {

    }

    void OnTimeBar2()
    {

    }

    void OnTimeBar()
    {
        //Metronome.Stop();
        Metronome.Play();
    }

    void OnTimeWhole()
    {
        colours.SetColour();

        if (!isStarting)
        {
            int step = 0;

            foreach (SequencerGameObjects sequencer in effects)
            {
                GameObject obj = sequencer.Evaluate();

                if (obj != null && Intensity_Magnitude >= step)
                {
                    Instantiate(obj, transform.position, new Quaternion());
                }

                step++;
            }
        }
    }

    void OnTimeHalf()
    {

    }

    void OnTimeQuarter()
    {
        
    }

    void OnTimeEighth()
    {

    }

    void OnTimeSixteenth()
    {

    }

    void OnTimeThirtySecond()
    {

    }
}
