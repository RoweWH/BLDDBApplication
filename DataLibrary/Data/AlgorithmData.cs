using Dapper;
using DataLibrary.Data.CubeLibrary;
using DataLibrary.Db;
using DataLibrary.Models;
using Microsoft.Identity.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
        /// <summary>
        /// There are 6 possible ways an edge case can be written due to buffer piece choice and edge orientation.
        /// This method determines which of the 6 possible combinations is the correct one stored in the database
        /// </summary>
        /// <param name="edgeCase"></param>
        /// <returns>corrected edge case</returns>
        public async Task<CycleModel> CorrectEdgeCaseCycle(CycleModel edgeCase)
        {
            List<CycleModel> possibleCases = new List<CycleModel>();
            possibleCases.Add(edgeCase);
            possibleCases.Add(CubeLogic.FlipEdgeCycle(edgeCase));
            possibleCases.Add(CubeLogic.RotateCycle(edgeCase));
            possibleCases.Add(CubeLogic.FlipEdgeCycle(CubeLogic.RotateCycle(edgeCase)));
            possibleCases.Add(CubeLogic.RotateCycle(CubeLogic.RotateCycle(edgeCase)));
            possibleCases.Add(CubeLogic.FlipEdgeCycle(CubeLogic.RotateCycle(CubeLogic.RotateCycle(edgeCase))));
            foreach (var possibleCase in possibleCases)
            {
                var foundId = await GetEdgeCaseId(possibleCase);
                if (foundId > 0)
                {
                    edgeCase = possibleCase;
                    break;
                }
            }
            return edgeCase;

        
        }
        /// <summary>
        /// There are 9 possible ways a corner case can be written due to buffer piece choice and orientation
        /// This method determines which of the 9 combinations is the correct one stored in the database
        /// </summary>
        /// <param name="cornerCase"></param>
        /// <returns>corrected corner case</returns>
        public async Task<CycleModel> CorrectCornerCaseCycle(CycleModel cornerCase)
        {
            List<CycleModel> possibleCases = new List<CycleModel>();
            possibleCases.Add(cornerCase);
            possibleCases.Add(CubeLogic.TwistCornerCycle(cornerCase));
            possibleCases.Add(CubeLogic.TwistCornerCycle(CubeLogic.TwistCornerCycle(cornerCase)));
            possibleCases.Add(CubeLogic.RotateCycle(cornerCase));
            possibleCases.Add(CubeLogic.TwistCornerCycle(CubeLogic.RotateCycle(cornerCase)));
            possibleCases.Add(CubeLogic.TwistCornerCycle(CubeLogic.TwistCornerCycle((CubeLogic.RotateCycle(cornerCase)))));
            possibleCases.Add(CubeLogic.RotateCycle(CubeLogic.RotateCycle(cornerCase)));
            possibleCases.Add(CubeLogic.TwistCornerCycle(CubeLogic.RotateCycle(CubeLogic.RotateCycle(cornerCase))));
            possibleCases.Add(CubeLogic.TwistCornerCycle(CubeLogic.TwistCornerCycle((CubeLogic.RotateCycle(CubeLogic.RotateCycle(cornerCase))))));
            foreach (var possibleCase in possibleCases)
            {
                var foundId = await GetCornerCaseId(possibleCase);
                if (foundId > 0)
                {
                    cornerCase = possibleCase;
                    break;
                }
            }
            return cornerCase;
        }
        public async Task<List<AlgorithmModel>> LoadEdgeAlgorithms(CycleModel edgeCase)
        {
            edgeCase = await CorrectEdgeCaseCycle(edgeCase);
            return await _dataAccess.LoadData<AlgorithmModel, dynamic>("dbo.spEdgeAlgorithms_GetByCycle",
                                                                       new { Buffer = edgeCase.Buffer, First = edgeCase.First, Second = edgeCase.Second },
                                                                       _connectionString.SqlConnectionName);
        }
        public async Task<List<AlgorithmModel>> LoadCornerAlgorithms(CycleModel cornerCase)
        {
            cornerCase = await CorrectCornerCaseCycle(cornerCase);
            return await _dataAccess.LoadData<AlgorithmModel, dynamic>("dbo.spCornerAlgorithms_GetByCycle",
                                                                       new { Buffer = cornerCase.Buffer, First = cornerCase.First, Second = cornerCase.Second },
                                                                       _connectionString.SqlConnectionName);

        }

        public async Task<List<AlgorithmModel>> LoadCornerAlgorithms(int id)
        {
            return await _dataAccess.LoadData<AlgorithmModel, dynamic>("dbo.spCornerAlgorithms_GetByCycleId",
                                                                       new { id = id },
                                                                       _connectionString.SqlConnectionName);
        }
        public async Task<List<AlgorithmModel>> LoadEdgeAlgorithms(int id)
        {
            return await _dataAccess.LoadData<AlgorithmModel, dynamic>("dbo.spEdgeAlgorithms_GetByCycleId",
                                                                       new { id = id },
                                                                       _connectionString.SqlConnectionName);
        }

        public async Task<int> InsertEdgeAlg(AlgorithmModel newAlgorithm)
        {
            var foundCase = CubeLogic.FindEdgeCase(newAlgorithm.Algorithm);
            if (foundCase.Buffer != string.Empty)
            {
                var trueCase = await CorrectEdgeCaseCycle(foundCase);
                DynamicParameters p = new DynamicParameters();
                p.Add("Buffer", trueCase.Buffer);
                p.Add("First", trueCase.First);
                p.Add("Second", trueCase.Second);
                p.Add("Algorithm", newAlgorithm.Algorithm);
                p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);

                await _dataAccess.SaveData("dbo.spEdgeAlgorithms_InsertByCycle", p, _connectionString.SqlConnectionName);
                return p.Get<int>("Id");
            }
            else return 0;
        }

        public async Task<int> InsertEdgeAlgByCase(CycleModel edgeCase, AlgorithmModel newAlgorithm)
        {
            var foundCase = CubeLogic.FindEdgeCase(newAlgorithm.Algorithm);
            if (foundCase.Buffer != null)
            {
                var trueCase = await CorrectEdgeCaseCycle(foundCase);
                var correctedEdgeCase = await CorrectEdgeCaseCycle(edgeCase);
                if (trueCase.Buffer == correctedEdgeCase.Buffer && trueCase.First == correctedEdgeCase.First && trueCase.Second == correctedEdgeCase.Second)
                {
                    DynamicParameters p = new DynamicParameters();
                    p.Add("Buffer", trueCase.Buffer);
                    p.Add("First", trueCase.First);
                    p.Add("Second", trueCase.Second);
                    p.Add("Algorithm", newAlgorithm.Algorithm);
                    p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
                    await _dataAccess.SaveData("dbo.spEdgeAlgorithms_InsertByCycle", p, _connectionString.SqlConnectionName);
                    return p.Get<int>("Id");
                }
                else return 0;
            }
            else return 0;
        }

        public async Task<int> InsertCornerAlg(AlgorithmModel newAlgorithm)
        {
            var foundCase = CubeLogic.FindCornerCase(newAlgorithm.Algorithm);
            if (foundCase.Buffer != string.Empty)
            {
                var trueCase = await CorrectCornerCaseCycle(foundCase);
                DynamicParameters p = new DynamicParameters();
                p.Add("Buffer", trueCase.Buffer);
                p.Add("First", trueCase.First);
                p.Add("Second", trueCase.Second);
                p.Add("Algorithm", newAlgorithm.Algorithm);
                p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);

                await _dataAccess.SaveData("dbo.spCornerAlgorithms_InsertByCycle", p, _connectionString.SqlConnectionName);
                return p.Get<int>("Id");
            }
            else return 0;

        }

        public async Task<int> InsertCornerAlgByCase(CycleModel cornerCase, AlgorithmModel newAlgorithm)
        {
            var foundCase = CubeLogic.FindCornerCase(newAlgorithm.Algorithm);
            if (foundCase.Buffer != null)
            {
                var trueCase = await CorrectCornerCaseCycle(foundCase);
                var correctedCornerCase = await CorrectCornerCaseCycle(cornerCase);
                if (trueCase.Buffer == correctedCornerCase.Buffer && trueCase.First == correctedCornerCase.First && trueCase.Second == correctedCornerCase.Second)
                {
                    DynamicParameters p = new DynamicParameters();
                    p.Add("Buffer", trueCase.Buffer);
                    p.Add("First", trueCase.First);
                    p.Add("Second", trueCase.Second);
                    p.Add("Algorithm", newAlgorithm.Algorithm);
                    p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
                    await _dataAccess.SaveData("dbo.spCornerAlgorithms_InsertByCycle", p, _connectionString.SqlConnectionName);
                    return p.Get<int>("Id");
                }
                else return 0;
            }
            else return 0;
        }

        public async Task<int> DeleteEdgeAlg(int algId)
        {
            return await _dataAccess.SaveData("dbo.spEdgeAlgorithms_Delete",
                                        new
                                        {
                                            Id = algId
                                        },
                                        _connectionString.SqlConnectionName);
        }

        public async Task<int> DeleteCornerAlg(int algId)
        {
            return await _dataAccess.SaveData("dbo.spCornerAlgorithms_Delete",
                                        new
                                        {
                                            Id = algId
                                        },
                                        _connectionString.SqlConnectionName);
        }

        public async Task<int> GetEdgeCaseId(CycleModel edgeCase)
        {
            return (await _dataAccess.LoadData<int, dynamic>("dbo.spEdgeCases_GetId",
                                                                       new { Buffer = edgeCase.Buffer, First = edgeCase.First, Second = edgeCase.Second },
                                                                       _connectionString.SqlConnectionName)).FirstOrDefault();
        }

        public async Task<int> GetCornerCaseId(CycleModel cornerCase)
        {
            return (await _dataAccess.LoadData<int, dynamic>("dbo.spCornerCases_GetId",
                                                                       new { Buffer = cornerCase.Buffer, First = cornerCase.First, Second = cornerCase.Second },
                                                                       _connectionString.SqlConnectionName)).FirstOrDefault();
        }

    }
}