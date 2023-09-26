using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GM.PlayerDataSaver.Value;
#if UNITY_EDITOR
using UnityEditor;
#endif


/*
    types:
        PlayerData.intData["name", value]
        PlayerData.floatData["name", value]
        PlayerData.stringData["name", value]
    how to use:
        int a = PlayerData.intData["a"];
        PlayerData.intData["a"] = 1;
        PlayerData.intData["a"] ++;
    methods:
        Clear()
    editor:
        GM -> Data to view saved datas
*/

namespace GM.PlayerDataSaver
{
    public class PlayerData
    {
        public static GMIntData intData = new GMIntData();
        public static GMFloatData floatData = new GMFloatData();
        public static GMStringData stringData = new GMStringData();
        public static void Clear()
        {
            intData.Clear();
            floatData.Clear();
            stringData.Clear();
        }
    }
    namespace Value
    {
        public class DataParent<T>
        {
            public virtual void Set(string name) { }
        }
        public abstract partial class GMData<T> : DataParent<T>
        {
            public void Clear()
            {
                string a = PlayerPrefs.GetString("GMs-" + typeof(T).Name + "Names", "");
                string[] b = a.Split(',');
                foreach (var item in b)
                {
                    PlayerPrefs.DeleteKey(item);
                }
            }
        }
        public partial class GMIntData : GMData<int>
        {
            public int this[string name, int defaultValue = 0]
            {
                get => PlayerPrefs.GetInt(name, defaultValue);
                set
                {
                    PlayerPrefs.SetInt(name, value);
                    Set(name);
                }
            }
        }
        public partial class GMFloatData : GMData<float>
        {
            public float this[string name, float defaultValue = 0.0f]
            {
                get => PlayerPrefs.GetFloat(name, defaultValue);
                set
                {
                    PlayerPrefs.SetFloat(name, value);
                    Set(name);
                }
            }
        }
        public partial class GMStringData : GMData<string>
        {
            public string this[string name, string defaultValue = ""]
            {
                get => PlayerPrefs.GetString(name, defaultValue);
                set
                {
                    PlayerPrefs.SetString(name, value);
                    Set(name);
                }
            }
        }
    }
#if UNITY_EDITOR
    namespace Value
    {
        partial class GMData<T>
        {
            public List<string> names = new List<string>();
            public override void Set(string name)
            {
                string namesString = "";
                for (int i = 0; i < names.Count; i++)
                {
                    namesString += names[i];
                    if (i < names.Count - 1)
                    {
                        namesString += ",";
                    }
                }
                if (name == "")
                {

                }
                else
                {
                    if (!names.Contains(name))
                    {
                        names.Add(name);
                    }
                }
                Save();
            }
            public void Save()
            {

                string namesString = "";
                for (int i = 0; i < names.Count; i++)
                {
                    namesString += names[i];
                    if (i < names.Count - 1)
                    {
                        namesString += ",";
                    }
                }
                PlayerPrefs.SetString("GMs-" + typeof(T).Name + "Names", namesString);
            }
            T GetData(string name)
            {
                if (typeof(T) == typeof(int))
                {
                    return (T)(object)PlayerPrefs.GetInt(name, 0);
                }
                else if (typeof(T) == typeof(float))
                {
                    return (T)(object)PlayerPrefs.GetFloat(name, 0.0f);
                }
                else if (typeof(T) == typeof(string))
                {
                    return (T)(object)PlayerPrefs.GetString(name, "");
                }
                else
                {
                    return default(T);
                }
            }
            public virtual List<(string, T)> Load()
            {
                List<(string, T)> list = new List<(string, T)>();
                string namesString = PlayerPrefs.GetString("GMs-" + typeof(T).Name + "Names", "");
                string[] names = namesString.Split(',');
                for (int i = 0; i < names.Length; i++)
                {
                    if (names[i] == "")
                        continue;
                    string name = names[i];
                    T value = GetData(name);
                    list.Add((name, value));
                }
                return list;
            }
        }
    }

    // [CustomEditor(typeof(GMPlayerDataSaverEditor))]

    public class GMPlayerDataSaverEditor : EditorWindow
    {
        static List<(string, int)> intData = new List<(string, int)>();
        static bool showInt = true;
        static int intNum;
        static List<(string, float)> floatData = new List<(string, float)>();
        static bool showFloat = true;
        static int floatNum;
        static List<(string, string)> stringData = new List<(string, string)>();
        static bool showString = true;
        static int stringNum;

        static Vector2 scrollPos;
        static bool autoLoad = false;
        public static void Load()
        {
            intData = new List<(string, int)>(PlayerData.intData.Load());
            intNum = intData.Count;
            floatData = new List<(string, float)>(PlayerData.floatData.Load());
            floatNum = floatData.Count;
            stringData = new List<(string, string)>(PlayerData.stringData.Load());
            stringNum = stringData.Count;
        }
        public static void Save()
        {
            foreach (var item in intData)
            {
                if (item.Item1 == "")
                {
                    continue;
                }
                PlayerData.intData[item.Item1] = item.Item2;
            }
            foreach (var item in floatData)
            {
                if (item.Item1 == "")
                {
                    continue;
                }
                PlayerData.floatData[item.Item1] = item.Item2;
            }
            foreach (var item in stringData)
            {
                if (item.Item1 == "")
                {
                    continue;
                }
                PlayerData.stringData[item.Item1] = item.Item2;
            }

            PlayerData.intData.names = new List<string>();
            intData.ForEach((x) =>
            {
                PlayerData.intData.names.Add(x.Item1);
            });
            PlayerData.intData.Save();
            PlayerData.floatData.names = new List<string>();
            floatData.ForEach((x) =>
            {
                PlayerData.floatData.names.Add(x.Item1);
            });
            PlayerData.floatData.Save();
            PlayerData.stringData.names = new List<string>();
            stringData.ForEach((x) =>
            {
                PlayerData.stringData.names.Add(x.Item1);
            });
            PlayerData.stringData.Save();
        }

        public static void Clear()
        {
            intData.Clear();
            intNum = 0;
            floatData.Clear();

            floatNum = 0;

            stringData.Clear();

            stringNum = 0;
            PlayerData.intData.Clear();
            PlayerData.floatData.Clear();
            PlayerData.stringData.Clear();
            Save();
            Load();
        }
        [MenuItem("GM/Data")]
        public static void Open()
        {
            EditorWindow window = GetWindow<GMPlayerDataSaverEditor>();
            window.titleContent = new GUIContent("Data");
            Load();
        }
        private void OnGUI()
        {
            if (!autoLoad)
            {
                Load();
                autoLoad = true;
            }
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            EditorGUILayout.LabelField("PlayerDataSaver", EditorStyles.boldLabel);
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Int Data", GUILayout.Width(70));
            showInt = EditorGUILayout.Toggle(showInt);
            EditorGUILayout.IntField(intNum);
            if (GUILayout.Button("+"))
            {
                intData.Add(("", 0));
                intNum++;
            }
            if (GUILayout.Button("-"))
            {
                if (intData.Count > 0)
                {
                    intData.RemoveAt(intData.Count - 1);
                    intNum--;
                }
            }

            GUILayout.EndHorizontal();
            if (showInt)
            {
                for (int i = 0; i < intData.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField((i + 1) + ". ", GUILayout.Width(20));
                    EditorGUILayout.LabelField("Name ", GUILayout.Width(40));
                    string name = EditorGUILayout.TextField(intData[i].Item1);
                    GUILayout.Space(10);
                    EditorGUILayout.LabelField("Value ", GUILayout.Width(50));
                    int value = EditorGUILayout.IntField(intData[i].Item2);
                    intData[i] = (name, value);
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Float Data", GUILayout.Width(70));
            showFloat = EditorGUILayout.Toggle(showFloat);
            EditorGUILayout.IntField(floatNum);
            if (GUILayout.Button("+"))
            {
                floatData.Add(("", 0));
                floatNum++;
            }
            if (GUILayout.Button("-"))
            {
                if (floatData.Count > 0)
                {
                    floatData.RemoveAt(floatData.Count - 1);
                    floatNum--;
                }
            }

            GUILayout.EndHorizontal();
            if (showFloat)
            {
                for (int i = 0; i < floatData.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField((i + 1) + ". ", GUILayout.Width(20));
                    EditorGUILayout.LabelField("Name ", GUILayout.Width(40));
                    string name = EditorGUILayout.TextField(floatData[i].Item1);
                    GUILayout.Space(10);
                    EditorGUILayout.LabelField("Value ", GUILayout.Width(50));
                    float value = EditorGUILayout.FloatField(floatData[i].Item2);
                    floatData[i] = (name, value);
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("String Data", GUILayout.Width(70));
            showString = EditorGUILayout.Toggle(showString);
            EditorGUILayout.IntField(stringNum);
            if (GUILayout.Button("+"))
            {
                stringData.Add(("", ""));
                stringNum++;
            }
            if (GUILayout.Button("-"))
            {
                if (stringData.Count > 0)
                {
                    stringData.RemoveAt(stringData.Count - 1);
                    stringNum--;
                }
            }

            GUILayout.EndHorizontal();
            if (showString)
            {
                for (int i = 0; i < stringData.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField((i + 1) + ". ", GUILayout.Width(20));
                    EditorGUILayout.LabelField("Name ", GUILayout.Width(40));
                    string name = EditorGUILayout.TextField(stringData[i].Item1);
                    GUILayout.Space(10);
                    EditorGUILayout.LabelField("Value ", GUILayout.Width(50));
                    string value = EditorGUILayout.TextField(stringData[i].Item2);
                    stringData[i] = (name, value);
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.Space(30);

            if (GUILayout.Button("Load"))
            {
                Load();
            }
            if (GUILayout.Button("Save"))
            {
                Save();
            }
            if (GUILayout.Button("Clear"))
            {
                Clear();
            }
            EditorGUILayout.EndScrollView();
        }
    }
#endif
}
