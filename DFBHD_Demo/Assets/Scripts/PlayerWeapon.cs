using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    #region Varriables
    public GameObject bulletPrefab;
    public GameObject enemy;
    public Transform bulletSpawn;
    public float bulletSpeed = 30f;
    public float lifeTimePerBullet = 3f;
    public float bulletDMG = 20f;
    public ParticleSystem muzzleFalsh;
    private float fireRate = 6f;
    private float nextTimeToShoot = 0f;
    public Animation noAimAnimation;
    public Animation onAimAnimation;
    public AimDown aimGun;
    #endregion


    void Update()
    {
        FireChecksLogic();
    }

    #region CustomLogic

    private void FireChecksLogic()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToShoot)
        {
            nextTimeToShoot = Time.time + 1f / fireRate;
            Fire();
            GetComponent<AudioSource>().Play();
            GetComponent<AimDown>();
            if (aimGun.Aim())
            {
                onAimAnimation.Play("AimRecoilAnimation");
            }
            else if (!aimGun.Aim())
            {
                noAimAnimation.Play("RecoilAnimation");
            }
        }
    }


    public void Fire()
    {

        GameObject bullet = Instantiate(bulletPrefab);
        Physics.IgnoreCollision(bullet.GetComponent<Collider>(),
            bulletSpawn.parent.GetComponent<Collider>()); //Ignoring the weapon and bullet colliders

        bullet.transform.position = bulletSpawn.position;
        Vector3 rotation = bullet.transform.rotation.eulerAngles;
        bullet.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z); //converting the position to eurler rotation 
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward * bulletSpeed,
        ForceMode.Impulse); // adding a forward force to the bullet //simulating an powder detonation 
        muzzleFalsh.Play();

        //TODO: DO NOT KILL THE RAM
        StartCoroutine(DestroyBulletAfterTime(bullet, lifeTimePerBullet)); //starting a countdown to destroy the bullet
        
        
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }

    #endregion
}
