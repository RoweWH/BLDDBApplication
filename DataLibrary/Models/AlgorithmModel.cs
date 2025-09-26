using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public class AlgorithmModel
    {
        public int Id { get; set; }
        public string Algorithm { get; set; } = string.Empty;
        public AlgorithmModel() { }
        public AlgorithmModel(string algorithm)
        {
            Algorithm = algorithm;
        }
        
    }
}
