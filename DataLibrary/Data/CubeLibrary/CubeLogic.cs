using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Data.CubeLibrary
{
    public static class CubeLogic
    {
        public static List<List<string>> cornerStrings = new List<List<string>>()
        {
            new List<string> { "UFR", "RUF", "FUR" },
            new List<string> { "UFL", "FUL", "LUF" },
            new List<string> { "UBL", "LUB", "BUL" },
            new List<string> { "UBR", "BUR", "RUB" },
            new List<string> { "DBL", "BDL", "LDB" },
            new List<string> { "DFL", "LDF", "FDL" },
            new List<string> { "DFR", "FDR", "RDF" },
            new List<string> { "DBR", "RDB", "BDR" }
        };
        public static string CreateString(string[] moveList)
        {
            var sb = new StringBuilder();
            foreach (var m in moveList)
            {
                sb.Append(m);
                sb.Append(' ');
            }
            return sb.ToString();
        }
        public static string CreateInverseString(string[] moveList)
        {
            var reversedMoves = new List<string>();

            for (int i = moveList.Length - 1; i >= 0; i--)
            {
                if (moveList[i].EndsWith('\''))
                {
                    reversedMoves.Add(moveList[i].Substring(0, moveList[i].Length - 1));
                }
                else if (moveList[i].EndsWith('2'))
                {
                    reversedMoves.Add(moveList[i]);
                }
                else if (!moveList[i].EndsWith('\''))
                {
                    reversedMoves.Add(moveList[i] + "'");
                }
            }
            return CreateString(reversedMoves.ToArray());

        }
        public static string[] RemoveBrackets(string[] moveArray)
        {
            List<string> moveList = new List<string>();
            for (int i = 0; i < moveArray.Length; i++)
            {
                var sb = new StringBuilder();
                foreach (var c in moveArray[i])
                {
                    if (c != '[' && c != ']' && c != ' ' && c != '(' && c != ')')
                    {
                        sb.Append(c);
                    }
                }
                moveList.Add(sb.ToString());
            }
            return moveList.ToArray();
        }
        /// <summary>
        /// This method takes any of the acceptable algorithm notations and expands the algorithm
        /// Any unwanted punctuation will also be removed
        /// Any algorithms that are already in expanded form will pass through unaffected
        /// 
        /// The two main notations:
        /// 
        /// 1. a, b = a b a' b'
        /// 2. a/b = a b a2 b' a
        /// 
        /// Algorithms in this form will also often have setup moves
        /// 
        /// 1. c : a,b = c a b a' b' c'
        /// 2. c : a/b = c a b a2 b' a c'
        /// 
        /// </summary>
        /// <param name="algorithm">Algorithm to be expanded</param>
        /// <returns>A string of valid turns separated by spaces</returns>
        public static string ExpandAlgorithm(string algorithm)
        {
            string[] setupList = Array.Empty<string>();
            if (algorithm.Contains(':'))
            {
                int index = algorithm.IndexOf(':');
                string setupString = algorithm.Substring(0, index).Trim();
                algorithm = algorithm.Substring(index + 1).Trim();
                setupList = RemoveBrackets(setupString.Split(" "));
            }
            if (algorithm.Contains(','))
            {
                int index = algorithm.IndexOf(',');
                string interchangeString = algorithm.Substring(0, index).Trim();
                string insertString = algorithm.Substring(index + 1).Trim();
                string[] interchangeList = RemoveBrackets(interchangeString.Split(" "));
                string[] insertList = RemoveBrackets(insertString.Split(" "));
                return CreateString(setupList)
                    + CreateString(interchangeList)
                    + CreateString(insertList)
                    + CreateInverseString(interchangeList)
                    + CreateInverseString(insertList)
                    + CreateInverseString(setupList);
            }
            else if (algorithm.Contains('/'))
            {
                int index = algorithm.IndexOf('/');
                string doubleInterchange = algorithm.Substring(0, index).Trim() + " ";
                string insertString = algorithm.Substring(index + 1).Trim();
                string[] doubleInterchangeList = RemoveBrackets(doubleInterchange.Split(" "));
                string[] insertList = RemoveBrackets(insertString.Split(" "));
                return CreateString(setupList)
                    + doubleInterchange
                    + CreateString(insertList)
                    + doubleInterchange
                    + doubleInterchange
                    + CreateInverseString(insertList)
                    + doubleInterchange
                    + CreateInverseString(setupList);
            }
            else if (algorithm.Contains(")2") || algorithm.Contains(")x2") || algorithm.Contains("x2") || algorithm.Contains(") 2") || algorithm.Contains(") x2"))
            {
                int index = 0;
                if (algorithm.Contains(")2")){
                    index = algorithm.IndexOf(")2");
                }
                else if (algorithm.Contains(")x2"))
                {
                    index = algorithm.IndexOf(")x2");
                }
                else if (algorithm.Contains("x2"))
                {
                    index = algorithm.IndexOf("x2");
                }
                else if (algorithm.Contains(") 2"))
                {
                    index = algorithm.IndexOf(") 2");
                }
                else if (algorithm.Contains(") x2"))
                {
                    index = algorithm.IndexOf(") x2");
                }
                string doubledAlgorithm = algorithm.Substring(0, index).Trim() + " " + algorithm.Substring(0, index).Trim();
                string[] doubledAlgorithmList = RemoveBrackets(doubledAlgorithm.Split(" "));
                return CreateString(doubledAlgorithmList);
            }
            else
            {
                string[] algorithmList = RemoveBrackets(algorithm.Split(" "));
                return CreateString(algorithmList);
            }
        }
        /// <summary>
        /// This method determines a 3 cycle by comparing a list of unsolved pieces with the list of solved pieces
        /// </summary>
        /// <param name="pieces">These are the unsolved pieces</param>
        /// <param name="solvedPieces">These are the solved pieces</param>
        /// <returns>A CycleModel containing the pieces that are cycled</returns>
        public static CycleModel TracePieces(string[] pieces, string[] solvedPieces)
        {
            string buffer = string.Empty;
            string first = string.Empty;
            string second = string.Empty;
            int index = 0;
            bool foundBuffer = false;
            while (!foundBuffer)
            {
                if (pieces[index] == solvedPieces[index]) index++;
                else foundBuffer = true;
            }
            first = pieces[index];
            buffer = solvedPieces[index];
            if (pieces[0].Length == 2)
            {
                for (int i = 0; i < pieces.Length; i++)
                {
                    if (pieces[i] == buffer)
                    {
                        second = solvedPieces[i];
                    }
                    else if (FlipEdge(pieces[i]) == buffer)
                    {
                        second = FlipEdge(solvedPieces[i]);
                    }
                }
            }
            else if (pieces[0].Length == 3)
            {
                for (int i = 0; i < pieces.Length; i++)
                {
                    if (pieces[i] == buffer)
                    {
                        second = solvedPieces[i];
                    }
                    else if (TwistCorner(pieces[i]) == buffer)
                    {
                        second = TwistCorner(solvedPieces[i]);
                    }
                    else if (TwistCorner(TwistCorner(pieces[i])) == buffer)
                    {
                        second = TwistCorner(TwistCorner(solvedPieces[i]));
                    }
                }
            }
            return new CycleModel(buffer, first, second);

        }
        //This method determines if a cube is solved except for 3 edges
        public static bool IsValidEdgeCycle(Cube cube)
        {
            if (cube.Centers.SequenceEqual(SolvedCube.Centers))
            {
                if (cube.Corners.SequenceEqual(SolvedCube.Corners) && !cube.Edges.SequenceEqual(SolvedCube.Edges))
                {
                    int counter = 0;
                    for (int i = 0; i < cube.Edges.Length; i++)
                    {
                        if (cube.Edges[i] != SolvedCube.Edges[i])
                        {
                            counter++;
                        }
                    }
                    return (counter == 3);
                }
                else return false;
            }
            else return false;
        }
        //This method determines if a cube is solved except for 3 corners
        public static bool IsValidCornerCycle(Cube cube)
        {
            if (cube.Centers.SequenceEqual(SolvedCube.Centers))
            {
                if (cube.Edges.SequenceEqual(SolvedCube.Edges) && !cube.Corners.SequenceEqual(SolvedCube.Corners))
                {
                    int counter = 0;
                    for (int i = 0; i < cube.Corners.Length; i++)
                    {
                        if (cube.Corners[i] != SolvedCube.Corners[i])
                        {
                            counter++;
                        }
                    }
                    return (counter == 3);
                }
                else return false;
            }
            else return false;
        }

        //This method determines which edge case the algorithm solves
        public static CycleModel FindEdgeCase(string algorithm)
        {
            CycleModel edgeCase = new CycleModel();
            string expandedAlgorithm = ExpandAlgorithm(algorithm);
            var moves = expandedAlgorithm.Split(' ');
            Cube newCube = new Cube();
            for (int i = 0; i < 2; i++)
            {
                foreach (var m in moves)
                {
                    newCube.turnCube(m);
                }
            }
            if (IsValidEdgeCycle(newCube))
            {
                return TracePieces(newCube.Edges, SolvedCube.Edges);

            }
            else return new CycleModel();

        }
        //This method determines what corner case the algorithm solves
        public static CycleModel FindCornerCase(string algorithm)
        {
            CycleModel cornerCase = new CycleModel();
            string expandedAlgorithm = ExpandAlgorithm(algorithm);
            var moves = expandedAlgorithm.Split(' ');
            Cube newCube = new Cube();
            for (int i = 0; i < 2; i++)
            {
                foreach (var m in moves)
                {
                    newCube.turnCube(m);
                }
            }
            if (IsValidCornerCycle(newCube))
            {
                return TracePieces(newCube.Corners, SolvedCube.Corners);
            }
            else return new CycleModel();
            
        }
        //This method rotates a cycle clockwise
        public static CycleModel RotateCycle(CycleModel newCase)
        {
            return new CycleModel(newCase.Second, newCase.Buffer, newCase.First);
        }
        public static string FlipEdge(string edge)
        {
            return new string(edge.Reverse().ToArray());
        }

        public static string TwistCorner(string corner)
        {
            for (int i = 0; i < cornerStrings.Count; i++)
            {
                for (int j = 0; j < cornerStrings[i].Count; j++)
                {
                    if (corner == cornerStrings[i][j])
                    {
                        return cornerStrings[i][(j + 1) % cornerStrings[i].Count];
                    }
                }
            }

            return corner;
        }
        public static CycleModel FlipEdgeCycle(CycleModel newCase)
        {
            return new CycleModel(FlipEdge(newCase.Buffer), FlipEdge(newCase.First), FlipEdge(newCase.Second));
        }

        public static CycleModel TwistCornerCycle(CycleModel newCase)
        {
            return new CycleModel(TwistCorner(newCase.Buffer), TwistCorner(newCase.First), TwistCorner(newCase.Second));
        }
    }
}
