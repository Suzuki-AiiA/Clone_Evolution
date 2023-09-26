using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Coffee.UIEffects;
public class SceneSwtich : MonoBehaviour
{
    UITransitionEffect transitionEffect;
    public float duration;
    public static SceneSwtich instance;

    private void Awake()
    {
        instance = this;
        transitionEffect = GetComponent<UITransitionEffect>();
        DOTween.KillAll();
        DOTween.To(() => 1f, (i) => { transitionEffect.effectFactor = i; }, 0f, duration).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    public void SwitchScene(string name = "")
    {
        if (name == "")
        {
            name = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        }
        gameObject.SetActive(true);
        DOTween.KillAll();
        DOTween.To(() => 0f, (i) => { transitionEffect.effectFactor = i; }, 1f, duration).OnComplete(() =>
        {
            DOTween.KillAll();
            UnityEngine.SceneManagement.SceneManager.LoadScene(name);

        });
    }

    public void Call(UnityEngine.Events.UnityAction action)
    {
        gameObject.SetActive(true);
        DOTween.To(() => 0f, (i) => { transitionEffect.effectFactor = i; }, 1f, duration).OnComplete(() =>
        {
            action();
            DOTween.To(() => 1f, (i) => { transitionEffect.effectFactor = i; }, 0f, duration).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        });
    }

}
