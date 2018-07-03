using System.Collections;
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
    public int enemiesCount;

    // Use this for initialization
    void Start () {
        // Сразу при старте запускаем генерацию врагов. 
        StartCoroutine(EnemyGenerationCycle(enemiesCount));
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
