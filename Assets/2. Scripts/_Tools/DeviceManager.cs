using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceManager : MonoBehaviour
{


    public static DeviceType GetDeviceType()
    {
        int processorFrequency = SystemInfo.processorFrequency;
        int graphicsMemorySize = SystemInfo.graphicsMemorySize;
        int systemMemorySize = SystemInfo.systemMemorySize;
        if (processorFrequency >= 2500 && graphicsMemorySize >= 2048 && systemMemorySize >= 7000)
        {
            return DeviceType.HighEnd;
        }
        else if (processorFrequency >= 2000 && graphicsMemorySize >= 1024 && systemMemorySize >= 4000)
        {
            return DeviceType.MiddleEnd;
        }
        else
        {
            return DeviceType.LowEnd;
        }
    }

    public enum DeviceType
    {
        LowEnd,
        MiddleEnd,
        HighEnd
    }
}
