Public Class Rslt2DT
    Dim _Stress As Double
    Dim _Strain As Double
    Dim _Force As Double
    Dim _SN As Node
    Dim _EN As Node

    Dim _ssertsRed As Double
    Dim _ssertsgreen As Double
    Dim _ssertsBlue As Double

    Dim _niartsRed As Double
    Dim _niartsgreen As Double
    Dim _niartsBlue As Double

    Dim _ecorfRed As Double
    Dim _ecorfgreen As Double
    Dim _ecorfBlue As Double

    Public ReadOnly Property SsertsRed() As Double
        Get
            Return _ssertsRed
        End Get
    End Property

    Public ReadOnly Property SsertsGreen() As Double
        Get
            Return _ssertsgreen
        End Get
    End Property

    Public ReadOnly Property SsertsBlue() As Double
        Get
            Return _ssertsBlue
        End Get
    End Property

    Public ReadOnly Property NiartsRed() As Double
        Get
            Return _niartsRed
        End Get
    End Property

    Public ReadOnly Property NiartsGreen() As Double
        Get
            Return _niartsgreen
        End Get
    End Property

    Public ReadOnly Property NiartsBlue() As Double
        Get
            Return _niartsBlue
        End Get
    End Property

    Public ReadOnly Property EcorfRed() As Double
        Get
            Return _ecorfRed
        End Get
    End Property

    Public ReadOnly Property EcorfGreen() As Double
        Get
            Return _ecorfgreen
        End Get
    End Property

    Public ReadOnly Property EcorfBlue() As Double
        Get
            Return _ecorfBlue
        End Get
    End Property

    Public ReadOnly Property Stress() As Double
        Get
            Return _Stress
        End Get
    End Property

    Public ReadOnly Property Strain() As Double
        Get
            Return _Strain
        End Get
    End Property

    Public ReadOnly Property Force() As Double
        Get
            Return _Force
        End Get
    End Property

    Public Property SN() As Rslt2DT.Node
        Get
            Return _SN
        End Get
        Set(ByVal value As Rslt2DT.Node)

        End Set
    End Property

    Public Property EN() As Rslt2DT.Node
        Get
            Return _EN
        End Get
        Set(ByVal value As Rslt2DT.Node)

        End Set
    End Property

    Public Class Node
        Dim _Coord As System.Drawing.Point
        Dim _DisplacedCoord As System.Drawing.Point
        Dim _Xdisplacement As Double
        Dim _Ydisplacement As Double
        Dim _Xreaction As Double
        Dim _Yreaction As Double
        Dim _displacementStr As String
        Dim _reactionStr As String

        Public Property Coord() As System.Drawing.Point
            Get
                Return _Coord
            End Get
            Set(ByVal value As System.Drawing.Point)

            End Set
        End Property

        Public ReadOnly Property DisplacedCoord() As System.Drawing.Point
            Get
                Return _DisplacedCoord
            End Get
        End Property

        Public ReadOnly Property Xdisplacement() As Double
            Get
                Return _Xdisplacement
            End Get
        End Property

        Public ReadOnly Property Ydisplacement() As Double
            Get
                Return _Ydisplacement
            End Get
        End Property

        Public ReadOnly Property Xreaction() As Double
            Get
                Return _Xreaction
            End Get
        End Property

        Public ReadOnly Property Yreaction() As Double
            Get
                Return _Yreaction
            End Get
        End Property

        Public ReadOnly Property displacementStr() As String
            Get
                Return _displacementStr
            End Get
        End Property

        Public ReadOnly Property reactionStr() As String
            Get
                Return _reactionStr
            End Get
        End Property

        Private Sub FixDisplacementReactionString()
            Dim r, d As String
            '----- Displacement String
            If Math.Round(_Xdisplacement, 6) <> 0 And Math.Round(_Ydisplacement, 6) <> 0 Then
                r = _Xdisplacement
                d = _Ydisplacement
                _displacementStr = " ( " + r + " , " + d + " ) "
            ElseIf Math.Round(_Xdisplacement, 6) <> 0 Then
                r = _Xdisplacement
                _displacementStr = " ( " + r + ", 0 )"
            ElseIf Math.Round(_Ydisplacement, 6) <> 0 Then
                d = _Ydisplacement
                _displacementStr = " ( 0," + d + " ) "
            Else
                _displacementStr = ""
            End If
            '----- Reaction String
            Dim TempStr As String = ""
            If _displacementStr <> "" Then
                TempStr = vbNewLine
            End If
            '_displacementStr + "  "
            'For i = 0 To (_displacementStr.Length)
            '    TempStr = TempStr + ""
            'Next

            If Math.Round(_Xreaction, 2) <> 0 And Math.Round(_Yreaction, 2) <> 0 Then
                r = _Xreaction
                d = _Yreaction
                _reactionStr = TempStr + " ( " + r + " , " + d + " ) "
            ElseIf Math.Round(_Xreaction, 2) <> 0 Then
                r = _Xreaction
                _reactionStr = TempStr + " ( " + r + ", 0 )"
            ElseIf Math.Round(_Yreaction, 2) <> 0 Then
                d = _Yreaction
                _reactionStr = TempStr + " ( 0," + d + " ) "
            Else
                _reactionStr = TempStr + ""
            End If
        End Sub


        Public Sub New(ByVal Crd As Point, ByVal dx As Double, ByVal dy As Double, ByVal rx As Double, ByVal ry As Double, ByVal Smax As Double)
            _Coord = Crd
            _Xdisplacement = dx
            _Ydisplacement = dy
            _Xreaction = rx
            _Yreaction = ry
            _DisplacedCoord = New Point(Crd.X + (WA2dt.Loadhtfactor * (dx / Smax)), Crd.Y + (WA2dt.Loadhtfactor * (dy / Smax)))
            FixDisplacementReactionString()
        End Sub

    End Class

    Public Sub New(ByVal _Sdx As Double, ByVal _Sdy As Double, ByVal _Edx As Double, ByVal _Edy As Double, ByVal _Srx As Double, ByVal _Sry As Double, ByVal _Erx As Double, ByVal _Ery As Double, ByVal ScaleMax As Double, ByVal El As Line2DT)
        Dim Lcos As Double = (El.EN.Coord.X - El.SN.Coord.X) _
                    / Math.Round( _
                            Math.Sqrt( _
                                Math.Pow((El.SN.Coord.X - El.EN.Coord.X), 2) + _
                                Math.Pow((El.SN.Coord.Y - El.EN.Coord.Y), 2)))
        Dim Msin As Double = (El.EN.Coord.Y - El.SN.Coord.Y) _
                    / Math.Round( _
                            Math.Sqrt( _
                                Math.Pow((El.SN.Coord.X - El.EN.Coord.X), 2) + _
                                Math.Pow((El.SN.Coord.Y - El.EN.Coord.Y), 2)))

        Dim TSx, TSy, TEx, TEy As Double
        If El.SN.Support.PJ = False Then
            TSx = _Sdx * Math.Cos(El.SN.Support.supportinclination - (Math.PI / 2)) - _Sdy * Math.Sin(El.SN.Support.supportinclination - (Math.PI / 2))
            TSy = _Sdx * Math.Sin(El.SN.Support.supportinclination - (Math.PI / 2)) + _Sdy * Math.Cos(El.SN.Support.supportinclination - (Math.PI / 2))
        Else
            TSx = _Sdx
            TSy = _Sdy
        End If
        If El.EN.Support.PJ = False Then
            TEx = _Edx * Math.Cos(El.EN.Support.supportinclination - (Math.PI / 2)) - _Edy * Math.Sin(El.EN.Support.supportinclination - (Math.PI / 2))
            TEy = _Edx * Math.Sin(El.EN.Support.supportinclination - (Math.PI / 2)) + _Edy * Math.Cos(El.EN.Support.supportinclination - (Math.PI / 2))
        Else
            TEx = _Edx
            TEy = _Edy
        End If

        '_Strain = ((Lcos * _Sdx) + (Msin * _Sdy) + (-Lcos * _Edx) + (-Msin * _Edy)) / El.Length
        _Strain = ((Lcos * TSx) + (Msin * TSy) + (-Lcos * TEx) + (-Msin * TEy)) / El.Length
        _Stress = El.E * _Strain
        _Force = El.A * _Stress

        _SN = New Rslt2DT.Node(El.SN.Coord, TSx, TSy, _Srx, _Sry, ScaleMax)
        _EN = New Rslt2DT.Node(El.EN.Coord, TEx, TEy, _Erx, _Ery, ScaleMax)
    End Sub

    Public Sub FixStressColorVariation(ByVal Bscale As Double, ByVal mpt As Double, ByVal Rscale As Double)
        If _Stress < mpt Then
            _ssertsRed = 0
            _ssertsgreen = Math.Abs((_Stress / mpt) * (128))
            _ssertsBlue = Math.Abs(255 - Math.Abs(((_Stress / mpt) * (128) * 2)))
        Else
            _ssertsBlue = 0
            _ssertsgreen = Math.Abs((mpt / _Stress) * (128))
            _ssertsRed = Math.Abs(255 - Math.Abs(((mpt / _Stress) * (128) * 2)))
        End If
        _ssertsRed = If(_ssertsRed < 255, _ssertsRed, 255)
        _ssertsgreen = If(_ssertsgreen < 128, _ssertsgreen, 128)
        _ssertsBlue = If(_ssertsBlue < 255, _ssertsBlue, 255)
    End Sub

    Public Sub FixStrainColorVariation(ByVal Bscale As Double, ByVal mpt As Double, ByVal Rscale As Double)
        If _Strain < mpt Then
            _niartsRed = 0
            _niartsgreen = Math.Abs((_Strain / mpt) * (128))
            _niartsBlue = Math.Abs(255 - Math.Abs(((_Strain / mpt) * (128) * 2)))
        Else
            _niartsBlue = 0
            _niartsgreen = Math.Abs((mpt / _Strain) * (128))
            _niartsRed = Math.Abs(255 - Math.Abs(((mpt / _Strain) * (128) * 2)))
        End If
        _niartsRed = If(_niartsRed < 255, _niartsRed, 255)
        _niartsgreen = If(_niartsgreen < 128, _niartsgreen, 128)
        _niartsBlue = If(_niartsBlue < 255, _niartsBlue, 255)
    End Sub

    Public Sub FixForceColorVariation(ByVal Bscale As Double, ByVal mpt As Double, ByVal Rscale As Double)
        If _Force < mpt Then
            _ecorfRed = 0
            _ecorfgreen = Math.Abs((_Force / mpt) * (128))
            _ecorfBlue = Math.Abs(255 - Math.Abs(((_Force / mpt) * (128) * 2)))
        Else
            _ecorfBlue = 0
            _ecorfgreen = Math.Abs((mpt / _Force) * (128))
            _ecorfRed = Math.Abs(255 - Math.Abs(((mpt / _Force) * (128) * 2)))
        End If
        _ecorfRed = If(_ecorfRed < 255, _ecorfRed, 255)
        _ecorfgreen = If(_ecorfgreen < 128, _ecorfgreen, 128)
        _ecorfBlue = If(_ecorfBlue < 255, _ecorfBlue, 255)
    End Sub
End Class
