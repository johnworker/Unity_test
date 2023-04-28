using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance;

    public GameObject ControlsPC;
    public GameObject ControlsMobile;
    public Text gameplayHint;
    public float gameplayHintTime = 2.0f;
    public GameObject newDayScreen;
    public Text newDayText;
    public GameObject dyingView;

    [System.Serializable]
    public class Localization
    {
        public Text text;
        public string code;
    }

    public Localization[] localizationTexts;

    private PlayerController player;
    private Localizer localizer;

    private void Awake()
    {
        instance = this;

        bool mobile = Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
        ControlsMobile.SetActive(mobile);
        ControlsPC.SetActive(!mobile);
    }

    void Start()
    {
        player = PlayerController.instance;
        localizer = Localizer.instance;
        EventManager.instance.OnLanguageChange += SetTexts;
        EventManager.instance.OnNewDay += NewDay;
        EventManager.instance.OnPlayerAttacked += PlayerAttacked;
        NewDay();
        SetTexts();
    }

    void SetTexts()
    {
        foreach(Localization local in localizationTexts)
        {
            local.text.text = localizer.GetText(local.code);
        }
    }

    public void ShowGameplayHint(string code)
    {
        gameplayHint.text = localizer.GetText(code);
        StartCoroutine(DisplayGameplayHint());
    }

    IEnumerator DisplayGameplayHint()
    {
        gameplayHint.gameObject.SetActive(true);
        yield return new WaitForSeconds(gameplayHintTime);
        gameplayHint.gameObject.SetActive(false);
    }

    private void NewDay()
    {
        gameplayHint.gameObject.SetActive(false);
        newDayScreen.SetActive(true);
        newDayText.text = string.Format(localizer.GetText("004002"), LevelManager.instance.day);
        dyingView.SetActive(false);
    }

    private void PlayerAttacked()
    {
        dyingView.SetActive(true);
    }
}
