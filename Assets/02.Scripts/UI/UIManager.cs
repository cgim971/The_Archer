using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIManager : MonoBehaviour
{

    [SerializeField] private Button _matchBtn;
    [SerializeField] private Button _settingBtn;
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _exitBtn;
    [SerializeField] private Button _endGameBtn;
    [SerializeField] private Button _continueGameBtn;

    [SerializeField] Text _moneyText;
    private void Start()
    {
        _matchBtn.onClick.AddListener(() => MatchScene());
        _settingBtn.onClick.AddListener(() => OnSetting());
        _exitBtn.onClick.AddListener(() => OffSetting());
        _endGameBtn.onClick.AddListener(() => EndGame());
        _continueGameBtn.onClick.AddListener(() => OffSetting());
    }

    private void Update()
    {
        SetMoneyText();
    }

    void MatchScene()
    {
        GameManager.Instance._PLAYERSAVE._HP = GameManager.Instance._PLAYERSAVE._MAXHP;
        GameManager.Instance.STAGENUMBER += 1;
        SceneManager.LoadScene(GameManager.Instance.STAGENUMBER);
    }

    void OnSetting()
    {
        _panel.SetActive(true);
    }

    public void OffSetting()
    {
        _panel.SetActive(false);
    }

    public void SetMoneyText()
    {
        _moneyText.text = $"MONEY : {GameManager.Instance._PLAYERSAVE._MONEY}";
    }

    public void EndGame()
    {
        Application.Quit();
    }
}
