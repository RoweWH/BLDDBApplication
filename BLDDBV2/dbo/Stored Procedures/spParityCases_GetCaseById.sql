CREATE PROCEDURE [dbo].[spParityCases_GetCaseById]
    @Id int
AS
SELECT
    pc.Id,
    firstEdge.Piece AS FirstEdge,
    secondEdge.Piece AS SecondEdge,
    firstCorner.Piece AS FirstCorner,
    secondCorner.Piece AS SecondCorner,
    twist.Piece AS Twist
FROM ParityCases pc
JOIN EdgePieces firstEdge
    ON pc.FirstEdge = firstEdge.Id
JOIN EdgePieces secondEdge
    ON pc.SecondEdge = secondEdge.Id
JOIN CornerPieces firstCorner
    ON pc.FirstCorner = firstCorner.Id
JOIN CornerPieces secondCorner
    ON pc.SecondCorner = secondCorner.Id
LEFT JOIN CornerPieces twist
    ON pc.Twist = twist.Id
WHERE pc.Id = @Id;