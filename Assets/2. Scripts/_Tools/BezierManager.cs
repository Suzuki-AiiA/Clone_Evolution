using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BezierManager : MonoBehaviour
{
    public List<OnBezierMove> onBezierMoves = new List<OnBezierMove>();
    protected Transform tar;
    protected Coroutine cor;
    public void AddObjectToBezier(Transform target, (Transform, Vector3) end, float height, float speed, float pickUpDelay, UnityAction onStart, UnityAction onComplete, Transform targetParent = null, bool isLocal = false)
    {
        onBezierMoves.Add(new OnBezierMove(
            this,
            target,
            end,
            ((isLocal ? target.localPosition : target.position) + ((end.Item1 == null ? end.Item2 : (isLocal ? end.Item1.localPosition : end.Item1.position)) + end.Item2)) / 2 + Vector3.up * (height == 0 ? 0 : (height)),
            0,
            speed,
            pickUpDelay,
            onStart,
            onComplete,
            targetParent == null ? tar : targetParent,
            isLocal
        ));
    }
    protected void StartStackCoroutine()
    {
        cor = StartCoroutine(CallBezier());
    }
    protected void StopStackCoroutine()
    {
        StopCoroutine(cor);
    }
    protected virtual void OnComplete()
    {

    }

    IEnumerator CallBezier()
    {
        while (true)
        {
            for (int i = 0; i < onBezierMoves.Count; i++)
            {
                if (!onBezierMoves[i].OnUpdate())
                {
                    onBezierMoves.RemoveAt(i);
                    if (onBezierMoves.Count == 0)
                    {
                        OnComplete();
                    }
                    i--;
                }
            }
            yield return null;
        }
    }
    public static Vector3 GetBezierPos(Vector3 start, Vector3 height, Vector3 end, float t)
    {
        return start * (1 - t) * (1 - t) + height * 2 * t * (1 - t) + end * t * t;
    }
    [System.Serializable]
    public class OnBezierMove
    {
        public Transform target;
        public (Transform, Vector3) end;
        public Vector3 height;
        float f;
        public int step;
        public float speed;
        public float pickUpDelay;
        bool canPick = false;
        float count = 0;
        Quaternion rotation;
        Vector3 position;
        BezierManager stack;
        public UnityAction onStart;
        public UnityAction onComplete;
        Transform targetParent;
        bool isLocal;
        public OnBezierMove(BezierManager stack, Transform target, (Transform, Vector3) end, Vector3 height,
                            int step, float speed, float pickUpDelay, UnityAction onStart, UnityAction onComplete, Transform parent, bool isLocal)
        {
            this.stack = stack;
            this.target = target;
            this.end = end;
            this.height = height;
            this.step = step;
            this.speed = speed;
            this.pickUpDelay = pickUpDelay;
            this.isLocal = isLocal;
            targetParent = parent;
            f = 0;
            if (pickUpDelay <= 0)
            {
                canPick = true;
            }
            else
            {
                canPick = false;
            }
            this.onStart = onStart;
            this.onComplete = onComplete;
        }
        public bool OnUpdate()
        {
            if (canPick)
            {
                f += Time.deltaTime * speed;
                if (isLocal)
                    target.localPosition = GetBezierPos(position, height, ((end.Item1 == null ? Vector3.zero : end.Item1.localPosition) + end.Item2), f);
                else
                    target.position = GetBezierPos(position, height, ((end.Item1 == null ? Vector3.zero : end.Item1.position) + end.Item2), f);
                target.rotation = Quaternion.Lerp(rotation, targetParent.rotation, f);
                if (f >= 1)
                {
                    if (isLocal)
                        target.localPosition = ((end.Item1 == null ? Vector3.zero : end.Item1.localPosition) + end.Item2);
                    else
                        target.position = ((end.Item1 == null ? Vector3.zero : end.Item1.position) + end.Item2);
                    target.rotation = targetParent.rotation;
                    onComplete?.Invoke();
                    return false;
                }
            }
            else
            {
                count += Time.deltaTime;
                if (count >= pickUpDelay)
                {
                    rotation = target.rotation;
                    position = (isLocal ? target.localPosition : target.position);
                    canPick = true;
                    onStart?.Invoke();
                }
            }
            return true;
        }
    }

}

