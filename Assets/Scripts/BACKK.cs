using Unity.VisualScripting;
using UnityEngine;

public class BACKK : MonoBehaviour
{
    public Transform playerTransform; // R�f�rence au transform du joueur
    public string enemyTag = "Enemy"; // Tag pour les ennemis


    public Animator enemyAnimator; // R�f�rence � l'Animator de l'ennemi

    public float anglemin = 10f;
    public float distancemin = 2f;

    void Update()
    {
        // R�cup�rer tous les ennemis avec le tag sp�cifi�
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        foreach (GameObject enemyObject in enemies)
        {
            Transform enemyTransform = enemyObject.transform;

            // Vecteur de diff�rence entre les positions du joueur et de l'ennemi
            Vector3 playerToEnemy = enemyTransform.position - playerTransform.position;

            // Calcul de l'angle entre le vecteur de direction du joueur et le vecteur joueur-vers-ennemi
            float angle = Vector3.Angle(playerTransform.forward, playerToEnemy);

            // Calcul de la distance entre le joueur et l'ennemi
            float distance = playerToEnemy.magnitude;


            // Direction joueur et ennemi
            Vector3 playerDirection = playerTransform.forward.normalized;
            Vector3 enemyDirection = playerToEnemy.normalized;


            // Produit scalaire : Regardent dans la m�me direction ?
            float dotProduct = Vector3.Dot(playerDirection, enemyDirection);

            // Produit
            Vector3 crossProduct = Vector3.Cross(playerDirection, playerToEnemy);

            // cross.z doit �tre positif
            bool isBehind = crossProduct.z > 0f;


            if (dotProduct > 0.8f && distance < distancemin && angle < anglemin && isBehind)
            {
                Debug.Log("Tu peux backstab !!");
            }
        }
    }
}