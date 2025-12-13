using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public class CaseModel
    {
        public int Id { get; set; }
        public List<AlgorithmModel> Algorithms { get; set; } = new List<AlgorithmModel>(); 
        public CaseModel()
        {

        }
    }
}
