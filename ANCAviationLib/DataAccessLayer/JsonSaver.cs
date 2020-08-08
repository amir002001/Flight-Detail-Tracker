using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Linq;

namespace ANCAviationLib.DataAccessLayer
{
    public static class JsonSaver
    {
        public static void Save<T>(string path, T toBeSaved)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                DataContractJsonSerializer jsonSerializer =
                    new DataContractJsonSerializer(typeof(T));
                jsonSerializer.WriteObject(fileStream, toBeSaved);
            }
        }
    }
}
