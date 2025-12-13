using DataLibrary.Models;

namespace DataLibrary.Data
{
    public interface IAlgorithmData
    {
        Task<int> AddNewCase(CaseModel newCase);
        Task<CaseModel> CorrectCase(CaseModel caseToFix);
        Task<int> DeleteAlg(AlgorithmModel algorithm);
        Task<int> GetCaseId(CaseModel caseModel);
        Task<int> InsertAlg(string newAlgorithm);
        Task<int> InsertAlgByCase(CaseModel caseAndAlgorithm);
        Task<List<AlgorithmModel>> LoadAlgorithms(CaseModel caseToLoad);
    }
}