using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector]public PlaySide side;
    [HideInInspector]public float damage;
    [HideInInspector]public int canHit;
    [HideInInspector]public int goesThrough = 1;
    [HideInInspector]public float? diesAfterTouchTime = null;
    private float countdown = 0;
    public float explosionRange = 0;
    public LayerMask troopMask;
    public GameObject effect;
    [HideInInspector] public float desiredY;
    private bool editedY = false;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (goesThrough <= 0) return;

        Troop troop = other.GetComponent<Troop>();

        bool destroy = true;

        if (troop == null)
        {
            Hit(null);
        }
        else if (troop.side != side)
        {
            Hit(troop);

            goesThrough--;

            if (diesAfterTouchTime != null) countdown = diesAfterTouchTime.Value;

            if (goesThrough > 0) destroy = false;
        }
        else destroy = false;

        if(destroy)
        Destroy(gameObject);
    }

    void Update()
    {
        if(countdown > 0)
        {
            countdown -= Time.deltaTime;

            if (countdown <= 0) Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        if (editedY) return;

        if (Mathf.Abs(transform.position.y - desiredY) <= 0.1f)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            editedY = true;
        }
    }

    void Hit(Troop troop)
    {
        if (effect != null) Instantiate(effect, transform.position, transform.rotation);

        TroopFunctions.Attack(canHit, troopMask, explosionRange, damage, transform.position, troop, side);
    }
}