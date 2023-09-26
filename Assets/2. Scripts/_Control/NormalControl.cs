using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class NormalControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Player move;
    public bool useStartPos;
    public bool resetWhenUp;
    Vector2 startPos;
    public float minValue;
    public float mult;
    public UnityEvent onPointerDown;
    public UnityEvent onPointerUp;
    private void Start() {
        mult = GManager.Data.sensutive;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (useStartPos)
        {
            if ((eventData.position - startPos).magnitude < minValue)
            {
                return;
            }
            move.Move((eventData.position - startPos) * mult);
        }
        else
        {
            if (eventData.delta.magnitude < minValue)
            {
                return;
            }
            move.Move(eventData.delta * mult);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startPos = eventData.position;
        onPointerDown.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (resetWhenUp)
        {
            move.Move(Vector2.zero);
        }
        onPointerUp.Invoke();
    }
}
