using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI instance;
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private Image _image;
    [SerializeField] private Text _stageText;
    [SerializeField] private Text _countText;
    PlayerSave _playerSave;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        _playerSave = GameManager.Instance._PLAYERSAVE;
        UpdateUI();
    }

    public void UpdateUI()
    {
        _healthSlider.maxValue = _playerSave._MAXHP;
        _healthSlider.value = _playerSave._HP;
    }

    public void Count(int count)
    {
        if (count == 0) _countText.gameObject.SetActive(false);
        _countText.text = count.ToString();
    }
    public void State(int stage)
    {
        _stageText.text = $"Stage {stage}";
    }
}
