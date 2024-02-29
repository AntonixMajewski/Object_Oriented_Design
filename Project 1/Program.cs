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
        { "C", data => CrewFactory.CreateCrew(data) },
        { "P", data => PassengerFactory.CreatePassenger(data) },
        { "CA", data => CargoFactory.CreateCargo(data) },
        { "CP", data => CargoPlaneFactory.CreateCargoPlane(data) },
        { "PP", data => PassengerPlaneFactory.CreatePassengerPlane(data) },
        { "AI", data => AirportFactory.CreateAirport(data) },
        { "FL", data => FlightFactory.CreateFlight(data) }
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
public class ImportManager
{
    public object[] ReadFTRFile(string filePath)
    {
        while (!Path.GetExtension(filePath).Equals(".ftr", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("This location is incorrect, or the file is not an FTR file. Please input the proper location.");
            filePath = Console.ReadLine();
        }

        object[] objects = new object[File.ReadAllLines(filePath).Length];
        string[] lines = File.ReadAllLines(filePath);
        int i = 0;
        foreach (string line in lines)
        {
            string[] data = line.Split(',');
            IObject obj = ObjectFactory.CreateObject(data);
            objects[i] = obj;
            i++;
        }

        Console.WriteLine($"FTR file was read properly. {objects.Length} objects were created.");
        return objects;
    }
}
public class ExportManager
{
    public void SerializeToJson(object[] objects, string fileLocation)
    {
        List<string> jsonObjects = new List<string>();

        foreach (var obj in objects)
        {
            string jsonObject = JsonSerializer.Serialize(obj);
            jsonObjects.Add(jsonObject);
        }

        File.WriteAllLines(fileLocation, jsonObjects);
        Console.WriteLine("JSON file has been created.");
    }
}
public class FileManager
{
    public string GetFileLocation()
    {
        Console.Write("File location: ");
        string filePath = Console.ReadLine();
        return filePath;
    }
}
public class CrewFactory
{
    public static Crew CreateCrew(string[] data)
    {
        Crew crew = new Crew();
        SetData(crew, data);
        return crew;
    }

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
    public static Passenger CreatePassenger(string[] data)
    {
        Passenger passenger = new Passenger();
        SetData(passenger, data);
        return passenger;
    }

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
    public static Cargo CreateCargo(string[] data)
    {
        Cargo cargo = new Cargo();
        SetData(cargo, data);
        return cargo;
    }

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
    public static CargoPlane CreateCargoPlane(string[] data)
    {
        CargoPlane cargoPlane = new CargoPlane();
        SetData(cargoPlane, data);
        return cargoPlane;
    }

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
    public static PassengerPlane CreatePassengerPlane(string[] data)
    {
        PassengerPlane passengerPlane = new PassengerPlane();
        SetData(passengerPlane, data);
        return passengerPlane;
    }

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
    public static Airport CreateAirport(string[] data)
    {
        Airport airport = new Airport();
        SetData(airport, data);
        return airport;
    }

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
    public static Flight CreateFlight(string[] data)
    {
        Flight flight = new Flight();
        SetData(flight, data);
        return flight;
    }

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

    public Crew()
    {

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

    public Passenger()
    {

    }
}
public class Cargo : IObject
{
    public ulong ID { get; set; }
    public float Weight { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }

    public Cargo()
    {

    }
}
public class CargoPlane : IObject, IPlane
{
    public ulong ID { get; set; }
    public string Serial { get; set; }
    public string Country { get; set; }
    public string Model { get; set; }
    public float MaxLoad { get; set; }

    public CargoPlane()
    {

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

    public PassengerPlane()
    {

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

    public Airport()
    {

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

    public Flight()
    {

    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Provide location of FTR file: ");
        // In other use case I would ask user to provide file location. For the sake of testing I provided filepath.
        string filePath = "../../../example_data.ftr";

        object[] objects = new ImportManager().ReadFTRFile(filePath);

        Console.WriteLine("Provide location for saving JSON: ");
        // In other use case I would ask user to provide file location with . For the sake of testing I provided filepath.
        string fileLocation = "../../../output.json";

        new ExportManager().SerializeToJson(objects, fileLocation);

        Console.ReadLine();
    }
}