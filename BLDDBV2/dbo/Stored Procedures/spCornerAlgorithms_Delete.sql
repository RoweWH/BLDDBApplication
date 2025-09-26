CREATE PROCEDURE [dbo].[spCornerAlgorithms_Delete]
	@Id int
AS
begin
set nocount on;
	DELETE FROM CornerAlgorithms WHERE Id = @Id;
end
