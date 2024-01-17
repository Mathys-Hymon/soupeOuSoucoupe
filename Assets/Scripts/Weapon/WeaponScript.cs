using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [SerializeField] private int damage, magazineSize, bulletPerShot,spread, semiAutoShootNum, amoDistance;
    [SerializeField] private float fireSpeed, reloadTime, cameraShake;
    [SerializeField] private weaponMode fireMode;

    [Header("References")]
    [SerializeField] private Vector3 aimPos;
    [SerializeField] private GameObject bulletRef, cartridgeRef;
    [SerializeField] private GameObject[] weaponMesh;
    [SerializeField] private Transform bulletSpawnPos, cartridgeSpawnPos;

    [Header("Recoil")]
    [SerializeField] private float recoilForce;
    [SerializeField] private float recoilRotation;


    private bool shooting, canShoot = true, reloading;
    private int bulletLeft, totalBullet, semiAutoShoot;
    private void Start()
    {
        setLayer(0);
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
            float spreadX = Random.Range(-spread, spread);
            float spreadY = Random.Range(-spread, spread);

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
            print(hit.collider.gameObject.name);

            GameObject bulletTrail = Instantiate(bulletRef, bulletSpawnPos.position, bulletSpawnPos.rotation);
            bulletTrail.GetComponent<BulletScript>().setTargetPos(hit.point);
            transform.localPosition = transform.localPosition + new Vector3(0, 0, -recoilForce / 20f);
            transform.localRotation = Quaternion.Euler(-recoilRotation*20f, 0, 0);
            CameraShake.instance.Shake(cameraShake, 3f);
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

    public bool isReloading()
    {
        return reloading;
    }
}
