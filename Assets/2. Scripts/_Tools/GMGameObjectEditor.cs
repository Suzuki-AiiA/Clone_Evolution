using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
public class GMGameObjectEditor : EditorWindow
{
    static int startIndex = 0;
    static string beforeNum = "GameObject -";
    static string afterNum = "";
    static bool numberSpace = true;
    static int rowNum = 10;
    static float rowOffset = 1;
    static int colNum = 0;
    static float colOffset = 1;
    static int depthNum = 0;
    static float depthOffset = 1;
    static int generateNum = 1;
    static GameObject generateObj;


    [MenuItem("GM/ObjectEditor")]
    public static void Open()
    {
        EditorWindow window = GetWindow<GMGameObjectEditor>();
        window.titleContent = new GUIContent("Data");
    }
    static void GenerateForChild(GameObject[] obj)
    {
        for (int i = 0; i < obj.Length; i++)
        {
            GameObject go = obj[i];
            List<GameObject> o = new List<GameObject>();
            for (int j = 0; j < generateNum; j++)
            {
                GameObject newGo = Instantiate(generateObj, go.transform);
                o.Add(newGo);
                newGo.transform.localPosition = Vector3.zero;
            }
            ChangeName(o.ToArray());
        }
        Save();
    }
    static void ChangeName(GameObject[] obj)
    {
        GameObject[] gos = obj;
        System.Array.Sort(gos, (go1, go2) =>
        {
            int depth1 = go1.transform.GetSiblingIndex();
            int depth2 = go2.transform.GetSiblingIndex();
            return depth1 - depth2;
        });
        for (int i = 0; i < gos.Length; i++)
        {
            GameObject go = gos[i];
            go.name = beforeNum + (numberSpace ? " " : "") + (startIndex + i) + (numberSpace ? " " : "") + afterNum;
        }
        Save();
    }
    static void Save()
    {
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
    }

    static void SetPosition(GameObject[] obj)
    {
        GameObject[] gos = obj;
        System.Array.Sort(gos, (go1, go2) =>
        {
            int depth1 = go1.transform.GetSiblingIndex();
            int depth2 = go2.transform.GetSiblingIndex();
            return depth1 - depth2;
        });
        int row = 0;
        int col = 0;
        int depth = 0;
        Vector3 initPos = gos[0].transform.position;
        for (int i = 0; i < gos.Length; i++)
        {
            gos[i].transform.position = initPos + new Vector3(row * rowOffset, depthOffset * depth, col * colOffset);
            row++;
            if (row >= rowNum && rowNum > 0)
            {
                row = 0;
                col++;
                if (col >= colNum && colNum > 0)
                {
                    col = 0;
                    depth++;
                    if (depth >= depthNum && depthNum > 0)
                    {
                        depth = 0;
                    }
                }
            }
        }

        Save();
    }
    private void OnGUI()
    {

        EditorGUILayout.LabelField("Change Name");
        EditorGUILayout.BeginHorizontal();

        GUILayout.Space(10);

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Before", GUILayout.MinWidth(20));
        beforeNum = EditorGUILayout.TextField(beforeNum, GUILayout.MinWidth(20));
        EditorGUILayout.EndVertical();

        GUILayout.Space(20);

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Start Index", GUILayout.MinWidth(20));
        startIndex = EditorGUILayout.IntField(startIndex, GUILayout.MinWidth(20));
        EditorGUILayout.EndVertical();

        GUILayout.Space(20);

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("After", GUILayout.MinWidth(20));
        afterNum = EditorGUILayout.TextField(afterNum, GUILayout.MinWidth(20));
        EditorGUILayout.EndVertical();
        GUILayout.Space(10);

        EditorGUILayout.EndHorizontal();

        numberSpace = EditorGUILayout.Toggle("Number Space", numberSpace);
        if (GUILayout.Button("Change Name"))
        {
            ChangeName(Selection.gameObjects);
        }
        if (GUILayout.Button("Change Child Name"))
        {
            foreach (GameObject go in Selection.gameObjects)
            {

                List<GameObject> o = new List<GameObject>();
                for (int i = 0; i < go.transform.childCount; i++)
                {
                    GameObject child = go.transform.GetChild(i).gameObject;
                    o.Add(child);
                }
                ChangeName(o.ToArray());
            }
        }

        GUILayout.Space(20);
        EditorGUILayout.LabelField("Set Grid Position");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Row Num", GUILayout.MinWidth(20));
        rowNum = EditorGUILayout.IntField(rowNum, GUILayout.MinWidth(20));
        EditorGUILayout.LabelField("Row Offset", GUILayout.MinWidth(20));
        rowOffset = EditorGUILayout.FloatField(rowOffset, GUILayout.MinWidth(20));
        EditorGUILayout.EndVertical();
        //space
        GUILayout.Space(20);
        // column
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Column Num", GUILayout.MinWidth(20));
        colNum = EditorGUILayout.IntField(colNum, GUILayout.MinWidth(20));
        EditorGUILayout.LabelField("Column Offset", GUILayout.MinWidth(20));
        colOffset = EditorGUILayout.FloatField(colOffset, GUILayout.MinWidth(20));
        EditorGUILayout.EndVertical();
        //space
        GUILayout.Space(20);
        // depth
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Depth Num", GUILayout.MinWidth(20));
        depthNum = EditorGUILayout.IntField(depthNum, GUILayout.MinWidth(20));
        EditorGUILayout.LabelField("Depth Offset", GUILayout.MinWidth(20));
        depthOffset = EditorGUILayout.FloatField(depthOffset, GUILayout.MinWidth(20));
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Set Position"))
        {
            SetPosition(Selection.gameObjects);
        }
        if (GUILayout.Button("Set Child Position"))
        {
            foreach (GameObject go in Selection.gameObjects)
            {

                List<GameObject> o = new List<GameObject>();
                for (int i = 0; i < go.transform.childCount; i++)
                {
                    GameObject child = go.transform.GetChild(i).gameObject;
                    o.Add(child);
                }
                SetPosition(o.ToArray());
            }
        }

        GUILayout.Space(20);
        EditorGUILayout.LabelField("Generate");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Generate Num", GUILayout.MinWidth(20));
        generateNum = EditorGUILayout.IntField(generateNum, GUILayout.MinWidth(20));
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Generate Obj", GUILayout.MinWidth(20));
        generateObj = EditorGUILayout.ObjectField(generateObj, typeof(GameObject), true, GUILayout.MinWidth(20)) as GameObject;
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Generate"))
        {
            GenerateForChild(Selection.gameObjects);
        }


    }
}
#endif
