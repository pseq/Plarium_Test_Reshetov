using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class UnitTargetManager : MonoBehaviour {

    private GameObject gameControl;
    private GameObject fountain;
    private GameObject sofa;
    public GameObject target;
    public bool isMinion;
    private ArrayList opponentArray;
    public float targetUpdateDelay;
    private GameObject firstTarget;

    // Use this for initialization
    void Start()
    {
        // Получаем объект с общими параметрами игры, фонтан и диван.
        gameControl = GameObject.FindGameObjectWithTag("GameController");
        fountain = GameObject.FindGameObjectWithTag("Fountain");
        sofa = GameObject.FindGameObjectWithTag("DeveloperSofa");

        // На какой стороне юнит?
        isMinion = gameControl.GetComponent<GameControl>().IsMinion(gameObject);

        // Получаем ссылку на массив с противниками.
        if (isMinion) opponentArray = gameControl.GetComponent<GameControl>().GetEnemies();
        else opponentArray = gameControl.GetComponent<GameControl>().GetMinions();

        StartCoroutine(TargetUpdater());
    }

    private void Update()
    {
            if (target && isMinion) Debug.DrawLine(gameObject.transform.position, target.transform.position, Color.green);
            if (target && !isMinion) Debug.DrawLine(gameObject.transform.position, target.transform.position, Color.red);

    }

    public void AutosetTarget()
    {

        
        //TODO сократить if (opponentArray.Count > 0)
        // а если его нет - к фонтану или дивану
        if (isMinion)
        {
            //if (target) Debug.DrawRay(gameObject.transform.position, target.transform.position, Color.green);
            if (firstTarget) SetTarget(firstTarget);
                else
                {
                    if (opponentArray.Count > 0) ClosestEnemySearch();
                        else SetTarget(fountain);
                }
        }
        else
        {
            //if (target) Debug.DrawRay(gameObject.transform.position, target.transform.position, Color.red);
            // При отсутствии миньонов враги атакуют диван.
            // Если есть миньон ближе дивана и здоровье > 50% - враг атакует этого миньона
            if (opponentArray.Count > 0)
            {
                ClosestEnemySearch();
                bool sofaClosestThanMinion = (GObjDistance(gameObject, sofa) < GObjDistance(gameObject, target)); // НЕ РАБОТАЕТ
                bool badHealth = (gameObject.GetComponent<MortalScript>().hp < gameObject.GetComponent<MortalScript>().maxhp / 2);
                if (sofaClosestThanMinion || badHealth) SetTarget(sofa);
            }
            else SetTarget(sofa);
        }
    }

    public void ClosestEnemySearch()
    {
        SetTarget((GameObject)opponentArray[0]);
        foreach (GameObject opp in opponentArray) if (GObjDistance(gameObject, opp) < GObjDistance(gameObject, target)) target = opp;
    }

    private float GObjDistance(GameObject a, GameObject b)
    {
        return Vector3.Distance(a.transform.position, b.transform.position);
    }

    public void SetFirstTarget(GameObject target)
    {
        SetTarget(target);
        firstTarget = target;
    }


    public void SetTarget(GameObject target)
    {
        // Назначить новую цель
        this.target = target;
        // Миньонам - переставить маркеры цели
        if (isMinion) gameControl.GetComponent<GameControl>().TargetMarkerUpdate();

        Debug.Log("is target = " + (bool)target);
        Debug.Log("set target = " + target);
        // При выборе цели - начинаем к ней двигаться
        if (target) gameObject.GetComponent<UnitMoving>().SetNavTarget(target.transform);
        //else gameObject.GetComponent<UnitMoving>().SetTarget(null);
    }

    IEnumerator TargetUpdater()
    {
        while (true)
        {
            AutosetTarget();
            yield return new WaitForSeconds(targetUpdateDelay);
        }
    }


    public GameObject GetTarget()
    {
        return target;
    }

    public bool IsTargetEnemy()
    {
        if (target)
        {
            // Для минионов противники - враги.
            if (isMinion) return (opponentArray.Contains(target));
            // Для врагов - миньоны и диван
            else return (opponentArray.Contains(target) || (target.GetInstanceID() == sofa.GetInstanceID()));
        }
        else return false;
        
    }

    // Если нажали правой кнопкой на врага - назначить его целью для всех выделенных миньонов.
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && !gameObject.GetComponent<UnitTargetManager>().isMinion)
        {
            ArrayList selectedArr = gameControl.GetComponent<GameControl>().GetSelected();
            for (int i = 0; i < selectedArr.Count; ++i)
            {
                GameObject selected = (GameObject) selectedArr[i];
                if (selected)
                {
                    selected.GetComponent<UnitTargetManager>().SetFirstTarget(gameObject);
                }
            }
        }
    }

    void GetEnemies()
    {

    }

    void GetMinNavDistance()
    {
        // получить расстояние
        // найти минимальное
    }

    void GetMinDistanceTarget()
    {

    }


}
