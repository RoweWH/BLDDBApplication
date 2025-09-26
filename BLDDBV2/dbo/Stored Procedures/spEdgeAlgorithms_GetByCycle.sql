CREATE PROCEDURE [dbo].[spEdgeAlgorithms_GetByCycle]
	@Buffer nvarchar(2),
	@First nvarchar(2),
	@Second nvarchar(2)
AS
select Id, [Algorithm]
from EdgeAlgorithms
where CycleId = 
(select Id from EdgeCycles
where Buffer = (select Id from EdgePieces where Piece = @Buffer) 
and First = (select Id from EdgePieces where Piece = @First) 
and Second = (select Id from EdgePieces where Piece = @Second));