using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Character character;
    public void Shoot()
    {
        // switch (character.level)
        // {
        //     case 0:
                Bullet g = PoolingManager.instance.bulletPooling.GetPoolingObject();
                g.transform.position = transform.position;
                g.transform.localScale = character.size.status * Vector3.one;
                g.SetWeapon(this);
                g.SetLevel(character.level);
                MergedUpdate.UpdateData updateData = null;
                Counter.CounterData countData = null;
                updateData = Player.instance.mergedUpdate.AddUpdate(() =>
                {
                    g.OnUpdate();
                    if (!g.gameObject.activeInHierarchy)
                    {
                        Player.instance.mergedUpdate.Remove(updateData);
                        character.counter.Remove(countData);
                    }
                });
                countData = character.counter.AddCounter(() =>
                {
                    Player.instance.mergedUpdate.Remove(updateData);
                    g.gameObject.SetActive(false);
                }, Player.instance.lifeTime * character.bulletLife.status);
        //         break;
        //     case 1:
        //         break;
        //     case 2:
        //         break;
        //     case 3:
        //         break;
        //     case 4:
        //         break;
        //     case 5:
        //         break;
        //     default:
        //         break;
        // }
    }
}
