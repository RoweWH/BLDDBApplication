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

        public static string RemoveBrackets(string input)
        {
            var sb = new StringBuilder();
            foreach (var c in input)
            {
                if (c != '[' && c != ']')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }


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
        
        public static List<List<string>> FindCycles(string[] piecesArray, string[] solvedPiecesArray)
        {
            List<string> pieces = piecesArray.ToList();
            List<string> solvedPieces = solvedPiecesArray.ToList();
            List<List<string>> cycles = new List<List<string>>();

            for (int i = 0; i < pieces.Count; i++)
            {
                if (pieces[i] == solvedPieces[i])
                {
                    pieces.RemoveAt(i);
                    solvedPieces.RemoveAt(i);
                    i--;
                }
            }

            while (pieces.Count != 0)
            {
                List<string> cycle = new List<string>();
                string bufferPiece = solvedPieces[0];
                string next = bufferPiece;
                do
                {
                    cycle.Add(next);

                    if (solvedPieces.Contains(next))
                    {
                        next = pieces[solvedPieces.IndexOf(next)];
                    }
                    else if (solvedPieces.Contains(CubeLogic.Orient(next)))
                    {
                        if (next.Length == 2)
                        {
                            next = CubeLogic.Orient(pieces[solvedPieces.IndexOf(CubeLogic.Orient(next))]);
                        }
                        else
                        {
                            next = CubeLogic.Orient(CubeLogic.Orient(pieces[solvedPieces.IndexOf(CubeLogic.Orient(next))]));
                        }
                    }
                    else if (solvedPieces.Contains(CubeLogic.Orient(CubeLogic.Orient(next))))
                    {
                        next = CubeLogic.Orient(pieces[solvedPieces.IndexOf(CubeLogic.Orient(CubeLogic.Orient(next)))]);
                    }
                } while (next != bufferPiece && next != CubeLogic.Orient(bufferPiece) && next != CubeLogic.Orient(CubeLogic.Orient(bufferPiece)));

                cycle.Add(next);

                foreach (var piece in cycle)
                {
                    if (pieces.Contains(piece))
                    {
                        pieces.Remove(piece);
                    }
                    else if (pieces.Contains(CubeLogic.Orient(piece)))
                    {
                        pieces.Remove(CubeLogic.Orient(piece));
                    }
                    else if (pieces.Contains(CubeLogic.Orient(CubeLogic.Orient(piece))))
                    {
                        pieces.Remove(CubeLogic.Orient(CubeLogic.Orient(piece)));
                    }

                    if (solvedPieces.Contains(piece))
                    {
                        solvedPieces.Remove(piece);
                    }
                    else if (solvedPieces.Contains(CubeLogic.Orient(piece)))
                    {
                        solvedPieces.Remove(CubeLogic.Orient(piece));
                    }
                    else if (solvedPieces.Contains(CubeLogic.Orient(CubeLogic.Orient(piece))))
                    {
                        solvedPieces.Remove(CubeLogic.Orient(CubeLogic.Orient(piece)));
                    }


                }
                cycles.Add(cycle);
            }
            
            return cycles;


        }
        public static CaseModel CaseTracer(Cube cube)
        {
            if (!cube.Centers.SequenceEqual(SolvedCube.Centers))
            {
                return new CaseModel();
            }
            List<List<string>> edgeCycles = FindCycles(cube.Edges, SolvedCube.Edges);
            if(edgeCycles.Count > 1)
            {
                return new CaseModel();
            }
            List<List<string>> cornerCycles = FindCycles(cube.Corners, SolvedCube.Corners);
            switch (edgeCycles.Count)
            {
                case 0:
                    if (cornerCycles.Count == 1)
                    {
                        if (cornerCycles[0].Count == 4)
                        {
                            //3 cycle corners
                            return new CornerCycleModel(cornerCycles[0][0], cornerCycles[0][1], cornerCycles[0][2]);
                        }
                    }
                    break;

                case 1:
                    if (edgeCycles[0].Count == 3)
                    {
                        if(cornerCycles.Count == 1 && cornerCycles[0].Count == 3)
                        {
                            //Parity case
                            return new ParityModel(edgeCycles[0][0], edgeCycles[0][1], cornerCycles[0][0], cornerCycles[0][1]);
                        }
                    }
                    else if (edgeCycles[0].Count == 4)
                    {
                        if(cornerCycles.Count == 0)
                        {
                            //3 cycle edges
                            return new EdgeCycleModel(edgeCycles[0][0], edgeCycles[0][1], edgeCycles[0][2]);
                        }
                    }
                    break;

                default:
                    return new CaseModel();

            }

            return new CaseModel();
        }

        public static CaseModel FindCase(string algorithm)
        {
            string expandedAlgorithm = ExpandAlgorithm(algorithm);
            var moves = expandedAlgorithm.Split(' ');
            var inverseMoves = CreateInverseString(moves).Split(' ');
            Cube newCube = new Cube();
            foreach(var m in inverseMoves)
            {
                newCube.turnCube(m);
            }
            return CaseTracer(newCube);

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
        public static string Orient(string piece)
        {
            if (piece.Length == 2)
            {
                return FlipEdge(piece);
            }
            else if (piece.Length == 3)
            {
                return TwistCorner(piece);
            }
            else
            {
                return piece;
            }
        }

    }
}
