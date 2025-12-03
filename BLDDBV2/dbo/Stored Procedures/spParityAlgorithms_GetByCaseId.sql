CREATE PROCEDURE [dbo].[spParityAlgorithms_GetByCaseId]
	@Id int
AS
begin
set nocount on;
	select Id, [Algorithm]
	from ParityAlgorithms
	where CaseId = @Id;
end
