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
    public bool IsFirebaseInitialized = false; // 초기화 상태 플래그

    #region Firebase 초기화
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
                IsFirebaseInitialized = true; // 초기화 완료
                print("Firebase를 사용할 준비가 됨");
            }
            else
            {
                print($"모든 Firebase 종속성을 해결할 수 없음: {dependencyStatus}");
            }
        });
    }
    #endregion

    #region 업로드

    // 캐릭터 생성 및 Firestore에 업로드하는 메서드
    public async Task<bool> CreateCharacter(string userId, string email, string job, string tribe, string serverName, string characterName)
    {
        bool isCharacterCreated = false;

        try
        {
            #region 캐릭터 스탯 변수
            /*** 기본 스탯 ***/
            int totalSTR = 0; // 힘 (striking power)
            int totalINT = 0; // 지능 (intelligence)
            int totalDEX = 0; // 민첩 (dexterity)
            int totalSPI = 0; // 정신력 (spirit)
            int totalVIT = 0; // 활력 (vitality)

            /*** 전투 스탯 ***/
            int totalCRT = 0; // 극대,치명타 (critical hit)
            int totalDH = 0; // 직격,명중 (direct hit rate)
            int totalDET = 0; // 의지,결의 (determination)
            int totalSKS = 0; // 기술 시전 속도,물리 공격 속도 (skill speed)
            int totalSPS = 0; // 마법 시전 속도,주문 속도 (spell speed)
            int totalTEN = 0; // 불굴,인내 (tenacity)
            int totalPIE = 0; // 신앙,마나 (piety)

            /*** 방어 스탯***/
            int totalDEF = 0; // 물리 방어력 (defense)
            int totalMEF = 0; // 마법 방어력 (magic defense)

            /*** 기타 스탯***/
            int totalLUK = 0; // 운 (luck)
            #endregion

            #region 캐릭터 생성 에셋 번들 로드 및 스탯 할당
            AssetBundleCreateRequest loadCharAssetBundle = AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, "AssetBundles", "createcharacter"));
            AssetBundle charBundle = await loadCharAssetBundle.ToTask();
            if (charBundle == null)
            {
                print("에셋 번들 로드 실패");
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
                print($"직업 데이터 로드 실패: {job}");
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
                print($"종족 데이터 로드 실패: {tribe}");
                return false;
            }

            string uniqueCharacterID = System.Guid.NewGuid().ToString(); // 캐릭터 ID 난수 생성

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

            // 데이터 업로드 경로 설정: users/{userId}/{serverName}/{characterId}
            DocumentReference docRef = db.Collection("users").Document("email").Collection(email).Document(userId).Collection(serverName).Document(uniqueCharacterID);
            // Firestore에 캐릭터 데이터 업로드
            await docRef.SetAsync(newCharacter);

            #region 캐릭터 기본 장비 추가
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
            print($"캐릭터 {characterName} 생성 및 업로드 완료.");
        }
        catch (Exception e)
        {
            isCharacterCreated = false;
            print($"캐릭터 생성 중 오류 발생: {e.Message}");
        }

        return isCharacterCreated;
    }

    // 캐릭터 현재 장비 데이터 업로드
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
            print($"현재 장비 데이터 업로드 오류: {e.Message}");
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

    // 캐릭터 장비함 데이터 업로드
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
            print($"장비함 데이터 업로드 오류: {e.Message}");
        }
        return false;
    }
    #endregion

    #region 로드

    // 사용자 데이터 로드
    public async Task LoadUserData(string userId, string email, Action<Dictionary<string, object>> onCompletion)
    {
        var docRef = db.Collection("users").Document("email").Collection(email);
        var userSnapshot = await docRef.GetSnapshotAsync();
        try
        {
            if (userSnapshot == null)
            {
                print("유저 데이터 로드 실패");
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
            print($"유저 데이터 로드 중 오류 발생: {ex.Message}");
            onCompletion(null);
        }
    }

    // 캐릭터 목록 로드
    public async Task LoadCharacter(string userId, string server, Action<List<Dictionary<string, object>>> onCompletion)
    {
        var user = auth.CurrentUser;
        try
        {
            var userDocRef = db.Collection("users").Document("email").Collection(user.Email); // "users" 컬렉션에서 로그인된 사용자의 문서를 탐색
            var userSnapshot = await userDocRef.GetSnapshotAsync();

            if (userSnapshot == null)
            {
                print("사용자 정보를 찾을 수 없습니다.");
                onCompletion(null);
                return;
            }
            
            var serverCharactersRef = userDocRef.Document(userId).Collection(server); // 가져온 유저의 서버에서 캐릭터 문서를 탐색
            var querySnapshot = await serverCharactersRef.GetSnapshotAsync();

            var characters = new List<Dictionary<string, object>>();
            foreach (var document in querySnapshot.Documents)
            {
                Dictionary<string, object> character = document.ToDictionary();

                var currentEquipRef = serverCharactersRef.Document(document.Id).Collection("currentEquipped");
                var equipQuery = await currentEquipRef.GetSnapshotAsync();

                if (equipQuery == null)
                {
                    Debug.Log("정보 없음");
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

            // 캐릭터가 없는 경우 null을 반환
            if (characters.Count == 0)
            {
                onCompletion(null);
                return;
            }

            onCompletion(characters);
        }
        catch (Exception e)
        {
            print($"캐릭터 로드 중 오류 발생: {e.Message}");
            onCompletion(null);
        }
    }

    #region (구) 장비함 로드 비동기 메서드
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
            print($"장비함 데이터 로드 중 오류 발생: {e.Message}");
        }
        return false;
    }
    #endregion

    #endregion

    #region 데이터 초기화
    // 사용자 데이터 초기화
    public async Task InitializeUserData(string userId, string email, Action<bool> onCompletion)
    {
        var docRef = db.Collection("users").Document("email").Collection(email);
        var userSnapshot = await docRef.GetSnapshotAsync();

        if (userSnapshot == null)
        {
            print("사용자 데이터 초기화 실패");
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

    #region 로그아웃
    //  로그아웃
    public void SignOut()
    {
        if (auth.CurrentUser != null)
        {
            auth.SignOut();
            Debug.Log("로그아웃 성공");
        }
    }
    #endregion

    #region 문서 필드-값 존재 여부
    // 문서 필드 값 존재 여부 확인
    public void CheckFieldValueExists(string document, string userValue, System.Action<bool> onResult)
    {
        if (!IsFirebaseInitialized)
        {
            print("Firebase가 아직 초기화되지 않았음");
            onResult(false);
            return;
        }

        DocumentReference docRef = db.Collection("answer").Document(document);
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                print("문서 조회 중 오류 발생");
                onResult(false);
                return;
            }

            var documentSnapshot = task.Result;
            if (!documentSnapshot.Exists)
            {
                print("korean 문서가 존재하지 않습니다.");
                onResult(false);
                return;
            }


            if (documentSnapshot.ContainsField(userValue)) // 사용자 입력 값이 필드로 존재하는지 확인
            {
                onResult(true); // print($"필드 '{userValue}'가 존재함");
            }
            else
            {
                onResult(false); // print($"필드 '{userValue}'가 존재하지 않음);
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

#region (구) 장비함 업로드
//switch (equipmentType)
//{
//    #region 무기 장비함 업로드
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
//    #region 머리 장비함 업로드
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
//    #region 몸통 장비함 업로드
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
//    #region 손 장비함 업로드
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
//    #region 다리 장비함 업로드
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
//    #region 신발 장비함 업로드
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
//    #region 보조 도구 장비함 업로드
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
//    #region 귀걸이 장비함 업로드
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
//    #region 목걸이 장비함 업로드
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
//    #region 팔찌 장비함 업로드
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
//    #region 반지 장비함 업로드
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