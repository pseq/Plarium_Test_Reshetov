﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortalScript : MonoBehaviour {

    public int hp;
    public int maxhp;
    public int healHPinSec;
    public float armor;
    public GameObject grave;
    public float attackBorder;
    private GameObject gameControl;
    private ArrayList selectableUnits;
    private bool isMinion;


    // Use this for initialization
    void Start () {
        // Получаем объект с общими параметрами игры.
        gameControl = GameObject.FindGameObjectWithTag("GameController");
    }

    public void Hit(int damage)
    {
        // При получении удара уменьшаем здоровье, с учетом брони.
        damage -= (int)(damage * armor);

        // Уменьшаем индикатор здоровья.
        gameObject.GetComponent<HPbarScript>().HPPercentDecrease((float)damage / hp);

        hp -= damage;

        // Если здоровье на нуле - умираем.
        if (hp <= 0) Death();

    }

    public void Death()
    {
        // Передаем индикатор цели следующему врагу.
        selectableUnits = gameControl.GetComponent<GameControl>().GetMinions();
        foreach (GameObject SelectableUnit in selectableUnits)
        {
            if (SelectableUnit.gameObject.GetComponent<UnitScript>().Selected)
            {
                SelectableUnit.gameObject.GetComponent<UnitScript>().UnsetSelected();
                SelectableUnit.gameObject.GetComponent<UnitScript>().SetSelected();
            }
        }

        // Рисуем могилку или руины.
        Instantiate(grave, transform.position, Quaternion.identity);

        // Сообщаем общему скрипту, что нужно удалить юнита из всех списков.
        gameControl.GetComponent<GameControl>().DeleteUnit(gameObject);

        // Удаляем юнита.
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        bool isMinion = gameControl.GetComponent<GameControl>().GetMinions().Contains(gameObject);
        if ((other.name == "Fountain") && isMinion) StartCoroutine("Healer");
    }

    private void OnTriggerExit(Collider other)
    {
        StopCoroutine("Healer");
    }

    IEnumerator Healer()
    {
        
        while (hp < maxhp)
        {
            // Увеличиваем индикатор хп
            gameObject.GetComponent<HPbarScript>().HPPercentIncrease((float)healHPinSec / hp);
            // Увеличиваем хп
            hp += healHPinSec;
            // Выжидаем и повторяем.
            yield return new WaitForSeconds(1);
        }
       
    }
}
