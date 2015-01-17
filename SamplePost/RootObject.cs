using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamplePost
{
    public class Item
    {
        //Yandex Disk için dosya klasör listeleme Json Yapısı için oluşturulmuş bir sınıf
        public string path { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string modified { get; set; }
        public string created { get; set; }
    }

    public class Embedded
    {
        public string sort { get; set; }
        public string path { get; set; }
        public List<Item> items { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
    }

    public class RootObject
    {
        public Embedded _embedded { get; set; }
        public string name { get; set; }
        public string created { get; set; }
        public string modified { get; set; }
        public string path { get; set; }
        public string type { get; set; }
    }
}
