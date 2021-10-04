using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clippy.UrlParamEncoder {
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class UrlParamAttribute : Attribute {
        private readonly string name;

        public UrlParamAttribute(string name) {
            this.name = name;
        }

        public virtual string Name {
            get { return this.name; }
        }
    }
}
