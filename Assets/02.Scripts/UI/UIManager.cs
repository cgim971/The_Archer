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
    private void Start()
    {
        _matchBtn.onClick.AddListener(() => MatchScene());
        _settingBtn.onClick.AddListener(() => OnSetting());
        _exitBtn.onClick.AddListener(() => OffSetting());
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


}
