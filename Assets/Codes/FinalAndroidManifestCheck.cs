#if UNITY_EDITOR && UNITY_ANDROID
using UnityEditor.Android;
using System.Xml;
using System.IO;
using System.Text;
using UnityEditor;

public class FinalAndroidManifestCheck : IPostGenerateGradleAndroidProject
{
    public int callbackOrder => 999; // High number to ensure this runs last

    public void OnPostGenerateGradleAndroidProject(string basePath)
    {
        string manifestPath = Path.Combine(basePath, "src/main/AndroidManifest.xml");
        AndroidXmlDocument manifestDoc = new AndroidXmlDocument(manifestPath);

        if (!PermissionExists(manifestDoc, "com.google.android.gms.permission.AD_ID"))
        {
            AddPermission(manifestDoc, "com.google.android.gms.permission.AD_ID");
            manifestDoc.Save();
        }
    }

    private bool PermissionExists(AndroidXmlDocument doc, string permissionName)
    {
        XmlNodeList permissions = doc.SelectNodes("/manifest/uses-permission");
        foreach (XmlNode perm in permissions)
        {
            string name = perm.Attributes["android:name"]?.InnerText;
            if (name == permissionName)
                return true;
        }
        return false;
    }

    private void AddPermission(AndroidXmlDocument doc, string permissionName)
    {
        XmlNode manifestNode = doc.SelectSingleNode("/manifest");
        XmlElement permElement = doc.CreateElement("uses-permission");
        permElement.SetAttribute(
            "name",
            "http://schemas.android.com/apk/res/android",
            permissionName
        );
        manifestNode.AppendChild(permElement);
    }
}

internal class AndroidXmlDocument : XmlDocument
{
    private string path;

    public AndroidXmlDocument(string path)
    {
        this.path = path;
        Load(path);
    }

    public void Save()
    {
        Save(path);
    }

    private void Save(string filePath)
    {
        using (var writer = new XmlTextWriter(filePath, new UTF8Encoding(false)))
        {
            writer.Formatting = Formatting.Indented;
            Save(writer);
        }
    }
}
#endif