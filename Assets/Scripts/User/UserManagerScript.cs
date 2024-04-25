using UnityEngine;
using TMPro;
using PageManagement;
using UnityEngine.Events;
using static Assets.Scripts.Database.DataStructures;

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
                PageManagerScript.Instance.UpdateMainPageProductButtons();
                PageManagerScript.Instance.SwitchFromTechPagesToUsables();
                foreach (string s in currentUser.ToList())
                    Debug.Log(s);
                OnLogin?.Invoke();
                return "";
            }
            else
                return "Such user does not exist";
        }
        public string Register(TMP_InputField loginField, TMP_InputField emailField, TMP_InputField passwordField, TMP_Dropdown userType, string currency="Euro")
        {
            string errorMessage;
            UserData user = new UserData()
            {
                login = loginField.text,
                email = emailField.text,
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
        public UserData GetCurrentUser()
        {
            return currentUser;
        }
    }
}