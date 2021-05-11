
Imports ChessBoard

<Serializable>
Public Class Rook
    Inherits Piece

    Public Property CanCastle As Boolean

    Public Sub New(row As Integer, col As Integer)
        MyBase.New(row, col)
        chletter = "r"
        CanCastle = True
    End Sub

    Public Sub New(square As Square)
        MyBase.New(square)
        chletter = "r"
        CanCastle = True
    End Sub

    Public Sub New()
        MyBase.New()
        chletter = "r"
        CanCastle = True
    End Sub

    Public Overrides Function GetMoves() As LinkedList(Of Square)()
        'Rooks move straight in any direction. Straight forward, left, right, or backward

        '4 possible paths
        Dim sMoves(3) As LinkedList(Of Square)
        Dim i As Integer = 0
        For i = 0 To sMoves.Length - 1
            sMoves(i) = New LinkedList(Of Square)
        Next

        'Vertical paths
        'Forward
        i = sqPos.Row
        Do Until i = 8
            i += 1
            sMoves(0).AddLast(New Square(i, sqPos.Column))
        Loop
        'Backward
        i = sqPos.Row
        Do Until i = 1
            i -= 1
            sMoves(1).AddLast(New Square(i, sqPos.Column))
        Loop

        'Horizontal
        'Right
        i = sqPos.Column
        Do Until i = 8
            i += 1
            sMoves(2).AddLast(New Square(sqPos.Row, i))
        Loop
        'Left
        i = sqPos.Column
        Do Until i = 1
            i -= 1
            sMoves(3).AddLast(New Square(sqPos.Row, i))
        Loop

        Return sMoves
    End Function

    Friend Overrides Function Clone() As Piece
        Dim p As New Rook(New Square(Position))
        p.Color = Color
        p.Captured = Captured
        p.CanCastle = CanCastle
        Return p
    End Function
End Class
