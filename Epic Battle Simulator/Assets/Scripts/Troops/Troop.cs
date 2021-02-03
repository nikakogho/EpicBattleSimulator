using UnityEngine;
using System.Collections;

public abstract class Troop : MonoBehaviour
{
    public LayerMask troopMask;
    public TroopData data;
    public Vector3 offset;
    public float Health { get; private set; }
    public float Damage { get; private set; }
    private float hitSpeed;
    private float moveSpeed;
    protected Troop target = null;
    private float countdown = 0;
    [HideInInspector]public Animator anim;
    public GameObject deathEffect;
    public Army army;
    public float rotateSpeed = 120;
    [HideInInspector] public Vector3 heartOffset = Vector3.one / 2;
    public float attackAfterTime;
    protected float restCountdown = 0;

    public PlaySide side;
    public TroopAnimType troopAnimType;

    protected virtual bool RbKin { get { return true; } }

    public Vector3 Heart { get { return transform.position + heartOffset; } }

    bool began = false;

    protected Rigidbody rb;

    [HideInInspector] public int currentLevel = 1;

    protected void AddRb()
    {
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = RbKin;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.useGravity = false;
    }

    public void Begin()
    {
        began = true;
        InvokeRepeating("UpdateTarget", 0, 0.25f);
    }

    public void OnClicked(bool playing = true)
    {
        if (began) return;
        if (army.ready) return;
        if (army == Army.red && GameMaster.levelType == LevelType.Actual) return;

        army.RemoveTroop(this);

        if (playing)
            Destroy(gameObject);
        else DestroyImmediate(gameObject);
    }

    GameMaster master;
    public TroopData.TroopBlueprint Blueprint { get; private set; }

    void Awake()
    {
        transform.position = new Vector3(transform.position.x, offset.y, transform.position.z);

        rb = GetComponent<Rigidbody>();

        AddRb();

        heartOffset = Vector3.up * 0.5f;
        
        anim = GetComponent<Animator>();
        if (anim == null) anim = GetComponentInChildren<Animator>();

        anim.applyRootMotion = false;

        Blueprint = data.LevelBlueprint(currentLevel);

        Health = Blueprint.health;
        Damage = Blueprint.damage;
        hitSpeed = Blueprint.hitSpeed;
        moveSpeed = Blueprint.moveSpeed;
    }

    public GameObject troopSign;

    void Start()
    {
        master = GameMaster.instance;

        var troopSign = side == PlaySide.Blue ? master.blueTroopBelowSign : master.redTroopBelowSign;
        Vector3 spawnPos = transform.position;
        spawnPos.y = 0.1f;

        this.troopSign = Instantiate(troopSign, spawnPos, transform.rotation);
        this.troopSign.transform.parent = transform;
    }

    protected virtual void UpdateTarget()
    {
        Troop nearest = null;
        float minDist = float.MaxValue;

        foreach(Collider col in Physics.OverlapSphere(transform.position, 10000, troopMask))
        {
            Vector3 dir = col.transform.position - transform.position;

            float dist = dir.magnitude;

            if(dist < minDist)
            {
                Troop troop = col.GetComponent<Troop>();

                if(troop.side != side)
                {
                    nearest = troop;
                    minDist = dist;
                }
            }
        }

        target = nearest;
    }

    protected virtual void Die()
    {
        army.RemoveTroop(this);

        if (deathEffect != null) Destroy(Instantiate(deathEffect, transform.position, transform.rotation), 5);

        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;

        if(Health <= 0)
        {
            Die();
        }
    }

    public void GetHealed(float heal)
    {
        Health += heal;

        if (Health > Blueprint.health) Health = Blueprint.health;
    }

    protected abstract IEnumerator Attack(Troop target = null);

    public void ReApply()
    {
        army.OnClicked(new Vector3(transform.position.x, 0, transform.position.z), data, currentLevel);

        army.RemoveTroop(this);

        DestroyImmediate(gameObject);
    }

    bool stoppedForSure = false;

    protected virtual void Move(bool shouldAttack = true)
    {
        if (target == null)
        {
            if (!stoppedForSure)
            {
                anim.SetBool("moving", false);
                stoppedForSure = true;
            }

            return;
        }

        if (stoppedForSure) stoppedForSure = false;

        countdown -= Time.fixedDeltaTime;
        if (countdown < 0) countdown = 0;

        Vector3 dir = target.transform.position - transform.position;
        dir.y = 0;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), rotateSpeed * Time.fixedDeltaTime);

        if (dir.magnitude <= data.range)
        {
            switch (troopAnimType)
            {
                case TroopAnimType.German:
                    anim.SetInteger("Status_walk", 0);
                    break;
                case TroopAnimType.Normal:
                    anim.SetBool("moving", false);
                    break;
            }

            if (countdown == 0 && shouldAttack)
            {
                StartCoroutine(Attack(target));
                countdown = 1f / hitSpeed;
            }
        }
        else
        {
            switch (troopAnimType)
            {
                case TroopAnimType.German:
                    anim.SetInteger("Status_walk", 1);
                    break;
                case TroopAnimType.Normal:
                    anim.SetBool("moving", true);
                    break;
            }

            rb.MovePosition(rb.position + transform.forward * Time.fixedDeltaTime * moveSpeed);
        }
    }

    void FixedUpdate()
    {
        if (!began) return;

        Move();
    }

    protected virtual void GizmoDraw()
    {
        if (data == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.range);
    }
    
    void OnDrawGizmosSelected()
    {
        GizmoDraw();
    }
}

public enum TroopAnimType { Normal, German, NotMoving }
public enum PlaySide { Red, Blue }