using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FESOVSE.Data
{
    class CharacterClassDatabase
    {
        readonly XDocument data;

        public CharacterClassDatabase()
        {
            string xml = Properties.Resources.Classes;
            System.IO.StringReader myXml = new(xml);
            data = XDocument.Load(myXml);
        }

        private static CharacterClass FromElement(XElement row)
        {
            var id = row.Attribute("id").Value;
            var name = row.Attribute("name").Value;
            return new CharacterClass
            {
                ClassID = id,
                Name = name
            };
        }

        public List<CharacterClass> GetAll()
        {
            IEnumerable<XElement> elements = from x in data.Root.Descendants("class") select x;
            List<CharacterClass> cc = [];
            foreach(XElement e in elements)
            {
                cc.Add(FromElement(e));
            }
            return cc;
        }
    }
}
