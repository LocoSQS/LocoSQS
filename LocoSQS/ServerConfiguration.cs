using LocoSQS.Model.Utils;

namespace LocoSQS;

public static class ServerConfiguration
{
    public static void Init()
    {
        string? json = Environment.GetEnvironmentVariable("JSON");
        if (!string.IsNullOrEmpty(json))
        {
            STORAGEBACKEND = StorageBackendType.Json;
            STORAGEBACKENDARG = json;
        }

        Console.WriteLine("Initialized with the following configuration:");
        Console.WriteLine($"Base URL: {BASEURL}");
        Console.WriteLine($"Storage backend: {STORAGEBACKEND} {STORAGEBACKENDARG}");
        Console.WriteLine($"Read Only storage: {READONLY}");
        Console.WriteLine($"Message Tracking: {!NOTRACK}");
        Console.WriteLine($"Debug: {DEBUG}");
        Console.WriteLine($"Implicitly create queue: {CREATEQUEUEIMPLICITLY}");
        Console.WriteLine();
    }

    // Port used for communication. Note: Urls will be generated with this in mind. If running trough docker, make sure to forward the same port
    public static int PORT { get; } = int.Parse(Environment.GetEnvironmentVariable("PORT") ?? "5314");
    // Host used for communication
    public static string HOST { get; } = Environment.GetEnvironmentVariable("HOST") ?? "localhost";
    // Protocol used for communication
    public static string PROTOCOL { get; } = Environment.GetEnvironmentVariable("PROTOCOL") ?? "http";
    // Storage backend type
    public static StorageBackendType STORAGEBACKEND { get; private set; } = StorageBackendType.Memory;
    // Argument (usually location, connection string) for storage backend
    public static string STORAGEBACKENDARG { get; private set; } = "";
    // Does not make writes to the storage backend
    public static bool READONLY { get; } = Environment.GetEnvironmentVariable("READONLY") == "1";
    // Turns of tracking/tracing of messages. Tracking needs to be enabled for webhook functionality to work
    public static bool NOTRACK { get; } = Environment.GetEnvironmentVariable("NOTRACK") == "1";
    // Gives extra debug messages in the console
    public static bool DEBUG { get; } = Environment.GetEnvironmentVariable("DEBUG") == "1";
    // Implicitly creates a queue when a request is made to a non-existant queue
    public static bool CREATEQUEUEIMPLICITLY { get; } = Environment.GetEnvironmentVariable("CREATEQUEUEIMPLICITLY") == "1";
    public static string BASEURL => $"{PROTOCOL}://{HOST}:{PORT}";

    public static void DebugLog(string message) 
    { 
        if (DEBUG)
            Console.WriteLine(message); 
    }
}