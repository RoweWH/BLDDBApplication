CREATE PROCEDURE [dbo].[spParityCases_GetId]
	@FirstEdge nvarchar(2),
	@SecondEdge nvarchar(2),
	@FirstCorner nvarchar(3),
	@SecondCorner nvarchar(3)
AS
begin
set nocount on;
select Id from ParityCases where
FirstEdge = (select Id from EdgePieces where Piece = @FirstEdge) and
SecondEdge = (select Id from EdgePieces where Piece = @SecondEdge) and
FirstCorner = (select Id from CornerPieces where Piece = @FirstCorner) and
SecondCorner = (select Id from CornerPieces where Piece = @SecondCorner);
end
