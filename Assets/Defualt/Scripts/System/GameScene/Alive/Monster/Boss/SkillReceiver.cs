using Photon.Pun;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum SkillEffectEnum
{
    Fireball,
    BloodFountain,
    Laser,
    Thunder,
    DeadSphere,
    FireBreath
}

public class SkillReceiver : MonoBehaviourPunCallbacks
{
    [Header("Mino")]
    [SerializeField] GameObject fireballEffect;
    [SerializeField] GameObject bloodFounainEffect;
    [SerializeField] GameObject laserEffect;
    [SerializeField] GameObject deadSphereEffect;
    [SerializeField] GameObject thunderEffect;

    [Header("Dragon")]
    [SerializeField] GameObject FireBreathEffect;


    public Dictionary<SkillEffectEnum, GameObject> skillDic = new();
    private void Awake()
    {
        AddSkillDic();
    }

    void AddSkillDic()
    {
        skillDic.Add(SkillEffectEnum.Fireball,fireballEffect);
        skillDic.Add(SkillEffectEnum.BloodFountain,bloodFounainEffect);
        skillDic.Add(SkillEffectEnum.Laser,laserEffect);    
        skillDic.Add(SkillEffectEnum.Thunder, thunderEffect);
        skillDic.Add(SkillEffectEnum.DeadSphere,deadSphereEffect);

        skillDic.Add(SkillEffectEnum.FireBreath, FireBreathEffect);
    }
}