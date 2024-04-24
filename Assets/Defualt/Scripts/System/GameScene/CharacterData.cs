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

    public Dictionary<string, object> characterData { get; private set; } // ĳ���� ������ ����

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

        GameManager.Instance.dataManager.characterData = this; // ������ �Ŵ����� ĳ���� ������ ����
    }

    public void SetCharacterData(Dictionary<string, object> characterData)
    {
        this.characterData = characterData; // ĳ���� ������ ����
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
        // ��� ĳ���Ϳ� ���� ������ �� ��� �ɷ�ġ �⺻ ��� ����
        const int BASE_STAT_GROWTH = 1; // �������� ��� �ɷ�ġ�� 1�� �⺻������ ����
        foreach (var key in stats.Keys.ToList())
        {
            stats[key] += BASE_STAT_GROWTH * level;
        }

        // ������ �ɷ�ġ ��� ����
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

        // ������ �ɷ�ġ ��� ����
        switch (race)
        {
            case "Human":
                // �ΰ��� ��� �ɷ�ġ�� �յ��ϰ� ���
                foreach (var key in stats.Keys.ToList())
                {
                    stats[key] += level;
                }
                break;
            case "Elf":
                // ������ ��ø���� ���ŷ¿� ����
                stats["dex"] += level * 2;
                stats["spi"] += level * 2;
                break;
            case "Dwarf":
                // ������� ü�°� ���¿� ����
                stats["vit"] += level * 3;
                stats["def"] += level * 2;
                break;
        }
    }

    #region �ɷ�ġ ���
    private void CalculateDerivedStats(ref Dictionary<string, int> stats)
    {
        // ���⿡�� �Ļ��� ����(���� ���ݷ�, ���� ���ݷ� ��)�� ���
        float dhMultiplier = CalculateMultiplier(stats["dh"]);
        float detMultiplier = CalculateMultiplier(stats["det"]);

        // ������ ���� �� ���� ����
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

        // ���� ���ݷ�, ���� ���ݷ� �� ���
        int pap = (int)((mainStat * 1.5) + (stats["dh"] * dhMultiplier) + (stats["det"] * detMultiplier));
        int map = (int)((stats["int"] * 1.5) + (stats["dh"] * dhMultiplier) + (stats["det"] * detMultiplier));
        int mhp = (int)((stats["spi"] * 1.5) + (stats["dh"] * dhMultiplier) + (stats["det"] * detMultiplier));
        int mph = (int)((stats["pie"] * CalculateMultiplier(stats["pie"])) * 1.5);
        int maxHp = (int)(stats["vit"] * CalculateStatHealthMultiplier(stats["vit"]));

        // �ڿ� ȸ���� ���
        int hpRecovery = CalculateHpRecovery(stats["vit"], stats["det"]);

        // ���� ������ �ٽ� characterData�� ����
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
        float multiplier = 1.2f; // �ʱ� ���� ����
        float decrement = 0.05f; // ���ҷ� ����

        while (statValue >= currentThreshold)
        {
            multiplier -= decrement; // �� �������� ���ҷ� ����
            if (decrement > 0.01f) decrement -= 0.01f; // ���ҷ� ���������� ����
            currentThreshold *= baseValue; // ���� ������ �Ӱ谪 ����
        }

        return multiplier;
    }

    private float CalculateStatHealthMultiplier(int vit)
    {
        int bitCount = BitCount(vit);
        return 10f + (0.5f * bitCount); // �⺻ ����ġ�� ��Ʈ ���� ���� �߰� ����ġ ����
    }

    // ü�� �ڿ� ȸ�� ��� �޼���
    private int CalculateHpRecovery(int vit, int det)
    {
        // vit�� det�� �տ� ����� ȸ���� ���
        int recoveryAmount = vit + (int)(det * 0.5); // det�� ����ġ 0.5�� ����

        return recoveryAmount;
    }

    private int BitCount(int value)
    {
        int count = 0;
        while (value > 0)
        {
            count += value & 1;
            value >>= 1; // ���������� ��Ʈ ����Ʈ
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
