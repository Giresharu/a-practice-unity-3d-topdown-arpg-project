using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScale : MonoBehaviour {
    [Range (0, 2)]
    public float targetScale = 1;
    public float scale = 1;
    public bool pauseGame;
    private bool lastPauseGame;
    public static TimeScale instance;

    public Dictionary<string, ActorController> Players = new Dictionary<string, ActorController> ();
    public Dictionary<string, ActorController> Enemys = new Dictionary<string, ActorController> ();
    public Dictionary<string, ActorController> Mechanicals = new Dictionary<string, ActorController> ();

    [Space (10)]
    [Header ("Effect Objects")]
    public bool All;
    private bool lastAll;
    public bool Player;
    private bool lastPlayer;
    public bool Enemy;
    private bool lastEnemy;
    public bool Boss;
    private bool lastBoss;
    public bool Mechanical;
    private bool lastMechanical;

    private void Awake () {
        instance = this;
    }

    private void Update () {
        if (All != lastAll) {
            Player = All;
            Enemy = All;
            Boss = All;
            Mechanical = All;
        }
        if ((!Player && lastPlayer) || (!Enemy && lastEnemy) || (!Boss && lastBoss) || (!Mechanical && lastMechanical)) {
            All = false;
        }
        lastAll = All;
        lastPlayer = Player;
        lastEnemy = Enemy;
        lastBoss = Boss;
        lastMechanical = Mechanical;

        if (pauseGame) {
            if (lastPauseGame)
                return;
            scale = 0;
            var ge = Players.GetEnumerator ();
            while (ge.MoveNext ()) {
                ge.Current.Value.scaleWithSystem = false;
            }
            ge = Enemys.GetEnumerator ();
            while (ge.MoveNext ()) {
                ge.Current.Value.scaleWithSystem = true;
            }
            ge = Mechanicals.GetEnumerator ();
            while (ge.MoveNext ()) {
                ge.Current.Value.scaleWithSystem = true;
            }
            ge.Dispose ();
            return;
        }
        lastPauseGame = pauseGame;

        scale = targetScale;
        if (All) {
            var ge = Players.GetEnumerator ();
            while (ge.MoveNext ()) {
                ge.Current.Value.scaleWithSystem = true;
            }
            ge = Enemys.GetEnumerator ();
            while (ge.MoveNext ()) {
                ge.Current.Value.scaleWithSystem = true;
            }
            ge = Mechanicals.GetEnumerator ();
            while (ge.MoveNext ()) {
                ge.Current.Value.scaleWithSystem = true;
            }
            ge.Dispose ();
            return;
        }
        if (Player) {
            var ge = Players.GetEnumerator ();
            while (ge.MoveNext ()) {
                ge.Current.Value.scaleWithSystem = true;
            }
            ge.Dispose ();
        }
        if (Enemy) {
            var ge = Enemys.GetEnumerator ();
            while (ge.MoveNext ()) {
                if (!ge.Current.Value.isBoss)
                    ge.Current.Value.scaleWithSystem = true;
            }
            ge.Dispose ();
        }
        if (Boss) {
            var ge = Enemys.GetEnumerator ();
            while (ge.MoveNext ()) {
                if (ge.Current.Value.isBoss)
                    ge.Current.Value.scaleWithSystem = true;
            }
            ge.Dispose ();
        }
        if (Mechanical) {
            var ge = Mechanicals.GetEnumerator ();
            while (ge.MoveNext ()) {
                ge.Current.Value.scaleWithSystem = true;
            }
            ge.Dispose ();
        }
    }
}