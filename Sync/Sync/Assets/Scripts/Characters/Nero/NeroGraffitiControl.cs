using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class NeroGraffitiControl : MonoBehaviour
{
    [Header("References")]
    public Transform location;
    new public SphereCollider collider;

    [Header("Distance")]
    public float nearDistance = 5.0f;
    public float farDistance = 25.0f;

    void Start()
    {
        collider.radius = farDistance;
    }

    public float Evaluate(Transform other)
    {
        float distance = Mathf.Clamp((other.position - location.position).magnitude, nearDistance, farDistance) / (farDistance - nearDistance);
        return Mathf.Lerp(0, 1, 1 - distance); 
    }
}
