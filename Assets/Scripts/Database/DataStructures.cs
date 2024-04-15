using System;
using System.Collections.Generic;
using UnityEngine;
using CorvusEnLignumDBSolutionsIncorporated;

namespace Assets.Scripts.Database
{
    internal static class DataStructures
    {
        /// <summary>
        /// Sandbox database
        /// </summary>
        public struct Database
        {
            public string databaseName;

            public const string someTableName = "someTableName";
            public const string someOtherTableName = "someOtherTableName";

            public List<SomeData> someData;
            public List<SomeOtherData> someOtherData;

            public Database(string name)
            {
                databaseName = name;
                someData = new List<SomeData>();
                someOtherData = new List<SomeOtherData>();
            }

            /// <summary>
            /// Player stats bit representing value of a certain in-game skill
            /// </summary>
            public void Clear()
            {
               // someData.Clear();
                //someOtherData.Clear();
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

        public struct SomeData : Data
        {
            public int id;
            public string someStringData;

            public int NumberOfParameters => 2;
            public int Id => id;
            public Data ToData(List<string> data)
            {
                if (data.Count != NumberOfParameters) throw new Exception("Wrong data size format");
                id = int.Parse(data[0]);
                someStringData = data[1];
                return this;
            }

            public List<string> ToList()
            {
                return new List<string>() { id.ToString(), someStringData };
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
    }
}
