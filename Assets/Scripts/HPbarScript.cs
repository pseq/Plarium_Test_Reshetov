using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPbarScript : MonoBehaviour {

    private Transform barBase;
    private Transform leader;
    private SpriteRenderer hpBar;
    private Vector2 size0;

    // Use this for initialization
    void Start () {
        // Получаем трансформ индикатора статуса здоровья.
        barBase = transform.Find("HP");
        hpBar = barBase.Find("HPbar").GetComponent<SpriteRenderer>();
        hpBar.drawMode = SpriteDrawMode.Sliced;
        leader = GameObject.FindWithTag("HPbarLeader").transform;

        size0 = hpBar.size;
    }

    // Update is called once per frame
    void Update () {
        // Поворачиваем индикатор вслед за камерой.
        barBase.LookAt(leader);
    }

    public void HPchange(float k)
    {
        if (hpBar)
        {
            hpBar.size = new Vector2(hpBar.size.x * k, hpBar.size.y);
        }
    }

    public void HPchange(bool reset)
    {
        if (hpBar)
        {
            if (reset) hpBar.size = size0;
            else hpBar.size = Vector2.zero;
        }
    }
}
