using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelEnd : MonoBehaviour
{
    [SerializeField] private Button _buttonRestart;

    private void Start()
    {
        _buttonRestart.onClick.AddListener(HandleOnButtonRestart);
    }

    private void HandleOnButtonRestart()
    {
        SceneTransition.Default.ReloadScene();
    }
}
