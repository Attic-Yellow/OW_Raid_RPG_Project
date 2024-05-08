using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
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
            if (CurrentEquipped.Instance.isStarted)
            {
                UpdateEquipData(Equip());
                
            }
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

    #region ���� ��� ��ųʸ� ����ŷ
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
    #endregion

    #region ����� ��ųʸ� ����ŷ
    public Dictionary<string, int> Equip()
    {
        List<string> gears = new List<string>
        {
            "weapons", "heads", "bodys", "hands", "legs", "feets", "auxiliarys", "earrings", "necklaces", "bracelets", "rings"
        };

        List<string> gearsDict = new List<string>
        {
            "itemId", "correction"
        };

        Dictionary<string, int> gearsInfo = new Dictionary<string, int>();

        foreach (var gear in gears)
        {
            for (int i = 0; i < 30; i++)
            {
                foreach (var gearDict in gearsDict)
                {
                    gearsInfo[$"{gear}{i}{gearDict}"] = Convert.ToInt32(characterData.ContainsKey($"{gear}{i}{gearDict}") ? characterData[$"{gear}{i}{gearDict}"] : -1);
                }
            }
        }

        return gearsInfo;
    }
    #endregion

    #region ����ǰ ��ųʸ� ����ŷ
    public Dictionary<string, int> InventoryUnpack()
    {
        List<string> dicts = new List<string>
        {
            "itemId", "itemCount"
        };

        Dictionary<string, int> inven = new Dictionary<string, int>();

        foreach (var dict in dicts)
        {
            for (int i = 0; i < 120; i++)
            {
                inven[$"inventory{i}{dict}"] = Convert.ToInt32(characterData.ContainsKey($"inventory{i}{dict}") ? characterData[$"inventory{i}{dict}"] : -1);
            }
        }
        Debug.Log(inven[$"inventory0itemId"]);
        Debug.Log(inven[$"inventory0itemCount"]);
        return inven;
    }
    #endregion

    #region ������ ���� �ɷ�ġ ����
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
    #endregion

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

    #region ĳ���� �ɷ�ġ ����
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
    #endregion

    #region ĳ���� ���� ��� ����
    public void UpdateCurrentEquipData(Dictionary<string, int> gears)
    {
        int count = 1;
        foreach (var gear in gears)
        {
            if (!Regex.IsMatch(gear.Key, @"correction"))
            {
                if (gear.Value != 0)
                {
                    characterData[gear.Key] = gear.Value; 
                    
                    if (ItemData.Instance.equip.ContainsKey(gear.Value))
                    {
                        Equipment equipment = ItemData.Instance.equip[gear.Value];
                        CurrentEquipped.Instance.currentEquippeds[count] = equipment;
                    }
                }
                count++;
            }
            else
            {
                characterData[gear.Key] = gear.Value;
            }
        }
        CalculateEquip();
    }
    #endregion

    #region ĳ���� ����� ������ �ҷ�����
    public void UpdateEquipData(Dictionary<string, int> equipData)
    {
        // ��� Ÿ�Ժ��� ó��
        string[] gearTypes = new string[] { "weapon", "head", "body", "hands", "legs", "feet", "auxiliary", "earring", "necklace", "bracelet", "ring" };

        foreach (string gearType in gearTypes)
        {
            var gearList = GetTypeList(gearType);

            int i = 0;
            // �� ���Ժ��� ��� ������ ������Ʈ
            foreach (var gear in equipData)
            {
                switch (gearType)
                {
                    #region ���� �����
                    case "weapon":
                        if (Regex.IsMatch(gear.Key, @"weapon"))
                        {
                            if (!Regex.IsMatch(gear.Key, @"correction"))
                            {
                                if (gear.Value != 0)
                                {
                                    if (ItemData.Instance.equip.ContainsKey(gear.Value))
                                    {
                                        gearList[i] = ItemData.Instance.equip[gear.Value];
                                    }
                                }
                            }
                            else
                            {
                                gearList[i].cor = gear.Value;
                                i++;
                                if (i == 30)
                                {
                                    i = 0;
                                }
                            }
                        }    
                        break;
                    #endregion
                    #region �Ӹ� �����
                    case "head":
                        if (!Regex.IsMatch(gear.Key, @"correction") && Regex.IsMatch(gear.Key, @"head"))
                        {
                            if (gear.Value != 0)
                            {
                                if (ItemData.Instance.equip.ContainsKey(gear.Value))
                                {
                                    gearList[i] = ItemData.Instance.equip[gear.Value];
                                }
                            }
                        }
                        else
                        {
                            gearList[i].cor = gear.Value;
                            i++; 
                            if (i == 30)
                            {
                                i = 0;
                            }
                        }
                        break;
                    #endregion
                    #region ���� �����
                    case "body":
                        if (!Regex.IsMatch(gear.Key, @"correction") && Regex.IsMatch(gear.Key, @"body"))
                        {
                            if (gear.Value != 0)
                            {
                                if (ItemData.Instance.equip.ContainsKey(gear.Value))
                                {
                                    gearList[i] = ItemData.Instance.equip[gear.Value];
                                }
                            }
                        }
                        else
                        {
                            gearList[i].cor = gear.Value;
                            i++;
                            if (i == 30)
                            {
                                i = 0;
                            }
                        }
                        break;
                    #endregion
                    #region �� �����
                    case "hands":
                        if (!Regex.IsMatch(gear.Key, @"correction") && Regex.IsMatch(gear.Key, @"hand"))
                        {
                            if (gear.Value != 0)
                            {
                                if (ItemData.Instance.equip.ContainsKey(gear.Value))
                                {
                                    gearList[i] = ItemData.Instance.equip[gear.Value];
                                }
                            }
                        }
                        else
                        {
                            gearList[i].cor = gear.Value;
                            i++;
                            if (i == 30)
                            {
                                i = 0;
                            }
                        }
                        break;
                    #endregion
                    #region �ٸ� �����
                    case "legs":
                        if (!Regex.IsMatch(gear.Key, @"correction") && Regex.IsMatch(gear.Key, @"leg"))
                        {
                            if (gear.Value != 0)
                            {
                                if (ItemData.Instance.equip.ContainsKey(gear.Value))
                                {
                                    gearList[i] = ItemData.Instance.equip[gear.Value];
                                }
                            }
                        }
                        else
                        {
                            gearList[i].cor = gear.Value;
                            i++;
                            if (i == 30)
                            {
                                i = 0;
                            }
                        }
                        break;
                    #endregion
                    #region �Ź� �����
                    case "feet":
                        if (!Regex.IsMatch(gear.Key, @"correction") && Regex.IsMatch(gear.Key, @"feet"))
                        {
                            if (gear.Value != 0)
                            {
                                if (ItemData.Instance.equip.ContainsKey(gear.Value))
                                {
                                    gearList[i] = ItemData.Instance.equip[gear.Value];
                                }
                            }
                        }
                        else
                        {
                            gearList[i].cor = gear.Value;
                            i++;
                            if (i == 30)
                            {
                                i = 0;
                            }
                        }
                        break;
                    #endregion
                    #region ���� ���� �����
                    case "auxiliary":
                        if (!Regex.IsMatch(gear.Key, @"correction") && Regex.IsMatch(gear.Key, @"auxiliary"))
                        {
                            if (gear.Value != 0)
                            {
                                if (ItemData.Instance.equip.ContainsKey(gear.Value))
                                {
                                    gearList[i] = ItemData.Instance.equip[gear.Value];
                                }
                            }
                        }
                        else
                        {
                            gearList[i].cor = gear.Value;
                            i++;
                            if (i == 30)
                            {
                                i = 0;
                            }
                        }
                        break;
                    #endregion
                    #region �Ͱ��� �����
                    case "earring":
                        if (!Regex.IsMatch(gear.Key, @"correction") && Regex.IsMatch(gear.Key, @"earring"))
                        {
                            if (gear.Value != 0)
                            {
                                if (ItemData.Instance.equip.ContainsKey(gear.Value))
                                {
                                    gearList[i] = ItemData.Instance.equip[gear.Value];
                                }
                            }
                        }
                        else
                        {
                            gearList[i].cor = gear.Value;
                            i++;
                            if (i == 30)
                            {
                                i = 0;
                            }
                        }
                        break;
                    #endregion
                    #region ����� �����
                    case "necklace":
                        if (!Regex.IsMatch(gear.Key, @"correction") && Regex.IsMatch(gear.Key, @"necklace"))
                        {
                            if (gear.Value != 0)
                            {
                                if (ItemData.Instance.equip.ContainsKey(gear.Value))
                                {
                                    gearList[i] = ItemData.Instance.equip[gear.Value];
                                }
                            }
                        }
                        else
                        {
                            gearList[i].cor = gear.Value;
                            i++;
                            if (i == 30)
                            {
                                i = 0;
                            }
                        }
                        break;
                    #endregion
                    #region ���� �����
                    case "bracelet":
                        if (!Regex.IsMatch(gear.Key, @"correction") && Regex.IsMatch(gear.Key, @"bracelet"))
                        {
                            if (gear.Value != 0)
                            {
                                if (ItemData.Instance.equip.ContainsKey(gear.Value))
                                {
                                    gearList[i] = ItemData.Instance.equip[gear.Value];
                                }
                            }
                        }
                        else
                        {
                            gearList[i].cor = gear.Value;
                            i++;
                            if (i == 30)
                            {
                                i = 0;
                            }
                        }
                        break;
                    #endregion
                    #region ���� �����
                    case "ring":
                        if (!Regex.IsMatch(gear.Key, @"correction") && Regex.IsMatch(gear.Key, @"ring"))
                        {
                            if (gear.Value != 0)
                            {
                                if (ItemData.Instance.equip.ContainsKey(gear.Value))
                                {
                                    gearList[i] = ItemData.Instance.equip[gear.Value];
                                }
                            }
                        }
                        else
                        {
                            gearList[i].cor = gear.Value;
                            i++;
                            if (i == 30)
                            {
                                i = 0;
                            }
                        }
                        break;
                        #endregion

                }

            }
        }

        // ��� ���� �˸�
        Equipped.Instance.onChangeGear?.Invoke();
    }
    #endregion

    #region ������ ������ �ҷ�����
    public void UpdateInventory(Dictionary<string, int> inven)
    {
        var invenList = Inventory.Instance.items;
        int i = 0;

        foreach (var invenSlot in inven)
        {
            if (!Regex.IsMatch(invenSlot.Key, @"Count"))
            {
                if (ItemData.Instance.itemsD.ContainsKey(invenSlot.Value))
                {
                    invenList[i] = ItemData.Instance.itemsD[invenSlot.Value];
                }

            }
            else
            {
                invenList[i].itemCount = invenSlot.Value;
                i++;
            }
        }
    }
    #endregion

    // ��� Ÿ�Կ� ���� ����Ʈ ���� ��ȯ
    private List<Equipment> GetTypeList(string gearType)
    {
        switch (gearType.ToLower())
        {
            case "weapon": return Equipped.Instance.weapon;
            case "head": return Equipped.Instance.head;
            case "body": return Equipped.Instance.body;
            case "hands": return Equipped.Instance.hands;
            case "legs": return Equipped.Instance.legs;
            case "feet": return Equipped.Instance.feet;
            case "auxiliary": return Equipped.Instance.auxiliary;
            case "earring": return Equipped.Instance.earring;
            case "necklace": return Equipped.Instance.necklace;
            case "bracelet": return Equipped.Instance.bracelet;
            case "ring": return Equipped.Instance.ring;
            default: return null;
        }
    }

    public void CalculateEquip()
    {
        string[] keys = new string[]
        {
            "weaponcorrection", "headcorrection", "bodycorrection",
            "handscorrection", "legscorrection", "feetcorrection",
            "auxiliarycorrection", "earringcorrection", "necklacecorrection",
            "braceletcorrection", "ringcorrection"
        };

        int count = 1;
        foreach (var key in keys)
        {
            int value = (int)characterData[key];

            if (CurrentEquipped.Instance.currentEquippeds[count].str != 0)
            {
                CurrentEquipped.Instance.currentEquippeds[count].str += CurrentEquipped.Instance.currentEquippeds[count].str * ((value / 10) + 1);
            }

            if (CurrentEquipped.Instance.currentEquippeds[count]._int != 0)
            {
                CurrentEquipped.Instance.currentEquippeds[count]._int += CurrentEquipped.Instance.currentEquippeds[count]._int * ((value / 10) + 1);
            }

            if (CurrentEquipped.Instance.currentEquippeds[count].dex != 0)
            {
                CurrentEquipped.Instance.currentEquippeds[count].dex += CurrentEquipped.Instance.currentEquippeds[count].dex * ((value / 10) + 1);
            }

            if (CurrentEquipped.Instance.currentEquippeds[count].spi != 0)
            {
                CurrentEquipped.Instance.currentEquippeds[count].spi += CurrentEquipped.Instance.currentEquippeds[count].spi * ((value / 10) + 1);
            }

            if (CurrentEquipped.Instance.currentEquippeds[count].luk != 0)
            {
                CurrentEquipped.Instance.currentEquippeds[count].luk += CurrentEquipped.Instance.currentEquippeds[count].luk * ((value / 10) + 1);
            }

            if (CurrentEquipped.Instance.currentEquippeds[count].crt != 0)
            {
                CurrentEquipped.Instance.currentEquippeds[count].crt += CurrentEquipped.Instance.currentEquippeds[count].crt * ((value / 10) + 1);
            }

            if (CurrentEquipped.Instance.currentEquippeds[count].dh != 0)
            {
                CurrentEquipped.Instance.currentEquippeds[count].dh += CurrentEquipped.Instance.currentEquippeds[count].dh * ((value / 10) + 1);
            }

            if (CurrentEquipped.Instance.currentEquippeds[count].det != 0)
            {
                CurrentEquipped.Instance.currentEquippeds[count].det += CurrentEquipped.Instance.currentEquippeds[count].det * ((value / 10) + 1);
            }

            if (CurrentEquipped.Instance.currentEquippeds[count].def != 0)
            {
                CurrentEquipped.Instance.currentEquippeds[count].def += CurrentEquipped.Instance.currentEquippeds[count].def * ((value / 10) + 1);
            }

            if (CurrentEquipped.Instance.currentEquippeds[count].mef != 0)
            {
                CurrentEquipped.Instance.currentEquippeds[count].mef += CurrentEquipped.Instance.currentEquippeds[count].mef * ((value / 10) + 1);
            }

            if (CurrentEquipped.Instance.currentEquippeds[count].sks != 0)
            {
                CurrentEquipped.Instance.currentEquippeds[count].sks += CurrentEquipped.Instance.currentEquippeds[count].sks * ((value / 10) + 1);
            }

            if (CurrentEquipped.Instance.currentEquippeds[count].sps != 0)
            {
                CurrentEquipped.Instance.currentEquippeds[count].sps += CurrentEquipped.Instance.currentEquippeds[count].sps * ((value / 10) + 1);
            }

            if (CurrentEquipped.Instance.currentEquippeds[count].ten != 0)
            {
                CurrentEquipped.Instance.currentEquippeds[count].ten += CurrentEquipped.Instance.currentEquippeds[count].ten * ((value / 10) + 1);
            }

            if (CurrentEquipped.Instance.currentEquippeds[count].pie != 0)
            {
                CurrentEquipped.Instance.currentEquippeds[count].pie += CurrentEquipped.Instance.currentEquippeds[count].pie * ((value / 10) + 1);
            }

            CurrentEquipped.Instance.currentEquippeds[count].cor += value;
            count++;
        }
    }
}
