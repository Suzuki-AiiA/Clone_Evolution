using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FingerPointer : MonoBehaviour
{

    public static FingerPointer Instance { get; private set; }


    private RectTransform canvasRectTransform;
    public bool guide;
    private RectTransform backgroundRectTransform;

    private RectTransform rectTransform;
    private Image FingerImage;

    public Sprite touch, idle;


    private void Awake()
    {
        Instance = this;
        canvasRectTransform = transform.parent.GetComponentInParent<RectTransform>();
        backgroundRectTransform = GetComponentInChildren<RectTransform>();

        rectTransform = transform.GetComponent<RectTransform>();

        FingerImage = GetComponentInChildren<Image>();

    }


    private void Update()
    {

        if (guide)
        {
            FingerImage.enabled = true;
            Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;

            rectTransform.anchoredPosition = anchoredPosition;
        }
        else
        {
            FingerImage.enabled = false;
        }

        if (Input.GetMouseButton(0))
        {
            FingerImage.sprite = touch;
        }
        else
        {
            FingerImage.sprite = idle;
        }
    }




    private void HideTooltip()
    {
        gameObject.SetActive(false);
    }




}
