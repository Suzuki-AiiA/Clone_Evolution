using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
// using Core.Code.Scripts.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace Core.Manager
{ }
public static class Extension
{
    #region Bazier

    [Serializable]
    public class BezierData
    {
        public Vector3 origin, destination, tangent1, tangent2;
    }

    [Serializable]
    public class BezierModifier
    {
        [Range(-2f, 2f)] public float x, y, z;
        [Range(0, 10)] public float divider;

        public BezierModifier(float xV = 0, float yV = 1.5f, float zV = 1f, float div = 5f)
        {
            x = xV;
            y = yV;
            z = zV;
            divider = div;
        }
    }

    public static BezierData CalculateBezier(this Transform origin, Transform destination, float lean = 0,
        BezierModifier modifier1 = null, BezierModifier modifier2 = null)
    {
        var data = new BezierData();
        var position1 = origin.position;
        var position2 = destination.position;
        var dist = Vector3.Distance(position1, position2);
        var orMod1 = (position2.ToXZ() - position1.ToXZ()).normalized;
        var orMod2 = (position1.ToXZ() - position2.ToXZ()).normalized;
        var dist2d = Vector2.Distance(position1.ToXZ(), position2.ToXZ());
        var upMod = dist2d.Remap(0.5f, 1, 0, 2);
        upMod = Mathf.Clamp(upMod, 0, 2);
        var mod1 = modifier1 ?? new BezierModifier(orMod1.x, upMod, orMod1.y);
        var mod2 = modifier2 ?? new BezierModifier(orMod2.x, upMod + 1, -orMod2.y);
        var dir1 = new Vector3(lean + mod1.x, mod1.y, mod1.z) * (dist / mod1.divider);
        var dir2 = new Vector3(lean + mod2.x, mod2.y, -mod2.z) * (dist / mod2.divider);
        data.origin = position1;
        data.destination = position2;
        data.tangent1 = position1 + origin.eulerAngles.BasicRotationMatrix(dir1);
        data.tangent2 = position2 + destination.eulerAngles.BasicRotationMatrix(dir2);
        return data;
    }
    public static float BezierLenght(this BezierData data)
    {
        var chordLength = Vector3.Distance(data.origin, data.destination);
        var controlNetLength = Vector3.Distance(data.origin, data.tangent1) +
                               Vector3.Distance(data.tangent1, data.tangent2) +
                               Vector3.Distance(data.tangent2, data.destination);
        return (controlNetLength + chordLength) / 2;
    }
    public static void BezierDrawer(this BezierData data, float width = 5)
    {
#if UNITY_EDITOR

        Handles.DrawBezier(data.origin, data.destination,
            data.tangent1, data.tangent2, Color.blue, null, width);
#endif

    }

    // public static Quaternion BezierRotate(this BezierData data, float value, float d = 0.01f)
    // {
    //     var pos1 = data.BezierFollower(value);
    //     var pos2 = data.BezierFollower(value + d);

    //     var dir = d >= 0 ? pos1.Direction(pos2) : pos2.Direction(pos1);
    //     return dir.sqrMagnitude > 0 ? Quaternion.LookRotation(dir): Quaternion.identity;
    // }
    public static Vector3 BezierFollower(this BezierData data, float value)
    {
        var point1 = Vector3.Lerp(data.origin, data.tangent1, value);
        var point2 = Vector3.Lerp(data.tangent1, data.tangent2, value);
        var point3 = Vector3.Lerp(data.tangent2, data.destination, value);

        var point4 = Vector3.Lerp(point1, point2, value);
        var point5 = Vector3.Lerp(point2, point3, value);

        var point6 = Vector3.Lerp(point4, point5, value);
        return point6;
    }


    #endregion

    #region ChechInsideUIRect

    // public static Vector2 WorldToUISpace(this Vector3 worldPosition, RectTransform canvas)
    // {
    //     var cam = CameraManager.MainCamera;
    //     var screenPos = RectTransformUtility.WorldToScreenPoint(cam, worldPosition);
    //     RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, screenPos, cam, out var movePos);
    //     return movePos;
    // }

    // public static Vector2 WorldToScreenPosition(this Vector3 worldPosition)
    // {
    //     return RectTransformUtility.WorldToScreenPoint(CameraManager.MainCamera, worldPosition);
    // }

    public static bool CheckInUI(this RectTransform rect, Vector2 localPoint)
    {
        return localPoint.x < rect.anchoredPosition.x + rect.sizeDelta.x / 2 &&
               localPoint.x > rect.anchoredPosition.x - rect.sizeDelta.x / 2 &&
               localPoint.y < rect.anchoredPosition.y + rect.sizeDelta.y / 2 &&
               localPoint.y > rect.anchoredPosition.y - rect.sizeDelta.y / 2;
    }

    // public static bool CheckInUI(this Vector2 screenPosition, RectTransform rect)
    // {
    //     return RectTransformUtility.RectangleContainsScreenPoint
    //         (rect, screenPosition, CameraManager.MainCamera);
    // }

    // public static bool CheckInUI(this Vector3 worldPosition, RectTransform rect)
    // {
    //     var cam = CameraManager.MainCamera;
    //     var screenPos = RectTransformUtility.WorldToScreenPoint(cam, worldPosition);
    //     return RectTransformUtility.RectangleContainsScreenPoint(rect, screenPos, cam);
    // }

    // public static void WorldToCanvasPoint(this RectTransform tr, Vector3 worldPoint, RectTransform canvas,
    //     Vector2 offset)
    // {
    //     var canvasSize = canvas.sizeDelta;
    //     var screenPoint = CameraManager.MainCamera.WorldToViewportPoint(worldPoint);
    //     var screenPoint2D = new Vector2(screenPoint.x, screenPoint.y);

    //     var canvasPointX = screenPoint2D.x * canvasSize.x - 0.5f * canvasSize.x;
    //     var canvasPointY = screenPoint2D.y * canvasSize.y - 0.5f * canvasSize.y;

    //     tr.anchoredPosition = Vector2.MoveTowards(tr.anchoredPosition,
    //         new Vector2(canvasPointX + offset.x, canvasPointY + offset.y), 500 * Time.fixedDeltaTime);
    // }

    /*public static Vector3 ScreenToWorld(Vector3 pos) {
    Plane plane = new Plane(Vector3.back, GameController.Instance.player.transform.position.z + 10);
    Ray ray = Camera.main.ScreenPointToRay(new Vector3(pos.x, pos.y, 0));
    float distance;
    if (plane.Raycast(ray, out distance))
        return ray.GetPoint(distance);
    else {
        return Vector3.zero;
    }
}*/

    #endregion

    #region Component & Monobehaviour

    public static GameObject CreateInstance(GameObject original, GameObject parent, bool isActive)
    {
        GameObject instance = Object.Instantiate(original, parent.transform, false);
        instance.SetActive(isActive);
        return instance;
    }

    public static T ComponentCompile<T>(this GameObject component) where T : Component
    {
        if (component.TryGetComponent(out T type))
        {
            return type;
        }
        else
        {
            return component.gameObject.AddComponent<T>();
        }
    }

    public static void Wait(this MonoBehaviour mono, float delay, UnityEvent action)
    {
        mono.StartCoroutine(ExecuteAction(delay, action, mono));
    }

    private static IEnumerator ExecuteAction(float delay, UnityEvent action, Object mono)
    {
        float time = 0;
        while (time < delay)
        {
            if (mono.IsNull())
                yield break;
            time += Time.deltaTime;
            yield return null;
        }

        action?.Invoke();
    }

    public static Coroutine Wait(this MonoBehaviour mono, float delay, UnityAction action)
    {
        return mono.StartCoroutine(ExecuteAction(delay, action, mono));
    }

    private static IEnumerator ExecuteAction(float delay, UnityAction action, Object mono)
    {
        float time = 0;
        while (time < delay)
        {
            if (mono.IsNull())
                yield break;
            time += Time.deltaTime;
            yield return null;
        }

        action?.Invoke();
    }

    private static List<Transform> GetParents(Transform t)
    {
        var parents = new List<Transform> { t };
        while (t.parent != null)
        {
            var parent = t.parent;
            parents.Add(parent);
            t = parent;
        }

        return parents;
    }

    #endregion

    #region Direction & Angle Conversion

    public static Vector3 BasicRotationMatrix(this Vector3 rotation, Vector3 dir)
    {
        var eulerAngles = rotation;
        return Matrix4x4.Rotate(Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z)).MultiplyVector(dir);
    }

    public static Vector3 GetSpaceDirection(this Vector3 rotation, float azimuth, float altitude,
        bool isGlobal = true)
    {
        azimuth += 90;
        altitude -= 90;

        azimuth *= Mathf.Deg2Rad;
        altitude *= Mathf.Deg2Rad;
        var c = Mathf.Cos(altitude);
        var value = new Vector3(Mathf.Sin(azimuth) * c, Mathf.Sin(altitude), Mathf.Cos(azimuth) * c);
        if (!isGlobal)
            value = rotation.BasicRotationMatrix(value);
        return value;
    }

    public static Vector3 AngleToDirY(this float angle)
    {
        return new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle));
    }

    public static float DirToAngleXZ(this Vector3 direction)
    {
        return Mathf.Atan2(direction.normalized.x, direction.normalized.z) * Mathf.Rad2Deg;
    }

    public static float DirToAngleXY(this Vector3 direction)
    {
        return Mathf.Atan2(direction.normalized.x, -direction.normalized.y) * Mathf.Rad2Deg;
    }

    public static float WrapAngle(this float angle)
    {
        angle %= 360;
        if (angle > 180)
            return angle - 360;
        return angle;
    }

    public static float UnWrapAngle(this float angle)
    {
        if (angle >= 0)
            return angle;
        angle = -angle % 360;
        return 360 - angle;
    }

    #endregion

    #region VectorShortcut

    public static Vector3 String2Vector3(string strVec)
    {
        if (strVec.StartsWith("(") && strVec.EndsWith(")"))
        {
            strVec = strVec.Substring(1, strVec.Length - 2);
        }

        string[] sArray = strVec.Split(',');
        return new Vector3(float.Parse(sArray[0]), float.Parse(sArray[1]), float.Parse(sArray[2]));
    }

    public static Vector3 ToVector3(this Vector2 v2)
    {
        return new Vector3(v2.x, v2.y, 0);
    }

    public static Vector2 ToVector2(this Vector3 v3)
    {
        return new Vector2(v3.x, v3.y);
    }

    public static Vector2 ToXZ(this Vector3 v3)
    {
        return new Vector2(v3.x, v3.z);
    }

    public static Vector3 Vec23XZ(this Vector2 value)
    {
        return new Vector3(value.x, 0, value.y);
    }

    public static Vector2 Vec32XZ(this Vector3 value)
    {
        return new Vector3(value.x, value.z);
    }

    public static Vector3 Vec3MinusY(this Vector3 value)
    {
        return new Vector3(value.x, 0, value.z);
    }

    #endregion

    #region Sorting

    public static void SortBySiblingIndex<T>(this List<T> list) where T : MonoBehaviour
    {
        list.Sort((a, b) =>
        {
            if (a.transform == b.transform)
                return 0;
            if (b.transform.IsChildOf(a.transform))
            {
                return -1;
            }

            if (a.transform.IsChildOf(b.transform))
            {
                return 1;
            }

            var aParentList = GetParents(a.transform);
            var bParentList = GetParents(b.transform);

            for (var aInd = 0; aInd < aParentList.Count; aInd++)
            {
                if (!b.transform.IsChildOf(aParentList[aInd])) continue;
                var bInd = bParentList.IndexOf(aParentList[aInd]) - 1;
                aInd -= 1;
                return aParentList[aInd].GetSiblingIndex() - bParentList[bInd].GetSiblingIndex();
            }

            return aParentList[^1].GetSiblingIndex() -
                   bParentList[^1].GetSiblingIndex();
        });
    }

    public static void SortByDistance(this List<Transform> list, Vector3 fromPosition)
    {
        list.Sort((a, b) =>
        {
            var distA = Vector3.Distance(fromPosition, a.position);
            var distB = Vector3.Distance(fromPosition, b.position);
            return distA.CompareTo(distB);
        });
    }

    public static List<Transform> SortByDistance(this Transform[] array, Vector3 fromPosition)
    {
        var list = array.ToList();
        list.Sort((a, b) =>
        {
            var distA = Vector3.Distance(fromPosition, a.position);
            var distB = Vector3.Distance(fromPosition, b.position);
            return distA.CompareTo(distB);
        });
        return list;
    }

    public static List<Transform> SortByDistance(this Transform parent, Vector3 fromPosition)
    {
        var list = parent.Cast<Transform>().ToList();

        list.Sort((a, b) =>
        {
            var distA = Vector3.Distance(fromPosition, a.position);
            var distB = Vector3.Distance(fromPosition, b.position);
            return distA.CompareTo(distB);
        });
        return list;
    }

    #endregion

    #region Remap

    public static Vector3 Remap(this int value, int from1, int to1, Vector3 from2, Vector3 to2)
    {
        return (float)(value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static Vector3 Remap(this float value, float from1, float to1, Vector3 from2, Vector3 to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static float Remap(this int value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    #endregion

    #region Check

    public static bool IsNull(this Object obj)
    {
        return ReferenceEquals(obj, null);
    }

    public static bool IsntNull(this Object obj)
    {
        return !ReferenceEquals(obj, null);
    }

    public static bool IsSame(this Object obj, Object compare)
    {
        return ReferenceEquals(obj, compare);
    }

    public static bool IsntSame(this Object obj, Object compare)
    {
        return !ReferenceEquals(obj, compare);
    }

    #endregion

    #region Convert

    public static string GetStringAfterChar(this string value, char substring)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            var index = value.LastIndexOf(substring);
            return index > 0 ? value.Substring(index + 1) : value;
        }

        return string.Empty;
    }

    public static string GetStringAfterChar(this string value, int indexAfter)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            return indexAfter > 0 ? value.Substring(indexAfter + 1) : value;
        }

        return string.Empty;
    }

    public static List<T> ToList<T>(this T[] array)
    {
        return Enumerable.ToList(array);
    }

    /// <summary>
    /// Get first positive, negative decimal number from string and this could be "Integer"
    /// </summary>
    /// <param name="text"> string data you referenced </param>
    /// <param name="asInt"> Decision of should you return integer or decimal</param>
    /// <returns>only return first numbers even string has many split-ed numbers</returns>
    public static float Parse(this string text, bool asInt = false)
    {
        var isThousand = false;
        var isNegative = false;
        var isDecimal = asInt;
        var lastNumberID = -1;
        var number = "";
        for (var i = 0; i < text.Length; i++)
        {
            if (lastNumberID == -1)
            {
                if (text[i] >= 48 && text[i] <= 57)
                {
                    number += text[i];
                    lastNumberID = i;
                }
                else if (text[i] == 45)
                {
                    isNegative = true;
                    lastNumberID = i;
                }
                else if (text[i] == 46)
                {
                    number += text[i];
                    lastNumberID = i;
                    isDecimal = true;
                }
            }
            else
            {
                if (Mathf.Abs(lastNumberID - i) == 1)
                {
                    if (text[i] >= 48 && text[i] <= 57)
                    {
                        number += text[i];
                        lastNumberID = i;
                    }
                    else if (text[i] == 46 && !isDecimal)
                    {
                        number += text[i];
                        lastNumberID = i;
                        isDecimal = true;
                    }

                    if (text[i] == 75 || text[i] == 107)
                    {
                        if (number[^1] != 46)
                        {
                            isThousand = true;
                        }
                    }
                }
            }
        }

        if (number.Length == 0)
        {
            number = "0";
        }

        return float.Parse(number) * (isNegative ? -1 : 1) * (isThousand ? 1000 : 1);
    }

    public static string FormatNumber(this int num)
    {
        return num switch
        {
            >= 1000000 => (num / 1000000D).ToString("0.#") + "M",
            >= 1000 => (num / 1000D).ToString("0.#") + "K",
            _ => num.ToString("#,0")
        };
    }

    //usage LayerMask.NameToLayer("layer name when raycast").ToMaskBit())
    public static int ToMaskBit(this int value)
    {
        int mask = 0;
        if (value != -1)
        {
            mask |= 1 << value;
        }

        return mask;
    }

    #endregion

    #region Additional

    //         public static void Screenshot(string path, string name)
    //         {
    //             IO.CheckCrtDir(path);
    //             string fileName = path + "/" + name + ".png";
    // #if UNITY_5
    //         Application.CaptureScreenshot (fileName, 1);
    // #else
    //             ScreenCapture.CaptureScreenshot(fileName, 1);
    // #endif
    //         }

    ///<summary> loop idx</summary>
    public static int RepIdx(int idx, int n)
    {
        return (int)Rep(idx, n);
    }

    private static float Rep(float f, float n)
    {
        return n >= 0 ? Mathf.Repeat(f, n) : -Mathf.Repeat(-f, -n);
    }

    public static float Damp(float smoothing, float dt)
    {
        return 1 - Mathf.Pow(smoothing, dt);
    }





    #endregion

    public static GameObject GetChild(this GameObject obj, int index)
    {
        return obj.transform.GetChild(index).gameObject;
    }
    public static T Last<T>(this List<T> list)
    {
        return list[list.Count - 1];
    }
}

