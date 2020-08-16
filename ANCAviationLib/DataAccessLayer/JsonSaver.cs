using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Linq;

namespace ANCAviationLib.DataAccessLayer
{
    /// <summary>
    /// A static class using the data contract Json Serializer. 
    /// </summary>
    public static class JsonSaver
    {
        /// <summary>
        /// Based on a generic, it uses a stream and an object to save it into that stream, then closes it.
        /// </summary>
        /// <typeparam name="T">type of object to be saved</typeparam>
        /// <param name="stream">stream to be saved into</param>
        /// <param name="toBeSaved">Object itself</param>
        public static void Save<T>(Stream stream, T toBeSaved)
        {
            using (stream)
            {
                DataContractJsonSerializer jsonSerializer =
                    new DataContractJsonSerializer(typeof(T));
                jsonSerializer.WriteObject(stream, toBeSaved);
            }
        }
    }
}
