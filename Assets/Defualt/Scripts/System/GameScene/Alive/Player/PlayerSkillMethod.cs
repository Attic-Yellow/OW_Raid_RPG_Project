using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillMethod : MonoBehaviour
{
    Player player;
    ThirdPersonController controller;

    [Header("������ �ϰ�")]
    [SerializeField] float addPowerValue = 200f;

    [Header("������ ¥����")]
    [SerializeField] float addHealingRate = 1.2f;
    [SerializeField] float boostedHRDuration = 10f;

    [Header("����")]
    [SerializeField] float spinyArmorDuration = 15f;
    [SerializeField] float ReduceRate = 1.3f;
    [SerializeField] float revangeDamage = 55f;
    public bool isRevenging = false;

    [Header("1��1 ����")]
    [SerializeField] float invincibilityDuration = 10f;

    [Header("��ȭ��")]
    [SerializeField] float posionArrowDamge = 100; //��� �ִ� ������
    [SerializeField] float posionDotduration = 12; //��Ʈ �� ���ӽð�
    [SerializeField] float dotDamage = 30; //��Ʈ ������

    [Header("������ ��ð�")]
    [SerializeField] float addCriticalValue = 2; //ũ��Ƽ�� ������
    [SerializeField] float philosophersDistance = 20; //�Ÿ�
    [SerializeField] float philosophersDuration = 10; //�����ð�

    [Header("������ ��ȥ��")]
    [SerializeField] float AddDamageIncrease = 3; //�޴� ������ ������
    Monster nearestMon = null;

    private void Awake()
    {
        player = GetComponent<Player>();
        controller = GetComponent<ThirdPersonController>();
    }

    #region ������ ��ų �޼���

    #region  ������ �ϰ�
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

    #region ������ ¥����
    public void BoostedHPRegen()
    {

        // ���� �ִ� HP ���
        float originalMaxHP = player.MaxHP;

        // �ִ� HP �� ȸ�� ȿ�� ����
        player.MaxHP *= addHealingRate; // �ִ� HP ����
        player.HealingRate *= addHealingRate;// �޴� HP ȸ�� ȿ�� ����
        player.HPRegen *= addHealingRate; //�⺻ HP ȸ�� ȿ�� ����

        // ���� ������ �ִ� HP ��� 20%�� HP ȸ��
        float healingAmount = originalMaxHP * 0.2f;
        player.CurrentHP += healingAmount;

        // ��ų ���� �ð���ŭ ��� �� ��ų ȿ�� ����
        StartCoroutine(BoostedHPRegenCoroutine(boostedHRDuration, originalMaxHP));
    }

    private IEnumerator BoostedHPRegenCoroutine(float delay, float originalMaxHP)
    {
        yield return new WaitForSeconds(delay);

        // ��ų ȿ�� ����
        player.MaxHP = originalMaxHP; // �ִ� HP ������� ����
        player.HPRegen /= addHealingRate; //�⺻ HP ȸ�� ȿ�� ����
        player.HealingRate /= addHealingRate;// �޴� HP ȸ�� ȿ�� ����
    }
    #endregion

    #region ����

    public void SpinyArmor()//���� ����
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


    #region 1��1 ����
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

    #region �ٵ� ��ų

    #region ��ȭ��

    public void PosionArrow(Monster mon)
    {
        mon.TakeDamage(posionArrowDamge);
        mon.DoDotCorouitne(posionDotduration,dotDamage);
    }

    #endregion

    #region ������ ��ð�

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

    #region ������ ��ȥ��

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
            // ������ Ȯ���ϰ�, �ڽ��� �ƴ� ��쿡�� ó��
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
