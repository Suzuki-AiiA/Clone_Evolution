using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardDragManager : MonoBehaviour
{
    public static CardDragManager instance;
    public float maxDis;
    public LayerMask layerSelect;
    public LayerMask layerGround;
    RaycastHit hit;
    Card selected = null;
    Camera c;
    Vector3 pos;
    public Acurate accurate;
    public float accurateDis;
    public List<Character> selectedChara = new List<Character>();
    public bool drag = false;
    private void Awake() {
        instance = this;
    }
    private void Start()
    {
        c = Camera.main;
        accurate.cardDragManager = this;
        accurate.gameObject.SetActive(false);
    }
    void Update()
    {
        if (!drag) return;
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(c.ScreenPointToRay(Input.mousePosition), out hit, maxDis, layerSelect))
            {
                if (hit.collider.CompareTag("card") && selected == null)
                {
                    selected = hit.collider.GetComponent<Card>();
                    pos = selected.transform.position;
                    accurate.gameObject.SetActive(true);
                }
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(c.ScreenPointToRay(Input.mousePosition), out hit, maxDis, layerGround))
            {
                if (selected != null)
                {
                    accurate.transform.position = hit.point + Vector3.forward * accurateDis;
                    selected.transform.position = hit.point;
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (selected != null)
            {
                if (selectedChara.Count == 0)
                {
                    selected.transform.position = pos;
                }
                else
                {
                    for (int i = 0; i < selectedChara.Count; i++)
                    {
                        if (selected.type == CardType.Add)
                        {
                            if (i == 0)
                            {
                                selectedChara[i].Upgrade(selected);
                            }
                        }
                        else
                        {
                            selectedChara[i].Upgrade(selected);

                        }
                        selectedChara[i].OnEventExit();

                    }
                    this.Wait(0, () =>
                    {
                        Player.instance.RePositionCharas();
                    });
                    selected.transform.DOMove(selectedChara[0].transform.position, 0.3f);
                    selectedChara.Clear();
                    selected.transform.DOScale(0, 0.5f);
                    TransLine.instance.Remove(selected);
                }
            }
            accurate.gameObject.SetActive(false);
            selected = null;
        }
    }
}
