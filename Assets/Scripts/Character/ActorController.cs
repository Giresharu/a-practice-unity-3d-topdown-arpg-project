using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;

public class ActorController : MonoBehaviour {

    [Range (0, 2)]
    public float timeScale = 1;
    public bool scaleWithSystem;
    public bool immortal;

    [HideInInspector]
    public bool isPlayer;
    public bool isBoss;
    public int indexInType;
    [HideInInspector]
    public bool isGround;
    [HideInInspector]
    public Vector2 targetAxis;
    [HideInInspector]
    public float AxisMag;
    [HideInInspector]
    public bool Attack;
    [HideInInspector]
    public bool newAttack;
    [HideInInspector]
    public Vector2 Axis;
    private bool lastAttack;

    [HideInInspector]
    public bool moveAble = true;
    [Space (10)]
    [Header ("===== NPC SETTING =====")]
    public float MagSmoothTime;
    public float AxisSmoothTime;
    private Vector2 AxisVelocity;
    [Space (10)]
    [Header ("===== ===== =====")]
    public GameObject model;
    public float moveSpeed = 3;
    [Space (5)]
    [Header ("翻滚")]
    public float rollSpeed;
    public float rollDis;
    [Header ("跳跃")]
    public float jumpSpeed = 4.5f;
    public float jumpDis;
    public Vector3 jumpVelocity;
    [Header ("受击")]
    public float hitLength;
    public float hitSpeed;
    [HideInInspector]
    public Vector3 hitDir;
    [HideInInspector]
    public float hitDis;
    public float hitFlashSpeed;
    [Header ("攻击")]
    public GameObject sword;
    [Header ("防御")]
    public float strikeDis;
    public GameObject guard;
    [Header ("弓箭")]
    public GameObject bow;
    public GameObject arrow_zero;
    protected GameObject arrow;
    public GameObject arrowNode;

    private Vector3 planarVec;
    private Vector3 thrustVec;
    private Vector3 planarVelocity;
    private float attackLayerLerpTartget;
    private float defenseLayerLerpTartget;
    private Vector3 deltaPosition;
    private float hitTime;

    private bool lockPlanar = false;
    private bool attackIdleUpdateFirst;
    private bool landIdleUpdateFirst;

    [HideInInspector]
    public Animator anim;
    private Rigidbody rigid;
    private CapsuleCollider col;
    private Renderer[] renderers;
    [HideInInspector]
    public SFXManager sfxManager;

    private IEnumerator moveToPosition;
    private IEnumerator moveToDirection;
    private IEnumerator repelMove;

    private void Awake () {
        switch (tag) {
            case "Player":
                TimeScale.instance.Players.Add (name, this);
                break;
            case "Enemy":
                TimeScale.instance.Enemys.Add (name, this);
                break;
            case "Mechanical":
                TimeScale.instance.Mechanicals.Add (name, this);
                break;
            default:
                break;
        }
        anim = model.GetComponent<Animator> ();
        rigid = GetComponent<Rigidbody> ();
        col = GetComponent<CapsuleCollider> ();
        renderers = GetComponentsInChildren<Renderer> ();
        if (this.tag == "Player")
            isPlayer = true;
        else
            isPlayer = false;
        sfxManager = GetComponentInChildren<SFXManager> ();
    }

    private void Update () {
        NPCsignalGet ();
        PlayerMoveMentsUpdate ();
        NPCMoveMentsUpdate ();
        if (scaleWithSystem)
            timeScale = TimeScale.instance.scale;
        anim.speed = timeScale;
    }

    private void NPCsignalGet () {
        if (isPlayer)
            return;
        if (!moveAble)
            targetAxis = Vector2.zero;
        else
            targetAxis = PlayerInput.instance.SquareToCircle (targetAxis);
        Axis = Vector2.SmoothDamp (Axis, targetAxis, ref AxisVelocity, AxisSmoothTime);
        AxisMag = Axis.magnitude;

        if (newAttack && !lastAttack) {
            Attack = true;
        } else
            Attack = false;
        lastAttack = Attack;
        newAttack = false;
    }

    private void PlayerMoveMentsUpdate () {
        if (!isPlayer)
            return;
        anim.SetFloat ("forward", Mathf.Lerp (anim.GetFloat ("forward"), PlayerInput.instance.AxisMag, 0.5f));
        if (PlayerInput.instance.Axis != Vector2.zero)
            model.transform.forward = PlayerInput.instance.Axis.x * transform.right + PlayerInput.instance.Axis.y * transform.forward;
        if (!lockPlanar)
            planarVec = model.transform.forward * PlayerInput.instance.AxisMag * moveSpeed;
        if (PlayerInput.instance.Roll && PlayerInput.instance.AxisMag > 0.1f && CheckState ("idle", "defense")) {
            anim.SetTrigger ("roll");
        }

        if (PlayerInput.instance.Attack && !CheckState ("defense", "defense")) {
            anim.SetLayerWeight (anim.GetLayerIndex ("defense"), 0);
            anim.SetTrigger ("attack");
        }
        if (PlayerInput.instance.Bow && !CheckState ("defense", "defense")) {
            anim.SetLayerWeight (anim.GetLayerIndex ("defense"), 0);
            anim.SetBool ("bow", true);
        } else {
            anim.SetBool ("bow", false);
        }

        if (PlayerInput.instance.Defense && (CheckState ("ground") || CheckState ("blocked") || CheckNextState ("ground")) && !CheckNextState ("jump") && !CheckNextState ("roll") && !CheckNextStateTag ("attack")) {
            anim.SetBool ("defense", true);
        } else {
            anim.SetBool ("defense", false);
        }
    }
    private void NPCMoveMentsUpdate () {
        if (isPlayer)
            return;
        anim.SetFloat ("forward", Mathf.Lerp (anim.GetFloat ("forward"), AxisMag, 0.5f));
        if (Axis != Vector2.zero)
            model.transform.forward = Axis.x * transform.right + Axis.y * transform.forward;
        if (!lockPlanar)
            planarVec = model.transform.forward * AxisMag * moveSpeed;
        if (Attack) {
            anim.SetTrigger ("attack");
        }
    }
    public void SetMoveAble (bool _bool, bool _allowRotate = false) {
        if (isPlayer) {
            PlayerInput.instance.inputAble = _bool;
            PlayerInput.instance.allowRotate = _allowRotate;
        } else
            moveAble = _bool;
    }

    private void FixedUpdate () {
        // rigid.velocity = new Vector3(planarVec.x, rigid.velocity.y, planarVec.z) + thrustVec;
        rigid.position += planarVec * Time.fixedDeltaTime * timeScale;
        rigid.velocity += thrustVec;
        // if (rigid.velocity.y <= maxFallVelocity) {
        //     rigid.velocity = new Vector3 (rigid.velocity.x, maxFallVelocity, rigid.velocity.y);
        // }
        thrustVec = Vector3.zero;
        deltaPosition = Vector3.zero;
    }

    private void OnDestroy () {

    }

    public bool CheckState (string stateName, string layerName = "Base Layer") {
        int i = anim.GetLayerIndex (layerName);
        i = Mathf.Clamp (i, 0, i);
        return anim.GetCurrentAnimatorStateInfo (i).IsName (stateName);
    }
    public bool CheckNextState (string stateName, string layerName = "Base Layer") {
        int i = anim.GetLayerIndex (layerName);
        i = Mathf.Clamp (i, 0, i);
        return anim.GetNextAnimatorStateInfo (i).IsName (stateName);
    }
    public bool CheckStateTag (string tagName, string layerName = "Base Layer") {
        int i = anim.GetLayerIndex (layerName);
        i = Mathf.Clamp (i, 0, i);
        return anim.GetCurrentAnimatorStateInfo (i).IsTag (tagName);
    }
    public bool CheckNextStateTag (string tagName, string layerName = "Base Layer") {
        int i = anim.GetLayerIndex (layerName);
        i = Mathf.Clamp (i, 0, i);
        return anim.GetNextAnimatorStateInfo (i).IsTag (tagName);
    }

    /// <summary>
    /// Message processing block
    /// </summary>
    public void Jump () {
        immortal = true;
        rigid.useGravity = false;
        anim.SetFloat ("jumpRandom", 1);
        SetMoveAble (false);
        lockPlanar = true;
        thrustVec = jumpVelocity;
        sfxManager.Play ("jump");
        anim.SetTrigger ("jump");
        moveToDirection = MoveToDirection (planarVec.normalized, jumpDis, jumpSpeed, "stateFinished");
        StartCoroutine (moveToDirection);
    }

    public void OnGroundEnter () { }

    public void OnGroundExit () { }

    public void OnFallEnter () {
        SetMoveAble (false);
        rigid.useGravity = true;
    }

    public void OnLandEnter () {
        if (moveToPosition != null)
            StopCoroutine (moveToPosition);
        immortal = false;
        rigid.useGravity = true;
        SetMoveAble (true);
        planarVec = Vector3.zero;
        lockPlanar = false;
    }

    public void OnRollEnter () {
        sfxManager.Play ("roll");
        lockPlanar = true;
        SetMoveAble (false);
        StartCoroutine ("RollMove");
        // planarVec = planarVec.normalized * rollSpeed;
    }

    public void OnRollUpdate () {
        // if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime >= 0.5f && anim.GetCurrentAnimatorStateInfo (0).IsName ("roll")) {
        //     planarVec = Vector3.SmoothDamp (planarVec, Vector3.zero, ref planarVelocity, 0.25f);
        // }
    }
    public IEnumerator RollMove () {
        moveToDirection = MoveToDirection (planarVec.normalized, rollDis, rollSpeed);
        StartCoroutine (moveToDirection);
        yield return 0;
    }

    public void OnRollExit () {
        lockPlanar = false;
        SetMoveAble (true);
    }

    public void OnAttackEnter () {
        sfxManager.Play ("attack");
        lockPlanar = true;
        SetMoveAble (false);
        //attackLayerLerpTartget = 1.0f;
        planarVec = Vector3.zero;
        //  Camera.main.SendMessageUpwards ("TempSmooth", true);
    }

    public void OnAttackUpdate () {
        //anim.SetLayerWeight(anim.GetLayerIndex("attack"), Mathf.Lerp(anim.GetLayerWeight(anim.GetLayerIndex("attack")), attackLayerLerpTartget, 0.4f));
    }

    public void OnAttackIdleEnter () {
        attackIdleUpdateFirst = true;
        attackLayerLerpTartget = 0;
    }

    public void OnAttackIdleUpdate () {
        if (!CheckStateTag ("attack") && attackIdleUpdateFirst) {
            attackIdleUpdateFirst = false;
            lockPlanar = false;
            SetMoveAble (true);
        }

        // Camera.main.SendMessageUpwards ("TempSmooth", false);
        //anim.SetLayerWeight(anim.GetLayerIndex("attack"), Mathf.Lerp(anim.GetLayerWeight(anim.GetLayerIndex("attack")), attackLayerLerpTartget, 0.4f));
    }

    public void OnDefenseEnter () {
        sfxManager.Play ("defense");
        defenseLayerLerpTartget = 1.0f;
        anim.SetLayerWeight (anim.GetLayerIndex ("defense"), defenseLayerLerpTartget);
        PlayerInput.instance.OnDefense = true;
        guard.SetActive (true);
    }
    public void OnDefenseUpdate () {
        //   anim.SetLayerWeight (anim.GetLayerIndex ("defense"), Mathf.Lerp (anim.GetLayerWeight (anim.GetLayerIndex ("defense")), defenseLayerLerpTartget, 0.4f));

    }

    public void OnDefenseIdleEnter () {
        defenseLayerLerpTartget = 0;
        anim.SetLayerWeight (anim.GetLayerIndex ("defense"), defenseLayerLerpTartget);
        PlayerInput.instance.OnDefense = false;
        guard.SetActive (false);
    }

    public void OnDefenseIdleUpdate () {
        //    anim.SetLayerWeight (anim.GetLayerIndex ("defense"), Mathf.Lerp (anim.GetLayerWeight (anim.GetLayerIndex ("defense")), defenseLayerLerpTartget, 0.4f));
    }

    public void OnBlockedEnter () {
        lockPlanar = true;
        planarVec = Vector3.zero;
        PlayerInput.instance.inputAble = false;
        anim.SetLayerWeight (anim.GetLayerIndex ("defense"), 0);
    }
    public void OnBlockedIdleEnter () {
        anim.SetLayerWeight (anim.GetLayerIndex ("defense"), defenseLayerLerpTartget);
    }
    public void OnHitEnter () {
        sfxManager.Play ("beHit");
        immortal = true;
        Repel ();
    }
    public void OnHitUpdate () {
        if (!isPlayer)
            return;
        hitTime += Time.deltaTime * timeScale;
        if (hitTime >= hitFlashSpeed) {
            foreach (Renderer ren in renderers)
                ren.enabled = !ren.enabled;
            hitTime = 0;
        }
    }
    public void OnHitExit () {
        immortal = false;
        foreach (Renderer ren in renderers)
            ren.enabled = true;
    }

    public void Repel () {
        if (repelMove != null) {
            StopCoroutine (repelMove);
        }
        SetMoveAble (false);
        lockPlanar = true;
        planarVec = Vector3.zero;
        hitTime = 0;
        repelMove = RepelMove ();
        StartCoroutine (repelMove);
    }

    public void OnPlayerDieEnter () {
        SetMoveAble (false);
        sfxManager.Stop ("beHit");
        sfxManager.Play ("die");
        //   rigid.isKinematic = true;
        rigid.velocity = Vector3.zero;
        TimeScale.instance.pauseGame = true;
        CameraController cc = Camera.main.GetComponentInParent<CameraController> ();
        cc.camSizeState = CameraController.CamSizeState._close;
        cc.camRotState = CameraController.CamRotState._24;

    }

    public void OnPlayerReviveExit () {
        TimeScale.instance.pauseGame = false;
        immortal = false;
        SetMoveAble (true);
        lockPlanar = false;
        planarVec = Vector2.zero;
        CameraController cc = Camera.main.GetComponentInParent<CameraController> ();
        cc.camSizeState = CameraController.CamSizeState._default;
        cc.camRotState = CameraController.CamRotState._0;
    }

    public IEnumerator RepelMove () {
        hitDis = hitDis < 0 ? 0.5f : hitDis;
        hitDir = new Vector3 (hitDir.x, 0, hitDir.z);
        moveToDirection = MoveToDirection (hitDir, hitDis, hitSpeed);
        hitDir = Vector3.zero;
        hitDis = 0;
        StartCoroutine (moveToDirection);
        yield return new WaitForSeconds (hitLength);
        if (!CheckState ("die")) {
            anim.SetTrigger ("stateFinished");
            SetMoveAble (true);
            lockPlanar = false;
            planarVec = Vector2.zero;
        }
    }

    /// <summary>
    /// GroundCheck
    /// </summary>
    /// 
    public void IsGround () {
        anim.SetBool ("isGround", true);
        isGround = true;
    }

    public void IsNotGround () {
        anim.SetBool ("isGround", false);
        isGround = false;
    }

    /// <summary>
    /// RootMotionSetting
    /// </summary>
    /// <param name="_deltaPosition"></param>
    public void OnUpdateRootMotion (Vector3 _deltaPosition) {
        //if (CheckStateTag("attack"))
        //    deltaPosition += _deltaPosition;
    }

    public IEnumerator MoveToPosition (Vector3 targetPosition, float speed, string triggerName = null) {
        moveAble = false;
        planarVec = Vector2.zero;
        while (Vector3.Distance (rigid.position, targetPosition) > 0.01f) {
            rigid.position = Vector3.MoveTowards (rigid.position, targetPosition, Time.fixedDeltaTime * timeScale * speed);
            yield return new WaitForFixedUpdate ();
        }
        if (triggerName != null)
            anim.SetTrigger (triggerName);
    }
    public IEnumerator MoveToDirection (Vector3 dir, float dis, float speed, string triggerName = null) {
        moveAble = false;
        planarVec = Vector2.zero;
        float leftTime = dis / speed;
        while (leftTime >= 0) {
            rigid.position += dir.normalized * speed * Time.fixedDeltaTime * timeScale;
            leftTime -= Time.fixedDeltaTime * timeScale;
            yield return new WaitForFixedUpdate ();
        }
        if (triggerName != null)
            anim.SetTrigger (triggerName);
    }

    /// <summary>
    /// IssueTrigger
    /// </summary>
    /// <param name="triggerName"></param>
    public void IssueTrigger (string triggerName) {
        anim.SetTrigger (triggerName);
    }
}