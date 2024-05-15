using UnityEngine;
using TMPro;
using PageManagement;
using UnityEngine.Events;
using static Assets.Scripts.Database.DataStructures;
using System.Text.RegularExpressions;
using static TMPro.TMP_InputField;

namespace UserManagement {
    public class UserManagerScript : MonoBehaviour
    {
        public static UserManagerScript Instance { get; private set; }        
        [HideInInspector]
        public UnityEvent OnLogin;
        
        private UserData currentUser = new UserData
        {
            displayName = "Nataniel", //from db
            email = "MisterTwister@rambler.ru",
            login = "SmokeDonutsBananaPizza",
            password = "not_a_password",
            type = UserType.Buyer     //from db
        };
        public UserData CurrentUser { get { return currentUser; } }
        private void Awake()
        {
            Instance = this;
        }
        public string Login(TMP_InputField loginField, TMP_InputField passwordField, string currency="Euro")
        {
            if (DatabaseManager.TryFindUser(loginField.text, passwordField.text, out currentUser))
            {
                currentUser.currency = currency;
                PageManagerScript.Instance.UpdateMainPage();
                if (currentUser.type == UserType.Moderator)
                    PageManagerScript.Instance.FillApprovalPage();
                PageManagerScript.Instance.ShowNavPanelAccordingToUserType(currentUser.type);
                PageManagerScript.Instance.SwitchFromTechPagesToUsables();
                foreach (string s in currentUser.ToList())
                    Debug.Log(s);
                OnLogin?.Invoke();
                return "";
            }
            else
                return "Such user does not exist";
        } 

        public string Register(TMP_InputField loginField, TMP_InputField emailOrPhoneField, TMP_InputField passwordField, TMP_Dropdown userType, string currency="Euro")
        {
            string errorMessage;
            if (!IsLoginOkay(loginField.text))
                return "Login length should exceed 6 symbols";
            if (!IsEmailOkay(emailOrPhoneField.text) & !IsPhoneOkay(emailOrPhoneField.text))
                return "Inacceptable e-mail or phone format";
            if (!IsPasswordOkay(passwordField.text))
                return "Password length should exceed 6 symbols, contain at least one digit, one upper and lower letters and a special symbol:" +
                        "\n #, ?, !, @, $, %, ^, &, * or -";
            UserData user = new UserData()
            {
                login = loginField.text,
                email = emailOrPhoneField.text,
                password = passwordField.text,
                type = (UserType)userType.value,
                currency = currency
            };
            string basicDisplayName;
            if (DatabaseManager.TryAddUser(user, out errorMessage, out basicDisplayName))
            {
                user.displayName = basicDisplayName;
                Login(loginField, passwordField);
                return "";
            }
            else
                return errorMessage;
        }
        private bool IsEmailOkay(string email)
        {
            return Regex.IsMatch(email, "^(?(\")(\".+?(?<!\\\\)\"@)|(([0-9a-z]((\\.(?!\\.))|[-!#\\$%&'\\*\\+/=\\?\\^`{}|~\\w])*)(?<=[0-9a-z])@))(?([)([(\\d{1,3}.){3}\\d{1,3}])|(([0-9a-z][-0-9a-z]*[0-9a-z]*.)+[a-z0-9][-a-z0-9]{0,22}[a-z0-9]))$");
        }
        private bool IsPhoneOkay(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, "\\(?\\d{3}\\)?-? *\\d{3}-? *-?\\d{4}");
        }
        private bool IsPasswordOkay(string password)
        {
            Debug.Log(Regex.IsMatch(password, "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{7,}$"));
            return password.Length > 6                    
                   & Regex.IsMatch(password, "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{7,}$");
        } 
        private bool IsLoginOkay(string login)
        {
            return login.Length > 6;
        }
        public UserData GetCurrentUser()
        {
            return currentUser;
        }
        public void ShowOrHidePassword(TMP_InputField passwordField)
        {
            if (passwordField.contentType == ContentType.Password) passwordField.contentType = ContentType.Standard;
            else passwordField.contentType = ContentType.Password;
            passwordField.ForceLabelUpdate();
        }
    }
}