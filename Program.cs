
var targetDirPath = AppContext.BaseDirectory;
var outputFilePath = "HashList.txt";
var hashAlgorithm = System.Security.Cryptography.SHA1.Create();

var ret = Directory.EnumerateFiles(targetDirPath, "*", SearchOption.AllDirectories).AsParallel()
    .Select(filePath => (filePath, ToHexString(ComputeHash(filePath)))).OrderBy(t => t.filePath);

using (var stream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.Read))
using (var writer = new StreamWriter(stream))
{
    foreach (var (filePath, hashString) in ret)
    {
        writer.WriteLine($"{hashString} \"{filePath}\"");
    }
}

return;

byte[] ComputeHash(string filePath)
{
    using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
    {
        lock (hashAlgorithm)
        {
            return hashAlgorithm.ComputeHash(stream);
        }
    }
}

string ToHexString(byte[] bytes)
{
    //return BitConverter.ToString(bytes).Replace("-", string.Empty).ToLower();
    return Convert.ToHexString(bytes).ToLower();
}
