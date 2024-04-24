using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    public static CharacterData Instance;

    public Dictionary<string, int> currentStatus = new Dictionary<string, int>();

    public Dictionary<string, object> characterData { get; private set; } // 캐릭터 데이터 저장

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        GameManager.Instance.dataManager.characterData = this; // 데이터 매니저에 캐릭터 데이터 설정
    }

    public void SetCharacterData(Dictionary<string, object> characterData)
    {
        this.characterData = characterData; // 캐릭터 데이터 설정
        CalculateAndSetStats();
    }

    public void CalculateAndSetStats()
    {
        if (this.characterData == null) return;

        if (CurrentEquipped.Instance != null)
        {
            currentStatus.Clear();
            var tempStatus = ExtractStats();
            UpdateCharacterData(tempStatus);
        }
        else
        {
            currentStatus.Clear();
            currentStatus = ExtractStats();
        }

        string jobStr = characterData["job"].ToString();
        string raceStr = characterData["tribe"].ToString();
        int level = Convert.ToInt32(characterData["level"]);

        ApplyStatGrowth(jobStr, raceStr, level, ref currentStatus);

        CalculateDerivedStats(ref currentStatus);
    }

    private Dictionary<string, int> ExtractStats()
    {
        List<string> statKeys = new List<string>
        {
            "str", "dex", "int", "spi", "vit", "pie", "dh", "det", "crt", "sks", "sps", "ten", "pie", "def", "mef", "luk"
        };

        Dictionary<string, int> stats = new Dictionary<string, int>();
        foreach (var key in statKeys)
        {
            stats[key] = Convert.ToInt32(characterData.ContainsKey(key) ? characterData[key] : 0);
        }

        return stats;
    }

    public Dictionary<string, int> CurrentEquip()
    {
        List<string> gears = new List<string>
        {
            "weapon", "head", "body", "hands", "legs", "feet", "auxiliary", "earring", "necklace", "bracelet", "ring"
        };

        List<string> gearsDict = new List<string>
        {
            "itemId", "correction"
        };

        Dictionary<string, int> gearsInfo = new Dictionary<string, int>();

        foreach (var gear in gears)
        {
            foreach (var gearDict in gearsDict)
            {
                gearsInfo[$"{gear}{gearDict}"] = Convert.ToInt32(characterData.ContainsKey($"{gear}{gearDict}") ? characterData[$"{gear}{gearDict}"] : -1);
            }
        }

        return gearsInfo;
    }

    private void ApplyStatGrowth(string job, string race, int level, ref Dictionary<string, int> stats)
    {
        // 모든 캐릭터에 대해 레벨업 시 모든 능력치 기본 상승 적용
        const int BASE_STAT_GROWTH = 1; // 레벨업당 모든 능력치가 1씩 기본적으로 증가
        foreach (var key in stats.Keys.ToList())
        {
            stats[key] += BASE_STAT_GROWTH * level;
        }

        // 직업별 능력치 상승 로직
        switch (job)
        {
            case "Warrior":
                stats["str"] += level * 3;
                stats["vit"] += level * 2;
                break;
            case "Dragoon":
                stats["str"] += level * 3;
                stats["dex"] += level * 2;
                break;
            case "Bard":
                stats["dex"] += level * 3;
                stats["crt"] += level * 2;
                break;
            case "BlackMage":
                stats["int"] += level * 4;
                stats["spi"] += level;
                break;
            case "WhiteMage":
                stats["spi"] += level * 4;
                stats["int"] += level;
                break;
        }

        // 종족별 능력치 상승 로직
        switch (race)
        {
            case "Human":
                // 인간은 모든 능력치가 균등하게 상승
                foreach (var key in stats.Keys.ToList())
                {
                    stats[key] += level;
                }
                break;
            case "Elf":
                // 엘프는 민첩성과 정신력에 초점
                stats["dex"] += level * 2;
                stats["spi"] += level * 2;
                break;
            case "Dwarf":
                // 드워프는 체력과 방어력에 초점
                stats["vit"] += level * 3;
                stats["def"] += level * 2;
                break;
        }
    }

    #region 능력치 계산
    private void CalculateDerivedStats(ref Dictionary<string, int> stats)
    {
        // 여기에서 파생된 스탯(물리 공격력, 마법 공격력 등)을 계산
        float dhMultiplier = CalculateMultiplier(stats["dh"]);
        float detMultiplier = CalculateMultiplier(stats["det"]);

        // 직업에 따른 주 스탯 결정
        string job = characterData["job"].ToString();
        int mainStat = 0;

        switch (job)
        {
            case "Warrior":
                mainStat = stats["str"];
                break;
            case "Dragoon":
                mainStat = stats["str"];
                break;
            case "Bard":
                mainStat = stats["dex"];
                break;
            case "WhiteMage":
                mainStat = stats["spi"];
                break;
            case "BlaclMage":
                mainStat = stats["int"];
                break;
        }

        // 물리 공격력, 마법 공격력 등 계산
        int pap = (int)((mainStat * 1.5) + (stats["dh"] * dhMultiplier) + (stats["det"] * detMultiplier));
        int map = (int)((stats["int"] * 1.5) + (stats["dh"] * dhMultiplier) + (stats["det"] * detMultiplier));
        int mhp = (int)((stats["spi"] * 1.5) + (stats["dh"] * dhMultiplier) + (stats["det"] * detMultiplier));
        int mph = (int)((stats["pie"] * CalculateMultiplier(stats["pie"])) * 1.5);
        int maxHp = (int)(stats["vit"] * CalculateStatHealthMultiplier(stats["vit"]));

        // 자연 회복력 계산
        int hpRecovery = CalculateHpRecovery(stats["vit"], stats["det"]);

        // 계산된 값들을 다시 characterData에 저장
        currentStatus["pap"] = pap;
        currentStatus["map"] = map;
        currentStatus["mhp"] = mhp;
        currentStatus["mph"] = mph;
        currentStatus["maxHp"] = maxHp;
        currentStatus["hpRecovery"] = hpRecovery;
    }

    private float CalculateMultiplier(int statValue)
    {
        float baseValue = 1.5f;
        float currentThreshold = baseValue;
        float multiplier = 1.2f; // 초기 곱셈 인자
        float decrement = 0.05f; // 감소량 시작

        while (statValue >= currentThreshold)
        {
            multiplier -= decrement; // 각 구간마다 감소량 적용
            if (decrement > 0.01f) decrement -= 0.01f; // 감소량 점진적으로 감소
            currentThreshold *= baseValue; // 다음 구간의 임계값 설정
        }

        return multiplier;
    }

    private float CalculateStatHealthMultiplier(int vit)
    {
        int bitCount = BitCount(vit);
        return 10f + (0.5f * bitCount); // 기본 가중치에 비트 수를 곱한 추가 가중치 적용
    }

    // 체력 자연 회복 계산 메서드
    private int CalculateHpRecovery(int vit, int det)
    {
        // vit과 det의 합에 기반한 회복량 계산
        int recoveryAmount = vit + (int)(det * 0.5); // det에 가중치 0.5를 적용

        return recoveryAmount;
    }

    private int BitCount(int value)
    {
        int count = 0;
        while (value > 0)
        {
            count += value & 1;
            value >>= 1; // 오른쪽으로 비트 시프트
        }
        return count;
    }
    #endregion

    private void UpdateCharacterData(Dictionary<string, int> stats)
    {
        Dictionary<string, int> beforeStats = new Dictionary<string, int>();

        Equipment tempStat = new Equipment();

        foreach (var gear in CurrentEquipped.Instance.currentEquippeds)
        {
            tempStat.str += gear.str;
            tempStat._int += gear._int;
            tempStat.dex += gear.dex;
            tempStat.spi += gear.spi;
            tempStat.vit += gear.vit;
            tempStat.crt += gear.crt;
            tempStat.dh += gear.dh;
            tempStat.det += gear.det;
            tempStat.def += gear.def;
            tempStat.mef += gear.mef;
            tempStat.sks += gear.sks;
            tempStat.sps += gear.sps;
            tempStat.ten += gear.ten;
            tempStat.pie += gear.pie;
        }

        foreach (var stat in stats)
        {
            switch (stat.Key)
            {
                case "str":
                    beforeStats[stat.Key] = stat.Value + tempStat.str;
                    break;
                case "int":
                    beforeStats[stat.Key] = stat.Value + tempStat._int;
                    break;
                case "dex":
                    beforeStats[stat.Key] = stat.Value + tempStat.dex;
                    break;
                case "spi":
                    beforeStats[stat.Key] = stat.Value + tempStat.spi;
                    break;
                case "vit":
                    beforeStats[stat.Key] = stat.Value + tempStat.vit;
                    break;
                case "crt":
                    beforeStats[stat.Key] = stat.Value + tempStat.crt;
                    break;
                case "dh":
                    beforeStats[stat.Key] = stat.Value + tempStat.dh;
                    break;
                case "det":
                    beforeStats[stat.Key] = stat.Value + tempStat.det;
                    break;
                case "def":
                    beforeStats[stat.Key] = stat.Value + tempStat.def;
                    break;
                case "mef":
                    beforeStats[stat.Key] = stat.Value + tempStat.mef;
                    break;
                case "sks":
                    beforeStats[stat.Key] = stat.Value + tempStat.sks;
                    break;
                case "sps":
                    beforeStats[stat.Key] = stat.Value + tempStat.sps;
                    break;
                case "ten":
                    beforeStats[stat.Key] = stat.Value + tempStat.ten;
                    break;
                case "pie":
                    beforeStats[stat.Key] = stat.Value + tempStat.pie;
                    break;
                case "luk":
                    beforeStats[stat.Key] = stat.Value + tempStat.luk;
                    break;
            }
        }

        foreach (var afterStat in beforeStats)
        {
            stats[afterStat.Key] = afterStat.Value;
        }
        currentStatus = stats;
    }

    public void UpdateEquipData(Dictionary<string, int> gears)
    {
        int count = 1;
        foreach (var gear in gears)
        {
            if (!Regex.IsMatch(gear.Key, @"correction"))
            {
                if (gear.Value != -1)
                {
                    characterData[gear.Key] = gear.Value;
                    Equipment equipment = ItemData.Instance.equip[gear.Value];
                    CurrentEquipped.Instance.currentEquippeds[count] = equipment;
                }
                count++;
            }
            else
            {
                characterData[gear.Key] = gear.Value / 10;
            }
        }
    }
}
