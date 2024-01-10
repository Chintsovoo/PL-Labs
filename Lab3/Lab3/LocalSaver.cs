
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Lab3;

public class LocalSaver
{
    private const string json = "Data/file.json";
    private const string xml = "Data/file.xml";
    
    
    public static void SaveToJson<T>(T obj)
    {
        try
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            File.WriteAllText(LocalSaver.json, json);
            Console.WriteLine("Saved successfully to json");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("Failed to save to json");
        }
    }

    public static void SaveToXml<T>(T obj)
    {
        try
        {
            var xmlSerializer = new XmlSerializer(typeof(T));

            using (TextWriter writer = new StreamWriter(xml))
            {
                xmlSerializer.Serialize(writer, obj);
            }

            Console.WriteLine("Saved successfully to xml");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("Failed to save to xml");
        }
    }
    
    public static T ReadFromJson<T>()
    {
        try
        {
            var tr1 = File.ReadAllText(LocalSaver.json);
            var obj = JsonConvert.DeserializeObject<T>(json);
            Console.WriteLine($"Object read from JSON file");
            return obj ?? throw new InvalidOperationException();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine($"Error reading object from JSON");
            throw new DidNotLoadException();
        }
    }

    public static T ReadFromXml<T>()
    {
        try
        {
            var xmlSerializer = new XmlSerializer(typeof(T));

            using TextReader reader = new StreamReader(xml);
            var obj = (T)xmlSerializer.Deserialize(reader)!;
            Console.WriteLine($"Object read from XML file: ");
            return obj ?? throw new InvalidOperationException();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine($"Error reading object from XML");
            throw new DidNotLoadException();

        }
    }
}

public class DidNotLoadException : Exception
{
}