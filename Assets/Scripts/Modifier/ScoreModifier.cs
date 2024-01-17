using UnityEngine;

public class ScoreModifier : MonoBehaviour
{
    [SerializeField] private int score;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.AddScore(score);
            Destroy(gameObject);
        }
    }
}
