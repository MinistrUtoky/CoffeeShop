using CorvusEnLignumDBSolutionsIncorporated;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Database.DataStructures;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }
    private static string dbName = "mainDB.mdf";
    void Start()
    {
        Instance = this;
        MSSQLServerConnector.cn_String = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + string.Join("\\", Application.dataPath.Split("/")) + "\\Data\\" + dbName + ";Integrated Security=True;Connect Timeout=30";

        foreach (string d in MSSQLServerConnector.GetColumnNames("app_users"))
            Debug.Log(d); // just testing db to see that it works        
    }

    public bool TryAddUser(UserData newUser, out string errorMessage, out string basicDisplayname)
    {
        errorMessage = "";
        byte[] pData = System.Text.Encoding.ASCII.GetBytes(newUser.password);
        pData = new System.Security.Cryptography.SHA256Managed().ComputeHash(pData);
        string passwordHash = System.Text.Encoding.ASCII.GetString(pData);
        basicDisplayname = "User" + MSSQLServerConnector.GetNextId("app_users");
        if (DoesUserExist(newUser.login, passwordHash))
        {
            errorMessage = "User with such name already exists";
            return false;
        }
        newUser.displayName = basicDisplayname;
        foreach (string s in newUser.ToList())
        {
            Debug.Log(s);
        }
        MSSQLServerConnector.DBInsert("app_users", newUser.ToList());
        return true;
    }
    private bool DoesUserExist(string login, string password)
    {
        if (MSSQLServerConnector.GetDataTable("SELECT * FROM [app_users] WHERE user_login='" + login + "'").Rows.Count == 0)
            return false;
        else
            return true;
    }
    public bool AreLoginAndPasswordRight(string login, string password)
    {
        if (MSSQLServerConnector.GetDataTable("SELECT * FROM [app_users] WHERE user_login='" + login + "' AND user_password='" + password + "'").Rows.Count == 0)
            return false;
        else
            return true;
    }
}

/*
 * add_users seed
 * MSSQLServerConnector.CreateNewTable("app_users", new List<string>() 
                                                    {
                                                                "display_name", "user_login", "email",
                                                                "user_password", "user_type", "home_currency" 
                                                    },
                                                    new List<string>()
                                                    {
                                                        "varchar(45)", "varchar(45)", "varchar(256)", "varchar(512)", "tinyint", "varchar(20)"
                                                    },
                                                    new List<string>() { "", "", "", "", "", "" });    
 */