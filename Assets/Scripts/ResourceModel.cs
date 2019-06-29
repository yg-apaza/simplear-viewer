[System.Serializable]
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
}