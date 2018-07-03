using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBattleController : MonoBehaviour {

    private GameObject target;
    public float attackRange;
    public int attackDamage;
    public float attackDelay;
    public float checkDelay;
    private SpriteRenderer AttackMarker;
    private Color markerColor;

    // Use this for initialization
    void Start () {
        // Получаем ссылку на маркер атаки, и выключаем его
        AttackMarker = transform.Find("AttackMarker").gameObject.GetComponent<SpriteRenderer>();
        markerColor = AttackMarker.material.color;
        markerColor.a = 0;
        AttackMarker.material.color = markerColor;

        // Запускаем проверку цели.
        StartCoroutine("TargetCheck");
    }

    public float GetAttackRange()
    {
        return attackRange;
    }

    IEnumerator TargetCheck()
    {
        // Циклично с задержкой проверяем цель.
        while (true)
        {
            // Проверка наличия цели.
            target = gameObject.GetComponent<UnitTargetManager>().GetTarget();
            if (target)
            {
                // Является ли цель врагом.
                if (gameObject.GetComponent<UnitTargetManager>().IsTargetEnemy())
                {
                    // Проверка расстояния до цели.
                    float attackBorder = target.GetComponent<MortalScript>().attackBorder;
                    float distance = Vector3.Distance(transform.position, target.transform.position);
                    if (distance <= (attackRange + attackBorder))
                    {
                        // Если всё совпадает - атакуем.
                        yield return StartCoroutine("Attack");
                    }
                }
            }
            yield return new WaitForSeconds(checkDelay);
        }
    }

    IEnumerator Attack()
    {
        // Если цель никуда не делась - атакуем её.
        if (target)
        {
            // Наносим удар.
            target.GetComponent<MortalScript>().Hit(attackDamage);
            // И рисуем маркер атаки.
            StartCoroutine("AttackAnimation");
        }
        yield return new WaitForSeconds(attackDelay);
    }
 
    IEnumerator AttackAnimation()
    {
        // Рисуем плавно гаснущий маркер атаки.
        for (float i = 1f; i > -0.1f; i -= 0.1f)
        {
            markerColor.a = i;
            AttackMarker.material.color = markerColor;
            yield return new WaitForSeconds(0.03f);
        }
    }

}
