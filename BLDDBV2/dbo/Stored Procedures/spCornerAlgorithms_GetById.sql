CREATE PROCEDURE [dbo].[spCornerAlgorithms_GetById]
	@Id int
AS
begin
set nocount on;
	select [Algorithm]
	from CornerAlgorithms
	where Id = @Id;
end
