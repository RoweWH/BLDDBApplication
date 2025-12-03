CREATE PROCEDURE [dbo].[spParityAlgorithms_InsertByCase]
	@FirstEdge nvarchar(2),
	@SecondEdge nvarchar(2),
	@FirstCorner nvarchar(3),
	@SecondCorner nvarchar(3),
	@Algorithm nvarchar(200),
	@Id int output
AS
	begin
	set nocount on;
	insert into ParityAlgorithms (CaseId, [Algorithm])
	values(
	(select Id from ParityCases
	where FirstEdge = (select Id from EdgePieces where Piece = @FirstEdge) 
	and SecondEdge = (select Id from EdgePieces where Piece = @SecondEdge) 
	and FirstCorner = (select Id from CornerPieces where Piece = @FirstCorner) 
	and SecondCorner = (select Id from CornerPieces where Piece = @SecondCorner)),
	@Algorithm);
	set @Id = SCOPE_IDENTITY();
	end
