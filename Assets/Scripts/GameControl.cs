using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameControl : MonoBehaviour {

    public int gold;
    private ArrayList minions;
    private ArrayList enemies;
    private ArrayList selectedMinions;
    public Text text;
    public Text goldIndicator;

    private void Awake()
    {
        // Инициируем массивы миньонов, врагов и выделенных
        minions = new ArrayList();
        enemies = new ArrayList();
        selectedMinions = new ArrayList();

        // Отображаем, сколько золота
        goldIndicator.text = ("Gold: " + gold.ToString());

        // Останавливаем производство всех юнитов
        UnitProducingSwitcher();
        StartCoroutine("Countdown");
    }

    private void UnitProducingSwitcher()
    {
        GameObject[] producers = GameObject.FindGameObjectsWithTag("Producer");
        foreach (GameObject producer in producers)
        {
            BarrackScript[] barracks = producer.GetComponents<BarrackScript>();
            EnemyRespawner[] enRespawners = producer.GetComponents<EnemyRespawner>();
            foreach (BarrackScript script in barracks)
            {
                if (script.enabled) script.enabled = false;
                else script.enabled = true;
            }
            foreach (EnemyRespawner script in enRespawners)
            {
                if (script.enabled) script.enabled = false;
                else script.enabled = true;
            }
        }
    }

    IEnumerator Countdown()
    {
        // Отсчитываем десять секунд.
        for (int i = 9; i >= -1; i--)
        {
            text.text = i.ToString();
            if (i < 0) text.text = "GO";
            yield return new WaitForSeconds(1);
        }
        text.text = "";
        UnitProducingSwitcher();
    }

    public void GameOver()
    {
        text.text = "Game over";
        UnitProducingSwitcher();
    }


    public void GoldIncrease(int inc)
    {
        // Добавляем золото
        gold += inc;
        goldIndicator.text = ("Gold: " + gold.ToString());
    }

    public void GoldDecrease(int decr)
    {
        // Тратим золото
        gold -= decr;
        goldIndicator.text = ("Gold: " + gold.ToString());
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
            Debug.Log("ENEMY" + enemy);

            enemy.GetComponent<UnitScript>().TargetMarkerOff();
            foreach (GameObject selectedMinion in selectedMinions)
            {
                int targetID = selectedMinion.GetComponent<UnitTargetManager>().GetTarget().GetInstanceID();
                if (targetID == enemy.GetInstanceID()) enemy.GetComponent<UnitScript>().TargetMarkerOn();
            }
        }
    }
}
