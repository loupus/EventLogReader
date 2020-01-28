using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace EventLogReader
{
    public static class Config
    {
        private static Dictionary<string, string> CurrentConfig;

        public static bool GetConfig() // fill Globals from dictionary
        {
            if (CurrentConfig == null)
                CurrentConfig = new Dictionary<string, string>();

            if (ReadConfig())
            {

                Type t = typeof(Globals.Config);
                FieldInfo[] fields = t.GetFields(BindingFlags.Static | BindingFlags.Public);

                foreach (var k in CurrentConfig)
                {
                    foreach (FieldInfo finf in fields)
                    {
                        if (k.Key == finf.Name)
                        {
                            Type tt = finf.FieldType;
                            try
                            {
                                finf.SetValue(null, Convert.ChangeType(k.Value, tt));
                            }
                            catch (Exception ex)
                            {
                                // Logger.SetLog("type conversion failed", LogType.error, ex.Message);
                                MessageBox.Show("type conversion failed " + ex.Message);
                            }

                            break;
                        }
                    }
                }
                Globals.SetSqlConStr();
                return true;
            }
            else
                return false;
        }

        public static void SetConfig() // fill dictionary from Globals
        {
            if (CurrentConfig == null)
                CurrentConfig = new Dictionary<string, string>();

            CurrentConfig.Clear();
            Type t = typeof(Globals.Config);
            FieldInfo[] fields = t.GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (FieldInfo finf in fields)
            {
                CurrentConfig.Add(finf.Name, finf.GetValue(null) == null ? "" : finf.GetValue(null).ToString());
            }

            WriteConfig();

            Globals.SetSqlConStr();

        }

        private static void WriteConfig() // write xml from dictionary
        {
            try
            {
                using (XmlWriter writer = XmlWriter.Create(Globals.ConfigFile))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Config");

                    foreach (var k in CurrentConfig)
                    {
                        writer.WriteElementString(k.Key, k.Value);
                    }
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
              MessageBox.Show("config dosyaya yazildi");
            }
            catch (Exception ex)
            {
                MessageBox.Show("config dosyaya yazılamadı " + ex.Message);
            }
        }

        private static bool ReadConfig() // fill dictionary from xml file
        {
            if (!File.Exists(Globals.ConfigFile))
            {
                MessageBox.Show("config dosyası yok");
                return false;
            }

            string strName = "", strValue = "";
            try
            {
                using (XmlReader reader = XmlReader.Create(Globals.ConfigFile))
                {

                    while (!reader.EOF)
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            if (reader.Name == "Config")
                            {
                                reader.Read();
                                continue;
                            }

                            strName = reader.Name;
                            strValue = reader.ReadElementContentAsString();
                            //CurrentConfig.Add(strName, strValue);
                            AddConfig(strName, strValue);
                        }
                        else
                        {
                            reader.Read();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("config dosyası okunamadı " + ex.Message);
                return false;
            }
            return true;

        }

        private static void AddConfig(String pKey, String pValue)
        {
            Boolean AlreadyThere = false;
            foreach (KeyValuePair<String, String> pk in CurrentConfig)
            {
                if (pk.Key == pKey)
                {
                    AlreadyThere = true;
                    break;
                }
            }
            if (!AlreadyThere)
                CurrentConfig.Add(pKey, pValue);
        }
    }
}
