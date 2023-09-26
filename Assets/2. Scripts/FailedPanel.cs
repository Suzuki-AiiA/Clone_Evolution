using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class FailedPanel : MonoBehaviour
{
    public Transform text;
    public Image button;
    public float duration;

    private void OnEnable()
    {
        button.color = new Color(1, 1, 1, 0);
        float y = text.localPosition.y;
        text.DOLocalMoveY(1000, 0.0001f);

        GetComponent<Image>().DOFade(0.4f, 0.1f).OnComplete(() =>
        {
            text.DOLocalMoveY(y, duration).SetEase(Ease.OutElastic).OnComplete(() =>
            {
                button.DOFade(1, duration / 4);
            });
        });

    }
}
