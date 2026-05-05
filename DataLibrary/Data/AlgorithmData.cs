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
using System.Runtime.CompilerServices;
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

        public async Task<int> GetCaseId<T>(T caseModel) where T : CaseModel
        {
            if (caseModel == null)
                return 0;

            return caseModel switch
            {
                EdgeCycleModel edgeCase =>
                    (await _dataAccess.LoadData<int, dynamic>(
                        "dbo.spEdgeCases_GetId",
                        new
                        {
                            Buffer = edgeCase.Buffer,
                            First = edgeCase.First,
                            Second = edgeCase.Second
                        },
                        _connectionString.SqlConnectionName)).FirstOrDefault(),

                CornerCycleModel cornerCase =>
                    (await _dataAccess.LoadData<int, dynamic>(
                        "dbo.spCornerCases_GetId",
                        new
                        {
                            Buffer = cornerCase.Buffer,
                            First = cornerCase.First,
                            Second = cornerCase.Second
                        },
                        _connectionString.SqlConnectionName)).FirstOrDefault(),

                ParityModel parityCase =>
                    (await _dataAccess.LoadData<int, dynamic>(
                        "dbo.spParityCases_GetId",
                        new
                        {
                            FirstEdge = parityCase.FirstEdge,
                            SecondEdge = parityCase.SecondEdge,
                            FirstCorner = parityCase.FirstCorner,
                            SecondCorner = parityCase.SecondCorner,
                            Twist = parityCase.Twist
                        },
                        _connectionString.SqlConnectionName)).FirstOrDefault(),

                _ => 0
            };
        }
        public async Task<T> CorrectCase<T>(T caseToFix) where T : CaseModel
        {
            if (caseToFix == null)
                return null;

            IEnumerable<T> variations = caseToFix switch
            {
                EdgeCycleModel e => e.Variations().Cast<T>(),
                CornerCycleModel c => c.Variations().Cast<T>(),
                ParityModel p => p.Variations().Cast<T>(),
                _ => Enumerable.Empty<T>()
            };

            foreach (var v in variations)
            {
                int id = v switch
                {
                    EdgeCycleModel e => await GetCaseId(e),
                    CornerCycleModel c => await GetCaseId(c),
                    ParityModel p => await GetCaseId(p),
                    _ => 0
                };

                if (id > 0)
                {
                    v.Id = id;
                    return v;
                }
            }

            return caseToFix;
        }

        public async Task<List<T>> LoadCasesByBuffer<T>(string buffer)
        {
            if (buffer.Length == 2 && typeof(T) == typeof(EdgeCycleModel))
            {
                List<EdgeCycleModel> cases = await _dataAccess.LoadData<EdgeCycleModel, dynamic>("dbo.spEdgeCases_GetByBuffer",
                                                                                                 new { Buffer = buffer, FlippedBuffer = CubeLogic.FlipEdge(buffer) },
                                                                                                 _connectionString.SqlConnectionName);
                for (int i = 0; i < cases.Count; i++)
                {
                    var variations = cases[i].Variations();
                    foreach (var v in variations)
                    {
                        if (v.Buffer == buffer)
                        {
                            cases[i] = v;
                            break;
                        }
                    }
                }
                return cases.Cast<T>().ToList();
            }
            else if (buffer.Length == 3 && typeof(T) == typeof(CornerCycleModel))
            {
                List<CornerCycleModel> cases = await _dataAccess.LoadData<CornerCycleModel, dynamic>("dbo.spCornerCases_GetByBuffer",
                                                                                                     new { Buffer = buffer, TwistedBufferOne = CubeLogic.TwistCorner(buffer), TwistedBufferTwo = CubeLogic.TwistCorner(CubeLogic.TwistCorner(buffer)) },
                                                                                                     _connectionString.SqlConnectionName);
                for (int i = 0; i < cases.Count; i++)
                {
                    var variations = cases[i].Variations();
                    foreach (var v in variations)
                    {
                        if (v.Buffer == buffer)
                        {
                            cases[i] = v;
                            break;
                        }
                    }
                }
                return cases.Cast<T>().ToList();
            }

            else return new List<T>();
        }

        public async Task<List<T>> LoadAll<T>() where T : CaseModel
        {
            if (typeof(T) == typeof(EdgeCycleModel))
            {
                return (await _dataAccess.LoadData<EdgeCycleModel, dynamic>(
                    "dbo.spEdgeCases_GetAll",
                    new { },
                    _connectionString.SqlConnectionName
                )).Cast<T>().ToList();
            }

            if (typeof(T) == typeof(CornerCycleModel))
            {
                return (await _dataAccess.LoadData<CornerCycleModel, dynamic>(
                    "dbo.spCornerCases_GetAll",
                    new { },
                    _connectionString.SqlConnectionName
                )).Cast<T>().ToList();
            }

            if (typeof(T) == typeof(ParityModel))
            {
                return (await _dataAccess.LoadData<ParityModel, dynamic>(
                    "dbo.spParityCases_GetAll",
                    new { },
                    _connectionString.SqlConnectionName
                )).Cast<T>().ToList();
            }

            throw new Exception("Unsupported type");
        }
        public async Task<List<AlgorithmModel>> LoadAlgorithms<T>(T caseToLoad) where T : CaseModel
        {
            if (caseToLoad == null)
                return new List<AlgorithmModel>();

            switch (caseToLoad)
            {
                case EdgeCycleModel edgeCase:
                    edgeCase = await CorrectCase(edgeCase);
                    caseToLoad.Id = edgeCase.Id;

                    return await _dataAccess.LoadData<AlgorithmModel, dynamic>(
                        "dbo.spEdgeAlgorithms_GetByCycle",
                        new
                        {
                            Buffer = edgeCase.Buffer,
                            First = edgeCase.First,
                            Second = edgeCase.Second
                        },
                        _connectionString.SqlConnectionName);

                case CornerCycleModel cornerCase:
                    cornerCase = await CorrectCase(cornerCase);
                    caseToLoad.Id = cornerCase.Id;

                    return await _dataAccess.LoadData<AlgorithmModel, dynamic>(
                        "dbo.spCornerAlgorithms_GetByCycle",
                        new
                        {
                            Buffer = cornerCase.Buffer,
                            First = cornerCase.First,
                            Second = cornerCase.Second
                        },
                        _connectionString.SqlConnectionName);

                case ParityModel parityCase:
                    parityCase = await CorrectCase(parityCase);
                    caseToLoad.Id = parityCase.Id;

                    return await _dataAccess.LoadData<AlgorithmModel, dynamic>(
                        "dbo.spParityAlgorithms_GetByCase",
                        new
                        {
                            FirstEdge = parityCase.FirstEdge,
                            SecondEdge = parityCase.SecondEdge,
                            FirstCorner = parityCase.FirstCorner,
                            SecondCorner = parityCase.SecondCorner,
                            Twist = parityCase.Twist
                        },
                        _connectionString.SqlConnectionName);

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
                            if (await IsDuplicateAlgorithm(newAlgorithm, trueCase))
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
                            if (await IsDuplicateAlgorithm(newAlgorithm, trueCase))
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
                            if (await IsDuplicateAlgorithm(newAlgorithm, foundCase))
                            {
                                return -1;
                            }
                            DynamicParameters p = new DynamicParameters();
                            p.Add("FirstEdge", trueCase.FirstEdge);
                            p.Add("SecondEdge", trueCase.SecondEdge);
                            p.Add("FirstCorner", trueCase.FirstCorner);
                            p.Add("SecondCorner", trueCase.SecondCorner);
                            p.Add("Twist", trueCase.Twist);
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
                        var foundCase = await CorrectCase(CubeLogic.FindCase(edgeCase.Algorithms[0].Algorithm));
                        if (foundCase is EdgeCycleModel && givenCase.Equals((EdgeCycleModel)foundCase))
                        {
                            if (await IsDuplicateAlgorithm(edgeCase.Algorithms[0].Algorithm, foundCase))
                            {
                                return -1;
                            }
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
                        var foundCase = await CorrectCase(CubeLogic.FindCase(cornerCase.Algorithms[0].Algorithm));
                        if (foundCase is CornerCycleModel && givenCase.Equals((CornerCycleModel)foundCase))
                        {
                            if (await IsDuplicateAlgorithm(cornerCase.Algorithms[0].Algorithm, foundCase))
                            {
                                return -1;
                            }
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
                        var foundCase = await CorrectCase(CubeLogic.FindCase(parityCase.Algorithms[0].Algorithm));
                        if (foundCase is ParityModel && givenCase.Equals((ParityModel)foundCase))
                        {
                            if (await IsDuplicateAlgorithm(parityCase.Algorithms[0].Algorithm, foundCase))
                            {
                                return -1;
                            }
                            DynamicParameters p = new DynamicParameters();
                            p.Add("FirstEdge", givenCase.FirstEdge);
                            p.Add("SecondEdge", givenCase.SecondEdge);
                            p.Add("FirstCorner", givenCase.FirstCorner);
                            p.Add("SecondCorner", givenCase.SecondCorner);
                            p.Add("Twist", givenCase.Twist);
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
                        p.Add("Twist", parityCase.Twist);
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