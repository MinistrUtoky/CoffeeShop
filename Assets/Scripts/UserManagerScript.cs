using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManagerScript : MonoBehaviour
{
    public static UserManagerScript Instance { get; private set; }
    [Serializable]
    enum UserType
    {
        Buyer,
        Seller,
        Moderator 
    }
    [Serializable] //temp
    struct User {
        public string displayName;
        public string login;
        public string password; // :D
        public UserType type;
        public float currentBalance; // in some gp idk
    }
    [SerializeField] // temp
    private User currentUser = new User
    {
        displayName = "Nataniel",
        login = "SmokeDonutsBananaPizza",
        password = "not_a_password",
        type = UserType.Buyer
        
    };

    private void Awake()
    {
        Instance = this;
    }
}
