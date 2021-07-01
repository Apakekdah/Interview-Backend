using System;
using System.Collections.Generic;

namespace Moduit.Interview.Models
{
    public class ObjectClass
    {
        public int Id { get; set; }
        public int Category { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Footer { get; set; }
        public List<string> Tags { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<ObjectClass> Items { get; set; }
    }

    public class FilterObject
    {
        public FilterData Filter { get; set; }
        public IEnumerable<ObjectClass> Response { get; set; }
    }

    public class FilterData
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Tags { get; set; }

    }
}
