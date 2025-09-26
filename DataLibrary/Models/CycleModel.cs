using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public class CycleModel
    {
        public int Id { get; set; }
        public string Buffer { get; set; } = string.Empty;
        public string First {  get; set; } = string.Empty;
        public string Second { get; set; } = string.Empty;
        public CycleModel() 
        {
        }
        public CycleModel(string buffer, string first, string second)
        {
            Buffer = buffer;
            First = first;
            Second = second;
        }
    }
}
