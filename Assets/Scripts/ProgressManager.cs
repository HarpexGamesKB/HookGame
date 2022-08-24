using UnityEngine;
using UnityEngine.UI;

public class ProgressManager : MonoBehaviour
{
    private int _levelUnlock;
    public Button[] Buttons;
    void Start()
    {
        _levelUnlock = PlayerPrefs.GetInt("LevelUnlocked");
        
        for(int i = 1; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = false;
        }
        if (_levelUnlock > 100)
        {
            _levelUnlock = 100;
        }
        for (int i = 0; i < _levelUnlock; i++)
        {
            Buttons[i].interactable = true;
        }
    }
}
