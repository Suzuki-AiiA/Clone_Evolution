using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace GM.SceneSwitch
{
    public class HorizontalSwitch : SwitchPanel
    {
        [Range(-1, 1)]
        public float positionY;
        public float duration;
        public Ease easeIn, easeOut;
        RectTransform rect;
        Vector2 init;
        protected override void Awake()
        {
            base.Awake();
            Vector3 pos = transform.position;
            pos.y = Camera.main.pixelHeight / 2 * (positionY + 1);
            transform.position = pos;
            rect = GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight * 2);
            init = rect.sizeDelta;
            rect.DOSizeDelta(new Vector2(init.x, 0), duration).OnComplete(() =>
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
            }).From(new Vector2(init.x, 0)).SetEase(easeOut);
        }

        public override void Run(UnityAction action)
        {
            gameObject.SetActive(true);
            rect.DOSizeDelta(init, duration).OnComplete(() =>
            {
                action.Invoke();
                rect.DOSizeDelta(new Vector2(init.x, 0), duration).OnComplete(() =>
                {
                    gameObject.SetActive(false);
                }).SetEase(easeIn).From(init);
            }).From(new Vector2(init.x, 0)).SetEase(easeOut);
        }
    }
}