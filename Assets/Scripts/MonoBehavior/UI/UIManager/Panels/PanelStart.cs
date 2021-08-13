using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PanelStart : MonoBehaviour
{
    [SerializeField] private ECSLoader _loader;
    [SerializeField] private Button _buttonStart;
    [SerializeField] private Transform _title;

    private RectTransform _titleRect;
    private Tween _titleTween;

    private void Start()
    {
        _buttonStart.onClick.AddListener(HandleOnButtonStart);

        _titleRect = (RectTransform)_title;
        _titleTween = _titleRect.DOAnchorPosY(_titleRect.anchoredPosition.y - 50f, 3f)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void HandleOnButtonStart() 
    {
        _titleTween.Kill();
        _titleTween = _titleRect.DOAnchorPosY(_titleRect.anchoredPosition.y + 1000f, 1f)
            .SetEase(Ease.InBack)
            .OnComplete(() => 
            {
                _loader.Load();
                UIManager.Default.CurentState = UIManager.State.Process;
            });
    }
}
