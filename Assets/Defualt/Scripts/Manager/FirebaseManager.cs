using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using System;
using Firebase.Auth;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.IO;
using Unity.VisualScripting;
using System.Linq;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;

    public FirebaseAuth auth { get; private set; }

    FirebaseFirestore db;
    public bool IsFirebaseInitialized = false; // �ʱ�ȭ ���� �÷���

    #region Firebase �ʱ�ȭ
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

        GameManager.Instance.firebaseManager = this;

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                print("Could not resolve all Firebase dependencies: " + task.Exception);
                return;
            }

            var dependencyStatus = task.Result;

            if (dependencyStatus == DependencyStatus.Available)
            {
                db = FirebaseFirestore.DefaultInstance;
                auth = FirebaseAuth.DefaultInstance;
                IsFirebaseInitialized = true; // �ʱ�ȭ �Ϸ�
                print("Firebase�� ����� �غ� ��");
            }
            else
            {
                print($"��� Firebase ���Ӽ��� �ذ��� �� ����: {dependencyStatus}");
            }
        });
    }
    #endregion

    #region ���ε�

    // ĳ���� ���� �� Firestore�� ���ε��ϴ� �޼���
    public async Task<bool> CreateCharacter(string userId, string email, string job, string tribe, string serverName, string characterName)
    {
        bool isCharacterCreated = false;

        try
        {
            #region ĳ���� ���� ����
            /*** �⺻ ���� ***/
            int totalSTR = 0; // �� (striking power)
            int totalINT = 0; // ���� (intelligence)
            int totalDEX = 0; // ��ø (dexterity)
            int totalSPI = 0; // ���ŷ� (spirit)
            int totalVIT = 0; // Ȱ�� (vitality)

            /*** ���� ���� ***/
            int totalCRT = 0; // �ش�,ġ��Ÿ (critical hit)
            int totalDH = 0; // ����,���� (direct hit rate)
            int totalDET = 0; // ����,���� (determination)
            int totalSKS = 0; // ��� ���� �ӵ�,���� ���� �ӵ� (skill speed)
            int totalSPS = 0; // ���� ���� �ӵ�,�ֹ� �ӵ� (spell speed)
            int totalTEN = 0; // �ұ�,�γ� (tenacity)
            int totalPIE = 0; // �ž�,���� (piety)

            /*** ��� ����***/
            int totalDEF = 0; // ���� ���� (defense)
            int totalMEF = 0; // ���� ���� (magic defense)

            /*** ��Ÿ ����***/
            int totalLUK = 0; // �� (luck)
            #endregion

            #region ĳ���� ���� ���� ���� �ε� �� ���� �Ҵ�
            AssetBundleCreateRequest loadCharAssetBundle = AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, "AssetBundles", "createcharacter"));
            AssetBundle charBundle = await loadCharAssetBundle.ToTask();
            if (charBundle == null)
            {
                print("���� ���� �ε� ����");
                return false;
            }

            TextAsset loadedJobAsset = await charBundle.LoadAssetAsync<TextAsset>(job).ToTask<TextAsset>();
            if (loadedJobAsset != null)
            {
                string dataAsJson = loadedJobAsset.text;
                Dictionary<string, int> jobData = JsonConvert.DeserializeObject<Dictionary<string, int>>(dataAsJson);

                totalSTR += jobData["str"];
                totalINT += jobData["int"];
                totalDEX += jobData["dex"];
                totalSPI += jobData["spi"];
                totalVIT += jobData["vit"];
                totalCRT += jobData["crt"];
                totalDH  += jobData["dh"];
                totalDET += jobData["det"];
                totalSKS += jobData["sks"];
                totalSPS += jobData["sps"];
                totalTEN += jobData["ten"];
                totalPIE += jobData["pie"];
                totalDEF += jobData["def"];
                totalMEF += jobData["mef"];
                totalLUK += jobData["luk"];
            }
            else
            {
                print($"���� ������ �ε� ����: {job}");
                return false;
            }

            TextAsset loadedTribeAsset = await charBundle.LoadAssetAsync<TextAsset>(tribe).ToTask<TextAsset>();
            if (loadedTribeAsset != null)
            {
                string dataAsJson = loadedTribeAsset.text;
                Dictionary<string, int> tribeData = JsonConvert.DeserializeObject<Dictionary<string, int>>(dataAsJson);

                totalSTR += tribeData["str"];
                totalINT += tribeData["int"];
                totalDEX += tribeData["dex"];
                totalSPI += tribeData["spi"];
                totalVIT += tribeData["vit"];
                totalCRT += tribeData["crt"];
                totalDH  += tribeData["dh"];
                totalDET += tribeData["det"];
                totalSKS += tribeData["sks"];
                totalSPS += tribeData["sps"];
                totalTEN += tribeData["ten"];
                totalPIE += tribeData["pie"];
                totalDEF += tribeData["def"];
                totalMEF += tribeData["mef"];
                totalLUK += tribeData["luk"];
            }
            else
            {
                print($"���� ������ �ε� ����: {tribe}");
                return false;
            }

            string uniqueCharacterID = System.Guid.NewGuid().ToString(); // ĳ���� ID ���� ����

            Dictionary<string, object> newCharacter = new Dictionary<string, object>
            {
                { "name", characterName },
                { "charId", uniqueCharacterID},
                { "server", serverName },
                { "tribe", tribe },
                { "job", job },
                { "level", 1 },
                { "str", totalSTR},
                { "int", totalINT},
                { "dex", totalDEX},
                { "spi", totalSPI},
                { "vit", totalVIT},
                { "crt", totalCRT},
                { "dh", totalDH},
                { "det", totalDET},
                { "sks", totalSKS},
                { "sps", totalSPS},
                { "ten", totalTEN},
                { "pie", totalPIE},
                { "def", totalDEF},
                { "mef", totalMEF},
                { "luk", totalLUK}
            };
            #endregion

            // ������ ���ε� ��� ����: users/{userId}/{serverName}/{characterId}
            DocumentReference docRef = db.Collection("users").Document("email").Collection(email).Document(userId).Collection(serverName).Document(uniqueCharacterID);
            // Firestore�� ĳ���� ������ ���ε�
            await docRef.SetAsync(newCharacter);

            #region ĳ���� �⺻ ��� �߰�
            string[] gears = new string[] { "weapon", "head", "body", "hands", "legs", "feet", "auxiliary", "earring", "necklace", "bracelet", "ring" };

            for (int i = 0; i < gears.Length; i++)
            {
                DocumentReference deepDocRef = docRef.Collection("currentEquipped").Document(gears[i]);
                int correction = UnityEngine.Random.Range(1, 5);

                switch (job)
                {
                    case "Warrior":
                        if (ItemData.Instance.equip.ContainsKey(10000 + (i * 100)))
                        {
                            Dictionary<string, int> newEquip = new Dictionary<string, int>
                            {
                                { "itemId", ItemData.Instance.equip[10000 + (i * 100)].itemId},
                                { "correction", correction }
                            };

                            await deepDocRef.SetAsync(newEquip);
                        }
                        else
                        {
                            Dictionary<string, int> newEquip = new Dictionary<string, int>
                            {
                                { "itemId" , -1 },
                                { "correction" , 0 }
                            }; 

                            await deepDocRef.SetAsync(newEquip);
                        }
                        break;
                    case "Dragoon":
                        if (ItemData.Instance.equip.ContainsKey(20000 + (i * 100)))
                        {
                            Dictionary<string, int> newEquip = new Dictionary<string, int>
                            {
                                { "itemId", ItemData.Instance.equip[20000 + (i * 100)].itemId},
                                { "correction", correction}
                            };

                            await deepDocRef.SetAsync(newEquip);
                        }
                        else
                        {
                            Dictionary<string, int> newEquip = new Dictionary<string, int>
                            {
                                { "itemId" , -1 },
                                { "correction" , 0 }
                            };

                            await deepDocRef.SetAsync(newEquip);
                        }
                        break;
                    case "Bard":
                        if (ItemData.Instance.equip.ContainsKey(30000 + (i * 100)))
                        {
                            Dictionary<string, int> newEquip = new Dictionary<string, int>
                            {
                                { "itemId", ItemData.Instance.equip[30000 + (i * 100)].itemId},
                                { "correction", correction}
                            };

                            await deepDocRef.SetAsync(newEquip);
                        }
                        else
                        {
                            Dictionary<string, int> newEquip = new Dictionary<string, int>
                            {
                                { "itemId" , -1 },
                                { "correction" , 0 }
                            };

                            await deepDocRef.SetAsync(newEquip);
                        }
                        break;
                    case "WhiteMage":
                        if (ItemData.Instance.equip.ContainsKey(40000 + (i * 100)))
                        {
                            Dictionary<string, int> newEquip = new Dictionary<string, int>
                            {
                                { "itemId", ItemData.Instance.equip[40000 + (i * 100)].itemId},
                                { "correction", correction}
                            };

                            await deepDocRef.SetAsync(newEquip);
                        }
                        else
                        {
                            Dictionary<string, int> newEquip = new Dictionary<string, int>
                            {
                                { "itemId" , -1 },
                                { "correction" , 0 }
                            };

                            await deepDocRef.SetAsync(newEquip);
                        }
                        break;
                    case "BlackMage":
                        if (ItemData.Instance.equip.ContainsKey(50000 + (i * 100)))
                        {
                            Dictionary<string, int> newEquip = new Dictionary<string, int>
                            {
                                { "itemId", ItemData.Instance.equip[50000 + (i * 100)].itemId},
                                { "correction", correction}
                            };

                            await deepDocRef.SetAsync(newEquip);
                        }
                        else
                        {
                            Dictionary<string, int> newEquip = new Dictionary<string, int>
                            {
                                { "itemId" , -1 },
                                { "correction" , 0 }
                            };

                            await deepDocRef.SetAsync(newEquip);
                        }
                        break;
                }
               
            }
            #endregion

            isCharacterCreated = true;
            print($"ĳ���� {characterName} ���� �� ���ε� �Ϸ�.");
        }
        catch (Exception e)
        {
            isCharacterCreated = false;
            print($"ĳ���� ���� �� ���� �߻�: {e.Message}");
        }

        return isCharacterCreated;
    }

    // ĳ���� ���� ��� ������ ���ε�
    public async Task<bool> UpLoadCurrentEquip(string userId, string email, string serverName, string uniqueCharacterID)
    {
        string[] gears = new string[] { "weapon", "head", "body", "hands", "legs", "feet", "auxiliary", "earring", "necklace", "bracelet", "ring" };

        var currentEquip = CurrentEquipped.Instance.currentEquippeds;
        int i = 1;

        try
        {
            foreach (var gear in gears)
            {
                DocumentReference docRef = db.Collection("users").Document("email").Collection(email).Document(userId).
                    Collection(serverName).Document(uniqueCharacterID).Collection("currentEquipped").Document(gear);

                if (currentEquip[i].cor == 0)
                {
                    currentEquip[i].cor = -1;
                }

                Dictionary<string, int> newEquip = new Dictionary<string, int>
                            {
                                { "itemId", currentEquip[i].itemId },
                                { "correction", currentEquip[i].cor }
                            };
                i++;
                await docRef.SetAsync(newEquip);
            }
            return true;
        }
        catch (Exception e)
        {
            print($"���� ��� ������ ���ε� ����: {e.Message}");
        }
        return false;
    }

    private string DocumentName(EquipmentType equipmentType)
    {
        switch (equipmentType)
        {
            case EquipmentType.Weapon: return "weapon";
            case EquipmentType.Head: return "head";
            case EquipmentType.Body: return "body";
            case EquipmentType.Hands: return "hand";
            case EquipmentType.Legs: return "leg";
            case EquipmentType.Feet: return "feet";
            case EquipmentType.Auxiliary: return "auxiliary";
            case EquipmentType.Earring: return "earring";
            case EquipmentType.Necklace: return "necklace";
            case EquipmentType.Bracelet: return "bracelet";
            case EquipmentType.Ring: return "ring";
            default: return null;
        }
    }

    // ĳ���� ����� ������ ���ε�
    public async Task<bool> UpLoadEquipped(string userId, string email, string serverName, string uniqueCharacterID, EquipmentType equipmentType)
    {
        try
        {
            if (equipmentType != EquipmentType.None)
            {
                var list = Equipped.Instance.GetEquipmentList(equipmentType).ToList();
                string documentName = DocumentName(equipmentType);
                int i = 0;

                foreach (var equip in list)
                {
                    DocumentReference docRef = db.Collection("users").Document("email").Collection(email).Document(userId).
                        Collection(serverName).Document(uniqueCharacterID).Collection($"{documentName}").Document($"{documentName}s{i}");

                    if (equip.cor == 0)
                    {
                        equip.cor = -1;
                    }

                    Dictionary<string, int> newEquip = new Dictionary<string, int>()
                        {
                            { $"itemId", equip.itemId },
                            { $"correction", equip.cor }
                        };

                    i++;
                    await docRef.SetAsync(newEquip);
                }
            }
            
            return true;
        }
        catch (Exception e)
        {
            print($"����� ������ ���ε� ����: {e.Message}");
        }
        return false;
    }
    #endregion

    #region �ε�

    // ����� ������ �ε�
    public async Task LoadUserData(string userId, string email, Action<Dictionary<string, object>> onCompletion)
    {
        var docRef = db.Collection("users").Document("email").Collection(email);
        var userSnapshot = await docRef.GetSnapshotAsync();
        try
        {
            if (userSnapshot == null)
            {
                print("���� ������ �ε� ����");
                onCompletion(null);
                return;
            }

            var userData = docRef.Document(userId);
            var userDocSnapshot = await userData.GetSnapshotAsync();
            var user = userDocSnapshot.ToDictionary();
            onCompletion(user);
        }
        catch (Exception ex)
        {
            print($"���� ������ �ε� �� ���� �߻�: {ex.Message}");
            onCompletion(null);
        }
    }

    // ĳ���� ��� �ε�
    public async Task LoadCharacter(string userId, string server, Action<List<Dictionary<string, object>>> onCompletion)
    {
        var user = auth.CurrentUser;
        try
        {
            var userDocRef = db.Collection("users").Document("email").Collection(user.Email); // "users" �÷��ǿ��� �α��ε� ������� ������ Ž��
            var userSnapshot = await userDocRef.GetSnapshotAsync();

            if (userSnapshot == null)
            {
                print("����� ������ ã�� �� �����ϴ�.");
                onCompletion(null);
                return;
            }
            
            var serverCharactersRef = userDocRef.Document(userId).Collection(server); // ������ ������ �������� ĳ���� ������ Ž��
            var querySnapshot = await serverCharactersRef.GetSnapshotAsync();

            var characters = new List<Dictionary<string, object>>();
            foreach (var document in querySnapshot.Documents)
            {
                Dictionary<string, object> character = document.ToDictionary();

                var currentEquipRef = serverCharactersRef.Document(document.Id).Collection("currentEquipped");
                var equipQuery = await currentEquipRef.GetSnapshotAsync();

                if (equipQuery == null)
                {
                    Debug.Log("���� ����");
                }
                else
                {
                    foreach (var gear in equipQuery.Documents)
                    {
                        Dictionary<string, object> tempGear = gear.ToDictionary();
                        foreach (var temp in tempGear)
                        {
                            character[$"{gear.Id}{temp.Key}"] = temp.Value;
                        }
                    }
                }

                string[] gearTypes = { "weapon", "head", "body", "hand", "leg", "feet", "auxiliary", "earring", "necklace", "bracelet", "ring" };

                foreach (string gearType in gearTypes)
                {
                    var gearCollectionRef = serverCharactersRef.Document(document.Id).Collection(gearType);
                    var gearCollectionSnapshot = await gearCollectionRef.GetSnapshotAsync();
                    if (gearCollectionSnapshot != null)
                    {
                        foreach (var gearDoc in gearCollectionSnapshot.Documents)
                        {
                            var gearData = gearDoc.ToDictionary();
                            foreach (var entry in gearData)
                            {
                                character[$"{gearDoc.Id}{entry.Key}"] = entry.Value;
                            }
                        }
                    }
                }

                characters.Add(character);
            }

            // ĳ���Ͱ� ���� ��� null�� ��ȯ
            if (characters.Count == 0)
            {
                onCompletion(null);
                return;
            }

            onCompletion(characters);
        }
        catch (Exception e)
        {
            print($"ĳ���� �ε� �� ���� �߻�: {e.Message}");
            onCompletion(null);
        }
    }

    #region (��) ����� �ε� �񵿱� �޼���
    public async Task<bool> LoadEquipped(string userId, string email, string serverName, string uniqueCharacterID)
    {
        string[] gears = new string[] { "weapon", "head", "body", "hands", "legs", "feet", "auxiliary", "earring", "necklace", "bracelet", "ring" };

        try
        {
            var weapons = Equipped.Instance.weapon;
            var head = Equipped.Instance.head;
            var body = Equipped.Instance.body;

            foreach (var gear in gears)
            {
                for (int i = 0; i < 30; i++)
                {
                    DocumentReference docRef = db.Collection("users").Document("email").Collection(email).Document(userId).Collection(serverName).Document(uniqueCharacterID).Collection(gear).Document($"{gear}{i}");
                    var equipQuery = await docRef.GetSnapshotAsync();
                    var equip = new Dictionary<string, object>();

                    if (equipQuery.Exists)
                    {
                        foreach (var field in equipQuery.ToDictionary())
                        {
                            equip[field.Key] = field.Value;
                        }
                        
                        if (equip.ContainsKey("itemId") && equip.ContainsKey("correction"))
                        {
                            int itemId = Convert.ToInt32(equip["itemId"]);
                            int correction = Convert.ToInt32(equip["correction"]);

                            switch (gear)
                            {
                                case "weapon":
                                    if (ItemData.Instance.equip.ContainsKey(itemId))
                                    {
                                        weapons[i] = ItemData.Instance.equip[itemId];
                                    }
                                    weapons[i].cor = correction;
                                    break;
                                case "head":
                                    if (ItemData.Instance.equip.ContainsKey(itemId))
                                    {
                                        head[i] = ItemData.Instance.equip[itemId];
                                    }
                                    head[i].cor = correction;
                                    break;
                                case "body":
                                    if (ItemData.Instance.equip.ContainsKey(itemId))
                                    {
                                        body[i] = ItemData.Instance.equip[itemId];
                                    }
                                    body[i].cor = correction;
                                    break;
                            }
                        }
                    }
                }
            }
            return true;
        }
        catch (Exception e)
        {
            print($"����� ������ �ε� �� ���� �߻�: {e.Message}");
        }
        return false;
    }
    #endregion

    #endregion

    #region ������ �ʱ�ȭ
    // ����� ������ �ʱ�ȭ
    public async Task InitializeUserData(string userId, string email, Action<bool> onCompletion)
    {
        var docRef = db.Collection("users").Document("email").Collection(email);
        var userSnapshot = await docRef.GetSnapshotAsync();

        if (userSnapshot == null)
        {
            print("����� ������ �ʱ�ȭ ����");
            onCompletion(false);
            return;
        }

        var user = new List<Dictionary<string, object>>
        {
            new Dictionary<string, object>
            {
                { "guest", GameManager.Instance.GetIsUserGuest() },
                { "emailauthentication", GameManager.Instance.GetIsEmailAuthentication() },
                { "manager", false }
            }
        };

        await docRef.Document(userId).SetAsync(user[0]);
        onCompletion(true);
    }
    #endregion

    #region �α׾ƿ�
    //  �α׾ƿ�
    public void SignOut()
    {
        if (auth.CurrentUser != null)
        {
            auth.SignOut();
            Debug.Log("�α׾ƿ� ����");
        }
    }
    #endregion

    #region ���� �ʵ�-�� ���� ����
    // ���� �ʵ� �� ���� ���� Ȯ��
    public void CheckFieldValueExists(string document, string userValue, System.Action<bool> onResult)
    {
        if (!IsFirebaseInitialized)
        {
            print("Firebase�� ���� �ʱ�ȭ���� �ʾ���");
            onResult(false);
            return;
        }

        DocumentReference docRef = db.Collection("answer").Document(document);
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                print("���� ��ȸ �� ���� �߻�");
                onResult(false);
                return;
            }

            var documentSnapshot = task.Result;
            if (!documentSnapshot.Exists)
            {
                print("korean ������ �������� �ʽ��ϴ�.");
                onResult(false);
                return;
            }


            if (documentSnapshot.ContainsField(userValue)) // ����� �Է� ���� �ʵ�� �����ϴ��� Ȯ��
            {
                onResult(true); // print($"�ʵ� '{userValue}'�� ������");
            }
            else
            {
                onResult(false); // print($"�ʵ� '{userValue}'�� �������� ����);
            }
        });
    }
    #endregion
}

public static class AsyncOperationExtensions
{
    public static Task<AssetBundle> ToTask(this AssetBundleCreateRequest request)
    {
        var completionSource = new TaskCompletionSource<AssetBundle>();
        request.completed += _ => completionSource.TrySetResult(request.assetBundle);
        return completionSource.Task;
    }

    public static Task<T> ToTask<T>(this AssetBundleRequest request) where T : UnityEngine.Object
    {
        var completionSource = new TaskCompletionSource<T>();
        request.completed += _ =>
        {
            if (request.asset != null)
            {
                completionSource.TrySetResult(request.asset as T);
            }
            else
            {
                completionSource.TrySetException(new NullReferenceException("AssetBundleRequest returned null asset."));
            }
        };
        return completionSource.Task;
    }
}

/*
            var JobStatus = await db.Collection("createCharacterJob").Document(job).GetSnapshotAsync();
            if (JobStatus.Exists)
            {
                print(JobStatus.GetValue<int>("str"));
            }

            var TribeStatus = await db.Collection("createCharacter").Document(tribe).GetSnapshotAsync();
            if (TribeStatus.Exists)
            {
                totalSTR += TribeStatus.GetValue<int>("str");
            }
*/

#region (��) ����� ���ε�
//switch (equipmentType)
//{
//    #region ���� ����� ���ε�
//    case EquipmentType.Weapon:

//        var weapons = Equipped.Instance.weapon.ToList();
//        foreach (var weapon in weapons)
//        {
//            DocumentReference docRef = db.Collection("users").Document("email").Collection(email).Document(userId).
//                Collection(serverName).Document(uniqueCharacterID).Collection("weapon").Document($"weapons{i}");

//            if (weapon.cor == 0)
//            {
//                weapon.cor = -1;
//            }

//            Dictionary<string, int> newEquip = new Dictionary<string, int>()
//                        {
//                            { $"itemId", weapon.itemId },
//                            { $"correction", weapon.cor }
//                        };

//            i++;
//            await docRef.SetAsync(newEquip);
//        }


//        break;
//    #endregion
//    #region �Ӹ� ����� ���ε�
//    case EquipmentType.Head:

//        var heads = Equipped.Instance.head.ToList();

//        foreach (var head in heads)
//        {
//            DocumentReference docRef = db.Collection("users").Document("email").Collection(email).Document(userId).
//                Collection(serverName).Document(uniqueCharacterID).Collection("head").Document($"heads{i}");

//            if (head.cor == 0)
//            {
//                head.cor = -1;
//            }

//            Dictionary<string, int> newEquip = new Dictionary<string, int>()
//                        {
//                            { $"itemId", head.itemId },
//                            { $"correction", head.cor }
//                        };

//            i++;
//            await docRef.SetAsync(newEquip);
//        }

//        break;
//    #endregion
//    #region ���� ����� ���ε�
//    case EquipmentType.Body:

//        var bodys = Equipped.Instance.body.ToList();

//        foreach (var body in bodys)
//        {
//            DocumentReference docRef = db.Collection("users").Document("email").Collection(email).Document(userId).
//                Collection(serverName).Document(uniqueCharacterID).Collection("body").Document($"bodys{i}");

//            if (body.cor == 0)
//            {
//                body.cor = -1;
//            }

//            Dictionary<string, int> newEquip = new Dictionary<string, int>()
//                        {
//                            { $"itemId", body.itemId },
//                            { $"correction", body.cor }
//                        };

//            i++;
//            await docRef.SetAsync(newEquip);
//        }

//        break;
//    #endregion
//    #region �� ����� ���ε�
//    case EquipmentType.Hands:

//        var hands = Equipped.Instance.hands.ToList();

//        foreach (var hand in hands)
//        {
//            DocumentReference docRef = db.Collection("users").Document("email").Collection(email).Document(userId).
//                Collection(serverName).Document(uniqueCharacterID).Collection("hands").Document($"hands{i}");

//            if (hand.cor == 0)
//            {
//                hand.cor = -1;
//            }

//            Dictionary<string, int> newEquip = new Dictionary<string, int>()
//                        {
//                            { $"itemId", hand.itemId },
//                            { $"correction", hand.cor }
//                        };

//            i++;
//            await docRef.SetAsync(newEquip);
//        }

//        break;
//    #endregion
//    #region �ٸ� ����� ���ε�
//    case EquipmentType.Legs:

//        var legs = Equipped.Instance.legs.ToList();

//        foreach (var leg in legs)
//        {
//            DocumentReference docRef = db.Collection("users").Document("email").Collection(email).Document(userId).
//                Collection(serverName).Document(uniqueCharacterID).Collection("legs").Document($"leg{i}");

//            if (leg.cor == 0)
//            {
//                leg.cor = -1;
//            }

//            Dictionary<string, int> newEquip = new Dictionary<string, int>()
//                        {
//                            { $"itemId", leg.itemId },
//                            { $"correction", leg.cor }
//                        };

//            i++;
//            await docRef.SetAsync(newEquip);
//        }

//        break;
//    #endregion
//    #region �Ź� ����� ���ε�
//    case EquipmentType.Feet:

//        var feets = Equipped.Instance.feet.ToList();

//        foreach (var feet in feets)
//        {
//            DocumentReference docRef = db.Collection("users").Document("email").Collection(email).Document(userId).
//                Collection(serverName).Document(uniqueCharacterID).Collection("feet").Document($"feets{i}");

//            if (feet.cor == 0)
//            {
//                feet.cor = -1;
//            }

//            Dictionary<string, int> newEquip = new Dictionary<string, int>()
//                        {
//                            { $"itemId", feet.itemId },
//                            { $"correction", feet.cor }
//                        };

//            i++;
//            await docRef.SetAsync(newEquip);
//        }

//        break;
//    #endregion
//    #region ���� ���� ����� ���ε�
//    case EquipmentType.Auxiliary:

//        var auxiliarys = Equipped.Instance.auxiliary.ToList();

//        foreach (var auxiliary in auxiliarys)
//        {
//            DocumentReference docRef = db.Collection("users").Document("email").Collection(email).Document(userId).
//                Collection(serverName).Document(uniqueCharacterID).Collection("auxiliary").Document($"auxiliarys{i}");

//            if (auxiliary.cor == 0)
//            {
//                auxiliary.cor = -1;
//            }

//            Dictionary<string, int> newEquip = new Dictionary<string, int>()
//                        {
//                            { $"itemId", auxiliary.itemId },
//                            { $"correction", auxiliary.cor }
//                        };

//            i++;
//            await docRef.SetAsync(newEquip);
//        }

//        break;
//    #endregion
//    #region �Ͱ��� ����� ���ε�
//    case EquipmentType.Earring:

//        var earrings = Equipped.Instance.earring.ToList();

//        foreach (var earring in earrings)
//        {
//            DocumentReference docRef = db.Collection("users").Document("email").Collection(email).Document(userId).
//                Collection(serverName).Document(uniqueCharacterID).Collection("earring").Document($"earrings{i}");

//            if (earring.cor == 0)
//            {
//                earring.cor = -1;
//            }

//            Dictionary<string, int> newEquip = new Dictionary<string, int>()
//                        {
//                            { $"itemId", earring.itemId },
//                            { $"correction", earring.cor }
//                        };

//            i++;
//            await docRef.SetAsync(newEquip);
//        }

//        break;
//    #endregion
//    #region ����� ����� ���ε�
//    case EquipmentType.Necklace:

//        var necklaces = Equipped.Instance.necklace.ToList();

//        foreach (var necklace in necklaces)
//        {
//            DocumentReference docRef = db.Collection("users").Document("email").Collection(email).Document(userId).
//                Collection(serverName).Document(uniqueCharacterID).Collection("necklasce").Document($"necklaces{i}");

//            if (necklace.cor == 0)
//            {
//                necklace.cor = -1;
//            }

//            Dictionary<string, int> newEquip = new Dictionary<string, int>()
//                        {
//                            { $"itemId", necklace.itemId },
//                            { $"correction", necklace.cor }
//                        };

//            i++;
//            await docRef.SetAsync(newEquip);
//        }

//        break;
//    #endregion
//    #region ���� ����� ���ε�
//    case EquipmentType.Bracelet:

//        var bracelets = Equipped.Instance.bracelet.ToList();

//        foreach (var bracelet in bracelets)
//        {
//            DocumentReference docRef = db.Collection("users").Document("email").Collection(email).Document(userId).
//                Collection(serverName).Document(uniqueCharacterID).Collection("bracelet").Document($"bracelets{i}");

//            if (bracelet.cor == 0)
//            {
//                bracelet.cor = -1;
//            }

//            Dictionary<string, int> newEquip = new Dictionary<string, int>()
//                        {
//                            { $"itemId", bracelet.itemId },
//                            { $"correction", bracelet.cor }
//                        };

//            i++;
//            await docRef.SetAsync(newEquip);
//        }

//        break;
//    #endregion
//    #region ���� ����� ���ε�
//    case EquipmentType.Ring:

//        var rings = Equipped.Instance.ring.ToList();

//        foreach (var ring in rings)
//        {
//            DocumentReference docRef = db.Collection("users").Document("email").Collection(email).Document(userId).
//                Collection(serverName).Document(uniqueCharacterID).Collection("ring").Document($"rings{i}");

//            if (ring.cor == 0)
//            {
//                ring.cor = -1;
//            }

//            Dictionary<string, int> newEquip = new Dictionary<string, int>()
//                        {
//                            { $"itemId", ring.itemId },
//                            { $"correction", ring.cor }
//                        };

//            i++;
//            await docRef.SetAsync(newEquip);
//        }

//        break;
//        #endregion
//}
#endregion