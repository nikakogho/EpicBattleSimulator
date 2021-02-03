using UnityEngine;
using System.Collections;

public class Shooter : Troop
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public int goesThrough = 1;
    private float destroyBulletAfter = 10;
    public bool directInY = false;

    [HideInInspector] public string weaponName = "";
    [HideInInspector] public bool capitalS = false;
    public bool diesAfterTime = false;
    [HideInInspector] public float timeForDeath = 0;

    protected override IEnumerator Attack(Troop target = null)
    {
        if (target == null) target = this.target;

        switch (troopAnimType)
        {
            case TroopAnimType.German:
                StartCoroutine(GermanShoot());
                break;
            default:
                anim.SetTrigger("hit");
                break;
        }

        yield return new WaitForSeconds(attackAfterTime);

        if(target != null)
        {
            Vector3 direction = target.Heart - firePoint.position;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(direction));
            Destroy(bullet, destroyBulletAfter);
            Bullet bull = bullet.GetComponent<Bullet>();

            bull.side = side;
            bull.damage = Damage;
            bull.canHit = data.canHit;
            bull.goesThrough = goesThrough;
            bull.desiredY = target.Heart.y;

            if (diesAfterTime) bull.diesAfterTouchTime = timeForDeath;

            if (!directInY)
                direction.y = 0;

            bullet.GetComponent<Rigidbody>().velocity = direction.normalized * bulletSpeed;
        }
    }

    IEnumerator GermanShoot()
    {
        char first = capitalS ? 'S' : 's';

        for(int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(0.05f);
            anim.SetInteger(first + "tatus_" + weaponName, i);
        }

        anim.SetInteger(first + "tatus_" + weaponName, 0);
    }
}