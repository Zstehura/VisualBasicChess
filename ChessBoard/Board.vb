<Serializable()>
Public Class Board
    Private oPieces(31) As Piece
    Private inCheck As Piece.PieceColor
    Private inCheckMate As Piece.PieceColor
    Private playerMove As Piece.PieceColor
    Private strLastMove As String


    Public ReadOnly Property LastMove As String
        Get
            Return strLastMove
        End Get
    End Property
    Public ReadOnly Property PlayerTurn As Piece.PieceColor
        Get
            Return playerMove
        End Get
    End Property

    Public ReadOnly Property IsCheck As Piece.PieceColor
        Get
            Return inCheck
        End Get
    End Property
    Public ReadOnly Property IsMate As Piece.PieceColor
        Get
            Return inCheckMate
        End Get
    End Property

    Sub New(bNewBoard As Boolean)
        'First set the white
        ' Row 1
        oPieces(0) = New Rook(1, 1)
        oPieces(1) = New Knight(1, 2)
        oPieces(2) = New Bishop(1, 3)
        oPieces(3) = New Queen(1, 4)
        oPieces(4) = New King(1, 5)
        oPieces(5) = New Bishop(1, 6)
        oPieces(6) = New Knight(1, 7)
        oPieces(7) = New Rook(1, 8)
        ' Row 2
        For i = 0 To 7
            oPieces(i + 8) = New Pawn(2, i + 1)
        Next
        'Set the piece colors
        For i = 0 To 15
            oPieces(i).Color = Piece.PieceColor.White
        Next

        'Next set the black
        ' Row 7
        For i = 0 To 7
            oPieces(i + 16) = New Pawn(7, i + 1)
        Next
        'Row 8
        oPieces(24) = New Rook(8, 1)
        oPieces(25) = New Knight(8, 2)
        oPieces(26) = New Bishop(8, 3)
        oPieces(27) = New Queen(8, 4)
        oPieces(28) = New King(8, 5)
        oPieces(29) = New Bishop(8, 6)
        oPieces(30) = New Knight(8, 7)
        oPieces(31) = New Rook(8, 8)
        'Set the piece colors
        For i = 16 To 31
            oPieces(i).Color = Piece.PieceColor.Black
        Next

        playerMove = Piece.PieceColor.White
        inCheck = Piece.PieceColor.None
        inCheckMate = Piece.PieceColor.None
    End Sub

    Sub New()
    End Sub

    Public Function GetPieces() As Piece()
        'Returns all Pieces on the board
        Return oPieces
    End Function

    Public Function GetPieceAt(sq As Square) As Piece
        Return GetPieceAt(sq, oPieces)
    End Function

    Private Function GetPieceAt(sq As Square, ByRef pSet() As Piece) As Piece
        'Returns the piece located at the square indicated 
        ' Returns nothing if square is empty
        Dim pReturn As Piece = Nothing
        For Each p As Piece In pSet
            If p.Position = sq.ToString And p.Captured = False Then
                pReturn = p
            End If
        Next

        Return pReturn
    End Function

    Private Function getOptionsShortlist(sq As Square, ByRef pSet() As Piece) As LinkedList(Of Square)()
        'Returns a list of possible moves for the given piece, DOES NOT LOOK FROM CHECK/MATE
        Dim llReturn() As LinkedList(Of Square) = Nothing
        Dim p As Piece = GetPieceAt(sq, pSet)

        'Check if the square is empty, returns nothing if it is
        If p IsNot Nothing Then
            'Get the possible moves for the piece at the given position
            llReturn = p.GetMoves()

            If TypeOf p Is Pawn Then
                'Pawns have a strange system of attack/Moves
                'Pawns can only move straight ahead and can only attack diagonally
                ' I will use this distinction to filter the results

                'iterate each path
                For Each ll In llReturn
                    'Check all sqaures in each path (the movement path may have 2 elements so this is necessary)
                    Dim endPath As Boolean = False
                    For i As Integer = 0 To ll.Count - 1

                        If Not endPath Then
                            Dim p2 As Piece = GetPieceAt(ll.ElementAt(i), pSet)

                            If ll.ElementAt(i).Column <> p.Column Then
                                'move is attacking
                                If p2 IsNot Nothing Then
                                    'Check piece's color
                                    If p2.Color = p.Color Then
                                        'Pawn can attack other color
                                        'If the colors are the same, then no attack
                                        ll.RemoveLast()
                                        endPath = True
                                    End If
                                Else
                                    'square is empty
                                    ll.RemoveLast()
                                    endPath = True
                                End If
                            Else
                                'Move is not attacking
                                If p2 IsNot Nothing Then
                                    'If any piece is in the way then end path
                                    ll.RemoveLast()
                                    endPath = True
                                End If
                            End If
                        Else
                            ll.RemoveLast()
                        End If
                    Next i

                Next ll

            Else

                'piece is not a pawn
                'iterate over each path to check for blocks
                For Each ll As LinkedList(Of Square) In llReturn
                    Dim endPath As Boolean = False

                    For i As Integer = 0 To ll.Count - 1

                        If endPath Then
                            ll.RemoveLast()
                        Else
                            Dim p2 As Piece = GetPieceAt(ll.ElementAt(i), pSet)

                            If p2 IsNot Nothing Then
                                ' Space is occupied, check what color
                                If p2.Color = p.Color Then
                                    'Same color means same team, means end path before this
                                    endPath = True
                                    ll.RemoveLast()
                                Else
                                    'Different color, end after this one
                                    endPath = True
                                End If 'p2.Color = p.Color
                            End If 'p2 is not nothing
                        End If 'endPath
                    Next i
                Next ll
            End If ' p is pawn

            If TypeOf p Is King Then
                'Only add castling option to King's moves
                Dim pKing As King = p
                ReDim Preserve llReturn(9)
                llReturn(8) = New LinkedList(Of Square)
                llReturn(9) = New LinkedList(Of Square)
                If pKing.CanCastle Then
                    'Check for each rook
                    'Left most
                    Dim p2 As Piece = GetPieceAt(New Square(pKing.Row, 1), pSet)
                    If p2 IsNot Nothing Then
                        If TypeOf p2 Is Rook Then
                            Dim pRook As Rook = p2
                            If pRook.CanCastle And pRook.Color = pKing.Color Then
                                ' Both can castle, check for pieces in the way
                                If GetPieceAt(New Square(pKing.Row, 2), pSet) Is Nothing And
                                        GetPieceAt(New Square(pKing.Row, 3), pSet) Is Nothing And
                                        GetPieceAt(New Square(pKing.Row, 4), pSet) Is Nothing Then
                                    llReturn(8).AddLast(New Square(pKing.Row, 3))
                                End If
                            End If
                        End If
                    End If

                    'Right most
                    p2 = GetPieceAt(New Square(pKing.Row, 8), pSet)
                    If p2 IsNot Nothing Then
                        If TypeOf p2 Is Rook Then
                            Dim pRook As Rook = p2
                            If pRook.CanCastle And pRook.Color = pKing.Color Then
                                ' Both can castle, check for pieces in the way
                                If GetPieceAt(New Square(pKing.Row, 6), pSet) Is Nothing And
                                        GetPieceAt(New Square(pKing.Row, 7), pSet) Is Nothing Then
                                    llReturn(9).AddLast(New Square(pKing.Row, 7))
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If 'p is nothing

        Return llReturn
    End Function

    Public Function GetPieceOptions(p As Piece) As LinkedList(Of Square)()
        Return GetPieceOptions(New Square(p.Row, p.Column))
    End Function

    Public Function GetPieceOptions(sq As Square) As LinkedList(Of Square)()
        'Gets ALL Options for a given piece, eliminating moves that are illegal like ones that 
        '  would endanger the King, etc.
        Dim llTemp() As LinkedList(Of Square) = getOptionsShortlist(sq, oPieces)
        Dim ll As LinkedList(Of Square)
        Dim llReturn(llTemp.Length - 1) As LinkedList(Of Square)
        Dim p As Piece = GetPieceAt(sq, oPieces)

        For i As Integer = 0 To llTemp.Length - 1
            llReturn(i) = New LinkedList(Of Square)
            ll = llTemp(i)
            If ll IsNot Nothing Then
                For Each testSq As Square In ll
                    If TestMoveForCheck(sq, testSq) <> p.Color Then
                        llReturn(i).AddLast(testSq)
                    End If
                Next
            End If
        Next

        Return llReturn
    End Function

    Private Function TestMoveForCheck(fromSq As Square, toSq As Square) As Piece.PieceColor
        'Returns boolean saying if the move results in check on the moving side
        Dim isCheck As Boolean = False
        Dim pTest(31) As Piece
        For i As Integer = 0 To 31
            pTest(i) = oPieces(i).Clone
        Next

        Dim checkTeam As Piece.PieceColor = Piece.PieceColor.None

        'Move the piece
        'Check for a piece already in the to square
        For Each p In pTest
            If p.Position = toSq.ToString And p.Captured = False Then
                'Set it to captured if found
                p.Captured = True
            End If
        Next
        'Find main test piece
        For Each p In pTest
            If p.Position = fromSq.ToString And p.Captured = False Then
                p.MovePiece(toSq)
            End If
        Next

        'Search for the friendly king
        For Each p In pTest
            If TypeOf p Is King Then
                'See if he is in danger
                If UnderAttack(p, pTest) Then checkTeam = p.Color
            End If
        Next

        Return checkTeam
    End Function

    Private Sub LookForCheck()
        'Checks the positions of pieces on the board to test for Check (King is in danger)
        '  or Checkmate (King is in danger and has no way to escape/block/eliminate threat)

        'Look for Check
        inCheck = Piece.PieceColor.None     'Reset variable
        For Each p In oPieces
            If TypeOf p Is King Then    'Find the kings
                If UnderAttack(p, oPieces) = True Then
                    'King is under attack, that piece is in check
                    inCheck = p.Color
                End If
            End If
        Next

        'Look for checkmate
        Dim found As Boolean = False
        'Only look if there is a check
        If inCheck <> Piece.PieceColor.None Then
            'Check to see if its mate
            'Cycle through the checked team's pieces
            For Each p As Piece In oPieces
                If p.Color = inCheck And p.Captured = False Then
                    'Only check if its a friendly piece and not captured
                    Dim moves = GetPieceOptions(p)
                    For Each ll In moves
                        If Not IsNothing(ll) Then
                            For Each sq In ll
                                found = True
                                Exit Sub
                            Next
                        End If
                    Next
                End If
            Next
        End If
        If found = False Then
            inCheckMate = inCheck
        End If
    End Sub

    Private Function UnderAttack(testPiece As Piece, ByRef pSet() As Piece) As Boolean
        'Checks to see if the given piece is 
        Dim mySq As Square = New Square(testPiece.Position)

        For Each p As Piece In pSet
            If p.Color <> testPiece.Color Then
                Dim arrLl = getOptionsShortlist(New Square(p.Position), pSet)
                If arrLl IsNot Nothing Then
                    For Each ll In arrLl
                        If ll IsNot Nothing Then
                            For Each s As Square In ll
                                If s.ToString = mySq.ToString Then
                                    Return True
                                    Exit Function
                                End If
                            Next s
                        End If
                    Next ll
                End If
            End If
        Next p

        Return False
    End Function

    Public Sub MovePiece(fromSquare As String, toSquare As String)
        Dim ts As Square = New Square(toSquare)
        Dim fs As Square = New Square(fromSquare)
        MovePiece(fs, ts)

    End Sub

    Public Sub MovePiece(fromSquare As Square, toSquare As Square)
        Dim llMoves() As LinkedList(Of Square) = GetPieceOptions(fromSquare)
        Dim found As Boolean = False

        For Each ll In llMoves
            If ll IsNot Nothing Then
                For Each sq In ll
                    If sq.ToString = toSquare.ToString Then
                        found = True
                    End If
                Next sq
            End If
        Next ll

        If found Then
            Dim p As Piece = GetPieceAt(toSquare, oPieces)
            If p IsNot Nothing Then
                p.Captured = True
            End If
            p = GetPieceAt(fromSquare, oPieces)
            p.MovePiece(toSquare)
        Else
            Throw New InvalidMoveException
        End If
        LookForCheck()

        If playerMove = Piece.PieceColor.Black Then
            playerMove = Piece.PieceColor.White
        Else
            playerMove = Piece.PieceColor.Black
        End If
        strLastMove = GetPieceAt(toSquare).Letter & toSquare.ToString

    End Sub

End Class
