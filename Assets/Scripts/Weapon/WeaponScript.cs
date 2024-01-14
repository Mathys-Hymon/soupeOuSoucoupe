using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [SerializeField] private int damage, magazineSize, bulletPerShot, semiAutoShootNum;
    [SerializeField] private float fireSpeed, reloadTime;
    [SerializeField] private weaponMode fireMode;
    [SerializeField] private GameObject bulletRef, cartridgeRef;
    [SerializeField] private Transform bulletSpawnPos, cartridgeSpawnPos;

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
            canShoot = false;
            bulletLeft--;
            if (fireMode == weaponMode.semiAuto)
            {
                semiAutoShoot++;
            }

            Instantiate(bulletRef, bulletSpawnPos.position, bulletSpawnPos.rotation);
            GameObject cartridge = Instantiate(cartridgeRef, cartridgeSpawnPos.position, Quaternion.identity);
            cartridge.GetComponent<Rigidbody>().AddForce(transform.right * -70);

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
        print("reload");
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
