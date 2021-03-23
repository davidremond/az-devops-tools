using System.Collections.Generic;

namespace Az.DevOps.Services.Models
{
    public class TemplateReference
    {
        public TemplateReference()
        {
            Versions = new HashSet<int>();
        }

        public string Version { get; set; }
        public string TemplateName { get; set; }
        public HashSet<int> Versions { get; }
    }
}