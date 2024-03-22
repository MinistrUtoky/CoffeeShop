using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductButtonScript : MonoBehaviour
{
    private float moveDetectRange;
    private float timeDetectRange;
    [SerializeField] private GameObject nextMenu;
    
    public void OpenOtherMenu()
    {
        if (Input.touchCount > 0)
        {

            if (Input.GetTouch(0).phase!=TouchPhase.Moved)
            {
                Debug.Log("Ty tyknul eblan");
            }
        }
    }
}
