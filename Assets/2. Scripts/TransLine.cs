using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class TransLine : MonoBehaviour
{
    public static TransLine instance;
    public List<Card> cards = new List<Card>();
    public int height;
    public float duration;
    public float duration2;
    public Transform endTarget;
    public Ease ease;
    public Transform listCardTar;
    public float durationMove;
    public float delay;
    public PositionGet getPos;
    public Transform startPos;
    public float afterCardSize;
    private void Awake()
    {
        instance = this;
    }
    public void AddToLine(Card card)
    {
        Vector3 endPos = endTarget.position;
        endPos.z = Player.instance.transform.position.z;
        cards.Add(card);
        StartCoroutine(Cor(card.transform, endPos, () =>
        {
            card.transform.DOMoveZ(endTarget.position.z, duration2).SetEase(ease);
        }));
    }
    IEnumerator Cor(Transform tr, Vector3 endPos, UnityAction action)
    {
        Vector3 startPos = tr.position;
        Vector3 height = (startPos + endPos) / 2 + Vector3.up * this.height;
        Quaternion initRot = tr.rotation;
        Quaternion endRot = endTarget.rotation;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / duration;
            tr.position = BezierManager.GetBezierPos(startPos, height, endPos, t);
            tr.rotation = Quaternion.Lerp(initRot, endRot, t);
            yield return 0;
        }
        tr.position = endPos;
        tr.rotation = endRot;
        action();
        yield return 0;
    }
    Vector3 RandomPos(Vector3 pos1, Vector3 pos2)
    {
        Vector3 pos = new Vector3();
        pos.x = Random.Range(pos1.x, pos2.x);
        pos.y = pos1.y;
        pos.z = Random.Range(pos1.z, pos2.z);
        return pos;
    }
    public void CallCard(Transform tr)
    {
        StartCoroutine(Move(tr));
    }
    IEnumerator Move(Transform tr)
    {
        WaitForSeconds wait = new WaitForSeconds(delay);
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].transform.DOScale(afterCardSize, durationMove);
            cards[i].transform.DOMove(getPos.GetPos(i) + startPos.position, durationMove);
            cards[i].transform.DORotateQuaternion(listCardTar.rotation, durationMove);
            yield return wait;
        }
        CardDragManager.instance.drag = true;
    }
    public void Remove(Card card){
        cards.Remove(card);
        if(cards.Count == 0){
            UpgradeZone.instance.Resume();
        }
    }
}
