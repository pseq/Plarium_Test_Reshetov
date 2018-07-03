using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameControl : MonoBehaviour {

    public int gold;
    private ArrayList minions;
    private ArrayList enemies;
    private ArrayList selectedMinions;

    private void Awake()
    {
        // Инициируем массивы миньонов, врагов и выделенных
        minions = new ArrayList();
        enemies = new ArrayList();
        selectedMinions = new ArrayList();
    }

    public void GoldIncrease(int inc)
    {
        // Добавляем золото
        gold += inc;
    }

    public void GoldDecrease(int decr)
    {
        // Тратим золото
        gold -= decr;
    }

    public int GetGoldReserve()
    {
        return gold;
    }

    public ArrayList GetMinions()
    {
        return minions;
    }

    public ArrayList GetEnemies()
    {
        return enemies;
    }

    public ArrayList GetSelected()
    {
        return selectedMinions;
    }

    public void AddMinion(GameObject unit)
    {
        minions.Add(unit);
    }

    public void AddEnemy(GameObject unit)
    {
        enemies.Add(unit);
    }

    public void AddSelected(GameObject unit)
    {
        selectedMinions.Add(unit);
    }

    public void DeleteSelected(GameObject unit)
    {
        if (selectedMinions.Contains(unit)) selectedMinions.Remove(unit);
    }

    public void DeleteUnit(GameObject unit)
    {
        if (enemies.Contains(unit)) enemies.Remove(unit);
        if (minions.Contains(unit)) minions.Remove(unit);
        DeleteSelected(unit);
    }

    public bool IsMinion(GameObject unit)
    {
        return minions.Contains(unit);
    }

    public bool IsEnemy(GameObject unit)
    {
        return enemies.Contains(unit);
    }

    public void TargetMarkerUpdate()
    {
        // Метод обновления маркера цели. Перебираем всех врагов и всех миньонов, проверяем, кто куда нацелен, и расставляем маркеры.
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<UnitScript>().TargetMarkerOff();
            foreach (GameObject selectedMinion in selectedMinions)
            {
                int targetID = selectedMinion.GetComponent<UnitTargetManager>().GetTarget().GetInstanceID();
                if (targetID == enemy.GetInstanceID()) enemy.GetComponent<UnitScript>().TargetMarkerOn();
            }
        }
    }
}
