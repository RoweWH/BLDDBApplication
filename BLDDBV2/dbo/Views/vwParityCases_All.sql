CREATE VIEW [dbo].[vwParityCases_All]
AS
SELECT ParityCases.Id as Id, FE.Piece as FirstEdge, SE.Piece as SecondEdge, FC.Piece as FirstCorner, SC.Piece as SecondCorner, T.Piece as Twist
FROM ParityCases
inner join EdgePieces FE
on ParityCases.FirstEdge = FE.Id
inner join EdgePieces SE
on ParityCases.SecondEdge = SE.Id
inner join CornerPieces FC
on ParityCases.FirstCorner = FC.Id
inner join CornerPieces SC
on ParityCases.SecondCorner = SC.Id
left join CornerPieces T
on ParityCases.Twist = T.Id;