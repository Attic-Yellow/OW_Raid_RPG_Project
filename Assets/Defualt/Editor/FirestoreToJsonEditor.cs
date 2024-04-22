#if UNITY_EDITOR
using UnityEditor;
using Firebase.Firestore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Firebase.Extensions;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class FirestoreToJsonEditor : EditorWindow
{
    // ���� ID ����� �����մϴ�.
    private static readonly string[] collections = { "createCharacter", "createCharacterJob", "ItemData" };
    private static readonly string[] tribes = { "Human", "Elf", "Dwarf" };
    private static readonly string[] jobs = { "Warrior", "Dragoon", "Bard", "WhiteMage", "BlackMage" };
    private static readonly string[] itemData = { "EquipmentData", "ConsumableData" };


    [MenuItem("Firestore/Export Data to JSON")]
    public static async void ShowWindow() // �񵿱� �޼���� ����
    {
        EditorWindow.GetWindow(typeof(FirestoreToJsonEditor));
        foreach (var collection in collections)
        {
            if (collection == "createCharacter")
            {
                foreach (var document in tribes)
                {
                    await ExportFirestoreDataToJson(collection, document); // �񵿱� �޼��� ȣ�� �� await ���
                }
            }
            else if (collection == "createCharacterJob")
            {
                foreach (var document in jobs)
                {
                    await ExportFirestoreDataToJson(collection, document); 
                }
            }
            else if (collection == "ItemData")
            {
                foreach (var document in itemData)
                {
                    await ExportFirestoreDataToJson(collection, document); 
                }
            }
        }
    }

    private static async Task ExportFirestoreDataToJson(string collection, string document)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        if (collection != "ItemData")
        {
            DocumentSnapshot snapshot = await db.Collection(collection).Document(document).GetSnapshotAsync();
            if (!snapshot.Exists)
            {
                UnityEngine.Debug.Log($"Document does not exist: {document}");
                return;
            }

            Dictionary<string, object> documentData = snapshot.ToDictionary();
            string json = JsonConvert.SerializeObject(documentData, Formatting.Indented);

            string folderPath = Path.Combine(Application.dataPath, "createCharacter");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string filePath = Path.Combine(folderPath, $"{document}.json");
            File.WriteAllText(filePath, json);

            UnityEngine.Debug.Log($"Data exported to {filePath}");
        }
        else
        {
            QuerySnapshot snapshot = await db.Collection(collection).Document(document).Collection("ItemID").GetSnapshotAsync();
            if (snapshot.Count == 0)
            {
                UnityEngine.Debug.Log($"No data found in sub-collection of '{document}'");
                return;
            }

            Dictionary<string, Dictionary<string, object>> documentsData = new Dictionary<string, Dictionary<string, object>>();
            foreach (DocumentSnapshot docSnapshot in snapshot.Documents)
            {
                if (docSnapshot.Exists)
                {
                    string docId = docSnapshot.Id;
                    Dictionary<string, object> documentData = docSnapshot.ToDictionary();
                    documentsData[docId] = documentData;
                }
            }

            // ��ųʸ��� JSON���� ����ȭ�մϴ�.
            string json = JsonConvert.SerializeObject(documentsData, Formatting.Indented);
            string folderPath = Path.Combine(Application.dataPath, "DataFolder");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string filePath = Path.Combine(folderPath, $"{document}.json");
            File.WriteAllText(filePath, json);
            UnityEngine.Debug.Log($"All ItemID data exported to {filePath}");
        }

        AssetDatabase.Refresh(); // Unity �����Ͱ� �� ������ �ν��ϵ��� ����
    }
}
#endif
