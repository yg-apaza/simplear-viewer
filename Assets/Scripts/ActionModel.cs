[System.Serializable]
public class ActionModel
{
    public string type;
    public object[] inputs;

    public ActionModel(string type, object[] inputs)
    {
        this.type = type;
        this.inputs = inputs;
    }
}