using System;
using System.Collections.Generic;

namespace Az.DevOps.Services.Models
{
    public class Pipeline
    {
        public Pipeline()
        {
            References = new List<TemplateReference>();
        }

        public string Name => Path.Trim('/').Substring(Path.LastIndexOf("/", StringComparison.InvariantCulture));
        public string Path { get; set; }

        public string Repository { get; set; }

        public IList<TemplateReference> References { get; }
    }
}