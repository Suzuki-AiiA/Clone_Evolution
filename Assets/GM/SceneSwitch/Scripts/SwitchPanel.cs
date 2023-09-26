using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GM.SceneSwitch
{
    public abstract class SwitchPanel : MonoBehaviour
    {
        public static SwitchPanel instance;
        protected virtual void Awake()
        {
            instance = this;
        }
        public abstract void Switch(string n = "");
        public abstract void Run(UnityAction action);
    }
}