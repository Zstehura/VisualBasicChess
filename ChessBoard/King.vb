Imports ChessBoard

<Serializable>
Public Class King
    Inherits Piece

    Public Property CanCastle As Boolean

    Public Sub New(row As Integer, col As Integer)
        MyBase.New(row, col)
        CanCastle = True
        chletter = "k"
    End Sub

    Public Sub New(square As Square)
        MyBase.New(square)
        CanCastle = True
        chletter = "k"
    End Sub

    Public Sub New()
        MyBase.New()
        CanCastle = True
        chletter = "k"
    End Sub

    Public Overrides Function GetMoves() As LinkedList(Of Square)()
        'The King can move 1 square in any direction

        '8 possible spots, no paths
        Dim sMoves(7) As LinkedList(Of Square)
        For i As Integer = 0 To sMoves.Length - 1
            sMoves(i) = New LinkedList(Of Square)
        Next
        Dim r As Integer = sqPos.Row, c As Integer = sqPos.Column
        If r > 1 Then sMoves(0).AddLast(New Square(r - 1, c))
        If r < 8 Then sMoves(1).AddLast(New Square(r + 1, c))
        If c > 1 Then sMoves(2).AddLast(New Square(r, c - 1))
        If c < 8 Then sMoves(3).AddLast(New Square(r, c + 1))
        If r > 1 And c > 1 Then sMoves(4).AddLast(New Square(r - 1, c - 1))
        If r < 8 And c > 1 Then sMoves(5).AddLast(New Square(r + 1, c - 1))
        If r > 1 And c < 8 Then sMoves(6).AddLast(New Square(r - 1, c + 1))
        If r < 8 And c < 8 Then sMoves(7).AddLast(New Square(r + 1, c + 1))

        Return sMoves
    End Function

    Friend Overrides Function Clone() As Piece
        Dim p As New King(New Square(Position))
        p.CanCastle = CanCastle
        p.Color = Color
        p.Captured = Captured
        Return p
    End Function
End Class
