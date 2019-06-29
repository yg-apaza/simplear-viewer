using Firebase.Database;

[System.Serializable]
public class ComponentModel
{
    public string id;
    public ResourceModel[] inputs;
    public string workspace;
    public string configurationSerialized;
    public Configuration configuration;
    public string type;

    public ComponentModel(string id, ResourceModel[] inputs, string workspace, string configurationSerialized, Configuration configuration, string type)
    {
        this.id = id;
        this.inputs = inputs;
        this.workspace = workspace;
        this.configurationSerialized = configurationSerialized;
        this.configuration = configuration;
        this.type = type;
    }

    public static ComponentModel FromDataSnapshot(DataSnapshot snapshot)
    {
        string id = (string) snapshot.Child("id").Value;
        ResourceModel[] inputs = (ResourceModel[]) snapshot.Child("inputs").Value;
        string workspace = (string)snapshot.Child("workspace").Value;
        string configurationSerialized = (string) snapshot.Child("configuration").Value;
        Configuration configuration = new Configuration(new ActionModel("rotation", new object[] {}));
        string type = (string)snapshot.Child("type").Value;

        return new ComponentModel(id, inputs, workspace, configurationSerialized, configuration, type);
    }
}