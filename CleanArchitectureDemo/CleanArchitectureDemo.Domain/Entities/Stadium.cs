using CleanArchitectureDemo.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureDemo.Domain.Entities
{
    public class Stadium : BaseAuditableEntity
    {
        public string Name { get; set; }
    }
}
