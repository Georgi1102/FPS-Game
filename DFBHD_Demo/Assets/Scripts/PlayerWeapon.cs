using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    #region Varriables
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletSpeed = 30f;
    public float lifeTimePerBullet = 3f;
    public float bulletDMG = 20f;
    public ParticleSystem muzzleFalsh;
    private float fireRate = 6f;
    private float nextTimeToShoot = 0f;
    public Animation noAimAnimation;
    public Animation onAimAnimation;
    public Animation reloadAnimation;
    public Animation walkingAnimation;
    public AimDown aimGun;
    public int maxBullets = 10;
    public float reloadTime = 1f;
    private int currentBullets;
    private bool isReloading = false;
    public AimDown aim;
    public Movement movement;
    #endregion

    void Start()
    {
        currentBullets = maxBullets;
        GetComponent<Movement>();
    }

    void Update()
    {
        FireChecksLogic();
        
    }

    #region CustomLogic

    private void OnEnable()
    {
        //if we switch weapons it will always reloading 
        isReloading = false; //fixed!!!
    }

    private void FireChecksLogic()
    {
        if (isReloading)
            return;

        if (currentBullets <= 0 || Input.GetKeyUp(KeyCode.R) && currentBullets != maxBullets)
        {
           StartCoroutine(Reload());
            return;
        }

        if (Input.GetKey(KeyCode.W) && !aim.Aim() && !movement.Run() && !movement.Crouch())
        {
            walkingAnimation.Play("MoveAnimation");
        }
        

        if (movement.Run() && !aim.Aim() && !movement.Jump())
        {
            walkingAnimation.Play("RunningAnimation");
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToShoot)
        {
            nextTimeToShoot = Time.time + 1f / fireRate;
            Fire();
            currentBullets--;
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

    IEnumerator Reload()
    {
        isReloading = true;      
        reloadAnimation.Play("WeaponReloading");
        yield return new WaitForSeconds(reloadTime);       
        currentBullets = maxBullets;
        isReloading = false;
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
