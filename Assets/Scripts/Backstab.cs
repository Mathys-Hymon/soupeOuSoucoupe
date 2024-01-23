using UnityEngine;

public class BackstabDetector : MonoBehaviour
{
    public Transform playerTransform;
    public Transform enemyTransform;

    public float anglemin = 10f; 
    public float distancemin = 2f;

    public string enemyTag = "Enemy"; // Tag pour les ennemis

    void Update()
    {

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        foreach (GameObject enemyObject in enemies)
        {

            // Différence de positions
            Vector3 playerToEnemy = enemyTransform.position - playerTransform.position;


            // Diff angle regard et direction ennemi
            float angle = Vector3.Angle(playerTransform.forward, playerToEnemy);


            // Distance entre les deux
            float distance = playerToEnemy.magnitude;

            // Direction joueur et ennemi
            Vector3 playerDirection = playerTransform.forward.normalized;
            Vector3 enemyDirection = playerToEnemy.normalized;


            // Produit scalaire : Regardent dans la même direction ?
            float dotProduct = Vector3.Dot(playerDirection, enemyDirection);



            // Produit
            Vector3 crossProduct = Vector3.Cross(playerDirection, playerToEnemy);

            // cross.z doit être positif
            bool isBehind = crossProduct.z > 0f;


            if (dotProduct > 0.8f && distance < distancemin && angle < anglemin && isBehind)
            {
                Debug.Log("Tu peux backstab !!");
            }



        }



    }
}//