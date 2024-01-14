using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [SerializeField] private int damage, magazineSize, bulletPerShot, semiAutoShootNum;
    [SerializeField] private float fireSpeed, spread, reloadTime;
    [SerializeField] private weaponMode fireMode;
    [SerializeField] private GameObject bulletRef, cartridgeRef;
    [SerializeField] private Transform bulletSpawnPos, cartridgeSpawnPos;

    private bool shooting, canShoot, reloading;
    private int bulletLeft, totalBullet, semiAutoShoot;

    private enum weaponMode
    {
        automatic,
        semiAuto,
        manual,
    }

    public void ShootButtonPressed(bool isShooting)
    {
        shooting = isShooting;
        print("shoot : " + shooting);
        if(shooting )
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if(bulletLeft > 0 && canShoot && !reloading)
        {
            canShoot = false;
            bulletLeft--;

            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);

            Instantiate(bulletRef, bulletSpawnPos.position, Quaternion.identity);

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
        if(bulletLeft < magazineSize)
        {
            reloading = true;
            bulletLeft++;
            Invoke("Reload", reloadTime/magazineSize);
        }
        else
        {
            reloading = false;
        }
       
    }
}
