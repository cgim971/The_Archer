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

}
