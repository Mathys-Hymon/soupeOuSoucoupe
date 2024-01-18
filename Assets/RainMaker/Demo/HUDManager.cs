using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemiesRemainingTxt;
    [SerializeField] private TextMeshProUGUI munTxt;
    [SerializeField] private TextMeshProUGUI maxMunTxt;
    [SerializeField] private TextMeshProUGUI lifeTxt;
    [SerializeField] private Slider lifeSlider;
    [SerializeField] private GameObject enemiesRemainingGo;
    [SerializeField] private GameObject timerGo;
    [SerializeField] private TextMeshProUGUI timerTxt;
    [SerializeField] private TextMeshProUGUI grenadeTxt;

    [SerializeField] private GameObject gunGo;
    [SerializeField] private GameObject akGo;
    [SerializeField] private GameObject fusilGo;
    [SerializeField] private GameObject uziGo;

    [SerializeField] private GameObject munGo;
    [SerializeField] private GameObject maxMunGo;

    [SerializeField] private GameObject playerHudGo;
    [SerializeField] private GameObject pauseMenuGo;
    private bool isGamePaused;
    [SerializeField] private InputActionReference pause;

    [SerializeField] private TextMeshProUGUI scoreTxt;
    [SerializeField] private TextMeshProUGUI waveTxt;
    [SerializeField] private TextMeshProUGUI killsTxt;
    [SerializeField] private TextMeshProUGUI bestScoreTxt;

    [SerializeField] private Button quitBtn;
    [SerializeField] private Button resumeBtn;

    public static HUDManager instance; 

    private void Start()
    {
        instance = this;
        quitBtn.onClick.AddListener(QuitGame);
        resumeBtn.onClick.AddListener(PauseTheGame);
    }

    private void OnEnable()
    {
        pause.action.started += PauseGame;
    }

    private void OnDisable()
    {
        pause.action.started -= PauseGame;
    }

    private void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void PauseGame(InputAction.CallbackContext obj)
    {
        PauseTheGame();
    }

    private void PauseTheGame()
    {
        if (!isGamePaused)
        {
            isGamePaused = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            playerHudGo.SetActive(false);
            pauseMenuGo.SetActive(true);
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            playerHudGo.SetActive(true);
            Time.timeScale = 1;
            pauseMenuGo.SetActive(false);
            isGamePaused = false;
        }
    }

    public void UpdateEnemiesRemaining(int enemies)
    {
        enemiesRemainingTxt.SetText(enemies + "");  
    }
    public void UpdateMunTxt(int mun, int maxMun)
    {
        munTxt.SetText(mun + "");
        maxMunTxt.SetText(maxMun + "");
    }
    public void UpdateLife(float life)
    {
        lifeTxt.SetText(Mathf.RoundToInt(life) + "");
        lifeSlider.value = life;
    }
    public void ShowTimer()
    {
        timerGo.SetActive(true);
        enemiesRemainingGo.SetActive(false);
    }
    public void HideTimer()
    {
        timerGo.SetActive(false);
        enemiesRemainingGo.SetActive(true);
    }
    public void UpdateTimerTxt(int timer)
    {
        timerTxt.SetText(timer + "");
    }
    public void UpdateGrenadeTxt(int grenade)
    {
        if (grenade > 10)
        {
            grenadeTxt.SetText(grenade + "");
        }
        else
        {
            grenadeTxt.SetText("0"+grenade);
        }
    }
    public void UpdateGunImg(bool gun, bool ak, bool fusil, bool uzi)
    {
        gunGo.SetActive(gun);
        akGo.SetActive(ak);
        fusilGo.SetActive(fusil);
        uziGo.SetActive(uzi);
    }

    public void MunInfos(bool info)
    {
        munGo.SetActive(info);
        maxMunGo.SetActive(info);
    }
    public void UpdateScoreTxt(int score)
    {
        scoreTxt.SetText(score + "");
    }
    public void UpdateWaveTxt(int wave)
    {
        waveTxt.SetText(wave + "");
    }
    public void UpdateKillsTxt(int kills)
    {
        killsTxt.SetText(kills + "");
    }
}
