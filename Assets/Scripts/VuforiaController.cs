using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Vuforia;

// TODO: Hide API keys for Poly and Vuforia
// TODO: Organize within namespaces each class
public class VuforiaController : MonoBehaviour {

    private DatabaseReference componentsReference;
    private Dictionary<string, TrackableBehaviour> inactiveMarkers = new Dictionary<string, TrackableBehaviour>();
    private static Dictionary<string, GameObject> markers = new Dictionary<string, GameObject>();
    private static Dictionary<string, GameObject> polys = new Dictionary<string, GameObject>();
    //private Dictionary<string, ComponentModel> components = new Dictionary<string, ComponentModel>();
    public delegate void GetResourceCallback(GameObject resource);

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
                inactiveMarkers.Add(tb.TrackableName, tb);
            }
        }
    }

    // Read components from Firebase and apply their configuration
    private void LoadComponents()
    {
        componentsReference.GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted) {
                // TODO: Handle the error...
            } else if (task.IsCompleted) {
                // TODO: Load trackables only if Vuforia markers are going to be used
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot componentSnapshot in snapshot.Children) {
                    ComponentModel component = ComponentModel.FromDataSnapshot(componentSnapshot);
                    //components.Add(componentSnapshot.Key, component);
                    switch(component.type) {
                        case "augment_marker":
                            LoadComponent_AugmentMarker(component);
                            break;
                        default:
                            break;
                    }
                }

            }
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    private void LoadComponent_AugmentMarker(ComponentModel component)
    {
        CreateResource(component.inputs[1], polyGameObject => {
            CreateResource(component.inputs[0], markerGameObject => {

                // Augment Poly 3D Model over marker
                polyGameObject.SetActive(true);
                polyGameObject.transform.parent = markerGameObject.transform;
                if (component.configuration.touch_poly != null)
                {
                    // TODO: Send only component
                    TouchPoly tp = polyGameObject.AddComponent<TouchPoly>();
                    tp.component = component;
                    tp.actions = component.configuration.touch_poly;
                }
            });
        });
    }

    private void CreateResource(ResourceModel resource, GetResourceCallback callback)
    {
        // TODO: Use static strings or enums
        switch (resource.type)
        {
            case "marker":
                CreateResource_Marker(resource, callback);
                break;
            case "poly":
                CreateResource_Poly(resource, callback);
                break;
            default:
                break;
        }
    }

    private void CreateResource_Marker(ResourceModel resource, GetResourceCallback callback)
    {
        TrackableBehaviour tb = inactiveMarkers[resource.content];
        tb.gameObject.name = resource.name;
        tb.gameObject.AddComponent<DefaultTrackableEventHandler>();
        tb.gameObject.AddComponent<TurnOffBehaviour>();
        //tb.GetComponent<DefaultTrackableEventHandler>().OnTrackableStateChanged(TrackableBehaviour.Status.TRACKED, TrackableBehaviour.Status.NO_POSE);
        markers.Add(tb.gameObject.name, tb.gameObject);
        callback(tb.gameObject);
    }

    private void CreateResource_Poly(ResourceModel resource, GetResourceCallback callback)
    {
        PolyUtil.GetPolyResult(resource.content, (polyResult) =>
        {
            polyResult.name = resource.name;
            polys.Add(polyResult.name, polyResult);
            callback(polyResult);
        });
    }

    public static IEnumerator DoActions(ComponentModel component, ActionModel[] actions)
    {
        GameObject marker = markers[component.inputs[0].name];
        GameObject poly = polys[component.inputs[1].name];
        
        foreach (ActionModel action in actions)
        {
            switch (action.type)
            {
                case "rotation":
                    int angle = Int32.Parse(action.inputs[0]);
                    char axis = action.inputs[1][0];
                    bool direction = action.inputs[2].Equals("clock") ? true : false;
                    Rotate(poly, angle, axis, direction);
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private static void Rotate(GameObject gameObject, int angle, char axis, bool direction)
    {
        if(axis == 'x' && direction)
        {
            gameObject.transform.Rotate(Vector3.left * angle);
        }
        else if (axis == 'x' && !direction)
        {
            gameObject.transform.Rotate(Vector3.right * angle);
        }
        else if (axis == 'y' && direction)
        {
            gameObject.transform.Rotate(Vector3.up * angle);
        }
        else if (axis == 'y' && !direction)
        {
            gameObject.transform.Rotate(Vector3.down * angle);
        }
        else if (axis == 'z' && direction)
        {
            gameObject.transform.Rotate(Vector3.forward * angle);
        }
        else if (axis == 'z' && !direction)
        {
            gameObject.transform.Rotate(Vector3.back * angle);
        }

        
    }
}
