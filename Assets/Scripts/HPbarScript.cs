using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPbarScript : MonoBehaviour {

    public Color colorHP = Color.green;
    private Transform bar;

    // Use this for initialization
    void Start () {
        // Получаем трансформ индикатора статуса здоровья.
        bar = transform.Find("HP");
    }

    // Update is called once per frame
    void Update () {
        // Поворачиваем индикатор вслед за камерой.
        bar.LookAt(Camera.main.transform);
	}

    public void HPPercentDecrease(float decr)
    {
        // Укорачиваем маркер при получении урона.
        if (bar) bar.localScale -= new Vector3((bar.localScale.x * decr), 0f, 0f);
    }

    void HPPercentIncrease(float inc)
    {
        // Удлинняем маркер при лечении.
        if (bar) bar.localScale += new Vector3((bar.localScale.x * inc), 0f, 0f);
    }
}
