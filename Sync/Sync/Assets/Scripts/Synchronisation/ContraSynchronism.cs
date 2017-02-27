using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ContraSynchronism : MonoBehaviour
{
    [Header("References")]
    public Material contraMaterial;
    public GameObject discoBall;
    public Material discoBallMaterial;

    [Header("Emission")]
    public SequencerGradient emissionSequencer;
    public Gradient emissionGradient;

    [Header("Disco Ball Emission")]
    public SequencerGradient discoBallEmissionSequencer;
    public Gradient discoBallEmissionGradient;

    void Start()
    {

    }

    void Update()
    {
        if (!emissionSequencer.isInitialised)
            emissionSequencer.Initialise();

        contraMaterial.SetColor("_EmissionColor", emissionGradient.Evaluate(emissionSequencer.Evaluate()));
        discoBallMaterial.SetColor("_EmissionColor", discoBallEmissionGradient.Evaluate(discoBallEmissionSequencer.Evaluate()));
    }

    void FixedUpdate()
    {

    }
}
