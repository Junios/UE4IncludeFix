using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UE4IncludeFix
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: UE4IncludeFix ProjectName");
                return;
            }

            UE4XML(args[0]);
        }

        static void UE4XML(String projectName)
        {
            // XML문서를 불러온다
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load("./Intermediate/ProjectFiles/" + projectName + ".vcxproj");

            XmlNodeList searchNode = XmlDoc.GetElementsByTagName("NMakeIncludeSearchPath");
            if (searchNode.Count > 0)
            {
                return;
            }

            XmlNodeList nodes = XmlDoc.GetElementsByTagName("PropertyGroup");
            foreach (XmlNode node in nodes)
            {
                if (node.Attributes[0].Name == "Label")
                {
                    if (node.Attributes[0].Value == "Globals")
                    {
                        //< NMakeIncludeSearchPath > C:\Program Files\Epic Games\UE_4.25\Engine\Source\Runtime\Engine\Classes;$(NMakeIncludeSearchPath) </ NMakeIncludeSearchPath >
                        XmlElement xmlEle = XmlDoc.CreateElement("NMakeIncludeSearchPath"); // 추가할 Node 생성

                        xmlEle.InnerText = "C:\\Program Files\\Epic Games\\UE_4.25\\Engine\\Source\\Runtime\\Engine\\Classes;$(NMakeIncludeSearchPath)";
                        xmlEle.RemoveAllAttributes();
                        xmlEle.RemoveAttribute("xmlns");
                        node.InnerXml += "<NMakeIncludeSearchPath>C:\\Program Files\\Epic Games\\UE_4.25\\Engine\\Source\\Runtime\\Engine\\Classes;$(NMakeIncludeSearchPath)</NMakeIncludeSearchPath>";
                        //node.AppendChild(xmlEle);
                        break;
                    }
                }
            }

            XmlDoc.Save("./Intermediate/ProjectFiles/ProjectBD.vcxproj");
        }
    }
}
