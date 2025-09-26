using DataLibrary.Models;

namespace DataLibrary.Data
{
    public interface IAlgorithmData
    {
        Task<CycleModel> CorrectCornerCaseCycle(CycleModel cornerCase);
        Task<CycleModel> CorrectEdgeCaseCycle(CycleModel edgeCase);
        Task<int> DeleteCornerAlg(int algId);
        Task<int> DeleteEdgeAlg(int algId);
        Task<int> GetCornerCaseId(CycleModel cornerCase);
        Task<int> GetEdgeCaseId(CycleModel edgeCase);
        Task<int> InsertCornerAlg(AlgorithmModel newAlgorithm);
        Task<int> InsertCornerAlgByCase(CycleModel cornerCase, AlgorithmModel newAlgorithm);
        Task<int> InsertEdgeAlg(AlgorithmModel newAlgorithm);
        Task<int> InsertEdgeAlgByCase(CycleModel edgeCase, AlgorithmModel newAlgorithm);
        Task<List<AlgorithmModel>> LoadCornerAlgorithms(CycleModel cornerCase);
        Task<List<AlgorithmModel>> LoadCornerAlgorithms(int id);
        Task<List<AlgorithmModel>> LoadEdgeAlgorithms(CycleModel edgeCase);
        Task<List<AlgorithmModel>> LoadEdgeAlgorithms(int id);
    }
}