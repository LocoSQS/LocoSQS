using LocoSQS.Exeptions;
using LocoSQS.Model.Interfaces;
using LocoSQS.Test.Parser.Data;

namespace LocoSQS.Test.Parser;

public class Parser
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestStringParse()
    {
        IQueueAction action = LocoSQS.Parser.Parser.Parse<IQueueAction>(new Dictionary<string, string>()
        {
            {"Action", "Data1"},
            {"Data", "Hello" }
        });

        Data1 data = action as Data1;
        Assert.That(data.Data, Is.EqualTo("Hello"));
    }
    
    [Test]
    public void TestIntParse()
    {
        IQueueAction action = LocoSQS.Parser.Parser.Parse<IQueueAction>(new Dictionary<string, string>()
        {
            {"Action", "Data4"},
            {"Data", "10" }
        });
        
        Data4 data = action as Data4;
        Assert.That(data.Data, Is.EqualTo(10));
    }
    
    [Test]
    public void TestLongParse()
    {
        IQueueAction action = LocoSQS.Parser.Parser.Parse<IQueueAction>(new Dictionary<string, string>()
        {
            {"Action", "Data5"},
            {"Data", "12345678900000" }
        });
        
        Data5 data = action as Data5;
        Assert.That(data.Data, Is.EqualTo(12345678900000L));
    }
    
    [Test]
    public void TestMultiTypeParse()
    {
        IQueueAction action = LocoSQS.Parser.Parser.Parse<IQueueAction>(new Dictionary<string, string>()
        {
            {"Action", "Data6"},
            {"DataString", "Hello" },
            {"DataInt", "10" },
            {"DataLong", "12345678900000" }
        });
        
        Data6 data = action as Data6;
        Assert.That(data.DataString, Is.EqualTo("Hello"));
        Assert.That(data.DataInt, Is.EqualTo(10));
        Assert.That(data.DataLong, Is.EqualTo(12345678900000L));
    }
    
    [Test]
    public void TestListStringParse()
    {
        IQueueAction action = LocoSQS.Parser.Parser.Parse<IQueueAction>(new Dictionary<string, string>()
        {
            {"Action", "Data2"},
            {"Data.1", "Hello1" },
            {"Data.3", "Hello3" },
            {"Data.2", "Hello2" },
        });

        Data2 data = action as Data2;
        
        Assert.That(data.Data.Count, Is.EqualTo(3));
        Assert.That(data.Data[0], Is.EqualTo("Hello1"));
        Assert.That(data.Data[1], Is.EqualTo("Hello2"));
        Assert.That(data.Data[2], Is.EqualTo("Hello3"));
    }
    
    [Test]
    public void TestListDynamicParse()
    {
        IQueueAction action = LocoSQS.Parser.Parser.Parse<IQueueAction>(new Dictionary<string, string>()
        {
            {"Action", "Data3"},
            {"Data.1.Data", "Hello1" },
            {"Data.2.Data", "Hello2" }
        });

        Data3 data = action as Data3;
        
        Assert.That(data.Data.Count, Is.EqualTo(2));
        Assert.That(data.Data[0].Data, Is.EqualTo("Hello1"));
        Assert.That(data.Data[1].Data, Is.EqualTo("Hello2"));
    }
    
    [Test]
    public void TestObjectDynamicParse()
    {
        IQueueAction action = LocoSQS.Parser.Parser.Parse<IQueueAction>(new Dictionary<string, string>()
        {
            {"Action", "Data7"},
            {"Data.Data", "Hello1" },
        });

        Data7 data = action as Data7;
        
        Assert.That(data.Data.Data, Is.EqualTo("Hello1"));
    }

    [Test]
    public void TestMissingActionException()
    {
        Assert.Throws<MissingAction>(() => LocoSQS.Parser.Parser.Parse<IQueueAction>(new Dictionary<string, string>()));
    }

    public void TestTypeNotFoundException()
    {
        // Tests that a class that does not inherit from IQueueAction cannot be used
        Assert.Throws<InvalidAction>(() => LocoSQS.Parser.Parser.Parse<IQueueAction>(new Dictionary<string, string>()
        {
            { "Action", "Parser" }
        }));

        // Tests that an interface cannot be used
        Assert.Throws<InvalidAction>(() => LocoSQS.Parser.Parser.Parse<IQueueAction>(new Dictionary<string, string>()
        {
            { "Action", "IQueueAction" }
        }));
    }

    [Test]
    public void TestMissingFieldsException()
    {
        Assert.Throws<MissingParameter>(() => LocoSQS.Parser.Parser.Parse<IQueueAction>(new Dictionary<string, string>()
        {
            { "Action", "Data1" }
        }));
    }
}