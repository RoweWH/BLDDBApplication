CREATE PROCEDURE [dbo].[spParityCases_Add]
	@FirstEdge nvarchar(2),
	@SecondEdge nvarchar(2),
	@FirstCorner nvarchar(3),
	@SecondCorner nvarchar(3),
	@Id int output
AS
	begin
	set nocount on;
	insert into ParityCases (FirstEdge, SecondEdge, FirstCorner, SecondCorner)
	values(
	(select Id from EdgePieces where Piece = @FirstEdge),
	(select Id from EdgePieces where Piece = @SecondEdge),
	(select Id from CornerPieces where Piece = @FirstCorner),
	(select Id from CornerPieces where Piece = @SecondCorner));
	set @Id = SCOPE_IDENTITY();
	end