using TMPro;
using UnityEngine;

namespace UserManagement
{
    public class LoginerScript : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField loginField;
        [SerializeField]
        private TMP_InputField emailFieldIfExists;
        [SerializeField]
        private TMP_InputField passwordField;
        private void Start()
        {
            loginField.lineType = TMP_InputField.LineType.SingleLine;
            if (emailFieldIfExists != null)
            {
                emailFieldIfExists.contentType = TMP_InputField.ContentType.EmailAddress;
                emailFieldIfExists.lineType = TMP_InputField.LineType.SingleLine;
            }
            passwordField.contentType = TMP_InputField.ContentType.Password;
            passwordField.lineType = TMP_InputField.LineType.SingleLine;
        }
        public void Login() => UserManagerScript.Instance.Login(loginField, passwordField);
        public void Register() => UserManagerScript.Instance.Register(loginField, emailFieldIfExists, passwordField);
       
    }
}