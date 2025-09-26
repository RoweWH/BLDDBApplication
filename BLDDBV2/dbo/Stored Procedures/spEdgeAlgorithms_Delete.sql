CREATE PROCEDURE [dbo].[spEdgeAlgorithms_Delete]
	@Id int
AS
begin
set nocount on;
	DELETE FROM EdgeAlgorithms WHERE Id = @Id;
end

