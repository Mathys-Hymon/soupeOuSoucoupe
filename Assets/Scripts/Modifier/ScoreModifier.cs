using UnityEngine;

public class ScoreModifier : MonoBehaviour
{
    [SerializeField] private int score;
    private void OnTriggerEnter(Collider other)
    {
        GameManager.instance.AddScore(score);
        Destroy(gameObject);
    }
}
