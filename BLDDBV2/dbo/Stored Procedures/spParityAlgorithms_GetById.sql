CREATE PROCEDURE [dbo].[spParityAlgorithms_GetById]
	@Id int
AS
begin
set nocount on;
	select [Algorithm]
	from ParityAlgorithms
	where Id = @Id;
end
