using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
    public static PlayerInput instance;

    [Header ("===== Signal =====")]
    public Vector2 Axis;
    private Vector2 targetAxis;
    public float AxisMag;
    public bool Walk;

    public bool Jump;
    public bool Attack;
    public bool Roll;
    public bool Defense;
    public bool OnDefense;
    public bool Bow;
    public bool Inter;

    [Header ("===== Setting =====")]
    public bool inputAble = true;
    public bool allowRotate = true;
    public float MagSmoothTime;
    public float AxisSmoothTIme;
    private Vector2 AxisVelocity;

    void Awake () {
        instance = this;
    }

    private void Update () {
        targetAxis = new Vector2 ((Input.GetKey (KeyCode.D) ? 1 : 0) - (Input.GetKey (KeyCode.A) ? 1 : 0) + Input.GetAxis ("Horizontal"), (Input.GetKey (KeyCode.W) ? 1 : 0) - (Input.GetKey (KeyCode.S) ? 1 : 0) + Input.GetAxis ("Vertical"));
        if (!inputAble && !allowRotate) {
            targetAxis = Vector2.zero;
        } else {
            targetAxis = SquareToCircle (targetAxis);
        }
        Axis = Vector2.SmoothDamp (Axis, targetAxis, ref AxisVelocity, AxisSmoothTIme);
        Walk = Input.GetKey (KeyCode.LeftShift) || OnDefense;
        AxisMag = Axis.magnitude;
        if (!inputAble)
            AxisMag = 0;
        if (Walk)
            AxisMag = Mathf.Clamp (AxisMag, 0, 0.5f);

        Jump = Input.GetKeyDown (KeyCode.Space);
        Attack = Input.GetKeyDown (KeyCode.J) || Input.GetButtonDown ("X");
        Roll = Input.GetKeyDown (KeyCode.K) || Input.GetButtonDown ("A");

        Defense = Input.GetKey (KeyCode.I) || Input.GetButton ("RB");
        Bow = Input.GetKey (KeyCode.U) || Input.GetButton ("Y");
        Inter = Input.GetKeyDown (KeyCode.Space) || Input.GetButtonDown ("B");

    }

    public Vector2 SquareToCircle (Vector2 input) {
        Vector2 output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt (1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt (1 - (input.x * input.x) / 2.0f);
        return output;
    }
}