﻿using SRogue.Core.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SRogue.Core.Modules
{
    public class EntityLoader
    {
        protected Dictionary<Type, ICloneable> Cache = new Dictionary<Type, ICloneable>();

        protected const bool CreateNewInstanceIfLoadingFailed = true;
        protected const bool CreateNewFileIfLoadingFailed = true;

        public TType Load<TType>()
            where TType : class, ICloneable
        {
            string xml = string.Empty;

            if (!Cache.ContainsKey(typeof(TType)))
            {
                var path = "res/{0}.xml".FormatWith(typeof(TType).Name);
                try
                {
                    using (var reader = new StreamReader(path))
                    {
                        xml = reader.ReadToEnd();
                        var doc = new XmlDocument();
                        doc.LoadXml(xml);

                        Cache.Add(typeof(TType), doc.DeserializeAs<TType>());
                    }
                }
                catch (IOException ex)
                {
                    using (var writer = new StreamWriter("log.txt"))
                    {
                        writer.WriteLine("============={0}================".FormatWith(DateTime.Now.ToString()));
                        writer.WriteLine("Exception while reading entity data:");
                        writer.WriteLine("File: {0}".FormatWith(path));
                        writer.WriteLine("Exception: {0}".FormatWith(ex.Message));
                        writer.WriteLine("Stack Trace: {0}".FormatWith(ex.StackTrace));
                    }
                    if (CreateNewFileIfLoadingFailed)
                    {
                        using (var writer = new StreamWriter(path))
                        {
                            writer.Write(Activator.CreateInstance<TType>().Serialize());
                        }
                    }
                    if (CreateNewInstanceIfLoadingFailed)
                    {
                        return Activator.CreateInstance<TType>(); 
                    }
                    throw;
                }
                
            }

            return Cache[typeof(TType)].Clone() as TType;
        }
    }
}
