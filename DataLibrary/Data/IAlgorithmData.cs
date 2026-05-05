using DataLibrary.Models;

namespace DataLibrary.Data
{
    public interface IAlgorithmData
    {
        Task<int> AddNewCase(CaseModel newCase);
        Task<T> CorrectCase<T>(T caseToFix) where T : CaseModel;
        Task<int> DeleteAlg(AlgorithmModel algorithm);
        Task<int> GetCaseId<T>(T caseModel) where T : CaseModel;
        Task<int> InsertAlg(string newAlgorithm);
        Task<int> InsertAlgByCase(CaseModel caseAndAlgorithm);
        Task<bool> IsDuplicateAlgorithm(string newAlgorithm, CaseModel caseToLoad);
        Task<List<AlgorithmModel>> LoadAlgorithms<T>(T caseToLoad) where T : CaseModel;
        Task<List<T>> LoadAll<T>() where T : CaseModel;
        Task<List<T>> LoadCasesByBuffer<T>(string buffer);
    }
}