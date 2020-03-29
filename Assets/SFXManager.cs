using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
[System.Serializable]
public class SFX_Object {
    public string name;
    public StudioEventEmitter see;
    public ParamRef[] Params = new ParamRef[0];

    public SFX_Object (StudioEventEmitter _see) {
        see = _see;
        Params = see.Params;
        name = see.name;
    }
}

public class SFXManager : MonoBehaviour {

    public Dictionary<string, SFX_Object> sfxs = new Dictionary<string, SFX_Object> ();

    private void Awake () {
        sfxs.Clear ();
        foreach (StudioEventEmitter see in GetComponentsInChildren<StudioEventEmitter> ()) {
            SFX_Object sfx = new SFX_Object (see);
            sfxs.Add (sfx.name, sfx);
        }
    }

    public void Play (string _name) {
        SFX_Object sfx;
        if (sfxs.TryGetValue (_name, out sfx)) {
            sfx.see.Play ();
        }
    }

    public void Stop (string _name) {
        SFX_Object sfx;
        if (sfxs.TryGetValue (_name, out sfx)) {
            sfx.see.Stop ();
        }
    }

    public void SetParams (string _name, string _param, float _value, bool _ignorSeekSpeed = false) {
        SFX_Object sfx;
        if (sfxs.TryGetValue (_name, out sfx)) {
            sfx.see.SetParameter (_param, _value, _ignorSeekSpeed);
        }
    }
}