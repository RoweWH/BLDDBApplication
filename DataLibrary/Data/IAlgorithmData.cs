using DataLibrary.Models;

namespace DataLibrary.Data
{
    public interface IAlgorithmData
    {
        Task<CaseModel> CorrectCase(CaseModel caseToFix);
        Task<int> DeleteAlg(AlgorithmModel algorithm);
        Task<int> GetCaseId(CaseModel caseModel);
        Task<int> InsertAlg(AlgorithmModel newAlgorithm);
        Task<int> InsertAlgByCase(AlgorithmModel newAlgorithm);
        Task<List<AlgorithmModel>> LoadAlgorithms(CaseModel caseToLoad);
    }
}