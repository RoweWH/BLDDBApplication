using DataLibrary.Data;
using DataLibrary.Data.CubeLibrary;
using DataLibrary.Models;

namespace BLDAPI.Validation
{
    public static class InputValidation
    {
        public static bool IsValidEdgeRequest(CycleModel edgeCase)
        {
            List<string> validEdges = SolvedCube.Edges.ToList();
            if (validEdges.Contains(edgeCase.Buffer))
            {
                validEdges.Remove(edgeCase.Buffer);
            }
            else if (validEdges.Contains(CubeLogic.FlipEdge(edgeCase.Buffer)))
            {
                validEdges.Remove(CubeLogic.FlipEdge(edgeCase.Buffer));
            }
            else return false;

            if (validEdges.Contains(edgeCase.First))
            {
                validEdges.Remove(edgeCase.First);
            }
            else if (validEdges.Contains(CubeLogic.FlipEdge(edgeCase.First)))
            {
                validEdges.Remove(CubeLogic.FlipEdge(edgeCase.First));
            }
            else return false;

            if (validEdges.Contains(edgeCase.Second))
            {
                validEdges.Remove(edgeCase.Second);
            }
            else if (validEdges.Contains(CubeLogic.FlipEdge(edgeCase.Second)))
            {
                validEdges.Remove(CubeLogic.FlipEdge(edgeCase.Second));
            }
            else return false;

            return true;

        }

        public static bool IsValidCornerRequest(CycleModel cornerCase)
        {
            List<string> validCorners = SolvedCube.Corners.ToList();
            if (validCorners.Contains(cornerCase.Buffer))
            {
                validCorners.Remove(cornerCase.Buffer);
            }
            else if (validCorners.Contains(CubeLogic.TwistCorner(cornerCase.Buffer)))
            {
                validCorners.Remove(CubeLogic.TwistCorner(cornerCase.Buffer));
            }
            else if (validCorners.Contains(CubeLogic.TwistCorner(CubeLogic.TwistCorner(cornerCase.Buffer))))
            {
                validCorners.Remove(CubeLogic.TwistCorner(CubeLogic.TwistCorner(cornerCase.Buffer)));
            }
            else return false;

            if (validCorners.Contains(cornerCase.First))
            {
                validCorners.Remove(cornerCase.First);
            }
            else if (validCorners.Contains(CubeLogic.TwistCorner(cornerCase.First)))
            {
                validCorners.Remove(CubeLogic.TwistCorner(cornerCase.First));
            }
            else if (validCorners.Contains(CubeLogic.TwistCorner(CubeLogic.TwistCorner(cornerCase.First))))
            {
                validCorners.Remove(CubeLogic.TwistCorner(CubeLogic.TwistCorner(cornerCase.First)));
            }
            else return false;

            if (validCorners.Contains(cornerCase.Second))
            {
                validCorners.Remove(cornerCase.Second);
            }
            else if (validCorners.Contains(CubeLogic.TwistCorner(cornerCase.Second)))
            {
                validCorners.Remove(CubeLogic.TwistCorner(cornerCase.Second));
            }
            else if (validCorners.Contains(CubeLogic.TwistCorner(CubeLogic.TwistCorner(cornerCase.Second))))
            {
                validCorners.Remove(CubeLogic.TwistCorner(CubeLogic.TwistCorner(cornerCase.Second)));
            }
            else return false;

            return true;
        }
    }
}
