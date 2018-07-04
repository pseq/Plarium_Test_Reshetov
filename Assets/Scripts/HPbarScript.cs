using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPbarScript : MonoBehaviour {

    private Transform bar;
    private Transform leader;

    // Use this for initialization
    void Start () {
        // Получаем трансформ индикатора статуса здоровья.
        bar = transform.Find("HP");
        leader = GameObject.FindWithTag("HPbar").transform;
    }

    // Update is called once per frame
    void Update () {
        // Поворачиваем индикатор вслед за камерой.
        bar.LookAt(leader);
        //bar.LookAt(Camera.main.transform);
	}

    public void HPPercentDecrease(float decr)
    {
        // Укорачиваем маркер при получении урона.
        if (bar)
        {
            float d = (32f / 10f) * bar.localScale.x * decr / 2f;
            bar.localScale -= new Vector3((bar.localScale.x * decr), 0f, 0f);
            bar.position -= Vector3.right * d;

        }
    }

    public void HPPercentIncrease(float inc)
    {
        // Удлинняем маркер при лечении.
        float d = (32f / 10f) * bar.localScale.x * inc / 2f;
        bar.localScale += new Vector3((bar.localScale.x * inc), 0f, 0f);
        bar.position += Vector3.right * d;
    }
}
