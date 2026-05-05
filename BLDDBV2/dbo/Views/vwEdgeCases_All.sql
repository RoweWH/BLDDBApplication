CREATE VIEW [dbo].[vwEdgeCases_All]
AS
SELECT EdgeCycles.Id as Id, B.Piece as Buffer, F.Piece as First, S.Piece as Second
FROM EdgeCycles
inner join EdgePieces B
on EdgeCycles.Buffer = B.Id
inner join EdgePieces F
on EdgeCycles.First = F.Id
inner join EdgePieces S
on EdgeCycles.Second = S.Id;
