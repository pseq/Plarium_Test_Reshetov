﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyRespawner : MonoBehaviour {

    public float respawnTimeDelta;
    public GameObject unit;
    public float respawnArea;
    public Material enemyMaterial;
    public GameObject gameControl;
    public GameObject InitialTarget;
    public int firstWavecount;
    public int waves;

    private int respawnersCount;
    private float testAliveDelay = 0.1f;

    // Use this for initialization
    void Start () {
        // Определяем, сколько в игре есть респаунов
        respawnersCount = FindObjectsOfType(typeof(EnemyRespawner)).Length;

        // Генерируем первую волну врагов - фиксированное количество
        waves--;
        StartCoroutine(EnemyGenerationCycle(firstWavecount));

        // Запускаем последующие волны генерации
        StartCoroutine(NextWave());
    }

    IEnumerator NextWave()
    {
        yield return new WaitForSeconds(respawnTimeDelta);
        StartCoroutine(WaveStart());
    }

    IEnumerator WaveStart()
    {
        for (int i = waves; i > 0; i--)
        {
            // Проверяем количество оставшихся юнитов периодически
            while (gameControl.GetComponent<GameControl>().GetEnemies().Count > 0)
            {
                yield return new WaitForSeconds(testAliveDelay);
            }
            // И как только убили всех - немного ждем и запускаем следующую волну
            yield return new WaitForSeconds(testAliveDelay);
            // С количеством равным текущему количеству миньонов, но примерно распределенным по всем респаунам
            int avengersCount = (gameControl.GetComponent<GameControl>().GetMinions().Count / respawnersCount);
            StartCoroutine(EnemyGenerationCycle(avengersCount));
        }
    }

    IEnumerator EnemyGenerationCycle(int number)
    {
        // Генерируем с задержкой столько врагов, сколько нужно.
        for (int i = 1; i <= number; i++)
        {
            EnemyGenerate();
            yield return new WaitForSeconds(respawnTimeDelta);
        }
    }

    void EnemyGenerate()
    {
        // Создаем врага.
        GameObject newBornEnemy = Instantiate(unit);
        //Debug.Log("after instantiate agent = " + newBornEnemy.GetComponent<NavMeshAgent>());

        // И добавляем его в массив с врагами.
        gameControl.GetComponent<GameControl>().AddEnemy(newBornEnemy);
        // Присваиваем свежесозданному врагу материал.
        newBornEnemy.GetComponent<MeshRenderer>().material = enemyMaterial;
        // Если задана исходная цель - указываем ему юниту.
        if (InitialTarget) newBornEnemy.GetComponent<UnitTargetManager>().SetTarget(InitialTarget);
        // Телепортируем свежесозданного юнита в случайную точку рядом с точкой респауна
        Vector2 newBornPositionDelta = Random.insideUnitCircle * respawnArea;
        newBornEnemy.GetComponent<NavMeshAgent>().Warp(gameObject.transform.position + new Vector3(newBornPositionDelta.x, 0, newBornPositionDelta.y));

    }
}
