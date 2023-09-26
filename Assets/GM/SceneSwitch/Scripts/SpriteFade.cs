using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.Events;

namespace GM.SceneSwitch
{

    public class SpriteFade : SwitchPanel
    {
        public Sprite sprite;
        public Image black;
        public Image back;
        RectTransform rect;
        float maxSize;
        public float duration;
        public Ease easeIn, easeOut;

        protected override void Awake()
        {
            base.Awake();
            rect = black.GetComponent<RectTransform>();
            back.GetComponent<RectTransform>().sizeDelta = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
            black.sprite = sprite;
            maxSize = Camera.main.pixelHeight * 3;
            rect.sizeDelta = Vector2.zero;
            rect.DOSizeDelta(Vector2.one * maxSize, duration).OnComplete(() =>
            {
                gameObject.SetActive(false);
            }).From(Vector2.one).SetEase(easeIn);
        }
        public override void Switch(string n = "")
        {
            if (n == "")
            {
                n = SceneManager.GetActiveScene().name;
            }
            gameObject.SetActive(true);
            rect.DOSizeDelta(Vector2.zero, duration).OnComplete(() =>
            {
                DOTween.KillAll();
                SceneManager.LoadScene(n);
            }).From(Vector2.one * maxSize).SetEase(easeOut);
        }

        public override void Run(UnityAction action)
        {
            gameObject.SetActive(true);
            rect.DOSizeDelta(Vector2.zero, duration).OnComplete(() =>
            {
                action.Invoke();
                rect.DOSizeDelta(Vector2.one * maxSize, duration).OnComplete(() =>
                {
                    gameObject.SetActive(false);
                }).From(Vector2.one).SetEase(easeIn);
            }).From(Vector2.one * maxSize).SetEase(easeOut);
        }
    }
}