using Firebase;
using Firebase.Auth;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    public FirebaseAuth auth { get; private set; }

    void Awake()
    {
        GameManager.instance.firebaseManager = this;
    }

    public void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Exception);
                return;
            }
            auth = FirebaseAuth.DefaultInstance;
        });
    }
}
