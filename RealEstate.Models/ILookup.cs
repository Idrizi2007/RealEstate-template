using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Models
{
    public interface ILookup
    {
        public int Id { get; }
        public string? Name { get; }
    }
}
