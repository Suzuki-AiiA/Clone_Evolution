using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class DataChanger : MonoBehaviour
{
    Datas data;
    public GameObject set;
    public List<DataChangerSet> sets;
    public delegate string ReturnFloat();
    public Transform parent;
    public float distance;
    public float height;
    public bool isSet = false;
    public void Set()
    {
        if (isSet)
            return;
        isSet = true;
        data = GManager.Data;
        sets = new List<DataChangerSet>();
        sets.Add(new DataChangerSet("sensutive", (string value) => { data.sensutive= float.Parse(value); }, () => { return data.sensutive.ToString(); }));
        sets.Add(new DataChangerSet("moveSpeed", (string value) => { data.playerSpeed = float.Parse(value); }, () => { return data.playerSpeed.ToString(); }));
        sets.Add(new DataChangerSet("age 1", (string value) => { data.playerAges[0] = int.Parse(value); }, () => { return data.playerAges[0].ToString(); }));
        sets.Add(new DataChangerSet("age 2", (string value) => { data.playerAges[1] = int.Parse(value); }, () => { return data.playerAges[1].ToString(); }));
        sets.Add(new DataChangerSet("age 3", (string value) => { data.playerAges[2] = int.Parse(value); }, () => { return data.playerAges[2].ToString(); }));
        sets.Add(new DataChangerSet("age 4", (string value) => { data.playerAges[3] = int.Parse(value); }, () => { return data.playerAges[3].ToString(); }));
        sets.Add(new DataChangerSet("age 5", (string value) => { data.playerAges[4] = int.Parse(value); }, () => { return data.playerAges[4].ToString(); }));
        sets.Add(new DataChangerSet("init rate", (string value) => { data.playerShootRate = float.Parse(value); }, () => { return data.playerShootRate.ToString(); }));
        sets.Add(new DataChangerSet("bullet range", (string value) => { data.bulletLifeTime = float.Parse(value); }, () => { return data.bulletLifeTime.ToString(); }));
        sets.Add(new DataChangerSet("shoot angle", (string value) => { data.shootAngle = float.Parse(value); }, () => { return data.shootAngle.ToString(); }));
        sets.Add(new DataChangerSet("hideUI", (string value) => { data.hideUI = value == "1"; }, () => { return data.hideUI ? "1" : "0"; }));
        sets.Add(new DataChangerSet("hideUIDis", (string value) => { data.hideUIDis = float.Parse(value); }, () => { return data.hideUIDis.ToString(); }));
        sets.Add(new DataChangerSet("sum hp", (string value) =>
        {
            string[] nums = value.Split(",");
            data.sumCard.hps = new int[nums.Length];
            for (int i = 0; i < nums.Length; i++)
            {
                data.sumCard.hps[i] = int.Parse(nums[i]);
            }
        }, () =>
        {
            string str = data.sumCard.hps[0].ToString();
            for (int i = 1; i < data.sumCard.hps.Length; i++)
            {
                str += "," + data.sumCard.hps[i].ToString();
            }
            return str;
        }));
        sets.Add(new DataChangerSet("sum value", (string value) =>
        {
            string[] nums = value.Split(",");
            data.sumCard.value = new int[nums.Length];
            for (int i = 0; i < nums.Length; i++)
            {
                data.sumCard.value[i] = int.Parse(nums[i]);
            }
        }, () =>
        {
            string str = data.sumCard.value[0].ToString();
            for (int i = 1; i < data.sumCard.value.Length; i++)
            {
                str += "," + data.sumCard.value[i].ToString();
            }
            return str;
        }));
        sets.Add(new DataChangerSet("mult hp", (string value) =>
        {
            string[] nums = value.Split(",");
            data.multCard.hps = new int[nums.Length];
            for (int i = 0; i < nums.Length; i++)
            {
                data.multCard.hps[i] = int.Parse(nums[i]);
            }
        }, () =>
        {
            string str = data.multCard.hps[0].ToString();
            for (int i = 1; i < data.multCard.hps.Length; i++)
            {
                str += "," + data.multCard.hps[i].ToString();
            }
            return str;
        }));
        sets.Add(new DataChangerSet("mult value", (string value) =>
        {
            string[] nums = value.Split(",");
            data.multCard.value = new int[nums.Length];
            for (int i = 0; i < nums.Length; i++)
            {
                data.multCard.value[i] = int.Parse(nums[i]);
            }
        }, () =>
        {
            string str = data.multCard.value[0].ToString();
            for (int i = 1; i < data.multCard.value.Length; i++)
            {
                str += "," + data.multCard.value[i].ToString();
            }
            return str;
        }));
        sets.Add(new DataChangerSet("year hp", (string value) =>
        {
            string[] nums = value.Split(",");
            data.yearCard.hps = new int[nums.Length];
            for (int i = 0; i < nums.Length; i++)
            {
                data.yearCard.hps[i] = int.Parse(nums[i]);
            }
        }, () =>
        {
            string str = data.yearCard.hps[0].ToString();
            for (int i = 1; i < data.yearCard.hps.Length; i++)
            {
                str += "," + data.yearCard.hps[i].ToString();
            }
            return str;
        }));
        sets.Add(new DataChangerSet("year value", (string value) =>
        {
            string[] nums = value.Split(",");
            data.yearCard.value = new int[nums.Length];
            for (int i = 0; i < nums.Length; i++)
            {
                data.yearCard.value[i] = int.Parse(nums[i]);
            }
        }, () =>
        {
            string str = data.yearCard.value[0].ToString();
            for (int i = 1; i < data.yearCard.value.Length; i++)
            {
                str += "," + data.yearCard.value[i].ToString();
            }
            return str;
        }));
        sets.Add(new DataChangerSet("shoot hp", (string value) =>
        {
            string[] nums = value.Split(",");
            data.shotCard.hps = new int[nums.Length];
            for (int i = 0; i < nums.Length; i++)
            {
                data.shotCard.hps[i] = int.Parse(nums[i]);
            }
        }, () =>
        {
            string str = data.shotCard.hps[0].ToString();
            for (int i = 1; i < data.shotCard.hps.Length; i++)
            {
                str += "," + data.shotCard.hps[i].ToString();
            }
            return str;
        }));
        sets.Add(new DataChangerSet("shoot value", (string value) =>
        {
            string[] nums = value.Split(",");
            data.shotCard.value = new int[nums.Length];
            for (int i = 0; i < nums.Length; i++)
            {
                data.shotCard.value[i] = int.Parse(nums[i]);
            }
        }, () =>
        {
            string str = data.shotCard.value[0].ToString();
            for (int i = 1; i < data.shotCard.value.Length; i++)
            {
                str += "," + data.shotCard.value[i].ToString();
            }
            return str;
        }));
        sets.Add(new DataChangerSet("damage Gate Start", (string value) => { data.damageGate.startValue = int.Parse(value); }, () => { return data.damageGate.startValue.ToString(); }));
        sets.Add(new DataChangerSet("damage Gate Step", (string value) => { data.damageGate.step = int.Parse(value); }, () => { return data.damageGate.step.ToString(); }));
        sets.Add(new DataChangerSet("damage Gate Mult", (string value) => { data.damageGate.multiplier = float.Parse(value); }, () => { return data.damageGate.multiplier.ToString(); }));
        sets.Add(new DataChangerSet("damage Gate Max", (string value) => { data.damageGate.max = int.Parse(value); }, () => { return data.damageGate.max.ToString(); }));
        sets.Add(new DataChangerSet("speed Gate Start", (string value) => { data.speedGate.startValue = int.Parse(value); }, () => { return data.speedGate.startValue.ToString(); }));
        sets.Add(new DataChangerSet("speed Gate Step", (string value) => { data.speedGate.step = int.Parse(value); }, () => { return data.speedGate.step.ToString(); }));
        sets.Add(new DataChangerSet("speed Gate Mult", (string value) => { data.speedGate.multiplier = float.Parse(value); }, () => { return data.speedGate.multiplier.ToString(); }));
        sets.Add(new DataChangerSet("speed Gate Max", (string value) => { data.speedGate.max = int.Parse(value); }, () => { return data.speedGate.max.ToString(); }));
        sets.Add(new DataChangerSet("range Gate Start", (string value) => { data.rangeGate.startValue = int.Parse(value); }, () => { return data.rangeGate.startValue.ToString(); }));
        sets.Add(new DataChangerSet("range Gate Step", (string value) => { data.rangeGate.step = int.Parse(value); }, () => { return data.rangeGate.step.ToString(); }));
        sets.Add(new DataChangerSet("range Gate Mult", (string value) => { data.rangeGate.multiplier = float.Parse(value); }, () => { return data.rangeGate.multiplier.ToString(); }));
        sets.Add(new DataChangerSet("range Gate Max", (string value) => { data.rangeGate.max = int.Parse(value); }, () => { return data.rangeGate.max.ToString(); }));
        sets.Add(new DataChangerSet("year Gate Start", (string value) => { data.yearGate.startValue = int.Parse(value); }, () => { return data.yearGate.startValue.ToString(); }));
        sets.Add(new DataChangerSet("year Gate Step", (string value) => { data.yearGate.step = int.Parse(value); }, () => { return data.yearGate.step.ToString(); }));
        sets.Add(new DataChangerSet("year Gate Mult", (string value) => { data.yearGate.multiplier = float.Parse(value); }, () => { return data.yearGate.multiplier.ToString(); }));
        sets.Add(new DataChangerSet("year Gate Max", (string value) => { data.yearGate.max = int.Parse(value); }, () => { return data.yearGate.max.ToString(); }));
        sets.Add(new DataChangerSet("gate 1", (string value) => { data.gates[0] = int.Parse(value); }, () => { return data.gates[0].ToString(); }));
        sets.Add(new DataChangerSet("gate 2", (string value) => { data.gates[1] = int.Parse(value); }, () => { return data.gates[1].ToString(); }));
        sets.Add(new DataChangerSet("gate 3", (string value) => { data.gates[2] = int.Parse(value); }, () => { return data.gates[2].ToString(); }));
        sets.Add(new DataChangerSet("gate 4", (string value) => { data.gates[3] = int.Parse(value); }, () => { return data.gates[3].ToString(); }));
        sets.Add(new DataChangerSet("gate 5", (string value) => { data.gates[4] = int.Parse(value); }, () => { return data.gates[4].ToString(); }));
        sets.Add(new DataChangerSet("gate 6", (string value) => { data.gates[5] = int.Parse(value); }, () => { return data.gates[5].ToString(); }));
        for (int i = 0; i < sets.Count; i++)
        {
            GameObject obj = Instantiate(set, parent);
            obj.transform.localPosition = new Vector3(0, -(i * (height + distance) + distance), 0);
            obj.GetChild(0).GetComponent<TextMeshProUGUI>().text = sets[i].name;
            obj.GetChild(1).GetComponent<TMP_InputField>().text = sets[i].returnFloat();
            obj.GetChild(1).GetComponent<TMP_InputField>().onValueChanged.AddListener(sets[i].onChange);
        }
        parent.GetComponent<RectTransform>().sizeDelta = new Vector2(parent.GetComponent<RectTransform>().sizeDelta.x, sets.Count * (height + distance) + distance);

    }
    public class DataChangerSet
    {
        public string name;
        public UnityAction<string> onChange;
        public ReturnFloat returnFloat;
        public DataChangerSet(string name, UnityAction<string> onChange, ReturnFloat returnFloat)
        {
            this.name = name;
            this.onChange = onChange;
            this.returnFloat = returnFloat;
        }
    }
}
