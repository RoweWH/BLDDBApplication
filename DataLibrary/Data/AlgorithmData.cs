using Dapper;
using DataLibrary.Data.CubeLibrary;
using DataLibrary.Db;
using DataLibrary.Models;
using Microsoft.Identity.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Data
{
    public class AlgorithmData : IAlgorithmData
    {
        private readonly IDataAccess _dataAccess;
        private readonly ConnectionStringData _connectionString;
        public AlgorithmData(IDataAccess dataAccess, ConnectionStringData connectionString)
        {
            _dataAccess = dataAccess;
            _connectionString = connectionString;
        }

        public async Task<int> GetCaseId(CaseModel caseModel)
        {
            switch (caseModel)
            {
                case EdgeCycleModel edgeCase:
                    {
                        return (await _dataAccess.LoadData<int, dynamic>("dbo.spEdgeCases_GetId",
                                                                       new { Buffer = edgeCase.Buffer, First = edgeCase.First, Second = edgeCase.Second },
                                                                       _connectionString.SqlConnectionName)).FirstOrDefault();
                    }
                case CornerCycleModel cornerCase:
                    {
                        return (await _dataAccess.LoadData<int, dynamic>("dbo.spCornerCases_GetId",
                                                                       new { Buffer = cornerCase.Buffer, First = cornerCase.First, Second = cornerCase.Second },
                                                                       _connectionString.SqlConnectionName)).FirstOrDefault();
                    }
                case ParityModel parityCase:
                    {
                        return (await _dataAccess.LoadData<int, dynamic>("dbo.spParityCases_GetId",
                                                                         new { FirstEdge = parityCase.FirstEdge, SecondEdge = parityCase.SecondEdge, FirstCorner = parityCase.FirstCorner, SecondCorner = parityCase.SecondCorner },
                                                                         _connectionString.SqlConnectionName)).FirstOrDefault();
                    }
                default:
                    return 0;
            }
        }
        public async Task<CaseModel> CorrectCase(CaseModel caseToFix)
        {
            switch (caseToFix)
            {
                case EdgeCycleModel edgeCase:
                    {
                        List<CaseModel> variations = edgeCase.Variations().Cast<CaseModel>().ToList();
                        foreach (var v in variations)
                        {
                            int id = await GetCaseId((EdgeCycleModel)v);
                            if (id > 0)
                            {
                                edgeCase = (EdgeCycleModel)v;
                                edgeCase.Id = id;
                                return edgeCase;
                            }
                        }
                        break;
                    }

                case CornerCycleModel cornerCase:
                    {
                        List<CaseModel> variations = cornerCase.Variations().Cast<CaseModel>().ToList();
                        foreach (var v in variations)
                        {
                            var foundId = await GetCaseId((CornerCycleModel)v);
                            if (foundId > 0)
                            {
                                cornerCase = (CornerCycleModel)v;
                                return cornerCase;
                            }
                        }
                        break;
                    }
                case ParityModel parityCase:
                    {
                        List<CaseModel> variations = parityCase.Variations().Cast<CaseModel>().ToList();
                        foreach (var v in variations)
                        {
                            var foundId = await GetCaseId((ParityModel)v);
                            if (foundId > 0)
                            {
                                parityCase = (ParityModel)v;
                                return parityCase;
                            }
                        }
                        break;
                    }
                default:
                    return new CaseModel();
            }
            // Ensure a return value for all code paths
            return new CaseModel();
        }
        public async Task<List<AlgorithmModel>> LoadAlgorithms(CaseModel caseToLoad)
        {
            List<AlgorithmModel> algorithms = new List<AlgorithmModel>();
            switch (caseToLoad)
            {
                case EdgeCycleModel:
                    {
                        EdgeCycleModel edgeCase = (EdgeCycleModel)await CorrectCase(caseToLoad);
                        algorithms = await _dataAccess.LoadData<AlgorithmModel, dynamic>("dbo.spEdgeAlgorithms_GetByCycle",
                                                       new { Buffer = edgeCase.Buffer, First = edgeCase.First, Second = edgeCase.Second },
                                                       _connectionString.SqlConnectionName);
                        return algorithms;
                    }
                case CornerCycleModel:
                    {
                        CornerCycleModel cornerCase = (CornerCycleModel)await CorrectCase(caseToLoad);
                        algorithms = await _dataAccess.LoadData<AlgorithmModel, dynamic>("dbo.spCornerAlgorithms_GetByCycle",
                                                                   new { Buffer = cornerCase.Buffer, First = cornerCase.First, Second = cornerCase.Second },
                                                                   _connectionString.SqlConnectionName);
                        return algorithms;
                    }
                case ParityModel:
                    {
                        ParityModel parityCase = (ParityModel)await CorrectCase(caseToLoad);
                        algorithms = await _dataAccess.LoadData<AlgorithmModel, dynamic>("dbo.spParityAlgorithms_GetByCase",
                                                                   new { FirstEdge = parityCase.FirstEdge, SecondEdge = parityCase.SecondEdge, FirstCorner = parityCase.FirstCorner, SecondCorner = parityCase.SecondCorner },
                                                                   _connectionString.SqlConnectionName);
                        return algorithms;
                    }
                default:
                    return new List<AlgorithmModel>();
            }
        }
        public async Task<bool> IsDuplicateAlgorithm(string newAlgorithm, CaseModel caseToLoad)
        {
            caseToLoad.Algorithms = await LoadAlgorithms(caseToLoad);
            string newAlgorithmExpanded = CubeLogic.ExpandAlgorithm(newAlgorithm);
            for (int i = 0; i < caseToLoad.Algorithms.Count; i++)
            {
                string thisAlgorithmExpanded = CubeLogic.ExpandAlgorithm(caseToLoad.Algorithms[i].Algorithm);
                if (thisAlgorithmExpanded == newAlgorithmExpanded)
                {
                    return true;
                }
            }
            return false;
        }
        public async Task<int> InsertAlg(string newAlgorithm)
        {
            var foundCase = CubeLogic.FindCase(newAlgorithm);
            if (foundCase != null)
            {
                switch (foundCase)
                {
                    case EdgeCycleModel:
                        {
                            var trueCase = await CorrectCase(foundCase) as EdgeCycleModel;
                            if (trueCase == null)
                            {
                                return 0;
                            }
                            if(await IsDuplicateAlgorithm(newAlgorithm, trueCase))
                            {
                                return -1;
                            }
                            DynamicParameters p = new DynamicParameters();
                            p.Add("Buffer", trueCase.Buffer);
                            p.Add("First", trueCase.First);
                            p.Add("Second", trueCase.Second);
                            p.Add("Algorithm", CubeLogic.FormatMoves(newAlgorithm));
                            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);

                            await _dataAccess.SaveData("dbo.spEdgeAlgorithms_InsertByCycle", p, _connectionString.SqlConnectionName);
                            return p.Get<int>("Id");
                        }

                    case CornerCycleModel:
                        {
                            var trueCase = await CorrectCase(foundCase) as CornerCycleModel;
                            if (trueCase == null)
                            {
                                return 0;
                            }
                            if(await IsDuplicateAlgorithm(newAlgorithm, trueCase))
                            {
                                return -1;
                            }
                            DynamicParameters p = new DynamicParameters();
                            p.Add("Buffer", trueCase.Buffer);
                            p.Add("First", trueCase.First);
                            p.Add("Second", trueCase.Second);
                            p.Add("Algorithm", CubeLogic.FormatMoves(newAlgorithm));
                            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);

                            await _dataAccess.SaveData("dbo.spCornerAlgorithms_InsertByCycle", p, _connectionString.SqlConnectionName);
                            return p.Get<int>("Id");
                        }

                    case ParityModel:
                        {
                            var trueCase = await CorrectCase(foundCase) as ParityModel;
                            if (trueCase == null)
                            {
                                int newCaseId = await AddNewCase(foundCase);
                                trueCase = await CorrectCase(foundCase) as ParityModel;
                            }
                            if (trueCase == null)
                            {
                                return 0;
                            }
                            if(await IsDuplicateAlgorithm(newAlgorithm, foundCase))
                            {
                                return -1;
                            }
                            DynamicParameters p = new DynamicParameters();
                            p.Add("FirstEdge", trueCase.FirstEdge);
                            p.Add("SecondEdge", trueCase.SecondEdge);
                            p.Add("FirstCorner", trueCase.FirstCorner);
                            p.Add("SecondCorner", trueCase.SecondCorner);
                            p.Add("Algorithm", CubeLogic.FormatMoves(newAlgorithm));
                            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
                            await _dataAccess.SaveData("dbo.spParityAlgorithms_InsertByCase", p, _connectionString.SqlConnectionName);
                            return p.Get<int>("Id");
                        }
                    default:
                        return 0;
                }
            }
            else
            {
                return 0;
            }

        }


        public async Task<int> InsertAlgByCase(CaseModel caseAndAlgorithm)
        {
            switch (caseAndAlgorithm)
            {
                case EdgeCycleModel edgeCase:
                    {
                        EdgeCycleModel givenCase = (EdgeCycleModel)await CorrectCase(edgeCase);
                        EdgeCycleModel foundCase = (EdgeCycleModel)await CorrectCase(CubeLogic.FindCase(edgeCase.Algorithms[0].Algorithm));
                        if (givenCase.Equals(foundCase))
                        {
                            DynamicParameters p = new DynamicParameters();
                            p.Add("Buffer", givenCase.Buffer);
                            p.Add("First", givenCase.First);
                            p.Add("Second", givenCase.Second);
                            p.Add("Algorithm", CubeLogic.FormatMoves(edgeCase.Algorithms[0].Algorithm));
                            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
                            await _dataAccess.SaveData("dbo.spEdgeAlgorithms_InsertByCycle", p, _connectionString.SqlConnectionName);
                            return p.Get<int>("Id");
                        }
                        else return 0;
                    }
                case CornerCycleModel cornerCase:
                    {
                        CornerCycleModel givenCase = (CornerCycleModel)await CorrectCase(cornerCase);
                        CornerCycleModel foundCase = (CornerCycleModel)await CorrectCase(CubeLogic.FindCase(cornerCase.Algorithms[0].Algorithm));
                        if (givenCase.Equals(foundCase))
                        {
                            DynamicParameters p = new DynamicParameters();
                            p.Add("Buffer", givenCase.Buffer);
                            p.Add("First", givenCase.First);
                            p.Add("Second", givenCase.Second);
                            p.Add("Algorithm", CubeLogic.FormatMoves(cornerCase.Algorithms[0].Algorithm));
                            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
                            await _dataAccess.SaveData("dbo.spCornerAlgorithms_InsertByCycle", p, _connectionString.SqlConnectionName);
                            return p.Get<int>("Id");
                        }
                        else return 0;
                    }
                case ParityModel parityCase:
                    {
                        ParityModel givenCase = (ParityModel)await CorrectCase(parityCase);
                        ParityModel foundCase = (ParityModel)await CorrectCase(CubeLogic.FindCase(parityCase.Algorithms[0].Algorithm));
                        if (givenCase.Equals(foundCase))
                        {
                            DynamicParameters p = new DynamicParameters();
                            p.Add("FirstEdge", givenCase.FirstEdge);
                            p.Add("SecondEdge", givenCase.SecondEdge);
                            p.Add("FirstCorner", givenCase.FirstCorner);
                            p.Add("SecondCorner", givenCase.SecondCorner);
                            p.Add("Algorithm", CubeLogic.FormatMoves(parityCase.Algorithms[0].Algorithm));
                            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
                            await _dataAccess.SaveData("dbo.spParityAlgorithms_InsertByCase", p, _connectionString.SqlConnectionName);
                            return p.Get<int>("Id");
                        }
                        else return 0;
                    }
                default:
                    return 0;
            }
        }
        public async Task<int> DeleteAlg(AlgorithmModel algorithm)
        {
            CaseModel caseType = await CorrectCase(CubeLogic.FindCase(algorithm.Algorithm));

            switch (caseType)
            {
                case EdgeCycleModel:
                    {
                        return await _dataAccess.SaveData("dbo.spEdgeAlgorithms_Delete",
                                        new
                                        {
                                            Id = algorithm.Id
                                        },
                                        _connectionString.SqlConnectionName);
                    }
                case CornerCycleModel:
                    {
                        return await _dataAccess.SaveData("dbo.spCornerAlgorithms_Delete",
                                        new
                                        {
                                            Id = algorithm.Id
                                        },
                                        _connectionString.SqlConnectionName);
                    }
                case ParityModel:
                    {
                        return await _dataAccess.SaveData("dbo.spParityAlgorithms_Delete",
                                        new
                                        {
                                            Id = algorithm.Id
                                        },
                                        _connectionString.SqlConnectionName);
                    }
                default:
                    return 0;
            }

        }

        public async Task<int> AddNewCase(CaseModel newCase)
        {
            switch (newCase)
            {
                case ParityModel parityCase:
                    {
                        DynamicParameters p = new DynamicParameters();
                        p.Add("FirstEdge", parityCase.FirstEdge);
                        p.Add("SecondEdge", parityCase.SecondEdge);
                        p.Add("FirstCorner", parityCase.FirstCorner);
                        p.Add("SecondCorner", parityCase.SecondCorner);
                        p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
                        await _dataAccess.SaveData("dbo.spParityCases_Add", p, _connectionString.SqlConnectionName);
                        return p.Get<int>("Id");
                    }
                default:
                    return 0;
            }

        }

    }
}