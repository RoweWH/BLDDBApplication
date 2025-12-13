using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary.Data.CubeLibrary;

namespace DataLibrary.Models
{
    public class EdgeCycleModel : CaseModel
    {
        public string Buffer { get; set; } = string.Empty;
        public string First {  get; set; } = string.Empty;
        public string Second { get; set; } = string.Empty;
        
        public EdgeCycleModel(string buffer, string first, string second)
        {
            Buffer = buffer;
            First = first;
            Second = second;
        }
        public EdgeCycleModel Reorient()
        {
            return new EdgeCycleModel(CubeLogic.FlipEdge(Buffer), CubeLogic.FlipEdge(First), CubeLogic.FlipEdge(Second));
        }
        public EdgeCycleModel Rotate()
        {
            return new EdgeCycleModel(Second, Buffer, First);
        }
        public List<EdgeCycleModel> Variations()
        {
            List<EdgeCycleModel> variations = new List<EdgeCycleModel>();
            //All rotations
            variations.Add(this);
            variations.Add(this.Rotate());
            variations.Add(this.Rotate().Rotate());
            //All rotations flipped
            variations.Add(this.Reorient());
            variations.Add(this.Rotate().Reorient());
            variations.Add(this.Rotate().Rotate().Reorient());

            return variations;
        }
        public bool Equals(EdgeCycleModel otherCase)
        {
            if (this.Buffer == otherCase.Buffer &&
               this.First == otherCase.First &&
               this.Second == otherCase.Second)
            {
                return true;
            }
            else return false;
        }
    }
}
