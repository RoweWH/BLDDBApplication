CREATE PROCEDURE [dbo].[spParityCases_Add]
	@FirstEdge nvarchar(2),
	@SecondEdge nvarchar(2),
	@FirstCorner nvarchar(3),
	@SecondCorner nvarchar(3),
	@Twist nvarchar(3) = null,
	@Id int output
AS
	begin
	set nocount on;
	insert into ParityCases (FirstEdge, SecondEdge, FirstCorner, SecondCorner, Twist)
	values(
	(select Id from EdgePieces where Piece = @FirstEdge),
	(select Id from EdgePieces where Piece = @SecondEdge),
	(select Id from CornerPieces where Piece = @FirstCorner),
	(select Id from CornerPieces where Piece = @SecondCorner),
	(select Id from CornerPieces where Piece = @Twist));
	set @Id = SCOPE_IDENTITY();
	end