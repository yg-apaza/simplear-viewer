using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections.Generic;
using UnityEngine;

// TODO: Hide API keys for Poly and Vuforia
// TODO: Organize within namespaces each class
public class VuforiaController : MonoBehaviour {
    private Dictionary<string, ComponentModel> components = new Dictionary<string, ComponentModel>();
    private DatabaseReference componentsReference;

    void Start() {
        // TODO: The Firebase Unity SDK for Android requires Google Play services, which must be up-to-date before the SDK can be used.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://simplear.firebaseio.com");
        componentsReference = FirebaseDatabase.DefaultInstance.GetReference("details/" + GlobalData.projectId + "/components");
        LoadComponents();
    }

    private void LoadComponents() {
        componentsReference.GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted) {
                // TODO: Handle the error...
            } else if (task.IsCompleted) {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot component in snapshot.Children) {
                    components.Add(component.Key, ComponentModel.FromDataSnapshot(component));
                }
            }
        });
    }
}
