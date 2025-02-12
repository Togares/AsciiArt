using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CommunicationModel
{
    public class CommunicationData
    {
        public static string Serialize(ClientData data)
        {
            XmlSerializer serializer = new XmlSerializer(data.GetType());
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, data);
                return writer.ToString();
            }
        }

        public static ClientData Deserialize(string data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ClientData));
            TextReader reader = new StringReader(data);
            return (ClientData) serializer.Deserialize(reader);
        }
    }
}
