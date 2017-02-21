using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour
{
    [SerializeField]
    private string _faction;
    public string Faction { get { return _faction; } set { _faction = value; } }

    [SerializeField]
    private Vector3 _movement;
    public Vector3 Movement { get { return _movement; } set { _movement = value; } }
    [SerializeField]
    private float _movementbackstepmult;
    public float MovementBackstepMult { get { return _movementbackstepmult; } set { _movementbackstepmult = value; } }
    [SerializeField]
    private float _movementsprintmult;
    public float MovementSprintMult { get { return _movementsprintmult; } set { _movementsprintmult = value; } }

    // Time control stuff
    private Vector3 lastPosition;
    public float Timestep { get; set; }

    // Use this for initialization
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {

    }
}
