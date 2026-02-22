CREATE PROCEDURE [dbo].[spParityAlgorithms_GetByCase]
	@FirstEdge nvarchar(2),
	@SecondEdge nvarchar(2),
	@FirstCorner nvarchar(3),
	@SecondCorner nvarchar(3),
	@Twist nvarchar(3) = null
AS
select Id, [Algorithm]
from ParityAlgorithms
where CaseId = 
(select Id from ParityCases
where FirstEdge = (select Id from EdgePieces where Piece = @FirstEdge) 
and SecondEdge = (select Id from EdgePieces where Piece = @SecondEdge) 
and FirstCorner = (select Id from CornerPieces where Piece = @FirstCorner)
and SecondCorner = (select Id from CornerPieces where Piece = @SecondCorner)
and (Twist = (select Id from CornerPieces where Piece = @Twist) or (@Twist is null and Twist is null)));
