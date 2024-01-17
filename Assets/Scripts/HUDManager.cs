using TMPro;
using UnityEngine;
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

    public static HUDManager instance; 

    private void Start()
    {
        instance = this;
    }

    public void UpdateEnemiesRemaining(int enemies)
    {
        enemiesRemainingTxt.SetText(enemies + "");  
    }
    public void UpdateMunTxt(int mun)
    {
        munTxt.SetText(mun + "");
    }
    public void UpdatemMaxMunTxt(int maxMun)
    {
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
}