using System.Xml;
using System.Xml.Serialization;
using LocoSQS.Model.Interfaces;
using Newtonsoft.Json;

namespace LocoSQS.Model.Utils;
public class ActionResult
{
    public string Xml
    {
        get
        {
            if (_xml == null)
                _xml = GenerateXML();

            return _xml;
        }
        private set
        {
            _xml = value;
        }
    }

    public string Json
    {
        get
        {
            if (_json == null)
                _json = GenerateJson();

            return _json;
        }
        private set
        {
            _json = value;
        }
    }

    private string? _xml = null;
    private string? _json = null;
    public int HttpStatusCode { get; }
    public IActionResultData Raw { get; }

    public ActionResult(int httpStatusCode, IActionResultData raw)
    {
        HttpStatusCode = httpStatusCode;
        Raw = raw;
    }
    
    public ActionResult(string xml, int httpStatusCode, IActionResultData raw)
    {
        Xml = xml;
        HttpStatusCode = httpStatusCode;
        Raw = raw;
    }

    private string GenerateXML()
    {
        string xml;
        XmlSerializer xs = new(Raw.GetType());

        using (StringWriter sw = new())
        {
            using (XmlWriter xw = XmlWriter.Create(sw))
            {
                xs.Serialize(xw, Raw);
                xml = sw.ToString();
            }
        }

        return xml;
    }

    private string GenerateJson()
        => JsonConvert.SerializeObject(Raw.JsonResult);
}

public class ActionResult<T> : ActionResult where T : IActionResultData
{
    public T Value { get; }
    
    public ActionResult(int httpStatusCode, T value) : base(httpStatusCode, value!)
    {
        Value = value;
    }

    public ActionResult(string xml, int httpStatusCode, T value) : base(xml, httpStatusCode, value!)
    {
        Value = value;
    }
}