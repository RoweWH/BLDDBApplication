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
        public void setSolvedState()
        {
            Edges = (string[])SolvedCube.Edges.Clone();
            Corners = (string[])SolvedCube.Corners.Clone();
            Centers = (string[])SolvedCube.Centers.Clone();
        }

        public void turnCube(string turn)
        {
            switch (turn)
            {
                case "R":
                    applyFaceTurn(SolvedCube.tableR);
                    break;
                case "F":
                    applyFaceTurn(SolvedCube.tableF);
                    break;
                case "L":
                    applyFaceTurn(SolvedCube.tableL);
                    break;
                case "B":
                    applyFaceTurn(SolvedCube.tableB);
                    break;
                case "U":
                    applyFaceTurn(SolvedCube.tableU);
                    break;
                case "D":
                    applyFaceTurn(SolvedCube.tableD);
                    break;
                case "R'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableR);
                    }
                    break;
                case "F'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableF);
                    }
                    break;
                case "L'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableL);
                    }
                    break;
                case "B'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableB);
                    }
                    break;
                case "U'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableU);
                    }
                    break;
                case "D'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableD);
                    }
                    break;
                case "R2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableR);
                    }
                    break;
                case "F2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableF);
                    }
                    break;
                case "L2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableL);
                    }
                    break;
                case "B2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableB);
                    }
                    break;
                case "U2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableU);
                    }
                    break;
                case "D2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableD);
                    }
                    break;
                case "E":
                    applySliceTurn(SolvedCube.tableE);
                    break;
                case "S":
                    applySliceTurn(SolvedCube.tableS);
                    break;
                case "M":
                    applySliceTurn(SolvedCube.tableM);
                    break;
                case "E'":
                    for (int i = 0; i < 3; i++)
                    {
                        applySliceTurn(SolvedCube.tableE);
                    }
                    break;
                case "S'":
                    for (int i = 0; i < 3; i++)
                    {
                        applySliceTurn(SolvedCube.tableS);
                    }
                    break;
                case "M'":
                    for (int i = 0; i < 3; i++)
                    {
                        applySliceTurn(SolvedCube.tableM);
                    }
                    break;
                case "E2":
                    for (int i = 0; i < 2; i++)
                    {
                        applySliceTurn(SolvedCube.tableE);
                    }
                    break;
                case "S2":
                    for (int i = 0; i < 2; i++)
                    {
                        applySliceTurn(SolvedCube.tableS);
                    }
                    break;
                case "M2":
                    for (int i = 0; i < 2; i++)
                    {
                        applySliceTurn(SolvedCube.tableM);
                    }
                    break;
                case "Uw":
                    applyFaceTurn(SolvedCube.tableU);
                    for (int i = 0; i < 3; i++)
                    {
                        applySliceTurn(SolvedCube.tableE);
                    }
                    break;
                case "Uw'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableU);
                    }
                    applySliceTurn(SolvedCube.tableE);
                    break;
                case "Uw2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableU);
                        applySliceTurn(SolvedCube.tableE);
                    }
                    break;
                case "Dw'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableD);
                        applySliceTurn(SolvedCube.tableE);
                    }
                    break;
                case "Dw":
                    {
                        applyFaceTurn(SolvedCube.tableD);
                        applySliceTurn(SolvedCube.tableE);
                    }
                    break;
                case "Dw2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableD);
                        applySliceTurn(SolvedCube.tableE);
                    }
                    break;
                case "Fw'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableF);
                        applySliceTurn(SolvedCube.tableS);
                    }
                    break;
                case "Fw":
                    {
                        applyFaceTurn(SolvedCube.tableF);
                        applySliceTurn(SolvedCube.tableS);
                    }
                    break;
                case "Fw2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableF);
                        applySliceTurn(SolvedCube.tableS);
                    }
                    break;
                case "Lw'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableL);
                        applySliceTurn(SolvedCube.tableM);
                    }
                    break;
                case "Lw":
                    {
                        applyFaceTurn(SolvedCube.tableL);
                        applySliceTurn(SolvedCube.tableM);
                    }
                    break;
                case "Lw2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableL);
                        applySliceTurn(SolvedCube.tableM);
                    }
                    break;
                case "Bw'":
                    applySliceTurn(SolvedCube.tableS);
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableB);

                    }
                    break;
                case "Bw":
                    {
                        applyFaceTurn(SolvedCube.tableB);
                        for (int i = 0; i < 3; i++)
                        {
                            applySliceTurn(SolvedCube.tableS);
                        }

                    }
                    break;
                case "Bw2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableB);
                        applySliceTurn(SolvedCube.tableS);
                    }
                    break;
                case "Rw'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableR);
                    }
                    applySliceTurn(SolvedCube.tableM);
                    break;
                case "Rw":
                    {
                        applyFaceTurn(SolvedCube.tableR);
                        for (int i = 0; i < 3; i++)
                        {
                            applySliceTurn(SolvedCube.tableM);
                        }

                    }
                    break;
                case "Rw2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableR);
                        applySliceTurn(SolvedCube.tableM);
                    }
                    break;
                case "u":
                    applyFaceTurn(SolvedCube.tableU);
                    for (int i = 0; i < 3; i++)
                    {
                        applySliceTurn(SolvedCube.tableE);
                    }
                    break;
                case "u'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableU);
                    }
                    applySliceTurn(SolvedCube.tableE);
                    break;
                case "u2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableU);
                        applySliceTurn(SolvedCube.tableE);
                    }
                    break;
                case "d'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableD);
                        applySliceTurn(SolvedCube.tableE);
                    }
                    break;
                case "d":
                    {
                        applyFaceTurn(SolvedCube.tableD);
                        applySliceTurn(SolvedCube.tableE);
                    }
                    break;
                case "d2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableD);
                        applySliceTurn(SolvedCube.tableE);
                    }
                    break;
                case "f'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableF);
                        applySliceTurn(SolvedCube.tableS);
                    }
                    break;
                case "f":
                    {
                        applyFaceTurn(SolvedCube.tableF);
                        applySliceTurn(SolvedCube.tableS);
                    }
                    break;
                case "f2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableF);
                        applySliceTurn(SolvedCube.tableS);
                    }
                    break;
                case "l'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableL);
                        applySliceTurn(SolvedCube.tableM);
                    }
                    break;
                case "l":
                    {
                        applyFaceTurn(SolvedCube.tableL);
                        applySliceTurn(SolvedCube.tableM);
                    }
                    break;
                case "l2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableL);
                        applySliceTurn(SolvedCube.tableM);
                    }
                    break;
                case "b'":
                    applySliceTurn(SolvedCube.tableS);
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableB);

                    }
                    break;
                case "b":
                    {
                        applyFaceTurn(SolvedCube.tableB);
                        for (int i = 0; i < 3; i++)
                        {
                            applySliceTurn(SolvedCube.tableS);
                        }

                    }
                    break;
                case "b2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableB);
                        applySliceTurn(SolvedCube.tableS);
                    }
                    break;
                case "r'":
                    for (int i = 0; i < 3; i++)
                    {
                        applyFaceTurn(SolvedCube.tableR);
                    }
                    applySliceTurn(SolvedCube.tableM);
                    break;
                case "r":
                    {
                        applyFaceTurn(SolvedCube.tableR);
                        for (int i = 0; i < 3; i++)
                        {
                            applySliceTurn(SolvedCube.tableM);
                        }

                    }
                    break;
                case "r2":
                    for (int i = 0; i < 2; i++)
                    {
                        applyFaceTurn(SolvedCube.tableR);
                        applySliceTurn(SolvedCube.tableM);
                    }
                    break;
            }
        }

        public string Orient(string piece, int direction)
        {
            //orient the piece based on the direction provided
            if (piece.Length == 2) //edge piece
            {
                if (direction == 1)
                {
                    return CubeLogic.FlipEdge(piece);
                }
                else return piece;
            }
            else if (piece.Length == 3) //corner piece
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

