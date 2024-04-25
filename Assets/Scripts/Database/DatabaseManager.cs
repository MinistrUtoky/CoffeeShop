using CorvusEnLignumDBSolutionsIncorporated;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using static Assets.Scripts.Database.DataStructures;

public class DatabaseManager : MonoBehaviour
{
    private static bool initiated = false;
    public static void Initiate(string dataPath)
    {
        MSSQLServerConnector.cn_String = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + dataPath + ";Integrated Security=True;Connect Timeout=30";

        foreach (string d in MSSQLServerConnector.GetColumnNames("app_users"))
            Debug.Log(d); // just testing db to see that it works        
        initiated = true;
    }

    public static bool TryAddUser(UserData newUser, out string errorMessage, out string basicDisplayname)
    {
        if (!initiated)
        {
            errorMessage = "Database not initiated";
            basicDisplayname = "";
            return false;
        }
        errorMessage = "";
        byte[] pData = System.Text.Encoding.ASCII.GetBytes(newUser.password);
        pData = new System.Security.Cryptography.SHA256Managed().ComputeHash(pData);
        string passwordHash = System.Text.Encoding.ASCII.GetString(pData);
        newUser.password = passwordHash;
        basicDisplayname = "User" + MSSQLServerConnector.GetNextId("app_users");
        if (DoesUserExist(newUser.login))
        {
            errorMessage = "User with such name already exists";
            return false;
        }
        newUser.displayName = basicDisplayname;
        MSSQLServerConnector.DBInsert("app_users", newUser.ToList());
        return true;
    }
    private static bool DoesUserExist(string login)
    {
        string sqlQuery = "SELECT * FROM [app_users] WHERE user_login='" + login + "'";
        if (MSSQLServerConnector.GetDataTable(sqlQuery).Rows.Count == 0)
            return false;
        else
            return true;
    }
    private static bool AreLoginAndPasswordRight(string login, string password, out DataTable table)
    {
        string sqlQuery = "SELECT * FROM [app_users] WHERE user_login='" + login + "' AND user_password='" + password + "'";
        Debug.Log(sqlQuery);
        if ((table=MSSQLServerConnector.GetDataTable(sqlQuery)).Rows.Count == 0)
            return false;
        else
            return true;
    }
    public static bool TryFindUser(string login, string password, out UserData user)
    {
        if (!initiated)
        {
            user = new UserData();
            return false;
        }
        DataTable table;
        user = new UserData();
        byte[] pData = System.Text.Encoding.ASCII.GetBytes(password);
        pData = new System.Security.Cryptography.SHA256Managed().ComputeHash(pData);
        string passwordHash = System.Text.Encoding.ASCII.GetString(pData);
        if (AreLoginAndPasswordRight(login, passwordHash, out table))
        {
            List<string> userData = new List<string>(){"0"};
            foreach (object? item in table.Rows[0].ItemArray)
            {
                userData.Add(item?.ToString());
            }
            user = (UserData)user.ToData(userData);
            return true;
        }
        else
            return false;
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