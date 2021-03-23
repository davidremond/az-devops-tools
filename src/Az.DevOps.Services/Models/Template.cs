using System.Collections.Generic;

namespace Az.DevOps.Services.Models
{
    public class Template
    {
        public Template()
        {
            Versions = new HashSet<string>();
        }

        public string Name { get; set; }
        public HashSet<string> Versions { get; }
    }
}