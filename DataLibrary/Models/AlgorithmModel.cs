using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary.Data.CubeLibrary;

namespace DataLibrary.Models
{
    public class AlgorithmModel
    {
        public int Id { get; set; }
        public string Algorithm { get; set; } = string.Empty;
        public CaseModel Case { get; set; } = new CaseModel();
        public AlgorithmModel() { }
        public AlgorithmModel(string algorithm)
        {
            Algorithm = algorithm;
        }
        public AlgorithmModel(string algorithm, CaseModel caseModel)
        {
            Algorithm = algorithm;
            Case = caseModel;
        }
        
    }
}
