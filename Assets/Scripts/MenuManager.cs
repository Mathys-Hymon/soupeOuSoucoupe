using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    static MenuManager instance;

    [SerializeField] private GameObject optionMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private InputActionReference escape;

    void Start()
    {
        instance = this;
    }

    private void OnEnable()
    {
        escape.action.started += ReturnBack;
    }

    private void OnDisable()
    {
        escape.action.started -= ReturnBack;
    }

    public void OptionMenu()
    {
        optionMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void ReturnBack(InputAction.CallbackContext obj)
    {
        optionMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void Play()
    {
        SceneManager.LoadScene("EnemieTestEvann");
    }


}
