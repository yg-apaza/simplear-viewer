using Firebase.Database;

public class ResourceModel
{
    public string id;
    public string name;
    public string content;
    public string thumbnail;
    public string type;

    public ResourceModel(string id, string name, string content, string thumbnail, string type)
    {
        this.id = id;
        this.name = name;
        this.content = content;
        this.thumbnail = thumbnail;
        this.type = type;
    }

    public static ResourceModel FromDataSnapshot(DataSnapshot snapshot) {
        string id = (string)snapshot.Child("id").Value;
        string name = (string)snapshot.Child("name").Value;
        string content = (string)snapshot.Child("content").Value;
        string thumbnail = (string)snapshot.Child("thumbnail").Value;
        string type = (string)snapshot.Child("type").Value;

        return new ResourceModel(id, name, content, thumbnail, type);
    }
}