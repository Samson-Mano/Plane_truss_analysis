<Serializable()> _
Public Class Line2DT
    Dim _SN As Node
    Dim _EN As Node
    Dim _E As Double
    Dim _A As Double
    Dim _Length As Double
    Dim _Theta As Double
    Dim _Inclination As Double
    Dim _Lcos As Double
    Dim _Msin As Double

#Region "Get Set Property"
    Public Property Length() As Double
        Get
            Return _Length
        End Get
        Set(ByVal value As Double)
            _Length = value
        End Set
    End Property

    '-Theta is the property used for analysis purpose - Normal cartetian coordinate system
    Public ReadOnly Property Theta() As Double
        Get
            Return _Theta
        End Get
    End Property

    Public ReadOnly Property Inclination() As Double
        Get
            Return _Inclination
        End Get
    End Property

    Public Property SN() As Node '---Have to change to Readonly property
        Get
            Return _SN
        End Get
        Set(ByVal value As Node)
            '_SN = value
        End Set
    End Property

    Public Property EN() As Node '---Have to change to Readonly property
        Get
            Return _EN
        End Get
        Set(ByVal value As Node)
            '_EN = value
        End Set
    End Property

    Public Property Lcos() As Double
        Get
            Return _Lcos
        End Get
        Set(ByVal value As Double)
            '----
        End Set
    End Property '---Have to change to Readonly property

    Public Property Msin() As Double
        Get
            Return _Msin
        End Get
        Set(ByVal value As Double)
            '----
        End Set
    End Property

    Public Property E() As Double
        Get
            Return _E
        End Get
        Set(ByVal value As Double)
            _E = value
        End Set
    End Property

    Public Property A() As Double
        Get
            Return _A
        End Get
        Set(ByVal value As Double)
            _A = value
        End Set
    End Property
#End Region

#Region "Structure"
    <Serializable()> _
    Public Class Node
        Private _Coord As System.Drawing.Point
        Private _Support As SupportCondition
        Private _Load As LoadCondition
        Private _index As Integer
        Private _XLoad As Double
        Private _YLoad As Double

        Public Property index() As Integer
            Get
                Return _index
            End Get
            Set(ByVal value As Integer)
                '_index = value
            End Set
        End Property

        Public Property Coord() As Point
            Get
                Return _Coord
            End Get
            Set(ByVal value As Point)

            End Set
        End Property

        Public Property Support() As SupportCondition
            Get
                Return _Support
            End Get
            Set(ByVal value As SupportCondition)
                _Support = value
            End Set
        End Property

        Public Property Load() As LoadCondition
            Get
                Return _Load
            End Get
            Set(ByVal value As LoadCondition)
                _Load = value
            End Set
        End Property

        <Serializable()> _
        Public Structure SupportCondition
            Private _PJ As Boolean
            Private _PS As Boolean
            Private _RS As Boolean
            Private _settlementdx As Double
            Private _settlementdy As Double
            Private _supportinclination As Double

            Public Property PJ() As Boolean
                Get
                    Return _PJ
                End Get
                Set(ByVal value As Boolean)
                    _PJ = value
                End Set
            End Property

            Public Property PS() As Boolean
                Get
                    Return _PS
                End Get
                Set(ByVal value As Boolean)
                    _PS = value
                End Set
            End Property

            Public Property RS() As Boolean
                Get
                    Return _RS
                End Get
                Set(ByVal value As Boolean)
                    _RS = value
                End Set
            End Property

            Public Property settlementdx() As Double
                Get
                    Return _settlementdx
                End Get
                Set(ByVal value As Double)
                    _settlementdx = value
                End Set
            End Property

            Public Property settlementdy() As Double
                Get
                    Return _settlementdy
                End Get
                Set(ByVal value As Double)
                    _settlementdy = value
                End Set
            End Property

            Public Property supportinclination() As Double
                Get
                    Return _supportinclination
                End Get
                Set(ByVal value As Double)
                    _supportinclination = value
                End Set
            End Property

            Public Sub New(ByVal Pjoint As Boolean, ByVal Psupport As Boolean, ByVal Rsupport As Boolean, ByVal supincln As Double, ByVal dx As Double, ByVal dy As Double)
                _PJ = Pjoint
                _PS = Psupport
                _RS = Rsupport

                If _PS = True Or _RS = True Then
                    _supportinclination = supincln
                    _settlementdx = If(dx <> 0, dx, Nothing)
                    _settlementdy = If(dy <> 0, dy, Nothing)
                End If
            End Sub
        End Structure

        <Serializable()> _
        Public Structure LoadCondition
            Private _Loadintensity As Double
            Private _Loadinclination As Double

            Public ReadOnly Property Loadintensity() As Double
                Get
                    Return _Loadintensity
                End Get
            End Property

            Public ReadOnly Property Loadinclination() As Double
                Get
                    Return _Loadinclination
                End Get
            End Property

            Public Sub New(ByVal intensity As Double, ByVal inclination As Double)
                _Loadintensity = intensity
                _Loadinclination = inclination
            End Sub
        End Structure

        Public Sub New(ByVal LCoord As Point, Optional ByVal ind As Integer = -1)
            _Coord = New Point(LCoord.X, LCoord.Y)
            _index = ind
        End Sub

        Public Sub ModifyIndex(ByVal I As Integer)
            _index = I
        End Sub

        'Public Sub New(ByVal LCoord As Point, ByVal TSupp As SupportCondition)
        '    _Coord = New Point(LCoord.X, LCoord.Y)
        '    _Support = TSupp
        'End Sub
    End Class
#End Region

#Region "Constructor"
    Public Sub New(ByVal S As Node, ByVal E As Node)
        AttachNode(S, _SN)
        AttachNode(E, _EN)
        _Length = Math.Round( _
                        Math.Sqrt( _
                            Math.Pow((SN.Coord.X - EN.Coord.X), 2) + _
                            Math.Pow((SN.Coord.Y - EN.Coord.Y), 2)), 1) / _
                            MDIMain.Nappdefaults.defaultScaleFactor
        _Lcos = (EN.Coord.X - SN.Coord.X) _
                        / Math.Round( _
                                Math.Sqrt( _
                                    Math.Pow((SN.Coord.X - EN.Coord.X), 2) + _
                                    Math.Pow((SN.Coord.Y - EN.Coord.Y), 2)))
        _Msin = (SN.Coord.Y - EN.Coord.Y) _
                        / Math.Round( _
                                Math.Sqrt( _
                                    Math.Pow((SN.Coord.X - EN.Coord.X), 2) + _
                                    Math.Pow((SN.Coord.Y - EN.Coord.Y), 2)))
        FixInclination(_Theta, _Inclination)
        'FixSupport(_SN, _EN)
        _E = MDIMain.Nappdefaults.defaultE
        _A = MDIMain.Nappdefaults.defaultA
    End Sub

    Private Sub FixInclination(ByRef _TH As Double, ByRef _Incln As Double)
        Dim LC, MS As Double
        If Math.Abs(EN.Coord.X - SN.Coord.X) <= Math.Abs(EN.Coord.Y - SN.Coord.Y) Then
            LC = (EN.Coord.X - SN.Coord.X) / _
                    (Math.Round( _
                        Math.Sqrt( _
                            Math.Pow((SN.Coord.X - EN.Coord.X), 2) + _
                            Math.Pow((SN.Coord.Y - EN.Coord.Y), 2))))
            _TH = 0 '(LC * 180) / Math.PI
            _Incln = If((EN.Coord.Y - SN.Coord.Y) < 1, Math.Acos(LC) * -1, Math.Acos(LC))
        Else
            MS = (EN.Coord.Y - SN.Coord.Y) / _
                    (Math.Round( _
                        Math.Sqrt( _
                            Math.Pow((SN.Coord.X - EN.Coord.X), 2) + _
                            Math.Pow((SN.Coord.Y - EN.Coord.Y), 2))))
            _TH = 0 '(MS * 180) / Math.PI
            _Incln = If((EN.Coord.X - SN.Coord.X) < 1, Math.Asin(MS) * -1, Math.Asin(MS))
        End If
    End Sub

    Private Sub FixLoad() '-----Have to Delete this

    End Sub

    Private Sub AttachNode(ByRef Pt As Node, ByRef Nd As Node)
        WA2dt.PD.Her = New Node(Pt.Coord)
        If WA2dt.Bob.Exists(WA2dt.PD.ExistingNodePD) = True Then
            Nd = WA2dt.Bob.Find(WA2dt.PD.ExistingNodePD)
            For Each mem In WA2dt.Mem
                If Nd.index = mem.SN.index Then
                    Nd.Support = mem.SN.Support
                    Exit For
                End If
                If Nd.index = mem.EN.index Then
                    Nd.Support = mem.EN.Support
                    Exit For
                End If
            Next
        Else
            WA2dt.Bob.Add(New Node(Pt.Coord, WA2dt.Bob.Count))
            Nd = WA2dt.Bob(WA2dt.Bob.Count - 1)
            Nd.Support = New Line2DT.Node.SupportCondition( _
                                MDIMain.Nappdefaults.defaultPJ, _
                                False, _
                                False, _
                                Nothing, _
                                Nothing, _
                                Nothing)
        End If

    End Sub

    Private Sub FixSupport(ByRef SN As Node, ByRef EN As Node)
        WA2dt.PD.Her = New Node(SN.Coord)
        If WA2dt.Bob.Exists(WA2dt.PD.ExistingNodePD) = True Then
            Dim E As Line2DT.Node = WA2dt.Bob.Find(WA2dt.PD.ExistingNodePD)
            SN.Support = New Line2DT.Node.SupportCondition( _
                                E.Support.PJ, _
                                E.Support.PS, _
                                E.Support.RS, _
                                E.Support.supportinclination, _
                                E.Support.settlementdx, _
                                E.Support.settlementdy)
        Else
            SN.Support = New Line2DT.Node.SupportCondition( _
                                MDIMain.Nappdefaults.defaultPJ, _
                                False, _
                                False, _
                                Nothing, _
                                Nothing, _
                                Nothing)
        End If

        WA2dt.PD.Her = New Node(EN.Coord)
        If WA2dt.Bob.Exists(WA2dt.PD.ExistingNodePD) = True Then
            Dim E As Line2DT.Node = WA2dt.Bob.Find(WA2dt.PD.ExistingNodePD)
            EN.Support = New Line2DT.Node.SupportCondition( _
                                E.Support.PJ, _
                                E.Support.PS, _
                                E.Support.RS, _
                                E.Support.supportinclination, _
                                E.Support.settlementdx, _
                                E.Support.settlementdy)
        Else
            EN.Support = New Line2DT.Node.SupportCondition( _
                                MDIMain.Nappdefaults.defaultPJ, _
                                False, _
                                False, _
                                Nothing, _
                                Nothing, _
                                Nothing)
        End If

        '---- Modifying Other Bob
        For Each Nodes In WA2dt.Bob
            If Nodes.Coord.Equals(SN) Then
                WA2dt.Bob(WA2dt.Bob.IndexOf(Nodes)).Support = SN.Support
                Exit For
            End If
            If Nodes.Coord.Equals(EN) Then
                WA2dt.Bob(WA2dt.Bob.IndexOf(Nodes)).Support = EN.Support
                Exit For
            End If
        Next

    End Sub '-----Have to Delete this

    Public Sub ReviseNodes()

    End Sub
#End Region
End Class
