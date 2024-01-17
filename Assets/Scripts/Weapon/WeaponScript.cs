using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [SerializeField] private int damage, magazineSize, bulletPerShot,spread, semiAutoShootNum;
    [SerializeField] private float fireSpeed, reloadTime;
    [SerializeField] private weaponMode fireMode;

    [Header("References")]
    [SerializeField] private Vector3 aimPos;
    [SerializeField] private GameObject bulletRef, cartridgeRef;
    [SerializeField] private Transform bulletSpawnPos, cartridgeSpawnPos;

    [Header("Recoil")]
    [SerializeField] private float recoilForce;
    [SerializeField] private float recoilRotation;

    private bool shooting, canShoot = true, reloading;
    private int bulletLeft, totalBullet, semiAutoShoot;
    private void Start()
    {
        totalBullet = 1000;
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
            float spreadX = Random.Range(-spread, spread);
            float spreadY = Random.Range(-spread, spread);

            canShoot = false;
            bulletLeft--;
            if (fireMode == weaponMode.semiAuto)
            {
                semiAutoShoot++;
            }

            for(int i  = 0; i < bulletPerShot; i++)
            {
                Instantiate(bulletRef, bulletSpawnPos.position, bulletSpawnPos.rotation);
            }

            transform.localPosition = transform.localPosition + new Vector3(0, 0, -recoilForce / 20f);
            transform.localRotation = Quaternion.Euler(-recoilRotation*20f, 0, 0);
            CameraShake.instance.Shake(1.5f, 5f);
            GameObject cartridge = Instantiate(cartridgeRef, cartridgeSpawnPos.position, Quaternion.identity);
            cartridge.GetComponent<Rigidbody>().AddForce(transform.right * 70);

            Invoke("ResetShoot", fireSpeed);
        }
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

    public void Reload()
    {
        if (bulletLeft < magazineSize && totalBullet > 0)
        {
            reloading = true;
            bulletLeft++;
            totalBullet--;
            Invoke("Reload", reloadTime/magazineSize);
        }
        else
        {
            reloading = false;
        }
       
    }
}
