using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortalScript : MonoBehaviour {

    public float hp;
    public float maxhp;
    public float healHPinSec;
    public int priseForHead;
    public float armor;
    public GameObject grave;
    public float attackBorder;
    private GameObject gameControl;
    public bool isMinion;


    // Use this for initialization
    void Start () {
        // Получаем объект с общими параметрами игры.
        gameControl = GameObject.FindGameObjectWithTag("GameController");
        isMinion = gameControl.GetComponent<GameControl>().GetMinions().Contains(gameObject);

        hp = maxhp;
    }

    public void Hit(int damage)
    {
        // При получении удара уменьшаем здоровье, с учетом брони.
        damage -= (int)(damage * armor);


        // Уменьшаем индикатор здоровья.
        gameObject.GetComponent<HPbarScript>().HPchange((hp - damage) / hp);

        hp -= damage;


        // Если здоровье на нуле - умираем.
        if (hp <= 0) Death();

    }

    public void Death()
    {
        // Сообщаем общему скрипту, что нужно удалить юнита из всех списков.
        gameControl.GetComponent<GameControl>().DeleteUnit(gameObject);
        
        //Если уничтожен диван - выводим геймовер
        if (gameObject.name == "DeveloperSofa") gameControl.GetComponent<GameControl>().GameOver();

        //TEST
        //GameObject[] respawners = (GameObject)FindObjectsOfType(typeof(EnemyRespawner));


        // Делаем проверку на победу
        if (gameControl.GetComponent<GameControl>().GetEnemies().Count < 1)
        {
            bool respawnInProgress = false;
            EnemyRespawner[] respawners = (EnemyRespawner[]) FindObjectsOfType(typeof(EnemyRespawner));
            foreach(EnemyRespawner resp in respawners)
            {
                respawnInProgress = respawnInProgress || resp.AnybodyElse(); //resp.GetComponent<EnemyRespawner>().AnybodyElse();
            }
            if (!respawnInProgress) gameControl.GetComponent<GameControl>().Win();
        }

        //Если убит враг + золото
        if (!isMinion) gameControl.GetComponent<GameControl>().GoldIncrease(priseForHead);

        // Рисуем могилку или руины.
        Instantiate(grave, transform.position, Quaternion.identity);
        // Удаляем юнита.
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.name == "Fountain") && isMinion) StartCoroutine(Healer());
    }

    private void OnTriggerExit(Collider other)
    {
        StopCoroutine("Healer");
    }

    public float GetHP ()
    {
        return hp;
    }

    IEnumerator Healer()
    {
        
        while (hp < maxhp)
        {
            // Увеличиваем индикатор хп
            gameObject.GetComponent<HPbarScript>().HPchange((hp + healHPinSec) / hp);
            // Увеличиваем хп
            hp += healHPinSec;
            // Выжидаем и повторяем.
            yield return new WaitForSeconds(1);
        }
       
    }
}
