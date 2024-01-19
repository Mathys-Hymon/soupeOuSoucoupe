using UnityEngine;
using TMPro;

public class ItemSpawnerText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI distanceText;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        transform.LookAt(Playermovement.instance.transform.position);
        float distance = Vector3.Distance(transform.position, Playermovement.instance.transform.position);
        distanceText.text = Mathf.RoundToInt(distance) + "m";
        transform.localScale = new Vector3(0.001f * distance / 10f, 0.001f * distance / 10f, 0.001f * distance / 10f);
        if(distance < 10)
        {
            gameObject.SetActive(false);
        }
    }
}
