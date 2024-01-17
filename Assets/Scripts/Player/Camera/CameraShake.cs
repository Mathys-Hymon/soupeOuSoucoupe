using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    private Vector3 maximumTranslationShake = Vector3.one, maximumAngularShake = Vector3.one * 15;
    private float traumaExponent = 1, duration = 1, force, seed;

    private void Start()
    {
        instance = this;
    }
    private void Awake()
    {
        seed = Random.value;
    }

    private void Update()
    {
        float shake = Mathf.Pow(force, traumaExponent);
        transform.localPosition = new Vector3( maximumTranslationShake.x * (Mathf.PerlinNoise(seed, Time.time * 25) * 2 - 1), maximumTranslationShake.y * (Mathf.PerlinNoise(seed + 1, Time.time * 25) * 2 - 1), maximumTranslationShake.z * (Mathf.PerlinNoise(seed + 2, Time.time * 25) * 2 - 1)) * shake;
        transform.localRotation = Quaternion.Euler(new Vector3( maximumAngularShake.x * (Mathf.PerlinNoise(seed + 3, Time.time * 25) * 2 - 1), maximumAngularShake.y * (Mathf.PerlinNoise(seed + 4, Time.time * 25) * 2 - 1), maximumAngularShake.z * (Mathf.PerlinNoise(seed + 5, Time.time * 25) * 2 - 1) ) * shake);
        force = Mathf.Clamp01(force - duration * Time.deltaTime);   
    }

    public void Shake(float shakeForce, float NewDuration)
    {
        duration = NewDuration;
        force = Mathf.Clamp01(force + (shakeForce / 10f));
    }
}
