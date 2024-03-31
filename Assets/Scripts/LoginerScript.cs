using TMPro;
using UnityEngine;

namespace UserManagement
{
    public class LoginerScript : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField loginField;
        [SerializeField]
        private TMP_InputField passwordField;
        public void Login() => UserManagerScript.Instance.Login(loginField, passwordField);        
    }
}