using UnityEngine;

public class ZombieIconScript : MonoBehaviour
{
    [SerializeField] float timeBeforeShowing;
    private GameObject arrowRef;
    void Start()
    {
        arrowRef = transform.GetChild(0).gameObject;
        arrowRef.SetActive(false);
        Invoke(nameof(DisplayArrow), timeBeforeShowing);
    }

    private void DisplayArrow()
    {
        arrowRef.SetActive(true);
    }

    private void Update()
    {
        transform.LookAt(Playermovement.instance.transform.position);
        float distance = Vector3.Distance(transform.position, Playermovement.instance.transform.position);
        transform.localScale = new Vector3(0.001f * distance / 10f, 0.001f * distance / 10f, 0.001f * distance / 10f);
    }
}
