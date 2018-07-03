using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitScript : MonoBehaviour {

    public bool Selected;
    private SpriteRenderer SelectMarker;
    private MeshRenderer targetMarker;
    private ArrayList selectableUnits;
    private GameObject gameControl;

    // Use this for initialization
    void Start () {
        // Получаем ссылку на маркер выбора юнита, и выключаем его.
        SelectMarker = transform.Find("SelectMarker").GetComponent<SpriteRenderer>();
        SelectMarker.enabled = false;
        // Получаем ссылку на маркер цели этого юнита, и выключаем его.
        targetMarker = transform.Find("TargetMarker").GetComponent<MeshRenderer>();
        targetMarker.enabled = false;
        // Получаем ссылку на маркер атаки этого юнита, устанавливаем его размер по радиусу атаки юнита.
        Transform attackMarker = transform.Find("AttackMarker");
        float attackRange = gameObject.GetComponent<UnitBattleController>().GetAttackRange();
        attackMarker.localScale = new Vector3(attackRange * 2, attackRange * 2, 1);
        // Получаем объект с общими параметрами игры.
        gameControl = GameObject.FindGameObjectWithTag("GameController");
    }
	
    public void Heal()
    {

    }

    public void SetSelected ()
    {
        // При выборе юнита - обозначаем его как выделенный.
        Selected = true;
        SelectMarker.enabled = true;

        // Добавляем в массив выбранных
        gameControl.GetComponent<GameControl>().AddSelected(gameObject);

        // Показываем маркер текущей цели миньона.
        gameControl.GetComponent<GameControl>().TargetMarkerUpdate();
    }

    public void UnsetSelected ()
    {
        // При отмене выбора юнита - снимаем выделение.
        Selected = false;
        SelectMarker.enabled = false;

        // Удаляем из массива выбранных
        gameControl.GetComponent<GameControl>().DeleteSelected(gameObject);

        // Выключаем маркер текущей цели
        if (targetMarker) gameControl.GetComponent<GameControl>().TargetMarkerUpdate();
    }

    public void TargetMarkerOn()
    {
        targetMarker.enabled = true;
    }

    public void TargetMarkerOff()
    {
        targetMarker.enabled = false;
    }

    private void OnMouseDown()
    {
        // По нажатию кнопки - снимаем все остальные выделения
        // и выделяем юнита. Но только если он миньон.
        selectableUnits = gameControl.GetComponent<GameControl>().GetMinions();
        if (selectableUnits.Contains(gameObject))
        {
            foreach (GameObject SelectableUnit in selectableUnits) SelectableUnit.gameObject.GetComponent<UnitScript>().UnsetSelected();
            SetSelected();
        }
    }





}
