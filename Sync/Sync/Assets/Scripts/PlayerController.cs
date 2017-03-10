﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : Controller
{
    public enum MovementAction
    {
        /// <summary>
        /// This character will perform a simple jump
        /// </summary>
        Jump,
        /// <summary>
        /// This character will teleport across the ground, at their current height in their forward direction
        /// </summary>
        Teleport,
        /// <summary>
        /// This character will fall far less slowly, turning most of the speed into forward speed
        /// </summary>
        Glide,
        /// <summary>
        /// This character will perform a jump and then transition into a glide
        /// </summary>
        JumpGlide,
        /// <summary>
        /// This character will hover
        /// </summary>
        Hover,
        /// <summary>
        /// This character will perform a jump and then transition into a hover
        /// </summary>
        JumpHover,
        /// <summary>
        /// This character will thrust themselves into the air
        /// </summary>
        Thrust,
        /// <summary>
        /// This character will jump and then thrust themselves into the air
        /// </summary>
        JumpThrust,
        /// <summary>
        /// This character will count walls as valid to jump off of, as well as the ground
        /// </summary>
        WallJump
    };

    [Header("References")]
    new public Rigidbody rigidbody;
    public Animator animator;

    [Header("Statistics")]


    [Header("Movement")]
    [Range(0, 15)]
    [Tooltip("The speed in m/s that this character will move forward")]
    public float speedForward;
    [Tooltip("By default the controller moves in a 360 direction, with independant camera control, with this true that changes to a camera controlled forward, strafing, and backstepping capable movement")]
    public bool canWalkBackward = false;
    [Range(0, 7)]
    [Tooltip("The speed in m/s that this character will move backwards")]
    public float speedBackward;
    [Range(0, 15)]
    [Tooltip("The speed in m/s that this character will move sideways")]
    public float speedSidestep;
    [Tooltip("The format for this characters movement")]
    public MovementAction movementAction = MovementAction.Jump;
    [Tooltip("Establishes at what timing this character is allowed to perform their movement action, whether it be jumping, teleporting, or whatever it may be")]
    public Synchronism.Synchronisations movementSync = Synchronism.Synchronisations.HALF_NOTE;
    // TELEPORT
    [Range(-25,25)]
    public float movementTeleportDistance = 2.5f;
    public bool movementTeleportThroughWalls = false;
    public bool movementTeleportToTarget = true;
    public Vector3 movementTeleportTarget = Vector3.zero;
    // GLIDE
    [Range(0,10)]
    public float movementGlideDownToForward = 0.9f;
    // THRUST
    public float movementThrustSpeed = 10.0f;
    public SequencerGradient movementThrustSequencer;
    public AnimationCurve movementThrustCurve = new AnimationCurve();
    // JUMP and HOVER
    [Range(0, 10)]
    public float movementHeight = 2;
    public bool movementVectoring = false;
    // GENERAL
    [Range(-1, 100)]
    [Tooltip("The number of times this character is able to perform their movement action, -1 for infinite actions")]
    public float movementCount = 1;
    public bool movementInheritVelocity = true;

    // Use this for initialization
    override protected void Start()
    {

    }

    // Update is called once per frame
    override protected void Update()
    {
        
    }

    // Fixed Update is called once per physics step
    override protected void FixedUpdate()
    {
        animator.SetFloat("moveSpeed", rigidbody.velocity.magnitude);
    }
}
