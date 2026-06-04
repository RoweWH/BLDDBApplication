CREATE PROCEDURE [dbo].[spEdgeAlgorithms_GetById]
	@Id int
AS
begin
set nocount on;
	select [Algorithm]
	from EdgeAlgorithms
	where Id = @Id;
end
