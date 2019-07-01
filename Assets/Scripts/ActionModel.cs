[System.Serializable]
public class ActionModel
{
    public string type;
    public string[] inputs;

    public ActionModel(string type, string[] inputs)
    {
        this.type = type;
        this.inputs = inputs;
    }
}