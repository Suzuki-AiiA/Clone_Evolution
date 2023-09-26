using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Character : MonoBehaviour
{
    GameObject currentLevel;
    public int level;
    public GameObject levelParent;
    // Weapon weapon;
    public Counter counter;
    float initScale;
    public Renderer rend;
    Color initColor;
    Interval.IntervalData intervalData;
    public int currentAge = 0;
    public float shootAngle;
    List<Weapon> weapon = new List<Weapon>();
    bool canUseGate = true;
    public Animator ani;
    public StatusUpdateSet speed;
    public StatusUpdateSet bulletLife;
    public StatusUpdateSet damage;
    public StatusUpdateSet size;
    public GameObject[] tools;
    public GameObject currentTools;
    public GameObject lv3Hat;
    public float randomNum;


    private void Start()
    {
        speed.multiplier = GManager.Data.speedGate.multiplier;
        bulletLife.multiplier = GManager.Data.rangeGate.multiplier;
        damage.multiplier = GManager.Data.damageGate.multiplier;
        size.multiplier = GManager.Data.sizeGate.multiplier;
    }
    public void Init(int level, int weaponNum = 1)
    {
        ani = GetComponent<Animator>();
        MoveStop();
        ani.speed = GManager.Data.playerShootRate;
        shootAngle = GManager.Data.shootAngle;
        initScale = transform.localScale.x;
        Weapon weapon = GetComponentInChildren<Weapon>();
        currentLevel = levelParent.GetChild(level);
        currentLevel.SetActive(true);
        currentTools = tools[level];
        currentTools.SetActive(true);
        weapon.character = this;
        initColor = rend.material.color;
        this.weapon.Add(weapon);
        if (weaponNum > 1)
        {
            for (int i = 0; i < weaponNum - 1; i++)
            {
                Weapon w = Instantiate(weapon, weapon.transform.parent);
                w.character = this;
                this.weapon.Add(w);
            }
        }
        SetWeaponAngle();
        ChangeLevel(level);
        Player.instance.characters.Add(this);
        transform.parent = Player.instance.moveBase;


    }
    public void ChangeLevel(int level)
    {
        if (currentLevel != null)
            currentLevel.SetActive(false);
        if (currentTools != null)
            currentTools.SetActive(false);

        lv3Hat.SetActive(level == 2);
        currentLevel = levelParent.GetChild(level);
        currentLevel.SetActive(true);
        currentTools = tools[level];
        currentTools.SetActive(true);
        this.level = level;
        ani.SetInteger("level", level);

    }
    public void GameStart()
    {
        MoveStart();
        counter.AddCounter(() =>
        {
            OffRun();
        }, Random.Range(0, randomNum));
        // counter.AddCounter(() =>
        // {
        //     intervalData = Player.instance.interval.AddInterval(() =>
        //     {
        //         Shoot();
        //     }, Player.instance.shootDelay);
        // }, Player.instance.moveStartDelay);

    }
    public void ChangeShootSpeed(int num)
    {
        speed.Set(num);
        // intervalData.SetTarget(Player.instance.shootDelay / (speed.status));
        ani.speed = speed.status * GManager.Data.playerShootRate;
    }
    public void ChangeBulletLife(int num)
    {
        bulletLife.Set(num);
    }
    public void ChangeDamage(int num)
    {
        damage.Set(num);
    }
    public void ChangeBulletSize(int num)
    {
        size.Set(num);
    }
    public void Dead()
    {
        Player.instance.characters.Remove(this);
        gameObject.SetActive(false);
        if (Player.instance.characters.Count == 0)
        {
            Player.instance.GameOver();
        }
        GManager.instance.DestroyPartcle(transform.position);
    }

    public void Shoot()
    {
        // weapon.Shoot();
        for (int i = 0; i < weapon.Count; i++)
        {
            weapon[i].Shoot();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("card"))
        {
            other.GetComponent<Card>().Complete();
        }
        else if (other.CompareTag("gate"))
        {
            UpgradeFromGate(other.GetComponent<Gate>());
        }
        else if (other.CompareTag("block"))
        {
            Dead();
        }
    }
    void UpgradeFromGate(Gate gate)
    {
        if (canUseGate && !gate.CheckUse())
        {
            Player.instance.UpgradeWithGate(gate);
            canUseGate = false;
            counter.AddCounter(() =>
            {
                canUseGate = true;
            }, 1);
        }

    }
    public void UpgradeGate(Gate gate)
    {
        switch (gate.type)
        {
            case GateType.Year:
                AddYear(gate.value);
                break;
            case GateType.Damage:
                ChangeDamage(gate.value);
                break;
            case GateType.Speed:
                ChangeShootSpeed(gate.value);
                break;
            case GateType.Range:
                ChangeBulletLife(gate.value);
                break;
            case GateType.Size:
                ChangeBulletSize(gate.value);
                break;
        }
    }

    public void MoveStart()
    {
        GetComponent<Animator>().SetBool("move", true);
    }
    public void MoveStop()
    {
        GetComponent<Animator>().SetBool("move", false);
    }
    public void Upgrade(Card card)
    {
        switch (card.type)
        {
            case CardType.Add:
                for (int i = 0; i < card.currentValue; i++)
                {
                    Add(card.currentValue);
                }
                break;
            case CardType.Multiply:
                for (int i = 0; i < card.currentValue - 1; i++)
                {
                    Add(card.currentValue);
                }
                break;
            case CardType.Year:
                AddYear(card.currentValue);
                break;
            case CardType.Shot:
                for (int i = 0; i < card.currentValue - 1; i++)
                {
                    Weapon w = Instantiate(weapon[0], weapon[0].transform.parent);
                    w.character = this;
                    weapon.Add(w);

                }
                SetWeaponAngle();
                break;
        }
    }
    public void AddYear(int year)
    {
        currentAge += year;
        while (level < Player.instance.targetAges.Length && currentAge >= Player.instance.targetAges[level])
        {
            level++;
        }
        ChangeLevel(level);
        OnlyRun();
        counter.AddCounter(() =>
        {
            OffRun();
        }, Random.Range(0, randomNum));
    }
    void OnlyRun()
    {
        ani.SetBool("isRun", true);
        ani.SetTrigger("run");
    }
    void OffRun()
    {
        ani.SetBool("isRun", false);

    }
    public void OnEventEnter()
    {
        // transform.localScale = Vector3.one * initScale * 1.2f;
        transform.DOScale(Vector3.one * initScale * 1.2f, 0.1f);
        rend.material.DOColor(Color.green, 0.1f);
    }
    public void OnEventExit()
    {
        // transform.localScale = Vector3.one * initScale
        transform.DOScale(Vector3.one * initScale, 0.1f);
        rend.material.DOColor(initColor, 0.1f); ;
    }
    public void Add(int num)
    {
        Character c = PoolingManager.instance.characterPooling.GetPoolingObject();
        c.currentAge = currentAge;
        c.Init(level, weapon.Count);
        c.transform.position = transform.position;
        c.OnlyRun();
        c.GameStart();
        // GetComponent<Animator>().Rebind();
        // MoveStart();
        // c.GetComponent<Animator>().SetTrigger("reset");
        // Player.instance.intervalData.SetCount(Player.instance.shootDelay - Player.instance.moveStartDelay);
    }
    public void SetWeaponAngle()
    {
        if (weapon.Count == 1)
        {
            weapon[0].transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            weapon[0].transform.localRotation = Quaternion.Euler(0, -shootAngle, 0);
            for (int i = 0; i < weapon.Count; i++)
            {
                weapon[i].transform.localRotation = Quaternion.Euler(0, shootAngle * 2 / (weapon.Count - 1) * i - shootAngle, 0);
            }
        }
    }
    [System.Serializable]
    public class StatusUpdateSet
    {
        public float status = 1;
        public int num = 0;
        public float multiplier = 1;
        public void Set(int value)
        {
            num += value;
            status = 1 + (num / multiplier);
        }
    }
}