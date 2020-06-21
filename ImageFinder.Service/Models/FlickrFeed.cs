using System.Collections.Generic;
using System.Xml.Serialization;

namespace ImageFinder.Service.Models
{
    [XmlRoot("feed", Namespace = "http://www.w3.org/2005/Atom")]
    public class FlickrFeed
    {
        [XmlElement("entry")]
        public List<Entry> Entries { get; set; }
    }

    public class Entry
    {
        [XmlElement("title")]
        public string Title { get; set; }
        [XmlElement("link")]
        public List<Link> Links { get; set; }
        [XmlElement("content")]
        public string Content { get; set; }
        [XmlElement("author")]
        public Author Author { get; set; }
    }

    public class Link
    {
        [XmlAttribute("type")]
        public string Type { get; set; }
        [XmlAttribute("href")]
        public string Href { get; set; }
    }

    public class Author
    {
        [XmlElement("name")]
        public string Name { get; set; }
    }
}