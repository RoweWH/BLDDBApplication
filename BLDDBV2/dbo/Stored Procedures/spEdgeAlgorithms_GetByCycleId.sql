CREATE PROCEDURE [dbo].[spEdgeAlgorithms_GetByCycleId]
	@Id int
AS
begin
set nocount on;
	select Id, [Algorithm]
	from EdgeAlgorithms
	where CycleId = @Id;
end
