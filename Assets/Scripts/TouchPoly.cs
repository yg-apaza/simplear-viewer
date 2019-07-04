using UnityEngine;

public class TouchPoly : MonoBehaviour
{
    public ComponentModel component;
    public ActionModel[] actions;
    public int cod = 1;
    public float angle = 90;

    void Update()
    {
        /*
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                Debug.Log("Something Hit");
                if (raycastHit.collider.name == name)
                {
                    Debug.Log("Soccer Ball clicked");
                    VuforiaController.DoActions(component, actions);
                }
            }
        }*/
        
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(VuforiaController.DoActions(component, actions));
        }
    }
}
