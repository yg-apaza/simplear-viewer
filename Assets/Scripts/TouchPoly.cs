using UnityEngine;
using UnityEngine.UI;

public class TouchPoly : MonoBehaviour
{
    public ComponentModel component;
    public ActionModel[] actions;

    void Update()
    {
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                if (raycastHit.collider.name == name)
                {
                    StartCoroutine(VuforiaController.DoActions(component, actions));
                }
            }
        }
        
    }
}
