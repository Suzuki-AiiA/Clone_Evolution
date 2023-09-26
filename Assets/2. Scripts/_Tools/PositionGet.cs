using UnityEngine;
[System.Serializable]
public class PositionGet
{
    public float width;
    public float lineDis;
    public int num1;
    public int num2;
    public bool isInit = false;
    public float[] x1;
    public float[] x2;
    public float y;
    public void Init()
    {
        isInit = true;
        x1 = new float[num1];
        x2 = new float[num2];
        float dis;
        if (num2 == 0)
        {
            if (num1 == 1)
            {
                x1 = new float[1];
                x1[0] = 0;
            }
            else
                for (int i = 0; i < num1; i++)
                {
                    x1[i] = width / (num1 - 1) * i - width / 2;
                }
        }
        else
        {
            if (num1 - num2 > 0)
            {
                for (int i = 0; i < num1; i++)
                {
                    x1[i] = width / (num1 - 1) * i - width / 2;
                }
                dis = x1[1] - x1[0];

                width -= dis;
                for (int i = 0; i < num2; i++)
                {
                    x2[i] = width / (num2 - 1) * i - width / 2;
                }
            }
            else
            {
                for (int i = 0; i < num2; i++)
                {
                    x2[i] = width / (num2 - 1) * i - width / 2;
                }
                dis = x2[1] - x2[0];
                width -= dis;
                for (int i = 0; i < num1; i++)
                {
                    x1[i] = width / (num1 - 1) * i - width / 2;
                }
            }
        }
    }
    public Vector3 GetPos(int num)
    {
        if (!isInit)
        {
            Init();
        }
        int set = num / (num1 + num2);
        int set2 = num % (num1 + num2);
        if (set2 == 0)
        {
            set2 = num1 + num2;
            set--;
        }
        float x, z;
        if (set2 <= num1 || num2 == 0)
        {
            x = x1[set2 - 1];
            z = set * lineDis * 2;
        }
        else
        {
            x = x2[set2 - num1 - 1];
            z = set * lineDis * 2 + lineDis;
        }

        return new Vector3(x, y, z);

    }
}