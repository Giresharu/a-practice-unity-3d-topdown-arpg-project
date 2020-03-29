using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleFactorSetting : MonoBehaviour {
    public Canvas cr;

    private void Awake () {
        cr = cr ?? GetComponent<Canvas> ();
    }
    void Update () {
        cr.scaleFactor = Screen.height / 160.0f;
    }
}