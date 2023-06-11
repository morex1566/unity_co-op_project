using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHp = 10;
    [SerializeField] float curHp;
    [SerializeField] float addHp;
    [SerializeField] float subHp;
    Slider hpbar;

    void Start()
    {
        hpbar = GetComponent<Slider>();
        curHp = maxHp;
        hpbar.value = curHp / maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if (curHp > 0)
        {
            if (curHp > 3)
            {
                curHp -= Time.deltaTime;
            }
        }
        else
            curHp = 0;
        hpbar.value = Mathf.Lerp(hpbar.value, curHp / maxHp, Time.deltaTime*10);

        if (curHp > maxHp)
            curHp = maxHp;

    }

    public void AddHp()
    {
        curHp += addHp;
    }

    public void SubHp()
    {
        curHp -= subHp;
    }

    public float GetHealthNum()
    {
        return curHp;
    }

    
}
