CREATE VIEW dbo.vwCornerCases_All
AS
SELECT CornerCycles.Id as Id, B.Piece as Buffer, F.Piece as First, S.Piece as Second
FROM CornerCycles
inner join CornerPieces B
on CornerCycles.Buffer = B.Id
inner join CornerPieces F
on CornerCycles.First = F.Id
inner join CornerPieces S
on CornerCycles.Second = S.Id;