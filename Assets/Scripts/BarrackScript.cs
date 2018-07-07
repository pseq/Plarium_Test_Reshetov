using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BarrackScript : MonoBehaviour {

    public float respawnTimeDelta;
    public int level;
    public float levelImprover;
    public int unitCost;
    public GameObject unit;
    public float respawnArea;
    public Material unitMaterial;
    public GameObject gameControl;
    public GameObject InitialTarget;

    // Use this for initialization
    void Start()
    {
        // В зависимости от уровня казармы меняется время генерации юнита.
        respawnTimeDelta = Mathf.Abs(respawnTimeDelta - level * levelImprover);
        // Начинаем генерацию юнитов.

        StartCoroutine(UnitGenerationCycle());

        //gameObject.GetComponent<HPbarScript>().HPchange(0.1f);

        //StartCoroutine(MinionGenerate());


    }

    IEnumerator UnitGenerationCycle()
    {
        while (true)
        {

            // Если хватает золота
            if (gameControl.GetComponent<GameControl>().GetGoldReserve() >= unitCost)
            {

                // Берем за юнита золото.
                gameControl.GetComponent<GameControl>().GoldDecrease(unitCost);
                yield return StartCoroutine(MinionGenerate());
            }
            // Выжидаем и повторяем.
            else yield return new WaitForSeconds(.1f);
        }
    }

    //old
    /*
    IEnumerator UnitGenerationCycle()
    {
        while (true)
        {
            // Если хватает золота
            if (gameControl.GetComponent<GameControl>().GetGoldReserve() >= unitCost)
            {
                // Генерируем юнита.
                MinionGenerate();
                gameControl.GetComponent<GameControl>().GoldDecrease(unitCost);
            }
            // Выжидаем и повторяем.
            yield return new WaitForSeconds(respawnTimeDelta);
        }
    }
    */

    IEnumerator MinionGenerate()
    {
        float delta = respawnTimeDelta / 100;
        float status = respawnTimeDelta;
        while (status > delta)
        {
            gameObject.GetComponent<HPbarScript>().HPchange((status - delta) / status);
            status -= delta;
            yield return new WaitForSeconds(delta);
        }
        // Создаем экземпляр нового юнита и помещаем его за казарму.
        GameObject newBornMinion = Instantiate(unit, gameObject.transform.position + Vector3.back * 10, Quaternion.identity);
        // Добавляем юнита в массив миньонов.
        gameControl.GetComponent<GameControl>().AddMinion(newBornMinion);
        // Применяем к юниту материал.
        newBornMinion.GetComponent<MeshRenderer>().material = unitMaterial;

        // Возвращаем прогресс-бар в исходное
        gameObject.GetComponent<HPbarScript>().HPchange(true);

    }
}
