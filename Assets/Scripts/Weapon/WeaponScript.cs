using Unity.VisualScripting;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [SerializeField] private int damage, magazineSize, totalBullet, bulletPerShot, semiAutoShootNum;
    [SerializeField] private float fireSpeed, reloadTime, cameraShake;
    [SerializeField] private weaponMode fireMode;

    [Header("References")]
    [SerializeField] private Vector3 aimPos;
    [SerializeField] private GameObject bulletRef, cartridgeRef;
    [SerializeField] private GameObject muzzleRef;
    [SerializeField] private GameObject[] weaponMesh;
    [SerializeField] private Transform bulletSpawnPos, cartridgeSpawnPos;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] weaponSounds;

    [Header("Recoil")]
    [SerializeField] private float recoilForce;
    [SerializeField] private float recoilRotation;



    private bool shooting, canShoot = true, reloading;
    private int bulletLeft, semiAutoShoot;
    private void Start()
    {
        setLayer(0);
        bulletLeft = magazineSize;
    }
    private enum weaponMode
    {
        automatic,
        semiAuto,
        manual,
    }

    public Vector3 GetAimPos()
    {
        return aimPos;
    }

    public void kickWeapon()
    {
        Invoke("enableSphereCollider", 0.4f);
        setLayer(0);
    }

    public void setLayer(int layer)
    {
        gameObject.layer = layer;

        for(int i = 0; i < weaponMesh.Length; i++) 
        {
            weaponMesh[i].layer = layer;
        }
    }

    private void enableSphereCollider()
    {
        GetComponent<SphereCollider>().enabled = true;
    }

    public void ShootButtonPressed(bool isShooting)
    {
        shooting = isShooting;
        if (shooting)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (bulletLeft > 0 && canShoot && !reloading)
        {
            muzzleRef.SetActive(true);
            canShoot = false;
            bulletLeft--;
            if (fireMode == weaponMode.semiAuto)
            {
                semiAutoShoot++;
            }

            RaycastHit hit;
            if (Physics.Raycast(bulletSpawnPos.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            { 
                if(hit.collider.gameObject.GetComponent<EnemyBehavior>() != null)
                {
                    hit.collider.gameObject.GetComponent<EnemyBehavior>().TakeDamage(damage);
                }
            }
                GameObject bulletTrail = Instantiate(bulletRef, bulletSpawnPos.position, bulletSpawnPos.rotation);
            if (hit.point == new Vector3(0, 0, 0))
            {
                bulletTrail.GetComponent<BulletScript>().setTargetPos(bulletSpawnPos.position + transform.TransformDirection(Vector3.forward * 100));
            }
            else
            {
                bulletTrail.GetComponent<BulletScript>().setTargetPos(hit.point);
            }       

            transform.localPosition = transform.localPosition + new Vector3(0, 0, -recoilForce / 20f);
            transform.localRotation = Quaternion.Euler(-recoilRotation*20f, 0, 0);
            CameraShake.instance.Shake(cameraShake, 3f);
            GameObject cartridge = Instantiate(cartridgeRef, cartridgeSpawnPos.position, Quaternion.identity);
            cartridge.GetComponent<Rigidbody>().AddForce(transform.right * 70);
            HUDManager.instance.UpdateMunTxt(bulletLeft, totalBullet);
            Invoke("ResetMuzzleFlash", 0.05f);
            Invoke("ResetShoot", fireSpeed);
        }

        else if(bulletLeft == 0)
        {
            Reload(true);
        }
    }
    private void ResetMuzzleFlash()
    {
        muzzleRef.SetActive(false);
    }

    private void ResetShoot()
    {
        canShoot = true;

        if(shooting == true && (fireMode == weaponMode.automatic || (fireMode == weaponMode.semiAuto && semiAutoShoot < semiAutoShootNum)))
        {  
            Shoot();
        }
        else if(semiAutoShoot >= semiAutoShootNum && fireMode == weaponMode.semiAuto)
        {
            semiAutoShoot = 0;
        }
    }

    public void Reload(bool reload)
    {
        if(!reload)
        {
            CancelInvoke("ReloadDelay");
        }
        if (bulletLeft < magazineSize && totalBullet > 0 && reload)
        {
            UpdateTxt();
            reloading = true;
            bulletLeft++;
            totalBullet--;
            Invoke("ReloadDelay", reloadTime/magazineSize);
        }
        else
        {
            if(reload)
            {
                UpdateTxt();
            }
            reloading = false;
        }
    }

    private void ReloadDelay()
    {
        Reload(true);
    }
    public bool isReloading()
    {
        return reloading;
    }

    public void UpdateTxt()
    {
        HUDManager.instance.UpdateMunTxt(bulletLeft, totalBullet);
    }
}