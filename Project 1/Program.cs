using System.Text.Json;

// Antoni Majewski, 299444

// For my implementation, I used Factory Method Pattern.

// Interfaces
public interface IObject
{
    ulong ID { get; set; }
}
public interface ICoordinates
{
    float Longitude { get; set; }
    float Latitude { get; set; }
    float AMSL { get; set; }
}
public interface IPerson
{
    string Name { get; set; }
    ulong Age { get; set; }
    string Phone { get; set; }
    string Email { get; set; }
}
public interface IPlane
{
    string Serial { get; set; }
    string Country { get; set; }
    string Model { get; set; }
}

// Factories
public class ObjectFactory
{
    private static readonly Dictionary<string, Func<string[], IObject>> objectCreators = new Dictionary<string, Func<string[], IObject>>
    {
        { "C", data => new Crew(data) },
        { "P", data => new Passenger(data) },
        { "CA", data => new Cargo(data) },
        { "CP", data => new CargoPlane(data) },
        { "PP", data => new PassengerPlane(data) },
        { "AI", data => new Airport(data) },
        { "FL", data => new Flight(data) }
    };

    public static IObject CreateObject(string[] data)
    {
        string objectType = data[0];

        if (objectCreators.ContainsKey(objectType))
        {
            return objectCreators[objectType](data);
        }
        else
        {
            throw new ArgumentException("Invalid object type specified in data.");
        }
    }
}
public class CrewFactory
{
    public static void SetData(Crew crew, string[] data)
    {
        crew.ID = ulong.Parse(data[1]);
        crew.Name = data[2];
        crew.Age = ulong.Parse(data[3]);
        crew.Phone = data[4];
        crew.Email = data[5];
        crew.Practice = ushort.Parse(data[6]);
        crew.Role = data[7];
    }
}
public class PassengerFactory
{
    public static void SetData(Passenger passenger, string[] data)
    {
        passenger.ID = ulong.Parse(data[1]);
        passenger.Name = data[2];
        passenger.Age = ulong.Parse(data[3]);
        passenger.Phone = data[4];
        passenger.Email = data[5];
        passenger.Class = data[6];
        passenger.Miles = ulong.Parse(data[7]);
    }
}
public class CargoFactory
{
    public static void SetData(Cargo cargo, string[] data)
    {
        cargo.ID = ulong.Parse(data[1]);
        cargo.Weight = float.Parse(data[2]);
        cargo.Code = data[3];
        cargo.Description = data[4];
    }
}
public class CargoPlaneFactory
{
    public static void SetData(CargoPlane cargoPlane, string[] data)
    {
        cargoPlane.ID = ulong.Parse(data[1]);
        cargoPlane.Serial = data[2];
        cargoPlane.Country = data[3];
        cargoPlane.Model = data[4];
        cargoPlane.MaxLoad = float.Parse(data[5]);
    }
}
public class PassengerPlaneFactory
{
    public static void SetData(PassengerPlane passengerPlane, string[] data)
    {
        passengerPlane.ID = ulong.Parse(data[1]);
        passengerPlane.Serial = data[2];
        passengerPlane.Country = data[3];
        passengerPlane.Model = data[4];
        passengerPlane.FirstClassSize = ushort.Parse(data[5]);
        passengerPlane.BusinessClassSize = ushort.Parse(data[6]);
        passengerPlane.EconomyClassSize = ushort.Parse(data[7]);
    }
}
public class AirportFactory
{
    public static void SetData(Airport airport, string[] data)
    {
        airport.ID = ulong.Parse(data[1]);
        airport.Name = data[2];
        airport.Code = data[3];
        airport.Longitude = float.Parse(data[4]);
        airport.Latitude = float.Parse(data[5]);
        airport.AMSL = float.Parse(data[6]);
        airport.Country = data[7];
    }
}
public class FlightFactory
{
    public static void SetData(Flight flight, string[] data)
    {
        flight.ID = ulong.Parse(data[1]);
        flight.OriginID = ulong.Parse(data[2]);
        flight.TargetID = ulong.Parse(data[3]);
        flight.TakeoffTime = data[4];
        flight.LandingTime = data[5];
        flight.Longitude = float.Parse(data[6]);
        flight.Latitude = float.Parse(data[7]);
        flight.AMSL = float.Parse(data[8]);
        flight.PlaneID = ulong.Parse(data[9]);
        flight.CrewIDs = Array.ConvertAll(data[10].Trim('[', ']').Split(';'), ulong.Parse);
        flight.LoadsIDs = Array.ConvertAll(data[11].Trim('[', ']').Split(';'), ulong.Parse);
    }
}

// Objects
public class Crew : IObject, IPerson
{
    public ulong ID { get; set; }
    public string Name { get; set; }
    public ulong Age { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public ushort Practice { get; set; }
    public string Role { get; set; }

    public Crew(string[] data)
    {
        CrewFactory.SetData(this, data);
    }
}
public class Passenger : IObject, IPerson
{
    public ulong ID { get; set; }
    public string Name { get; set; }
    public ulong Age { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Class { get; set; }
    public ulong Miles { get; set; }

    public Passenger(string[] data)
    {
        PassengerFactory.SetData(this, data);
    }
}
public class Cargo : IObject
{
    public ulong ID { get; set; }
    public float Weight { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }

    public Cargo(string[] data)
    {
        CargoFactory.SetData(this, data);
    }
}
public class CargoPlane : IObject, IPlane
{
    public ulong ID { get; set; }
    public string Serial { get; set; }
    public string Country { get; set; }
    public string Model { get; set; }
    public float MaxLoad { get; set; }

    public CargoPlane(string[] data)
    {
        CargoPlaneFactory.SetData(this, data);
    }
}
public class PassengerPlane : IObject, IPlane
{
    public ulong ID { get; set; }
    public string Serial { get; set; }
    public string Country { get; set; }
    public string Model { get; set; }
    public ushort FirstClassSize { get; set; }
    public ushort BusinessClassSize { get; set; }
    public ushort EconomyClassSize { get; set; }

    public PassengerPlane(string[] data)
    {
        PassengerPlaneFactory.SetData(this, data);
    }
}
public class Airport : IObject, ICoordinates
{
    public ulong ID { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public float Longitude { get; set; }
    public float Latitude { get; set; }
    public float AMSL { get; set; }
    public string Country { get; set; }

    public Airport(string[] data)
    {
        AirportFactory.SetData(this, data);
    }
}
public class Flight : IObject, ICoordinates
{
    public ulong ID { get; set; }
    public ulong OriginID { get; set; }
    public ulong TargetID { get; set; }
    public string TakeoffTime { get; set; }
    public string LandingTime { get; set; }
    public float Longitude { get; set; }
    public float Latitude { get; set; }
    public float AMSL { get; set; }
    public ulong PlaneID { get; set; }
    public ulong[] CrewIDs { get; set; }
    public ulong[] LoadsIDs { get; set; }

    public Flight(string[] data)
    {
        FlightFactory.SetData(this, data);
    }
}

class Program
{
    static void Main(string[] args)
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        Console.WriteLine("Current working directory: " + currentDirectory);

        Console.WriteLine();
        // Reading .ftr file at the given file path.
        object[] objects = ReadFTRFile();

        // Serializing and saving objects to .json file.
        SerializeToJson(objects);

        Console.ReadLine();
    }

    // Methods
    static object[] ReadFTRFile()
    {
        // This method is used for reading the contents of FTR file for a given file location. Returns array of objects.

        // In other use case I would replace fileLocation value with GetFileLocation method.
        Console.WriteLine("Provide location of FTR file. ");
        string filePath = "../../../example_data.ftr";

        // Loop ensuring that the file at the given file path has .ftr extension.
        while (!Path.GetExtension(filePath).Equals(".ftr", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("This location is incorrect, or the file is not FTR file. Please, input the proper location.");
            filePath = Console.ReadLine();
        }

        object[] objects = new object[File.ReadAllLines(filePath).Length];
        string[] lines = File.ReadAllLines(filePath);
        int i = 0;
        foreach (string line in lines)
        {
            // Splitting each line into strings array and then creating objects using factory and adding them to the objects list.
            string[] data = line.Split(',');
            IObject obj = ObjectFactory.CreateObject(data);
            objects[i] = obj;
            i++;
        }

        Console.WriteLine($"FTR file was read properly. {objects.Length} objects were created.");
        return objects;
    }
    static void SerializeToJson(object[] objects)
    {
        // This method is used for serializing data stored in list into a JSON file and saving it to a given file location.
        List<string> jsonObjects = new List<string>();

        foreach (var obj in objects)
        {
            string jsonObject = JsonSerializer.Serialize(obj);
            jsonObjects.Add(jsonObject);
        }

        // In other use case I would replace fileLocation value with GetFileLocation method.
        Console.WriteLine("Objects are ready to be saved in JSON file. Provide location for saving JSON. ");
        string fileLocation = "../../../output.json";

        File.WriteAllLines(fileLocation, jsonObjects);
        Console.WriteLine("JSON file has been created.");
    }
    static string GetFileLocation()
    {
        // This method is used for receiving file location. It is currently not in use, as I am not sure if we're supposed to get the file location from the user.
        Console.Write("File location: ");
        string filePath = Console.ReadLine();

        return filePath;
    }
}