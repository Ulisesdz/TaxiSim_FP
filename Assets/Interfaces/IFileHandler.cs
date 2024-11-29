public interface IFileHandler
{
    void Save(string path, string data);
    string Load(string path);
    bool Exists(string path);
}
