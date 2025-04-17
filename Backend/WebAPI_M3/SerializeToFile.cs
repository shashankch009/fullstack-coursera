
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Xml.Serialization;

public static class SerializeToFile 
{
    public static void SerializeToJson(string filePath, object data)
    {
        var options = new JsonSerializerOptions 
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
        var json = JsonSerializer.Serialize(data, options);
        File.WriteAllText(filePath, json);
    }

    public static void SerializeToXml(string filePath, object data)
    {
        var xmlSerializer = new XmlSerializer(data.GetType());
        using (var streamWriter = new StreamWriter(filePath))
        {
            xmlSerializer.Serialize(streamWriter, data);
        }
    }

    public static void SerializeToBinary(string filePath, object data)
    {
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        using (var binaryWriter = new BinaryWriter(fileStream))
        {
            var json = JsonSerializer.Serialize(data);
            binaryWriter.Write(json); //write JSON string as binary
        }
    }

    public static T DeserializeFromBinary<T>(string filePath)
    {
        using (var fileStream = new FileStream(filePath, FileMode.Open))
        using (var binaryReader = new BinaryReader(fileStream))
        {
            var json = binaryReader.ReadString(); // Read binary as JSON
            return JsonSerializer.Deserialize<T>(json); // Deserialize JSON back to object
        }
    }

    public static void SerializeToBinary(Person samplePerson)
    {
        // Binary serialization
        using (FileStream fs = new FileStream("person.dat", FileMode.Create))
        {
            BinaryWriter writer = new BinaryWriter(fs);
            writer.Write(samplePerson.UserName);
            writer.Write(samplePerson.UserAge);
        }
        Console.WriteLine("Binary serialization complete.");
    }

    public static Person DeserializeFromBinary()
    {
        // Binary deserialization
        using (FileStream fs = new FileStream("person.dat", FileMode.Open))
        using (var binaryReader = new BinaryReader(fs))
        {
            string userName = binaryReader.ReadString();
            int userAge = binaryReader.ReadInt32();
            return new Person { UserName = userName, UserAge = userAge };
        }
    }
}