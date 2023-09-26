using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.UI;


public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public UnityEvent onClick;
    public float moveY;
    public float moveX;
    Vector3 initPos;
    public float moveDuration;
    public Ease ease;
    public Image fade;
    public float fadeTarget;
    private void Start()
    {
        initPos = transform.localPosition;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOLocalMoveY(initPos.y + moveY, moveDuration).SetEase(ease);
        transform.DOLocalMoveX(initPos.x + moveX, moveDuration).SetEase(ease);
        fade?.DOFade(fadeTarget, moveDuration).SetEase(ease);
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        transform.DOLocalMoveY(initPos.y, moveDuration).SetEase(ease);
        transform.DOLocalMoveX(initPos.x, moveDuration).SetEase(ease);
        fade?.DOFade(0, moveDuration).SetEase(ease);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke();
    }
}
