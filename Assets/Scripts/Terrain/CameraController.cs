using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public enum CamPosState {
        followTarget,
        pinPosition
    }
    public enum CamRotState {
        _0,
        _24,
        _42
    }

    public enum CamSizeState {
        _default,
        _far,
        _close
    }

    public CamPosState camPosState;
    public CamRotState camRotState;
    public CamSizeState camSizeState;
    public GameObject Target;
    public Vector3 pinPosition;

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private float targetSize;
    private CamPosState lastCamPosState;
    private GameObject lastTarget;
    private bool posSmooth = false;
    private bool tempSmooth = false;

    // Update is called once per frame
    void FixedUpdate () {
        switch (camPosState) {
            case CamPosState.followTarget:
                if (Target == null)
                    Target = GameObject.FindWithTag ("Player");
                if (Target == null) {
                    camPosState = CamPosState.pinPosition;
                    break;
                }
                targetPosition = Target.transform.position + transform.up * 1.2f;
                break;
            case CamPosState.pinPosition:
                targetPosition = pinPosition + transform.up * 1.2f;
                break;
        }
        if (lastTarget != Target || lastCamPosState != camPosState || tempSmooth)
            posSmooth = true;
        transform.position = posSmooth ? Vector3.Lerp (transform.position, targetPosition, 0.1f) : targetPosition;
        lastCamPosState = camPosState;
        lastTarget = Target;
        if (Vector3.Distance (transform.position, targetPosition) <= 0.01)
            posSmooth = false;

        switch (camRotState) {
            case CamRotState._0:
                targetRotation = Quaternion.Euler (0, 0, 0);
                break;
            case CamRotState._24:
                targetRotation = Quaternion.Euler (-24, 0, 0);
                break;
            case CamRotState._42:
                targetRotation = Quaternion.Euler (-42, 0, 0);
                break;
        }
        transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, 0.1f);

        switch (camSizeState) {
            case CamSizeState._default:
                targetSize = 2.4f;
                break;
            case CamSizeState._close:
                targetSize = 1.5f;
                break;
            case CamSizeState._far:
                targetSize = 3f;
                break;
        }
        Camera.main.orthographicSize = Mathf.Lerp (Camera.main.orthographicSize, targetSize, 0.1f);
    }

    public void OnDialogueEnter () {
        camRotState = CamRotState._24;
        camSizeState = CamSizeState._close;
    }
    public void OnDialogueExit () {
        camRotState = CamRotState._0;
        camSizeState = CamSizeState._default;
    }

    public void TempSmooth (bool _tempSmooth) {
        tempSmooth = _tempSmooth;
    }
}