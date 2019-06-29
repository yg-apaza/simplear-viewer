using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections.Generic;
using UnityEngine;

// TODO: Hide API keys for Poly and Vuforia
// TODO: Organize within namespaces each class
public class VuforiaController : MonoBehaviour
{
    private Dictionary<string, Component> components = new Dictionary<string, Component>();
    private DatabaseReference componentsReference;

    void Start() {
        // TODO: The Firebase Unity SDK for Android requires Google Play services, which must be up-to-date before the SDK can be used.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://simplear.firebaseio.com");
        Debug.Log(GlobalData.projectId);
        componentsReference = FirebaseDatabase.DefaultInstance.GetReference("details/" + GlobalData.projectId).Child("components");
        LoadComponents();
    }

    private void LoadComponents() {
        componentsReference.GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted) {
                // TODO: Handle the error...
            } else if (task.IsCompleted) {
                DataSnapshot snapshot = task.Result;
                Debug.Log("COUNT: " + task.Result.ChildrenCount);
                ComponentModel com = ComponentModel.FromDataSnapshot(task.Result);
                Debug.Log(com.id);
            }
        });
    }
}
