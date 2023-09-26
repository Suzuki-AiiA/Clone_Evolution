using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GM.SceneSwitch
{
    public class NormalFade : SwitchPanel
    {
        public float duration;
        public Ease easeIn, easeOut;
        protected override void Awake()
        {
            base.Awake();
            GetComponent<Image>().DOFade(0, duration).From(1).OnComplete(() =>
            {
                gameObject.SetActive(false);
            }).SetEase(easeIn);
        }
        public override void Switch(string n = "")
        {
            if (n == "")
            {
                n = SceneManager.GetActiveScene().name;
            }
            gameObject.SetActive(true);
            GetComponent<Image>().DOFade(1, duration).From(0).OnComplete(() =>
            {
                DOTween.KillAll();
                SceneManager.LoadScene(n);
            }).SetEase(easeOut);
        }

        public override void Run(UnityAction action)
        {
            gameObject.SetActive(true);
            GetComponent<Image>().DOFade(1, duration).From(0).OnComplete(() =>
            {
                action.Invoke();
                GetComponent<Image>().DOFade(0, duration).From(1).OnComplete(() =>
                {
                    gameObject.SetActive(false);
                }).SetEase(easeIn);
            }).SetEase(easeOut);
        }
    }
}