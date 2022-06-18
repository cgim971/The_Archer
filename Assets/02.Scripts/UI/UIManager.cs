using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    [SerializeField] private Button _matchBtn;


    private void Start()
    {
        _matchBtn.onClick.AddListener(() => MatchScene());
    }

    void MatchScene()
    {
        SceneManager.LoadScene("Main");
    }


    private void OnGUI()
    {
            var labelStyle = new GUIStyle();
            labelStyle.fontSize = 50;
            labelStyle.normal.textColor = Color.white;
            GUILayout.Label("MONEY : " + GameManager.Instance._PLAYERSAVE._MONEY.ToString(), labelStyle);
    }
}
