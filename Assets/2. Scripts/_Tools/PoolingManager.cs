using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager instance;
    public NewPooling<Character> characterPooling;
    public NewPooling<Bullet> bulletPooling;
    public NewPooling<Money> moneyPooling;

    private void Awake() {
        instance = this;
        // characterPooling = new NewPooling<Character>(transform);
        // bulletPooling = new NewPooling<Bullet>(transform);
    }
}
