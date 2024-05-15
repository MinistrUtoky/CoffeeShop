using CorvusEnLignumDBSolutionsIncorporated;
using System;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using static Assets.Scripts.Database.DataStructures;
using Debug = UnityEngine.Debug;

public class DatabaseManager : MonoBehaviour
{
    private static bool initiated = false;
    private void OnApplicationQuit()
    {
        MSSQLServerConnector.CloseDBConnection();
    }
    public static void Initiate()//string dataPath)
    {
        MSSQLServerConnector.cn_String = "Data Source=sql.bsite.net\\MSSQL2016;Persist Security Info=True;User ID=mutoky_;Password=123;";//"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + dataPath + ";Integrated Security=True;Connect Timeout=30;Initial Catalog=mainDB;";
        Debug.Log(MSSQLServerConnector.cn_String);

        foreach (string d in MSSQLServerConnector.GetColumnNames("approvables"))
            Debug.Log(d); // just testing db to see that it works       
        initiated = true;
    }

    public static bool TryAddUser(UserData newUser, out string errorMessage, out string basicDisplayname)
    {
        if (!initiated)
        {
            errorMessage = "Database not initiated " + MSSQLServerConnector.cn_String;
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
        if ((table = MSSQLServerConnector.GetDataTable(sqlQuery)).Rows.Count == 0)
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
            List<string> userData = new List<string>() { "0" };
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
    public static bool TryAddProduct(ProductData product) => TryAddProductToTable(product, "approvables");
    public static bool TryRetrieveAllProducts(out List<ProductData> allProducts) => TryRetrieveProductsFromTable(out allProducts, "all_products");

    public static bool TryApproveProduct(ProductData product)
    {
        MSSQLServerConnector.DBRemove("approvables", product.id);
        return TryAddProductToTable(product, "all_products");
    }
    public static bool TryRetrieveApprovables(out List<ProductData> approvables) => TryRetrieveProductsFromTable(out approvables, "approvables");

    private static bool TryRetrieveProductsFromTable(out List<ProductData> products, string tableName)
    {
        try
        {
            string sqlQuery = "SELECT * FROM [" + tableName + "]";
            DataTable table = MSSQLServerConnector.GetDataTable(sqlQuery);
            products = new List<ProductData>();
            foreach (DataRow row in table.Rows)
            {
                List<string> productData = new List<string>();
                foreach (var item in row.ItemArray)
                    productData.Add(item?.ToString());
                ProductData product = new ProductData();
                product = (ProductData)product.ToData(productData);
                products.Add(product);
            }
            return true;
        }
        catch
        {
            products = new List<ProductData>();
            return false;
        }
    }
    private static bool TryAddProductToTable(ProductData product, string tableName)
    {
        try
        {
            Debug.Log("Inserting ProductData");
            product.id = MSSQLServerConnector.GetNextId(tableName);
            MSSQLServerConnector.DBInsert(tableName, product.ToList());
            return true;
        }
        catch
        {
            Debug.LogWarning("ProductData insertion failed");
            return false;
        }
    }

    public static bool TryDisapproveProduct(ProductData product)
    {
        try
        {
            MSSQLServerConnector.DBRemove("approvables", product.id);
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            return false;
        }
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


MSSQLServerConnector.ExecuteSQL("ALTER TABLE app_users ALTER COLUMN user_login nvarchar(45) NOT NULL;");
MSSQLServerConnector.ExecuteSQL("ALTER TABLE app_users ADD CONSTRAINT pk_userID PRIMARY KEY (user_login)");

 * all_products seed
    MSSQLServerConnector.DropTable("all_products");
    MSSQLServerConnector.CreateNewTable("all_products", new List<string>()
                                                {
                                                        "id", "name", "description", 
                                                        "pictureURL", "price", "vendorUserLogin",
                                                },
                                                new List<string>()
                                                {
                                                    "int IDENTITY(1,1) primary key", "nvarchar(45)", 
                                                    "nvarchar(2048)", "nvarchar(512)", "float", "nvarchar(45)"
                                                },
                                        new List<string>() { "", "", "", "", "", "app_users(user_login)" });


 * approvables seed
MSSQLServerConnector.DropTable("approvables");
MSSQLServerConnector.CreateNewTable("approvables", new List<string>()
                                            {
                                                    "id", "name", "description", 
                                                    "pictureURL", "price", "vendorUserLogin",
                                            },
                                            new List<string>()
                                            {
                                                "int IDENTITY(1,1) primary key", "nvarchar(45)", 
                                                "nvarchar(2048)", "nvarchar(512)", "float", "nvarchar(45)"
                                            },
                                    new List<string>() { "", "", "", "", "", "" });
 */