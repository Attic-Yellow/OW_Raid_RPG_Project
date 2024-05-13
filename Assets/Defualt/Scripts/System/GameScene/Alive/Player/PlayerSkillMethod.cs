using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillMethod : MonoBehaviour
{
    Player player;
    ThirdPersonController controller;

    [Header("육중한 일격")]
    [SerializeField] float addPowerValue = 200f;

    [Header("전투의 짜릿함")]
    [SerializeField] float addHealingRate = 1.2f;
    [SerializeField] float boostedHRDuration = 10f;

    [Header("보복")]
    [SerializeField] float spinyArmorDuration = 15f;
    [SerializeField] float ReduceRate = 1.3f;
    [SerializeField] float revangeDamage = 55f;
    public bool isRevenging = false;

    [Header("1대1 결투")]
    [SerializeField] float invincibilityDuration = 10f;

    [Header("독화살")]
    [SerializeField] float posionArrowDamge = 100; //즉시 주는 데미지
    [SerializeField] float posionDotduration = 12; //도트 뎀 지속시간
    [SerializeField] float dotDamage = 30; //도트 데미지

    [Header("현자의 담시곡")]
    [SerializeField] float addCriticalValue = 2; //크리티컬 증가량
    [SerializeField] float philosophersDistance = 20; //거리
    [SerializeField] float philosophersDuration = 10; //증가시간

    [Header("마인의 진혼곡")]
    [SerializeField] float AddDamageIncrease = 3; //받는 데미지 증가량
    Monster nearestMon = null;

    private void Awake()
    {
        player = GetComponent<Player>();
        controller = GetComponent<ThirdPersonController>();
    }

    #region 워리어 스킬 메서드

    #region  육중한 일격
    public void AddPower()
    {
        StartCoroutine(AddPowerCoroutine(addPowerValue));
    }

    IEnumerator AddPowerCoroutine( float addPowerValue)
    {
        player.Power += addPowerValue;

        yield return new WaitForSeconds(controller.ReturnCurrentAniTime());

        player.Power -= addPowerValue;

    }
    #endregion

    #region 전투의 짜릿함
    public void BoostedHPRegen()
    {

        // 현재 최대 HP 기록
        float originalMaxHP = player.MaxHP;

        // 최대 HP 및 회복 효과 증가
        player.MaxHP *= addHealingRate; // 최대 HP 증가
        player.HealingRate *= addHealingRate;// 받는 HP 회복 효과 증가
        player.HPRegen *= addHealingRate; //기본 HP 회복 효과 증가

        // 실행 시점의 최대 HP 대비 20%의 HP 회복
        float healingAmount = originalMaxHP * 0.2f;
        player.CurrentHP += healingAmount;

        // 스킬 지속 시간만큼 대기 후 스킬 효과 해제
        StartCoroutine(BoostedHPRegenCoroutine(boostedHRDuration, originalMaxHP));
    }

    private IEnumerator BoostedHPRegenCoroutine(float delay, float originalMaxHP)
    {
        yield return new WaitForSeconds(delay);

        // 스킬 효과 해제
        player.MaxHP = originalMaxHP; // 최대 HP 원래대로 복원
        player.HPRegen /= addHealingRate; //기본 HP 회복 효과 감소
        player.HealingRate /= addHealingRate;// 받는 HP 회복 효과 감소
    }
    #endregion

    #region 보복

    public void SpinyArmor()//가시 갑옷
    {
        player.DamageReduc *= ReduceRate;
        isRevenging = true;
        StartCoroutine(ReduceDamage(spinyArmorDuration, ReduceRate));
    }

    private IEnumerator ReduceDamage(float delay, float reduceValue)
    {
        yield return new WaitForSeconds(delay);
        player.DamageReduc /= ReduceRate;
        isRevenging = false;
    }

    #endregion


    #region 1대1 결투
     public void invincibility()
    {
        player.IsInvincibility = true;
        StartCoroutine(invincibilityCoroutine(invincibilityDuration));
    }

    IEnumerator invincibilityCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        player.IsInvincibility = false;
    }

    public float GetRevengeDamage()
    {
        return revangeDamage;
    }
    #endregion
    #endregion

    #region 바드 스킬

    #region 독화살

    public void PosionArrow(Monster mon)
    {
        mon.TakeDamage(posionArrowDamge);
        mon.DoDotCorouitne(posionDotduration,dotDamage);
    }

    #endregion

    #region 현자의 담시곡

    public void PhilosophersOde()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, philosophersDistance, 1 << 3);
        foreach (Collider collider in colliders)
        {
            if(collider.CompareTag("Player"))
            {
              StartCoroutine(BoostCriticalCoroutine(collider.GetComponent<Player>(),addCriticalValue,philosophersDuration)); 
            }
        }
    }

    IEnumerator BoostCriticalCoroutine(Player player, float _value ,float duration)
    {
        player.CriticalRate += _value;
        yield return new WaitForSeconds(duration);
        if(player != null)  player.CriticalRate -= _value;
    }

    #endregion

    #region 마인의 진혼곡

    public void DemonticRequiem(Skill skill)
    {
        if(skill.skillActive)
        {
            if(FindNearestMon() != null)
            FindNearestMon().DamageIncrease += AddDamageIncrease;
        }
        else if(nearestMon != null) 
        {
            nearestMon.DamageIncrease -= AddDamageIncrease;
        }
    }
    private Monster FindNearestMon()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, 1<<3);

        float nearestDistance = Mathf.Infinity;

        foreach (Collider collider in colliders)
        {
            // 적인지 확인하고, 자신이 아닌 경우에만 처리
            if (!collider.CompareTag("Player"))
            {
                float distanceToEnemy = Vector3.Distance(transform.position, collider.transform.position);
                if (distanceToEnemy < nearestDistance)
                {
                    nearestDistance = distanceToEnemy;
                    nearestMon = collider.GetComponent<Monster>();
                }
            }
        }

        return nearestMon;
    }


    #endregion

    #endregion

}
