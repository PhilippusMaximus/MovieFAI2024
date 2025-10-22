using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAI.Core.Attributes
{
    // 
    [AttributeUsage(AttributeTargets.Class)]
    public class MapServiceDependencyAttribute : Attribute
    {
        private string name;

        public MapServiceDependencyAttribute(string name)
        {
            this.name = name;
        }
    }
}
