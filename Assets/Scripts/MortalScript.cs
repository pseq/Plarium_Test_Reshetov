using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortalScript : MonoBehaviour {

    public int hp;
    public float armor;
    public GameObject grave;
    public float attackBorder;
    private GameObject gameControl;
    private ArrayList selectableUnits;


    // Use this for initialization
    void Start () {
        // Получаем объект с общими параметрами игры.
        gameControl = GameObject.FindGameObjectWithTag("GameController");
    }

    public void Hit(int damage)
    {
        // При получении удара уменьшаем здоровье, с учетом брони.
        damage -= (int)(damage * armor);
        int hp0 = hp;
        hp -= damage;

        // Уменьшаем индикатор здоровья.
        gameObject.GetComponent<HPbarScript>().HPPercentDecrease((float)damage / hp0);

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

}
