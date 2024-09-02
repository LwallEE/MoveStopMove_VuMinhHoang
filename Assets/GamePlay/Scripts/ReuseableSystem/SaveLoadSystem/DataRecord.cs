using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReuseSystem.SaveLoadSystem
{
    public class DataRecord 
    {
        public virtual void Save()
        {
            var isSuccess = SaveLoadManager.Instance.Save(this);
            if (isSuccess)
            {
                Debug.Log($"Save:{GetType().Name.AddColor("#Cb9ce0")} - {"Success".AddColor(Color.yellow)}");
            }
            else
            {
                Debug.Log($"Save:{GetType().Name.AddColor("#9025be")} - {"Fail".AddColor(Color.red)}-Please Check!");
            }
        }

        public virtual void Delete()
        {
            var isSuccess = SaveLoadManager.Instance.Delete(this);
            if (isSuccess)
            {
                Debug.Log($"Delete:{GetType().Name.AddColor("#Cb9ce0")} - {"Success".AddColor(Color.yellow)}");
            }
            else
            {
                Debug.Log($"Delete:{GetType().Name.AddColor("#9025be")} - {"Fail".AddColor(Color.red)}-Please Check!");
            }
        }
        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }
    }
}
