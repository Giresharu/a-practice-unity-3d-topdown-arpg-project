using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXOneShot : MonoBehaviour {
    public void OnIdleExit () {
        Destroy (gameObject);
    }
}