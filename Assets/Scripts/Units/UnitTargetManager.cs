using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTargetManager : MonoBehaviour {

    private GameObject gameControl;
    private GameObject fountain;
    private GameObject sofa;
    public GameObject target;
    public bool isMinion;
    private ArrayList opponentArray;

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

    }

    // Update is called once per frame
    void Update()
    {
        // Активируем автовыбор цели.
        if(!target) AutosetTarget();
    }

    public void AutosetTarget()
    {
        // Нацеливаем на ближайшего противника.
        SetTargetClosestEnemy();
    }

    public void SetTargetClosestEnemy()
    {
        //TODO вставить алг поиска ближ противника
        //TODO а пока двигаемся к первому (первому в массиве противников)
        // а если его нет - к фонтану или дивану
        Debug.Log(opponentArray.Count);
        if (isMinion)
        {
            if (opponentArray.Count > 0) SetTarget((GameObject)opponentArray[0]);
            else SetTarget(fountain);
        }
        else
        {
            if (opponentArray.Count > 0) SetTarget((GameObject)opponentArray[0]);
            else
                if (sofa) SetTarget(sofa);
        }
    }


    public void SetTarget(GameObject target)
    {
        // Назначить новую цель
        this.target = target;
        // Миньонам - переставить маркеры цели
        if (isMinion) gameControl.GetComponent<GameControl>().TargetMarkerUpdate();

        // При выборе цели - начинаем к ней двигаться
        if (target) gameObject.GetComponent<UnitMoving>().SetTarget(target.transform);
        else gameObject.GetComponent<UnitMoving>().SetTarget(null);
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
                    selected.GetComponent<UnitTargetManager>().SetTarget(gameObject);
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
