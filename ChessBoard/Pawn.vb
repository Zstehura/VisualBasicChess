Imports ChessBoard

<Serializable>
Public Class Pawn
    Inherits Piece

    Public Sub New(row As Integer, col As Integer)
        MyBase.New(row, col)
        chletter = "p"
    End Sub

    Public Sub New(square As Square)
        MyBase.New(square)
        chletter = "p"
    End Sub

    Public Sub New()
        MyBase.New()
        chletter = "p"
    End Sub

    Public Overrides Function GetMoves() As LinkedList(Of Square)()
        'Pawns have different behavior from other pieces. They can only ever move forward
        '   (which can be up or down depending on the color). They can only attack diagonally
        '   but not move diagonally, and can move straight forward but not attack straight forward
        '   The first time a pawn moves, it can move 2 spaces forward, but attacking stays the same
        '   It can also move just one space on the first move, but will not have that power again

        'Since some moves can only move for the pawn and others can only attack, I will use an X after
        '   the algebraic notation to denote an attack only move and an O to denote a movement only move

        'Three paths are possible for each pawn: Diagonal left, Diagonal Right, Straight ahead
        Dim sMoves(2) As LinkedList(Of Square)
        Dim iDir As Integer '(Will either be 1 or -1 to represent going up or down
        Dim iStart As Integer '(will be either 2 or 7 to represent the frontline of either color)

        For i As Integer = 0 To sMoves.Length - 1
            sMoves(i) = New LinkedList(Of Square)
        Next
        If Color = PieceColor.Black Then
            'black pawns start in row 7 and move down the board
            iStart = 7
            iDir = -1
        Else
            'White pawns start in row 2 and move up the board
            iStart = 2
            iDir = 1
        End If

        'Check if the pawn can move at all
        If sqPos.Row <> 1 And sqPos.Row <> 8 Then

            'Pawns can attack left or right diagonally
            'Attack left
            If sqPos.Column < 8 Then
                ' It is not at the absolute right of the board
                sMoves(0).AddLast(New Square(sqPos.Row + iDir, sqPos.Column + 1))
            End If
            'Attack Right
            If sqPos.Column > 1 Then
                ' It is not at the absolute Left of the board
                sMoves(1).AddLast(New Square(sqPos.Row + iDir, sqPos.Column - 1))
            End If

            'Pawns can move forward once, or twice if it is their first move
            sMoves(2).AddLast(New Square(sqPos.Row + iDir, sqPos.Column))
            If sqPos.Row = iStart Then
                'First move so it could move up 2
                sMoves(2).AddLast(New Square(sqPos.Row + (2 * iDir), sqPos.Column))
            End If

        End If

        Return sMoves
    End Function

    Friend Overrides Function Clone() As Piece
        Dim p As New Pawn(New Square(Position))
        p.Color = Color
        p.Captured = Captured
        Return p
    End Function
End Class
