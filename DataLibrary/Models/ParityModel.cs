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
        public string? Twist {  get; set; } = string.Empty;
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
        public ParityModel SwapEdges()
        {
            return new ParityModel(SecondEdge, FirstEdge, FirstCorner, SecondCorner, Twist);
        }
        public ParityModel SwapCorners()
        {
            return new ParityModel(FirstEdge, SecondEdge, SecondCorner, FirstCorner, Twist);
        }
        public ParityModel SwapCornersClockwise()
        {
            return new ParityModel(FirstEdge, SecondEdge, SecondCorner, CubeLogic.TwistCorner(FirstCorner), Twist);
        }
        public ParityModel SwapCornersCounterClockwise()
        {
            return new ParityModel(FirstEdge, SecondEdge, SecondCorner, CubeLogic.TwistCorner(CubeLogic.TwistCorner(FirstCorner)), Twist);
        }
        public int TwistDirection(string twist)
        {
            string copy = CubeLogic.TwistCorner(twist);
            if (copy[0] == 'U' || copy[0] == 'D')
            {
                return 1;
            }
            else return -1;
        }
        public List<ParityModel> Variations()
        {
            List<ParityModel> variations = new List<ParityModel>();
            variations.Add(this);
            variations.Add(this.SwapEdges());
            variations.Add(this.ReorientEdges());
            variations.Add(this.SwapEdges().ReorientEdges());
            if(this.Twist == null)
            {
                variations.Add(this.SwapCorners());
                variations.Add(this.SwapEdges().SwapCorners());
                variations.Add(this.ReorientEdges().SwapCorners());
                variations.Add(this.SwapEdges().ReorientEdges().SwapCorners());

                variations.Add(this.ReorientCorners());
                variations.Add(this.ReorientCorners().ReorientCorners());
                variations.Add(this.ReorientCorners().SwapCorners());
                variations.Add(this.ReorientCorners().ReorientCorners().SwapCorners());

                variations.Add(this.SwapEdges().ReorientCorners());
                variations.Add(this.SwapEdges().ReorientCorners().ReorientCorners());
                variations.Add(this.SwapEdges().ReorientCorners().SwapCorners());
                variations.Add(this.SwapEdges().ReorientCorners().ReorientCorners().SwapCorners());

                variations.Add(this.ReorientEdges().ReorientCorners());
                variations.Add(this.ReorientEdges().ReorientCorners().ReorientCorners());
                variations.Add(this.ReorientEdges().ReorientCorners().SwapCorners());
                variations.Add(this.ReorientEdges().ReorientCorners().ReorientCorners().SwapCorners());

                variations.Add(this.SwapEdges().ReorientEdges().ReorientCorners());
                variations.Add(this.SwapEdges().ReorientEdges().ReorientCorners().ReorientCorners());
                variations.Add(this.SwapEdges().ReorientEdges().ReorientCorners().SwapCorners());
                variations.Add(this.SwapEdges().ReorientEdges().ReorientCorners().ReorientCorners().SwapCorners());
            }
            
            else if(TwistDirection(this.Twist) == 1)
            {
                variations.Add(this.SwapCornersCounterClockwise());
                variations.Add(this.SwapEdges().SwapCornersCounterClockwise());
                variations.Add(this.ReorientEdges().SwapCornersCounterClockwise());
                variations.Add(this.SwapEdges().ReorientEdges().SwapCornersCounterClockwise());

                variations.Add(this.ReorientCorners());
                variations.Add(this.ReorientCorners().ReorientCorners());
                variations.Add(this.ReorientCorners().SwapCornersCounterClockwise());
                variations.Add(this.ReorientCorners().ReorientCorners().SwapCornersCounterClockwise());

                variations.Add(this.SwapEdges().ReorientCorners());
                variations.Add(this.SwapEdges().ReorientCorners().ReorientCorners());
                variations.Add(this.SwapEdges().ReorientCorners().SwapCornersCounterClockwise());
                variations.Add(this.SwapEdges().ReorientCorners().ReorientCorners().SwapCornersCounterClockwise());

                variations.Add(this.ReorientEdges().ReorientCorners());
                variations.Add(this.ReorientEdges().ReorientCorners().ReorientCorners());
                variations.Add(this.ReorientEdges().ReorientCorners().SwapCornersCounterClockwise());
                variations.Add(this.ReorientEdges().ReorientCorners().ReorientCorners().SwapCornersCounterClockwise());

                variations.Add(this.SwapEdges().ReorientEdges().ReorientCorners());
                variations.Add(this.SwapEdges().ReorientEdges().ReorientCorners().ReorientCorners());
                variations.Add(this.SwapEdges().ReorientEdges().ReorientCorners().SwapCornersCounterClockwise());
                variations.Add(this.SwapEdges().ReorientEdges().ReorientCorners().ReorientCorners().SwapCornersCounterClockwise());
            }
            else if(TwistDirection(this.Twist) == -1)
            {
                variations.Add(this.SwapCornersClockwise());
                variations.Add(this.SwapEdges().SwapCornersClockwise());
                variations.Add(this.ReorientEdges().SwapCornersClockwise());
                variations.Add(this.SwapEdges().ReorientEdges().SwapCornersClockwise());

                variations.Add(this.ReorientCorners());
                variations.Add(this.ReorientCorners().ReorientCorners());
                variations.Add(this.ReorientCorners().SwapCornersClockwise());
                variations.Add(this.ReorientCorners().ReorientCorners().SwapCornersClockwise());

                variations.Add(this.SwapEdges().ReorientCorners());
                variations.Add(this.SwapEdges().ReorientCorners().ReorientCorners());
                variations.Add(this.SwapEdges().ReorientCorners().SwapCornersClockwise());
                variations.Add(this.SwapEdges().ReorientCorners().ReorientCorners().SwapCornersClockwise());

                variations.Add(this.ReorientEdges().ReorientCorners());
                variations.Add(this.ReorientEdges().ReorientCorners().ReorientCorners());
                variations.Add(this.ReorientEdges().ReorientCorners().SwapCornersClockwise());
                variations.Add(this.ReorientEdges().ReorientCorners().ReorientCorners().SwapCornersClockwise());

                variations.Add(this.SwapEdges().ReorientEdges().ReorientCorners());
                variations.Add(this.SwapEdges().ReorientEdges().ReorientCorners().ReorientCorners());
                variations.Add(this.SwapEdges().ReorientEdges().ReorientCorners().SwapCornersClockwise());
                variations.Add(this.SwapEdges().ReorientEdges().ReorientCorners().ReorientCorners().SwapCornersClockwise());
            }
            
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
