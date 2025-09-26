CREATE PROCEDURE [dbo].[spCornerAlgorithms_GetByCycle]
	@Buffer nvarchar(3),
	@First nvarchar(3),
	@Second nvarchar(3)
AS
select Id, [Algorithm]
from CornerAlgorithms
where CycleId = 
(select Id from CornerCycles
where Buffer = (select Id from CornerPieces where Piece = @Buffer) 
and First = (select Id from CornerPieces where Piece = @First) 
and Second = (select Id from CornerPieces where Piece = @Second));
