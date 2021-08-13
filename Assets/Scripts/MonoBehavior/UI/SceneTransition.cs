using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SceneTransition : MonoBehaviour
{
    private static SceneTransition _default;
    public static SceneTransition Default => _default;

    [SerializeField] private Image _image;

    private void Awake()
    {
        _default = this;
    }

    private void Start()
    {
        _image.color = Color.black;
        _image.DOColor(new Color(0f, 0f, 0f, 0f), 1f);
    }

    public void ReloadScene() 
    {
        _image.DOColor(Color.black, 1f)
            .OnComplete(() => Application.LoadLevel("Scenes/SampleScene"));
    }
}
