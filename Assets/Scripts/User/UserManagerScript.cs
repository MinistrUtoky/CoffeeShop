using System;
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
        private void Awake()
        {
            Instance = this;
        }
        public void Login(TMP_InputField loginField, TMP_InputField passwordField, string currency="Euro")
        {
            if (DatabaseManager.Instance.AreLoginAndPasswordRight(loginField.text, passwordField.text))
            {
                PageManagerScript.Instance.SwitchFromTechPagesToUsables();
                currentUser.login = loginField.text;
                currentUser.password = passwordField.text;
                currentUser.currency = currency;
                OnLogin?.Invoke();
            }
            else
                Debug.Log("Such user does not exist");
        }
        public void Register(TMP_InputField loginField, TMP_InputField emailField, TMP_InputField passwordField, UserType userType=UserType.Buyer, string currency= "Euro")
        {
            string errorMessage = "";
            UserData user = new UserData()
            {
                login = loginField.text,
                email = emailField.text,
                password = passwordField.text,
                type = userType,
                currency = currency
            };
            string basicDisplayName;
            if (DatabaseManager.Instance.TryAddUser(user, out errorMessage, out basicDisplayName))
            {
                user.displayName = basicDisplayName;
                Login(loginField, passwordField);
            }
            else
                Debug.Log(errorMessage);
        }
        public UserData GetCurrentUser()
        {
            return currentUser;
        }
    }
}