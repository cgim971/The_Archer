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

    private void Start()
    {
        _matchBtn.onClick.AddListener(() => MatchScene());
        _settingBtn.onClick.AddListener(() => OnSetting());
    }
    void MatchScene()
    {
        GameManager.Instance._PLAYERSAVE._HP = GameManager.Instance._PLAYERSAVE._MAXHP;
        SceneManager.LoadScene("main");
    }
    
    void OnSetting()
    {

    }
    
    

}
