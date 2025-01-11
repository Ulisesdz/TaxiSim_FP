using System.IO;
using UnityEngine;

public class FileHandler : MonoBehaviour, IFileHandler
{
    public void Save(string path, string data)
    {
        File.WriteAllText(path, data);
    }

    public string Load(string path)
    {
        return File.ReadAllText(path);
    }

    public bool Exists(string path)
    {
        return File.Exists(path);
    }
}
