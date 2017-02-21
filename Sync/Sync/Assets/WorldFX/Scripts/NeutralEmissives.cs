﻿using UnityEngine;
using System.Collections;

public class NeutralEmissives : MonoBehaviour
{
    public Synchronism.Synchronisations ListenTo = Synchronism.Synchronisations.WHOLE_NOTE;
    public ColourTransition colours;
    public Color colour;

    // Use this for initialization
    void Start()
    {
        colours.SetColour();

        Synchronism.Instance.synchronisers[ListenTo].RegisterCallback(this, colours.SetColour);
    }

    // Update is called once per frame
    void Update()
    {
        // Lerp the colour of the light and the emitter layer of the shader
        colour = colours.Evaluate(Synchronism.Instance.synchronisers[Synchronism.Synchronisations.WHOLE_NOTE].Percent);

        Renderer rend = GetComponent<Renderer>();
        rend.material.shader = Shader.Find("Standard");
        rend.material.SetColor("_EmissionColor", colour);
    }
}