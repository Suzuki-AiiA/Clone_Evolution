using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace GM.SceneSwitch
{
    public class VerticalSwitch : SwitchPanel
    {
        [Range(-1, 1)]
        public float positionX;
        public float duration;
        public Ease easeIn, easeOut;
        RectTransform rect;
        Vector2 init;
        protected override void Awake()
        {
            base.Awake();
            Vector3 pos = transform.position;
            pos.x = Camera.main.pixelWidth / 2 * (positionX + 1);
            transform.position = pos;
            rect = GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(Camera.main.pixelWidth * 2, Camera.main.pixelHeight);
            init = rect.sizeDelta;
            rect.DOSizeDelta(new Vector2(0, init.y), duration).OnComplete(() =>
            {
                gameObject.SetActive(false);
            }).SetEase(easeIn).From(init);
        }
        public override void Switch(string n = "")
        {
            if (n == "")
            {
                n = SceneManager.GetActiveScene().name;
            }
            gameObject.SetActive(true);
            rect.DOSizeDelta(init, duration).OnComplete(() =>
            {
                DOTween.KillAll();
                SceneManager.LoadScene(n);
            }).From(new Vector2(0, init.y)).SetEase(easeOut);
        }

        public override void Run(UnityAction action)
        {
            gameObject.SetActive(true);
            rect.DOSizeDelta(init, duration).OnComplete(() =>
            {
                action.Invoke();
                rect.DOSizeDelta(new Vector2(0, init.y), duration).OnComplete(() =>
                {
                    gameObject.SetActive(false);
                }).SetEase(easeIn).From(init);
            }).From(new Vector2(0, init.y)).SetEase(easeOut);
        }
    }
}