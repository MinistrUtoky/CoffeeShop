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
        [SerializeField]
        private TMP_Dropdown userTypeDropdownIfExists;
        [SerializeField]
        private TMP_Text errorField;
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
        public void Login()
        {
            errorField.text = UserManagerScript.Instance.Login(loginField, passwordField);
        }

        public void Register()
        {
            errorField.text = UserManagerScript.Instance.Register(loginField, emailFieldIfExists, passwordField, userTypeDropdownIfExists);
            Vector4 errorFieldMargins = errorField.margin;
            errorField.ForceMeshUpdate();
            errorFieldMargins.w = -errorField.GetRenderedValues(true).y;
            errorField.margin = errorFieldMargins;
        }
    }
}
