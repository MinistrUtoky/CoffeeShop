using System;
using UnityEngine;
using TMPro;
using PageManagement;
using UnityEngine.Events;

namespace UserManagement {
    public class UserManagerScript : MonoBehaviour
    {
        public static UserManagerScript Instance { get; private set; }
        [HideInInspector]

        public UnityEvent OnLogin;
        
        [Serializable]
        public enum UserType
        {
            Buyer,
            Seller,
            Moderator 
        }

        public struct User {
            public string displayName;
            public string login; // gonna be just checked with db and proceed
            public string email;
            public string password; // same as login :D  
            public UserType type;
            public float currentBalance; // in some gp idk
            public string currency;
        }

        private User currentUser = new User
        {
            displayName = "Nataniel", //from db
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
            PageManagerScript.Instance.SwitchFromTechPagesToUsables();
            currentUser.login = loginField.text;
            currentUser.password = passwordField.text;
            currentUser.currency = currency;
            OnLogin?.Invoke();
        }
        public User GetCurrentUser()
        {
            return currentUser;
        }
    }
}