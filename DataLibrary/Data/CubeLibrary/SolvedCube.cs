using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Data.CubeLibrary
{
    /// <summary>
    /// This static class contains:
    /// 1. All starting positions for a solved cube (the original state)
    /// 2. All information for applying correct turns to a cube
    ///     Example: tableR contains all the information for turning the right (R) side of a cube
    ///             tableR = { { 3, 7, 6, 2, 3, 7, 11, 6 }, { -1, 1, -1, 1, 0, 0, 0, 0 } }
    ///             3, 7, 6, 2 are the indexes of the corners to be cycled
    ///             3, 7, 11, 6 are the indexes of the edges to be cycled
    ///             -1, 1, -1, 1 are the orientation changes to be applied to the corners
    ///             0, 0, 0, 0 are the orientation changes to be applied to the edges
    ///     For tables E, S, and M, the corner indexes are replaced with center indexes
    /// </summary>
    public static class SolvedCube
    {
        public static string[] Edges = { "UF", "UL", "UB", "UR", "FL", "BL", "BR", "FR", "DF", "DL", "DB", "DR" };

        public static string[] Corners = { "UFL", "UBL", "UBR", "UFR", "DFL", "DBL", "DBR", "DFR" };

        public static string[] Centers = { "U", "F", "L", "B", "R", "D" };

        public static int[,] tableR = new int[2, 8] { { 3, 7, 6, 2, 3, 7, 11, 6 }, { -1, 1, -1, 1, 0, 0, 0, 0 } };

        public static int[,] tableF = new int[2, 8] { { 0, 4, 7, 3, 0, 4, 8, 7 }, { -1, 1, -1, 1, 1, 1, 1, 1 } };

        public static int[,] tableL = new int[2, 8] { { 1, 5, 4, 0, 1, 5, 9, 4 }, { -1, 1, -1, 1, 0, 0, 0, 0 } };

        public static int[,] tableB = new int[2, 8] { { 2, 6, 5, 1, 2, 6, 10, 5 }, { -1, 1, -1, 1, 1, 1, 1, 1 } };

        public static int[,] tableU = new int[2, 8] { { 0, 3, 2, 1, 1, 0, 3, 2 }, { 0, 0, 0, 0, 0, 0, 0, 0 } };

        public static int[,] tableD = new int[2, 8] { { 7, 4, 5, 6, 11, 8, 9, 10 }, { 0, 0, 0, 0, 0, 0, 0, 0 } };

        public static int[,] tableE = new int[2, 8] { { 1, 2, 3, 4, 4, 5, 6, 7 }, { 0, 0, 0, 0, 1, 1, 1, 1 } };

        public static int[,] tableS = new int[2, 8] { { 0, 2, 5, 4, 1, 9, 11, 3 }, { 0, 0, 0, 0, 1, 1, 1, 1 } };

        public static int[,] tableM = new int[2, 8] { { 0, 3, 5, 1, 0, 2, 10, 8 }, { 0, 0, 0, 0, 1, 1, 1, 1 } };
    }
}
