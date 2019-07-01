using Firebase.Database;
using System.Collections.Generic;
using UnityEngine;

public class ComponentModel {
    public string id;
    public List<ResourceModel> inputs;
    public string workspace;
    public string configurationSerialized;
    public Configuration configuration;
    public string type;

    public ComponentModel(string id, List<ResourceModel> inputs, string workspace, string configurationSerialized, Configuration configuration, string type) {
        this.id = id;
        this.inputs = inputs;
        this.workspace = workspace;
        this.configurationSerialized = configurationSerialized;
        this.configuration = configuration;
        this.type = type;
    }

    public static ComponentModel FromDataSnapshot(DataSnapshot snapshot) {
        string id = (string) snapshot.Child("id").Value;
        List<ResourceModel> inputs = new List<ResourceModel>();
        DataSnapshot inputsSnapshot = snapshot.Child("inputs");
        foreach (DataSnapshot resourceSnapshot in inputsSnapshot.Children) {
            inputs.Insert(0, ResourceModel.FromDataSnapshot(resourceSnapshot));
        }
        string workspace = (string) snapshot.Child("workspace").Value;
        string configurationSerialized = (string) snapshot.Child("configuration").Value;
        Configuration configuration = new Configuration();
        JsonUtility.FromJsonOverwrite(configurationSerialized, configuration);
        string type = (string) snapshot.Child("type").Value;
        return new ComponentModel(id, inputs, workspace, configurationSerialized, configuration, type);
    }
}