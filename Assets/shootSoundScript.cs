using UnityEngine;

public class shootSoundScript : MonoBehaviour
{
    public void setSound(AudioClip sfx)
    {
        GetComponent<AudioSource>().clip = sfx;
        GetComponent<AudioSource>().Play();
    }

    private void Update()
    {
        if(!GetComponent<AudioSource>().isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
