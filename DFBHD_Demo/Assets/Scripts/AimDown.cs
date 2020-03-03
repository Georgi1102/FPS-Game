using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimDown : MonoBehaviour
{
    public Vector3 aimDown; // weapon aim position 
    public Vector3 hipFire;//current weapon position
     
    void Update()
    {
        Aim();
    }

    #region AimLogic
    public bool Aim()
    {
        //checks if we are holding down the mouse button
        if (Input.GetKey(KeyCode.Mouse1))
        {
            // Debug.Log("ButtonPresed");
            transform.localPosition = Vector3.Slerp(transform.localPosition, aimDown, 10 * Time.deltaTime);
            return true;
        }
        //we are not holding the button
        else
        {
            // Debug.Log("ButtonIsNotPresed");
            transform.localPosition = Vector3.Slerp(transform.localPosition, hipFire, 10 * Time.deltaTime);
            return false;
        }
    }
    #endregion

}
