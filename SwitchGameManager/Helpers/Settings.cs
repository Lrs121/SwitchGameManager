﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Windows.Forms;

namespace SwitchGameManager.Helpers
{
    public class Config
    {
        

        //public bool confirmMultiActions;
        public bool encryptCertificates;

        [JsonIgnore]
        public SecureString encryptPassword;

        public int formHeight = 475;
        public int formWidth = 975;
        public int listIconSize = 1;
        public List<string> localXciFolders = new List<string>();
        public string sdDriveLetter = string.Empty;
        public byte[] olvState;

        [JsonIgnore]
        public bool isSdEnabled = false;

        //public bool lowMemoryMode = false; //Reduce memory usage at cost of slower UI updates (stop caching info, etc)
    }

    public class Settings
    {
        public static Config config;
        public static List<XciItem> xciCache = new List<XciItem>();

        public static string cacheFileName = "Cache.json";

        public static string configFileName = "Config.json";

        internal static void RebuildCache()
        {
            xciCache = new List<XciItem>();
            File.Delete(cacheFileName);
            XciHelper.PopulateXciList();
        }

        public static bool LoadSettings(string fileName = "")
        {
            Settings.config = new Config();

            if (String.IsNullOrWhiteSpace(fileName))
                fileName = configFileName;

            try
            {
                config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(fileName));
                CheckForSdCard();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to load {fileName}: {ex.Message}");
            }

            return false;
        }

        public static void CheckForSdCard()
        {
            if (!string.IsNullOrWhiteSpace(config.sdDriveLetter) && Directory.Exists(config.sdDriveLetter))
                config.isSdEnabled = true;
            else
                config.isSdEnabled = false;
        }

        public static bool SaveSettings(string fileName = "")
        {
            try
            {
                if (String.IsNullOrWhiteSpace(fileName))
                    fileName = configFileName;

                File.WriteAllText(fileName, JsonConvert.SerializeObject(config, formatting: Formatting.Indented));
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to save {fileName}: {ex.Message}");
            }

            return false;
        }
    }
}