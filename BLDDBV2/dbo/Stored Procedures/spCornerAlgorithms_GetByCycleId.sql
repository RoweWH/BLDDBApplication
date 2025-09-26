CREATE PROCEDURE [dbo].[spCornerAlgorithms_GetByCycleId]
	@Id int
AS
begin
set nocount on;
	select Id, [Algorithm]
	from CornerAlgorithms
	where CycleId = @Id;
end
