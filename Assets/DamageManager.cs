using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour {

    public GameObject[] damages;
    // private DamageData[] _dds;
    // private DamageData[] dds {
    //     get {
    //         if (_dds == null) {
    //             _dds = new DamageData[damages.Length];
    //             for (int i = 0; i < damages.Length; i++) {
    //                 _dds[i] = damages[i].GetComponent<DamageData>();
    //             }
    //         }
    //         return _dds;

    //     }
    // }

    public void HitEnable (int index = 0) {
        damages[index].SetActive (true);
    }

    public void HitDisable (int index = 0) {
        damages[index].SetActive (false);
    }

    public void AllHitDisable(){
        foreach(GameObject dam in damages){
            dam.SetActive(false);
        }
    }
}