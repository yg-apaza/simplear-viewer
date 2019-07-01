﻿using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

// TODO: Hide API keys for Poly and Vuforia
// TODO: Organize within namespaces each class
public class VuforiaController : MonoBehaviour {

    private DatabaseReference componentsReference;
    private Dictionary<string, TrackableBehaviour> predefinedNaturalMarkers = new Dictionary<string, TrackableBehaviour>();
    private Dictionary<string, ComponentModel> components = new Dictionary<string, ComponentModel>();

    void Start() {
        // TODO: The Firebase Unity SDK for Android requires Google Play services, which must be up-to-date before the SDK can be used.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://simplear.firebaseio.com");
        componentsReference = FirebaseDatabase.DefaultInstance.GetReference("details/" + GlobalData.projectId + "/components");
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(SetupTrackables);
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(LoadComponents);
    }

    // Setup Vuforia markers before loading project
    private void SetupTrackables()
    {
        Debug.Log("SetupTrackables*********");
        // TODO: Avoid to load all trackables
        ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        DataSet dataSet = objectTracker.CreateDataSet();

        if (dataSet.Load("SimpleARDefaultMarkers")) {
            objectTracker.Stop();

            if (!objectTracker.ActivateDataSet(dataSet)) {
                // Note: ImageTracker cannot have more than 1000 total targets activated
                Debug.Log("<color=yellow>Failed to Activate DataSet: SimpleARDefaultMarkers</color>");
            }

            if (!objectTracker.Start()) {
                Debug.Log("<color=yellow>Tracker Failed to Start.</color>");
            }

            IEnumerable<TrackableBehaviour> tbs = TrackerManager.Instance.GetStateManager().GetTrackableBehaviours();
            foreach (TrackableBehaviour tb in tbs) {
                Debug.Log("ADD TRACKACBLE" + tb.TrackableName);
                predefinedNaturalMarkers.Add(tb.TrackableName, tb);
            }
        }
    }

    // Read components from Firebase and apply their configuration
    private void LoadComponents() {
        Debug.Log("LoadComponents11111111*********");
        componentsReference.GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted) {
                // TODO: Handle the error...
            } else if (task.IsCompleted) {
                // TODO: Load trackables only if Vuforia markers are going to be used
                Debug.Log("LoadComponents2222222222222*********");
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot componentSnapshot in snapshot.Children) {
                    ComponentModel component = ComponentModel.FromDataSnapshot(componentSnapshot);
                    components.Add(componentSnapshot.Key, component);

                    switch(component.type) {
                        case "augment_marker":
                            LoadComponent_AugmentMarker(component);
                            break;
                        default:
                            break;
                    }
                }

            }
        });
    }

    private void LoadComponent_AugmentMarker(ComponentModel component)
    {
        CreateResource(component.inputs[0]);
        //CreateResource(component.inputs[1]);
    }

    private GameObject CreateResource(ResourceModel resource)
    {
        // TODO: Use static strings or enums
        switch (resource.type)
        {
            case "marker":
                return CreateResource_Marker(resource);
            case "poly":
                return CreateResource_Poly(resource);
            default:
                return null;
        }
    }

    private GameObject CreateResource_Marker(ResourceModel resource)
    {
        Debug.Log("CREATE RESOURCE MARKERS");
        Debug.Log(resource.content);
        Debug.Log(predefinedNaturalMarkers.Count);
        TrackableBehaviour tb = predefinedNaturalMarkers[resource.content];
        Debug.Log("PASEEEEEEEEEEEEEEEEEEEE");
        Debug.Log(tb.gameObject);
        
        //tb.gameObject.name = "DynamicImageTarget-" + tb.TrackableName;
        //tb.gameObject.AddComponent<DefaultTrackableEventHandler>();
        //tb.gameObject.AddComponent<TurnOffBehaviour>();
        Debug.Log("WAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        return null;
        //resources.Add(resource.id, resource);
        //gameObjectresources.Add(resource.id, tb.gameObject);
    }

    private GameObject CreateResource_Poly(ResourceModel resource)
    {
        PolyUtil.GetPolyResult(resource.content, (polyResult) =>
        {
            //return polyResult;
        });
        return null;
    }
}
