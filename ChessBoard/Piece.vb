'Base Class for each piece
Imports ChessBoard

<Serializable>
Public MustInherit Class Piece
    Public Enum PieceColor
        White
        Black
        None
    End Enum

    Protected chletter As Char
    Protected sqPos As Square

    Public Property Color As PieceColor
    Public Property Captured As Boolean
    Public ReadOnly Property Letter As Char
        Get
            Return chletter
        End Get
    End Property
    Public ReadOnly Property Position As String
        Get
            Return sqPos.ToString
        End Get
    End Property
    Public ReadOnly Property Row As Integer
        Get
            Return sqPos.Row
        End Get
    End Property
    Public ReadOnly Property Column As Integer
        Get
            Return sqPos.Column
        End Get
    End Property

    Sub New(row As Integer, col As Integer)
        sqPos = New Square(row, col)
    End Sub

    Sub New(square As Square)
        'Should recieve pieces in algebraic notation like
        ' a1, b5, d2, etc.
        sqPos = square
    End Sub

    Sub New()
        Me.New(1, 1)
    End Sub

    'Multiple linked Lists are used to return the multiple lines of movement that the board 
    '   can trace through to find any obstacles
    Public MustOverride Function GetMoves() As LinkedList(Of Square)()

    Public Sub MovePiece(row As Integer, col As Integer)
        sqPos = New Square(row, col)
    End Sub

    Public Sub MovePiece(toSq As Square)
        sqPos = toSq
    End Sub

    Friend MustOverride Function Clone() As Piece
End Class
