CREATE PROCEDURE [dbo].[spParityAlgorithms_Delete]
	@Id int
AS
begin
set nocount on;
	DELETE FROM ParityAlgorithms WHERE Id = @Id;
end
