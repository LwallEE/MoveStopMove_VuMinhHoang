using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ReuseSystem.Helper.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace ReuseSystem.SaveLoadSystem
{
    public class SaveLoadManager : Singleton<SaveLoadManager>
    {
        const string PROFILE_DATA_FOLDER_NAME = "Profiles";
        const string PROFILE_DATA_FILE_NAME_SUFFIX = "json";

        public Dictionary<string, DataRecord> recordsCache = new Dictionary<string, DataRecord>();

        protected override void Awake()
        {
            base.Awake();
            
        }

        public void Start()
        {
            PreLoadAllProfileRecordsIntoCache();
        }

        public T Get<T>() where T : DataRecord
        {
            if (recordsCache.TryGetValue(typeof(T).Name, out DataRecord value))
                return value as T;

            return Load<T>();
        }

        public bool Save(DataRecord record)
        {
            bool result = false;
            try
            {
                string typeName = record.GetType().Name;
                string savePath = $"{GetProfileFolderPath()}/{typeName}.{PROFILE_DATA_FILE_NAME_SUFFIX}";

                string json = JsonUtility.ToJson(record);
                
                File.WriteAllText(savePath, json);

                SaveCache(record);
                result = true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            return result;
        }

        public bool Delete(DataRecord record)
        {
            bool result = false;
            try
            {
                string typeName = record.GetType().Name;
                string savePath = $"{GetProfileFolderPath()}";
                string[] filePaths = Directory.GetFiles(savePath, $"*.{PROFILE_DATA_FILE_NAME_SUFFIX}");
                foreach (string file in filePaths)
                {
                    string fileName = Path.GetFileName(file);
                    string fileType = fileName.Replace($".{PROFILE_DATA_FILE_NAME_SUFFIX}", "");
                    if (fileType == typeName)
                    {
                        File.Delete(file);
                        if(recordsCache.ContainsKey(typeName))
                            recordsCache.Remove(typeName);
                        result = true;
                        break;
                    }    
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            return result;
        }

        public T Load<T>() where T : DataRecord
        {
            DataRecord record = null;
            string savePath = $"{GetProfileFolderPath()}/{typeof(T).Name}.{PROFILE_DATA_FILE_NAME_SUFFIX}";
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                record = JsonUtility.FromJson<T>(json);
            }
            else
            {
                record = Activator.CreateInstance<T>();
            }

            SaveCache(record);

            return record as T;
        }

        private void SaveCache(DataRecord record)
        {
            string typeName = record.GetType().Name;
            if (!recordsCache.ContainsKey(typeName))
            {
                recordsCache.Add(typeName, record);
            }

            recordsCache[typeName] = record;
        }

        private void PreLoadAllProfileRecordsIntoCache()
        {
            Dictionary<string, Type> typeList = GetListTypeBaseOnProfile();

            string savePath = $"{GetProfileFolderPath()}";
            string[] filePaths = Directory.GetFiles(savePath, $"*.{PROFILE_DATA_FILE_NAME_SUFFIX}");
            foreach (string file in filePaths)
            {
                string json = File.ReadAllText(file);
                string fileName = Path.GetFileName(file);
                string fileType = fileName.Replace($".{PROFILE_DATA_FILE_NAME_SUFFIX}", "");
                try
                {
                    if (typeList[fileType] == null || recordsCache.ContainsKey(fileType)) continue;
                    
                    DataRecord record = JsonUtility.FromJson(json, typeList[fileType]) as DataRecord;
                    
                    recordsCache.Add(record.GetType().Name, record);
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogWarning(e);
                    File.Delete(file);
                }
            }
        }

        private Dictionary<string, Type> GetListTypeBaseOnProfile()
        {
            Dictionary<string, Type> objects = new Dictionary<string, Type>();
            IEnumerable<Type> types = typeof(DataRecord).GetAllSubclasses();
            foreach (Type type in types)
            {
                objects.Add(type.Name, type);
            }

            return objects;
        }

        public string GetProfileFolderPath()
        {
            string folderPath = $"{Application.persistentDataPath}/{PROFILE_DATA_FOLDER_NAME}";
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            return folderPath;
        }
        private static void ClearPersistantData()
        {
            //persitent folder
            DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        public static void ClearGameData()
        {
            ClearPersistantData();
            PlayerPrefs.DeleteAll();
        }
    }
}

