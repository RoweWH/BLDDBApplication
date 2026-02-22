using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary.Data.CubeLibrary;

namespace DataLibrary.Models
{
    public class ParityModel : CaseModel
    {
        public string FirstEdge { get; set; } = string.Empty;
        public string SecondEdge { get; set; } = string.Empty;
        public string FirstCorner { get; set; } = string.Empty;
        public string SecondCorner { get; set; } = string.Empty;
        public string Twist {  get; set; } = string.Empty;
        public ParityModel()
        {
        }
        public void PrintCase()
        {
            Console.WriteLine(FirstEdge);
            Console.WriteLine(SecondEdge);
            Console.WriteLine(FirstCorner);
            Console.WriteLine(SecondCorner);
            Console.WriteLine(Twist);
        }
        public ParityModel(string firstEdge, string secondEdge, string firstCorner, string secondCorner, string? twist)
        {
            FirstEdge = firstEdge;
            SecondEdge = secondEdge;
            FirstCorner = firstCorner;
            SecondCorner = secondCorner;
            Twist = twist;
        }
        public ParityModel ReorientEdges()
        {
            return new ParityModel(CubeLogic.FlipEdge(FirstEdge), CubeLogic.FlipEdge(SecondEdge), FirstCorner, SecondCorner, Twist);
        }
        public ParityModel ReorientCorners()
        {
            return new ParityModel(FirstEdge, SecondEdge, CubeLogic.TwistCorner(FirstCorner), CubeLogic.TwistCorner(SecondCorner), Twist);
        }
        public List<ParityModel> Variations()
        {
            List<ParityModel> variations = new List<ParityModel>();
            //All corner swap orientations with original edge swap orientation
            variations.Add(this);
            variations.Add(this.ReorientCorners());
            variations.Add(this.ReorientCorners().ReorientCorners());
            //All corner swap orientations with flipped edge swap orientation
            variations.Add(this.ReorientEdges());
            variations.Add(this.ReorientEdges().ReorientCorners());
            variations.Add(this.ReorientEdges().ReorientCorners().ReorientCorners());

            return variations;

        }
        public bool Equals(ParityModel otherCase)
        {
            if (this.FirstEdge == otherCase.FirstEdge &&
               this.SecondEdge == otherCase.SecondEdge &&
               this.FirstCorner == otherCase.FirstCorner &&
               this.SecondCorner == otherCase.SecondCorner &&
               this.Twist == otherCase.Twist)
            {
                return true;
            }
            else return false;
        }
    }
}
