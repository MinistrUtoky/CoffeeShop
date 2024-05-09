using System;
using System.Collections.Generic;
using UnityEngine;
using CorvusEnLignumDBSolutionsIncorporated;
using static Assets.Scripts.Database.DataStructures;

namespace Assets.Scripts.Database
{
    public static class DataStructures
    {
        /// <summary>
        /// Sandbox database
        /// </summary>
        public struct Database
        {
            public string databaseName;

            public const string someTableName = "someTableName";
            public const string someOtherTableName = "someOtherTableName";

            public List<UserData> someData;
            public List<SomeOtherData> someOtherData;

            public Database(string name)
            {
                databaseName = name;
                someData = new List<UserData>();
                someOtherData = new List<SomeOtherData>();
            }

            /// <summary>
            /// Player stats bit representing value of a certain in-game skill
            /// </summary>
            public void Clear()
            {
                someData.Clear();
                someOtherData.Clear();
            }
            /// <summary>
            /// Addition of another sandbox's values to this sandbox instance 
            /// </summary>
            /// <param name="anotherDatabase">
            /// Database which values will be added
            /// </param>
            public void Add(Database anotherDatabase)
            {
                someData.AddRange(anotherDatabase.someData);
                someOtherData.AddRange(anotherDatabase.someOtherData);
            }
        }
        [Serializable]
        public enum UserType
        {
            Buyer,
            Seller,
            Moderator
        }
        public struct UserData : Data
        {
            public int id;
            public string displayName;
            public string login; // gonna be just checked with db and proceed
            public string email;
            public string password; // same as login :D  
            public UserType type;
            public string currency;

            public int NumberOfParameters => 7;
            public int Id => id;
            public Data ToData(List<string> data)
            {
                if (data.Count != NumberOfParameters) throw new Exception("Wrong data size format");
                id = int.Parse(data[0]);
                displayName = data[1];
                login = data[2];
                email = data[3];
                password = data[4];
                type = (UserType)Enum.Parse(typeof(UserType), data[5]);
                currency = data[6];
                return this;
            }

            public List<string> ToList()
            {
                return new List<string>() { id.ToString(), displayName, login, email, password, ((int)type).ToString(), currency };
            }
        }

        [Serializable]
        public struct ProductData : Data
        {
            public int id;
            public string name;
            [TextArea(10, 10)]
            public string description;
            public string pictureURL;
            public float price;
            public string vendorUserLogin;

            public int NumberOfParameters => 6;
            public int Id => id;

            public override bool Equals(object obj)
            {
                return obj is ProductData product &&
                       id == product.id &&
                       name == product.name &&
                       description == product.description &&
                       pictureURL == product.pictureURL &&
                       price == product.price;
            }
            public static bool operator ==(ProductData product1, ProductData product2)
            {
                return product1.Equals(product2);
            }
            public static bool operator !=(ProductData product1, ProductData product2)
            {
                return !(product1 == product2);
            }

            public Data ToData(List<string> data)
            {
                if (data.Count != NumberOfParameters) throw new Exception("Wrong data size format");
                id = int.Parse(data[0]);
                name = data[1];
                description = data[2];
                pictureURL = data[3];
                price = float.Parse(data[4]);
                vendorUserLogin = data[5];
                return this;
            }

            public List<string> ToList()
            {
                return new List<string>() { id.ToString(), name, description, pictureURL, price.ToString(), vendorUserLogin };
            }
        }
        public struct SomeOtherData : Data
        {
            public int id;
            public SomeComplexData someComplexData;

            public int NumberOfParameters => 2;
            public int Id => id;
            public Data ToData(List<string> data)
            {
                if (data.Count != NumberOfParameters) throw new Exception("Wrong data size format");
                id = int.Parse(data[0]);
                someComplexData = JsonUtility.FromJson<SomeComplexData>(data[1]);
                return this;
            }

            public List<string> ToList()
            {
                return new List<string>() { id.ToString(), JsonUtility.ToJson(someComplexData) };
            }
        }
        public struct SomeComplexData {
            public int a;
            public string b;
        }

        #region JsonReleated
        [Serializable]
        public class MyList<T>
        {
            public List<T> list;
        }
        [Serializable]
        public struct Subscription
        {
            public string userLogin;
            public string subscriptionStart;
            public string subscriptionEnd;
            public int productID;
            public int productAmount;
            public static bool operator == (Subscription a, Subscription b)
            {
                return a.userLogin == b.userLogin
                    & a.subscriptionStart == b.subscriptionStart
                    & a.subscriptionEnd == b.subscriptionEnd
                    & a.productID == b.productID
                    & a.productAmount == b.productAmount;
            }
            public static bool operator != (Subscription a, Subscription b)
            {
                return !(a == b);
            }
        }
        #endregion
    }
}
