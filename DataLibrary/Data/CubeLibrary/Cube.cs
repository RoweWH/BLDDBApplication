using DataLibrary.Data;
using DataLibrary.Data.CubeLibrary;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;


namespace DataLibrary.Data.CubeLibrary
{

    public class Cube
    {

        public string[] Edges;

        public string[] Corners;

        public string[] Centers;
        public Cube()
        {
            setSolvedState();
        }
        public void Print()
        {
            foreach (var edge in Edges) Console.WriteLine(edge);
            foreach(var corner in Corners) Console.WriteLine(corner);
            foreach(var center in Centers) Console.WriteLine(center);
        }
        public void setSolvedState()
        {
            Edges = (string[])SolvedCube.Edges.Clone();
            Corners = (string[])SolvedCube.Corners.Clone();
            Centers = (string[])SolvedCube.Centers.Clone();
        }
        public bool CentersSolved()
        {
            if (Centers[0] == "U" && Centers[1] == "F" && Centers[2] == "L") return true;
            else return false;
        }
        public string[] GetCenterFix()
        {
            List<string> prefix = new List<string>();
            if (Centers[2] == "U" || Centers[2] == "D")
            {
                prefix.Add("z");
                turnCube("z");
            }
            while (Centers[0] != "U")
            {
                prefix.Add("x");
                turnCube("x");
            }
            while (Centers[1] != "F")
            {
                prefix.Add("y");
                turnCube("y");
            }
            return prefix.ToArray();
        }
        public bool turnCube(string turn)
        {
            switch (turn)
            {
                case "R":
                    applyFaceTurn(SolvedCube.tableR);
                    return true;
                case "F":
                    applyFaceTurn(SolvedCube.tableF);
                    return true;
                case "L":
                    applyFaceTurn(SolvedCube.tableL);
                    return true;
                case "B":
                    applyFaceTurn(SolvedCube.tableB);
                    return true;
                case "U":
                    applyFaceTurn(SolvedCube.tableU);
                    return true;
                case "D":
                    applyFaceTurn(SolvedCube.tableD);
                    return true;
                case "R'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableR);
                    }
                    return true;
                case "F'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableF);
                    }
                    return true;
                case "L'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableL);
                    }
                    return true;
                case "B'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableB);
                    }
                    return true;
                case "U'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableU);
                    }
                    return true;
                case "D'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableD);
                    }
                    return true;
                case "R2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableR);
                    }
                    return true;
                case "F2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableF);
                    }
                    return true;
                case "L2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableL);
                    }
                    return true;
                case "B2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableB);
                    }
                    return true;
                case "U2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableU);
                    }
                    return true;
                case "D2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableD);
                    }
                    return true;
                case "R2'":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableR);
                    }
                    return true;
                case "F2'":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableF);
                    }
                    return true;
                case "L2'":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableL);
                    }
                    return true;
                case "B2'":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableB);
                    }
                    return true;
                case "U2'":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableU);
                    }
                    return true;
                case "D2'":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableD);
                    }
                    return true;
                case "E":
                    applySliceTurn(SolvedCube.tableE);
                    return true;
                case "S":
                    applySliceTurn(SolvedCube.tableS);
                    return true;
                case "M":
                    applySliceTurn(SolvedCube.tableM);
                    return true;
                case "E'":
                    for (int i = 0; i < 3; i++)
                    {
                        applySliceTurn(SolvedCube.tableE);
                    }
                    return true;
                case "S'":
                    for (int i = 0; i < 3; i++)
                    {
                        applySliceTurn(SolvedCube.tableS);
                    }
                    return true;
                case "M'":
                    for (int i = 0; i < 3; i++)
                    {
                        applySliceTurn(SolvedCube.tableM);
                    }
                    return true;
                case "E2":
                    for (int i = 0; i < 2; i++)
                    {
                        applySliceTurn(SolvedCube.tableE);
                    }
                    return true;
                case "S2":
                    for (int i = 0; i < 2; i++)
                    {
                        applySliceTurn(SolvedCube.tableS);
                    }
                    return true;
                case "M2":
                    for (int i = 0; i < 2; i++)
                    {
                        applySliceTurn(SolvedCube.tableM);
                    }
                    return true;
                case "E2'":
                    for (int i = 0; i < 2; i++)
                    {
                        applySliceTurn(SolvedCube.tableE);
                    }
                    return true;
                case "S2'":
                    for (int i = 0; i < 2; i++)
                    {
                        applySliceTurn(SolvedCube.tableS);
                    }
                    return true;
                case "M2'":
                    for (int i = 0; i < 2; i++)
                    {
                        applySliceTurn(SolvedCube.tableM);
                    }
                    return true;
                case "Uw":
                    applyFaceTurn(SolvedCube.tableU);
                    for (int i = 0; i < 3; i++)
                    {
                        applySliceTurn(SolvedCube.tableE);
                    }
                    return true;
                case "Uw'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableU);
                    }
                    applySliceTurn(SolvedCube.tableE);
                    return true;
                case "Uw2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableU);
                        applySliceTurn(SolvedCube.tableE);
                    }
                    return true;
                case "Uw2'":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableU);
                        applySliceTurn(SolvedCube.tableE);
                    }
                    return true;
                case "Dw'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableD);
                        applySliceTurn(SolvedCube.tableE);
                    }
                    return true;
                case "Dw":
                    {
                        applyFaceTurn(SolvedCube.tableD);
                        applySliceTurn(SolvedCube.tableE);
                    }
                    return true;
                case "Dw2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableD);
                        applySliceTurn(SolvedCube.tableE);
                    }
                    return true;
                case "Dw2'":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableD);
                        applySliceTurn(SolvedCube.tableE);
                    }
                    return true;
                case "Fw'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableF);
                        applySliceTurn(SolvedCube.tableS);
                    }
                    return true;
                case "Fw":
                    {
                        applyFaceTurn(SolvedCube.tableF);
                        applySliceTurn(SolvedCube.tableS);
                    }
                    return true;
                case "Fw2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableF);
                        applySliceTurn(SolvedCube.tableS);
                    }
                    return true;
                case "Fw2'":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableF);
                        applySliceTurn(SolvedCube.tableS);
                    }
                    return true;
                case "Lw'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableL);
                        applySliceTurn(SolvedCube.tableM);
                    }
                    return true;
                case "Lw":
                    {
                        applyFaceTurn(SolvedCube.tableL);
                        applySliceTurn(SolvedCube.tableM);
                    }
                    return true;
                case "Lw2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableL);
                        applySliceTurn(SolvedCube.tableM);
                    }
                    return true;
                case "Lw2'":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableL);
                        applySliceTurn(SolvedCube.tableM);
                    }
                    return true;
                case "Bw'":
                    applySliceTurn(SolvedCube.tableS);
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableB);

                    }
                    return true;
                case "Bw":
                    {
                        applyFaceTurn(SolvedCube.tableB);
                        for (int i = 0; i < 3; i++)
                        {
                            applySliceTurn(SolvedCube.tableS);
                        }

                    }
                    return true;
                case "Bw2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableB);
                        applySliceTurn(SolvedCube.tableS);
                    }
                    return true;
                case "Bw2'":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableB);
                        applySliceTurn(SolvedCube.tableS);
                    }
                    return true;
                case "Rw'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableR);
                    }
                    applySliceTurn(SolvedCube.tableM);
                    return true;
                case "Rw":
                    {
                        applyFaceTurn(SolvedCube.tableR);
                        for (int i = 0; i < 3; i++)
                        {
                            applySliceTurn(SolvedCube.tableM);
                        }

                    }
                    return true;
                case "Rw2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableR);
                        applySliceTurn(SolvedCube.tableM);
                    }
                    return true;
                case "Rw2'":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableR);
                        applySliceTurn(SolvedCube.tableM);
                    }
                    return true;
                case "u":
                    applyFaceTurn(SolvedCube.tableU);
                    for (int i = 0; i < 3; i++)
                    {
                        applySliceTurn(SolvedCube.tableE);
                    }
                    return true;
                case "u'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableU);
                    }
                    applySliceTurn(SolvedCube.tableE);
                    return true;
                case "u2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableU);
                        applySliceTurn(SolvedCube.tableE);
                    }
                    return true;
                case "u2'":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableU);
                        applySliceTurn(SolvedCube.tableE);
                    }
                    return true;
                case "d'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableD);
                        applySliceTurn(SolvedCube.tableE);
                    }
                    return true;
                case "d":
                    {
                        applyFaceTurn(SolvedCube.tableD);
                        applySliceTurn(SolvedCube.tableE);
                    }
                    return true;
                case "d2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableD);
                        applySliceTurn(SolvedCube.tableE);
                    }
                    return true;
                case "d2'":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableD);
                        applySliceTurn(SolvedCube.tableE);
                    }
                    return true;
                case "f'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableF);
                        applySliceTurn(SolvedCube.tableS);
                    }
                    return true;
                case "f":
                    {
                        applyFaceTurn(SolvedCube.tableF);
                        applySliceTurn(SolvedCube.tableS);
                    }
                    return true;
                case "f2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableF);
                        applySliceTurn(SolvedCube.tableS);
                    }
                    return true;
                case "f2'":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableF);
                        applySliceTurn(SolvedCube.tableS);
                    }
                    return true;
                case "l'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableL);
                        applySliceTurn(SolvedCube.tableM);
                    }
                    return true;
                case "l":
                    {
                        applyFaceTurn(SolvedCube.tableL);
                        applySliceTurn(SolvedCube.tableM);
                    }
                    return true;
                case "l2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableL);
                        applySliceTurn(SolvedCube.tableM);
                    }
                    return true;
                case "l2'":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableL);
                        applySliceTurn(SolvedCube.tableM);
                    }
                    return true;
                case "b'":
                    applySliceTurn(SolvedCube.tableS);
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableB);

                    }
                    return true;
                case "b":
                    {
                        applyFaceTurn(SolvedCube.tableB);
                        for (int i = 0; i < 3; i++)
                        {
                            applySliceTurn(SolvedCube.tableS);
                        }

                    }
                    return true;
                case "b2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableB);
                        applySliceTurn(SolvedCube.tableS);
                    }
                    return true;
                case "b2'":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableB);
                        applySliceTurn(SolvedCube.tableS);
                    }
                    return true;
                case "r'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableR);
                    }
                    applySliceTurn(SolvedCube.tableM);
                    return true;
                case "r":
                    {
                        applyFaceTurn(SolvedCube.tableR);
                        for (int i = 0; i < 3; i++)
                        {
                            applySliceTurn(SolvedCube.tableM);
                        }

                    }
                    return true;
                case "r2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableR);
                        applySliceTurn(SolvedCube.tableM);
                    }
                    return true;
                case "r2'":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableR);
                        applySliceTurn(SolvedCube.tableM);
                    }
                    return true;
                case "x'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableR);
                    }
                    applySliceTurn(SolvedCube.tableM);
                    applyFaceTurn(SolvedCube.tableL);
                    return true;
                case "x":
                    {
                        applyFaceTurn(SolvedCube.tableR);
                        for (int i = 0; i < 3; i++)
                        {
                            applySliceTurn(SolvedCube.tableM);
                            applyFaceTurn(SolvedCube.tableL);
                        }

                    }
                    return true;
                case "x2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableR);
                        applySliceTurn(SolvedCube.tableM);
                        applyFaceTurn(SolvedCube.tableL);
                    }
                    return true;
                case "x2'":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableR);
                        applySliceTurn(SolvedCube.tableM);
                        applyFaceTurn(SolvedCube.tableL);
                    }
                    return true;
                case "y'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableU);
                    }
                    applySliceTurn(SolvedCube.tableE);
                    applyFaceTurn(SolvedCube.tableD);
                    return true;
                case "y":
                    {
                        applyFaceTurn(SolvedCube.tableU);
                        for (int i = 0; i < 3; i++)
                        {
                            applySliceTurn(SolvedCube.tableE);
                            applyFaceTurn(SolvedCube.tableD);
                        }

                    }
                    return true;
                case "y2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableU);
                        applySliceTurn(SolvedCube.tableE);
                        applyFaceTurn(SolvedCube.tableD);
                    }
                    return true;
                case "y2'":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableU);
                        applySliceTurn(SolvedCube.tableE);
                        applyFaceTurn(SolvedCube.tableD);

                    }
                    return true;
                case "z'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableF);
                        applySliceTurn(SolvedCube.tableS);
                    }
                    applyFaceTurn(SolvedCube.tableB);
                    return true;
                case "z":
                    {
                        applyFaceTurn(SolvedCube.tableF);
                        applySliceTurn(SolvedCube.tableS);
                        for (int i = 0; i < 3; i++)
                        {
                            applySliceTurn(SolvedCube.tableB);
                        }

                    }
                    return true;
                case "z2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableF);
                        applySliceTurn(SolvedCube.tableS);
                        applyFaceTurn(SolvedCube.tableB);
                    }
                    return true;
                case "z2'":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableF);
                        applySliceTurn(SolvedCube.tableS);
                        applyFaceTurn(SolvedCube.tableB);
                    }
                    return true;
                default:
                    return false;
            }
        }
        public string Orient(string piece, int direction)
        {
            if (piece.Length == 2)
            {
                if (direction == 1)
                {
                    return CubeLogic.FlipEdge(piece);
                }
                else return piece;
            }
            else if (piece.Length == 3)
            {
                if (direction == -1)
                {
                    return CubeLogic.TwistCorner(piece);
                }
                else if (direction == 1)
                {
                    return CubeLogic.TwistCorner(CubeLogic.TwistCorner(piece));
                }
                else
                {
                    return piece;
                }
            }
            else return piece;
        }
        public void applyFaceTurn(int[,] table)
        {
            string temp;
            for (int i = 0; i < 3; i++)
            {
                temp = Corners[table[0, i + 1]];
                Corners[table[0, i + 1]] = Corners[table[0, i]];
                Corners[table[0, i]] = temp;
            }
            for (int i = 0; i < 3; i++)
            {
                temp = Edges[table[0, i + 5]];
                Edges[table[0, i + 5]] = Edges[table[0, i + 4]];
                Edges[table[0, i + 4]] = temp;
            }
            for (int i = 0; i < 4; i++)
            {
                Corners[table[0, i]] = Orient(Corners[table[0, i]], table[1, i]);
            }
            for (int i = 4; i < 8; i++)
            {
                Edges[table[0, i]] = Orient(Edges[table[0, i]], table[1, i]);
            }


        }
        public void applySliceTurn(int[,] table)
        {
            string temp;
            for (int i = 0; i < 3; i++)
            {
                temp = Centers[table[0, i + 1]];
                Centers[table[0, i + 1]] = Centers[table[0, i]];
                Centers[table[0, i]] = temp;
            }
            for (int i = 0; i < 3; i++)
            {
                temp = Edges[table[0, i + 5]];
                Edges[table[0, i + 5]] = Edges[table[0, i + 4]];
                Edges[table[0, i + 4]] = temp;
            }
            for (int i = 0; i < 4; i++)
            {
                Centers[table[0, i]] = Orient(Centers[table[0, i]], table[1, i]);
            }
            for (int i = 4; i < 8; i++)
            {
                Edges[table[0, i]] = Orient(Edges[table[0, i]], table[1, i]);
            }
        }


    }
}

