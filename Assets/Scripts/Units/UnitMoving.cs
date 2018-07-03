using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMoving : MonoBehaviour {

    private NavMeshAgent agent;
    public Transform target;
    public float stoppingDistance = 0;
    public float recalculateMovingDelay;


    // Use this for initialization
    void Start () {
        // Получаем навигационного агента юнита.
        agent = GetComponent<NavMeshAgent>();
        // И запускаем пересчет движения.
        StartCoroutine("MovingRecalc");
    }

    public void SetTarget(Transform target)
    {
        // При получении новой цели - заставляем агента двигаться.
        this.target = target;
        agent.isStopped = false;
    }
	
    IEnumerator MovingRecalc()
    {
        // Циклично, с задержкой пересчитываем движение
        while (agent)
        {
            // Если обозначена цель - начинаем движение к ней.
            if (target) agent.SetDestination(target.position);
            // А если нет - останавливаемся.
            else agent.isStopped = true;
            // Проверяем тип цели - враг или нет
            bool isEnemy = gameObject.GetComponent<UnitTargetManager>().IsTargetEnemy();
            // Если враг - то подходим к нему не ближе, чем на расстояние атаки
            if (isEnemy) agent.stoppingDistance = gameObject.GetComponent<UnitBattleController>().attackRange;
            else agent.stoppingDistance = stoppingDistance;
            yield return new WaitForSeconds(recalculateMovingDelay);
        }
    }
}
