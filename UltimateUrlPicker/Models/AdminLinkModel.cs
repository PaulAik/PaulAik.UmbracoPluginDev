using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUrlPicker.Models
{
    public class AdminLinkModel
    {
        public string Title { get; set; }

        public string Target { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public List<int> Path { get; set; }

        public string ContentTypeOption { get; set; }
    }
}
