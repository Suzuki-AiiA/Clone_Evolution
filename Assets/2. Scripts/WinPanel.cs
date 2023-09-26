using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WinPanel : MonoBehaviour
{
    public Transform text;
    public Image button;
    public float duration;
    public ParticleSystem conteffi;

    private void OnEnable()
    {
        button.GetComponent<Button>().interactable = false;
        button.color = new Color(1, 1, 1, 0);
        text.localScale = Vector3.zero;
        GetComponent<Image>().DOFade(0.4f, 0.1f).OnComplete(() =>
        {
            conteffi.Play();
            text.DOScale(1, duration).From(0).SetEase(Ease.OutElastic).OnComplete(() =>
            {
                button.DOFade(1, duration / 4).OnComplete(() =>
                {
                    button.GetComponent<Button>().interactable = true;
                });
            });
        });
    }
}
