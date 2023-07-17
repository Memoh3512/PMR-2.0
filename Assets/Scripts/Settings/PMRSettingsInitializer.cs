using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;

namespace PMR
{
    public static class PMRSettings
    {
        private static Dictionary<Type, ScriptableObject> settingsItems = new();
        
        //Settings categories
        public static MenuSettings menuSettings => GetMenuItem<MenuSettings>();

        private static T GetMenuItem<T>() where T : ScriptableObject => (T)settingsItems[typeof(T)];
        public static void RegisterSetting(Type type, ScriptableObject settingsItem)
        {
            if (settingsItems.ContainsKey(type))
            {
                Debug.LogError($"Trying to register settings of type {type} twice.");
                return;
            }

            settingsItems.Add(type, settingsItem);
        }
    }

    public class PMRSettingsInitializer : MonoBehaviour
    {
        [Tooltip("List of settings to register on game load.")]
        [SerializeField] private List<ScriptableObject> settingsToRegister;
        private void Awake()
        {
            foreach (ScriptableObject setting in settingsToRegister)
            {
                PMRSettings.RegisterSetting(setting.GetType(), setting);
            }
            Destroy(gameObject);
        }
    }
}
