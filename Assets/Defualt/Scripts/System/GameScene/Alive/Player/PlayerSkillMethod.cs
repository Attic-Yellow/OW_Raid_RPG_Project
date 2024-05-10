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
    #endregion
    #endregion

    public float GetRevengeDamage()
    {
        return revangeDamage;
    }
}
