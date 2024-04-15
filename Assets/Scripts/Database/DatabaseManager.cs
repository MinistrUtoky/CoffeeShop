using CorvusEnLignumDBSolutionsIncorporated;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    private static string dbName = "mainDB.mdf";
    void Start()
    {
        MSSQLServerConnector.cn_String = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + string.Join("\\", Application.dataPath.Split("/")) + "\\Data\\" + dbName + ";Integrated Security=True;Connect Timeout=30";
        Debug.Log(MSSQLServerConnector.cn_String);
        foreach (string d in MSSQLServerConnector.GetTableNames())
            Debug.Log(d);
    }
}