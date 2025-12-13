using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary.Data.CubeLibrary;

namespace DataLibrary.Models
{
    public class CornerCycleModel : CaseModel
    {
        public string Buffer { get; set; } = string.Empty;
        public string First { get; set; } = string.Empty;
        public string Second { get; set; } = string.Empty;

        public CornerCycleModel(string buffer, string first, string second)
        {
            Buffer = buffer;
            First = first;
            Second = second;
        }
        public CornerCycleModel Reorient()
        {
            return new CornerCycleModel(CubeLogic.TwistCorner(Buffer), CubeLogic.TwistCorner(First), CubeLogic.TwistCorner(Second));
        }
        public CornerCycleModel Rotate()
        {
            return new CornerCycleModel(Second, Buffer, First);
        }
        public List<CornerCycleModel> Variations()
        {
            List<CornerCycleModel> variations = new List<CornerCycleModel>();
            //All rotations
            variations.Add(this);
            variations.Add(this.Rotate());
            variations.Add(this.Rotate().Rotate());
            //All rotations twisted clockwise
            variations.Add(this.Reorient());
            variations.Add(this.Rotate().Reorient());
            variations.Add(this.Rotate().Rotate().Reorient());
            //All rotations twisted counter-clockwise
            variations.Add(this.Reorient().Reorient());
            variations.Add(this.Rotate().Reorient().Reorient());
            variations.Add(this.Rotate().Rotate().Reorient().Reorient());

            return variations;
        }
        public bool Equals(CornerCycleModel otherCase)
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

