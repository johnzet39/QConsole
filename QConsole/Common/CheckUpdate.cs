using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows;

namespace QConsole.Common
{
    static class CheckUpdate
    {
        static Version newVersion { get; set; }
        static string url { get; set; }
        static XmlTextReader reader { get; set; }
        static string xmlURL { get; set; }

        static  CheckUpdate()
        {
            newVersion = null;
            url = "";
            reader = null;
            xmlURL = "https://johnzet39.github.io/apps/qconsole/updates.xml";
        }

        static public void ReadFileXML()
        {
            try
            {
                using (reader = new XmlTextReader(xmlURL))
                {
                    reader = new XmlTextReader(xmlURL);
                    reader.MoveToContent();
                    string elementName = "";
                    // we check if the xml starts with a proper  
                    // "ourfancyapp" element node  
                    if ((reader.NodeType == XmlNodeType.Element) &&
                        (reader.Name == "qconsole"))
                    {
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                                elementName = reader.Name;
                            else
                            {
                                if ((reader.NodeType == XmlNodeType.Text) &&
                                    (reader.HasValue))
                                {
                                    switch (elementName)
                                    {
                                        case "version":
                                            newVersion = new Version(reader.Value);
                                            break;
                                        case "url":
                                            url = reader.Value;
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        static public bool CompareVersions()
        {
            ReadFileXML();

            // get the running version  
            Version curVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            // compare the versions  
            if (curVersion.CompareTo(newVersion) < 0)
            {
                return true;
            }
            return false;
        }

        static public void GoDownLoadFiles()
        {
            // ask the user if he would like  
            // to download the new version  
            string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            string title = "Доступно обновление программы.";
            string question = string.Format("Загрузить новую версию {0} {1}.{2}.{3}.{4}?", appName, newVersion.Major, newVersion.Minor, newVersion.Build, newVersion.Revision);

            if (MessageBoxResult.Yes ==
             MessageBox.Show(question, title,
                             MessageBoxButton.YesNo,
                             MessageBoxImage.Information))
            {
                // navigate the default web  
                // browser to our app  
                // homepage (the url  
                // comes from the xml content)  
                try
                {
                    System.Diagnostics.Process.Start(url);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
