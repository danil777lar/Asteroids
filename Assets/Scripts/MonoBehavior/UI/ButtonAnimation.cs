using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Color _pointerEnterColor;
    [SerializeField] private Color _pointerExitColor;
    [SerializeField] private TextMeshProUGUI _text;

    private void Start()
    {
        _text.color = _pointerExitColor;
    }

    public void OnPointerEnter(PointerEventData data)
    {
        transform.DOScale(Vector2.one * 1.2f, 0.5f);
        _text.DOColor(_pointerEnterColor, 0.5f);
    }

    public void OnPointerExit(PointerEventData data)
    {
        transform.transform.DOScale(Vector2.one, 0.5f);
        _text.DOColor(_pointerExitColor, 0.5f);
    }
}
