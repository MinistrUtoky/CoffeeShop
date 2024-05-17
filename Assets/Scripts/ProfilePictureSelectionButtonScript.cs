using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UserManagement;

public class ProfilePictureSelectionButtonScript : MonoBehaviour
{
    public void SetCustomProfilePicture()
    {
        UserManagerScript.Instance.SetPictureId(this.transform.GetSiblingIndex());
    }
}
