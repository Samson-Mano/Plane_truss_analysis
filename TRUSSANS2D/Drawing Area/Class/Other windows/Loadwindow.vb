Public Class Loadwindow
    Dim TempEL As SelectedElement
    Dim SNdetails As NodeDetails
    Dim ENdetails As NodeDetails
    Dim _DrgFlg As Boolean = False

    Private Class SelectedElement
        Public Spoint As Point
        Public Epoint As Point
        Public Mpoint As Point
        Public inclination As Double
        Public ActualLength As Double
        Public CurrentLength As Double
        Public SNode As Integer
        Public ENode As Integer
        Public maxload As Double = 10
    End Class

    Private Class NodeDetails
        Dim _Supportinclination As Double
        Dim _PJ As Boolean = True
        Dim _PS As Boolean
        Dim _RS As Boolean
        Dim _dxsettlement As Double
        Dim _dysettlement As Double
        Dim _Loadintensity As Double
        Dim _Loadinclination As Double
        Dim _tempload As Double
        Dim _tempinclination As Double = 90 * (Math.PI / 180)

        Public Property Supportinclination() As Double
            Get
                Return _Supportinclination
            End Get
            Set(ByVal value As Double)
                _Supportinclination = value
            End Set
        End Property

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

        Public Property dxsettlement() As Double
            Get
                Return _dxsettlement
            End Get
            Set(ByVal value As Double)
                _dxsettlement = value
            End Set
        End Property

        Public Property dysettlement() As Double
            Get
                Return _dysettlement
            End Get
            Set(ByVal value As Double)
                _dysettlement = value
            End Set
        End Property

        Public Property Loadintensity() As Double
            Get
                Return _Loadintensity
            End Get
            Set(ByVal value As Double)
                _Loadintensity = value
            End Set
        End Property

        Public Property Loadinclination() As Double
            Get
                Return _Loadinclination
            End Get
            Set(ByVal value As Double)
                _Loadinclination = value
            End Set
        End Property

        Public Property tempload() As Double
            Get
                Return _tempload
            End Get
            Set(ByVal value As Double)
                _tempload = value
            End Set
        End Property

        Public Property tempinclination() As Double
            Get
                Return _tempinclination
            End Get
            Set(ByVal value As Double)
                _tempinclination = value
            End Set
        End Property

        Public Sub New(ByVal Pjoint As Boolean, ByVal Psupport As Boolean, ByVal Rsupport As Boolean, ByVal supincln As Double, ByVal dx As Double, ByVal dy As Double, ByVal _loadint As Double, ByVal _loadincln As Double)
            _PJ = Pjoint
            _PS = Psupport
            _RS = Rsupport
            _Loadintensity = _loadint
            _Loadinclination = _loadincln

            If _PS = True Or _RS = True Then
                _Supportinclination = supincln
                _dxsettlement = If(dx <> 0, dx, Nothing)
                _dysettlement = If(dy <> 0, dy, Nothing)
            End If
        End Sub
    End Class

    Public Sub New(ByVal El As Line2DT)
        TempEL = New SelectedElement
        SNdetails = New NodeDetails(El.SN.Support.PJ,
                                       El.SN.Support.PS,
                                       El.SN.Support.RS,
                                       El.SN.Support.supportinclination,
                                       El.SN.Support.settlementdx,
                                       El.SN.Support.settlementdy,
                                       El.SN.Load.Loadintensity,
                                       El.SN.Load.Loadinclination)
        ENdetails = New NodeDetails(El.EN.Support.PJ,
                                       El.EN.Support.PS,
                                       El.EN.Support.RS,
                                       El.EN.Support.supportinclination,
                                       El.EN.Support.settlementdx,
                                       El.EN.Support.settlementdy,
                                       El.EN.Load.Loadintensity,
                                       El.EN.Load.Loadinclination)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        '---------Fix Size of Window
        If MDIMain.Width > MDIMain.Height Then
            Me.Width = 450 '(MDIMain.Width / 3)
            Me.Height = (450 * (MDIMain.Height / MDIMain.Width)) + 235 + 25
        Else
            Me.Height = 450 + 235 + 25 '(MDIMain.Width / 3)
            Me.Width = (450 * (MDIMain.Width / MDIMain.Height))
        End If
        _NodeTab.ItemSize = New Size((Me.Width / 2) - 11, 25)
        For Each itm In WA2dt.Mem
            _memberBindingSource.Add(itm)
            If itm.Equals(El) Then
                _memberBindingSource.Position = WA2dt.Mem.IndexOf(itm) + 1
            End If
        Next
        CoordFix(El)
    End Sub

    Private Sub CoordFix(ByVal El As Line2DT)
        Dim Tx As Double
        Dim Ty As Double
        Dim Lcos As Double
        Dim Msin As Double
        Dim theta As Double
        Dim MinSide As Double

        If _LoadPic.Width <= _LoadPic.Height Then
            MinSide = _LoadPic.Width - 40
        Else
            MinSide = _LoadPic.Height - 40
        End If
        TempEL = New SelectedElement

        Tx = El.EN.Coord.X - El.SN.Coord.X
        Ty = El.EN.Coord.Y - El.SN.Coord.Y
        If Math.Abs(Tx) <= Math.Abs(Ty) Then
            Lcos = (El.EN.Coord.X - El.SN.Coord.X) / (Math.Round(Math.Sqrt(Math.Pow((El.SN.Coord.X - El.EN.Coord.X), 2) + Math.Pow((El.SN.Coord.Y - El.EN.Coord.Y), 2))))
            theta = Math.Acos(Lcos)
            TempEL.inclination = If((El.EN.Coord.Y - El.SN.Coord.Y) < 1, theta * -1, theta)
            Tx = MinSide / (Math.Tan(theta))
            Ty = If((El.EN.Coord.Y - El.SN.Coord.Y) < 1, MinSide * -1, MinSide)
        Else
            Msin = (El.EN.Coord.Y - El.SN.Coord.Y) / (Math.Round(Math.Sqrt(Math.Pow((El.SN.Coord.X - El.EN.Coord.X), 2) + Math.Pow((El.SN.Coord.Y - El.EN.Coord.Y), 2))))
            theta = Math.Asin(Msin)
            TempEL.inclination = If((El.EN.Coord.X - El.SN.Coord.X) < 1, theta * -1, theta)
            Tx = If((El.EN.Coord.X - El.SN.Coord.X) < 1, MinSide * -1, MinSide)
            Ty = MinSide * (Math.Tan(theta))
        End If

        TempEL.Mpoint = New Point((Me._LoadPic.Width / 2), (Me._LoadPic.Height / 2))
        TempEL.Spoint = New Point(TempEL.Mpoint.X - (Tx / 2), TempEL.Mpoint.Y - (Ty / 2))
        TempEL.Epoint = New Point(TempEL.Mpoint.X + (Tx / 2), TempEL.Mpoint.Y + (Ty / 2))
        TempEL.ActualLength = El.Length
        TempEL.CurrentLength = Math.Sqrt(Math.Pow((TempEL.Spoint.X - TempEL.Epoint.X), 2) + Math.Pow((TempEL.Spoint.Y - TempEL.Epoint.Y), 2))
        For Each D In WA2dt.Bob
            If D.Coord.X = El.SN.Coord.X And D.Coord.Y = El.SN.Coord.Y Then
                TempEL.SNode = WA2dt.Bob.IndexOf(D)
            End If
            If D.Coord.X = El.EN.Coord.X And D.Coord.Y = El.EN.Coord.Y Then
                TempEL.ENode = WA2dt.Bob.IndexOf(D)
            End If
        Next
        _NodeTab.TabPages(0).Text = "Node - " & TempEL.SNode
        _NodeTab.TabPages(1).Text = "Node - " & TempEL.ENode

        SNdetails = New NodeDetails(El.SN.Support.PJ, _
                                       El.SN.Support.PS, _
                                       El.SN.Support.RS, _
                                       El.SN.Support.supportinclination, _
                                       El.SN.Support.settlementdx, _
                                       El.SN.Support.settlementdy, _
                                       El.SN.Load.Loadintensity, _
                                       El.SN.Load.Loadinclination)
        ENdetails = New NodeDetails(El.EN.Support.PJ, _
                                       El.EN.Support.PS, _
                                       El.EN.Support.RS, _
                                       El.EN.Support.supportinclination, _
                                       El.EN.Support.settlementdx, _
                                       El.EN.Support.settlementdy, _
                                       El.EN.Load.Loadintensity, _
                                       El.EN.Load.Loadinclination)

        FixMaxLoad()
        _LoadPic.Refresh()
        _SNSupportPic.Refresh()
    End Sub

    Private Sub FixMaxLoad()
        Try
            TempEL.maxload = 0.000001
            If _NodeTab.SelectedIndex = 0 Then
                If Val(_SNLoad_txt.Text) > TempEL.maxload Then
                    TempEL.maxload = Val(_SNLoad_txt.Text)
                End If
            ElseIf _NodeTab.SelectedIndex = 1 Then
                If Val(_ENLoad_txt.Text) > TempEL.maxload Then
                    TempEL.maxload = Val(_ENLoad_txt.Text)
                End If
            End If
            If SNdetails.Loadintensity > TempEL.maxload Then
                TempEL.maxload = SNdetails.Loadintensity
            End If
            If ENdetails.Loadintensity > TempEL.maxload Then
                TempEL.maxload = ENdetails.Loadintensity
            End If
            _LoadPic.Refresh()
            _SNSupportPic.Refresh()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub SupportBoundaryConditionWindow_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        WA2dt.selLine.Clear()
        MDIMain._DMCMenable()
        WA2dt.MainPic.Refresh()
    End Sub

    Private Sub SupportBoundaryConditionWindow_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub _memberNavigator_RefreshItems(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _memberBindingSource.CurrentChanged
        If _memberBindingSource.Count <> 0 Then
            CoordFix(_memberBindingSource.Current)
            WA2dt.selLine(0) = _memberBindingSource.IndexOf(_memberBindingSource.Current)
            _LoadPic.Refresh()
            WA2dt.MainPic.Refresh()
        End If
    End Sub

    Private Sub _NodeTab_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _NodeTab.SelectedIndexChanged
        FixMaxLoad()
        _LoadPic.Refresh()
        _SNSupportPic.Refresh()
    End Sub

#Region "Paint Load Pic"
    Private Sub _LoadPic_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles _LoadPic.Paint
        Try
            LoadAreaPaintMember(e)
            LoadAreaPaintNode(e)
            LoadAreaPaintSupport(e)
            LoadAreaPaintLoad(e)
            tempPaintLoad(e)
        Catch ex As Exception
            'MsgBox("Zzzzzzzzsad")
        End Try
    End Sub

    Private Sub LoadAreaPaintMember(ByRef e As System.Windows.Forms.PaintEventArgs)
        e.Graphics.DrawLine(New Pen(OptionD.Mc, 2), TempEL.Spoint, TempEL.Epoint)
        e.Graphics.DrawString(Math.Round(TempEL.ActualLength, OptionD.Prec), New Font("Verdana", (8)), New Pen(OptionD.Lc, 2 / WA2dt.Zm).Brush, TempEL.Mpoint.X, TempEL.Mpoint.Y)
    End Sub

    Private Sub LoadAreaPaintNode(ByRef e As System.Windows.Forms.PaintEventArgs)
        ' Start Node
        'e.Graphics.FillEllipse(New Pen(OptionD.Nc, 4).Brush, TempEL.Spoint.X - (2), TempEL.Spoint.Y - (2), 4, 4)
        e.Graphics.DrawString(TempEL.SNode, New Font("Verdana", (8)), New Pen(OptionD.NNc, 2).Brush, TempEL.Spoint.X - (14), TempEL.Spoint.Y - (14))
        ' End Node
        'e.Graphics.FillEllipse(New Pen(OptionD.Nc, 4).Brush, TempEL.Epoint.X - (2), TempEL.Epoint.Y - (2), 4, 4)
        e.Graphics.DrawString(TempEL.ENode, New Font("Verdana", (8)), New Pen(OptionD.NNc, 2).Brush, TempEL.Epoint.X - (14), TempEL.Epoint.Y - (14))
    End Sub

    Private Sub LoadAreaPaintSupport(ByRef e As System.Windows.Forms.PaintEventArgs)
        If _NodeTab.SelectedIndex = 0 Then
            e.Graphics.DrawRectangle(Pens.Black, TempEL.Spoint.X - 30, TempEL.Spoint.Y - 30, 60, 60)

        ElseIf _NodeTab.SelectedIndex = 1 Then
            e.Graphics.DrawRectangle(Pens.Black, TempEL.Epoint.X - 30, TempEL.Epoint.Y - 30, 60, 60)
        End If
        '--- Start Node
        If SNdetails.PJ = True Then
            SupportPicPaintPinJoint(e, TempEL.Spoint, SNdetails.Supportinclination)
        ElseIf SNdetails.PS = True Then
            SupportPicPaintPinSupport(e, TempEL.Spoint, SNdetails.Supportinclination, 0.5)
        ElseIf SNdetails.RS = True Then
            SupportPicPaintRollerSupport(e, TempEL.Spoint, SNdetails.Supportinclination, 0.5)
        End If

        '--- End Node
        If ENdetails.PJ = True Then
            SupportPicPaintPinJoint(e, TempEL.Epoint, ENdetails.Supportinclination)
        ElseIf ENdetails.PS = True Then
            SupportPicPaintPinSupport(e, TempEL.Epoint, ENdetails.Supportinclination, 0.5)
        ElseIf ENdetails.RS = True Then
            SupportPicPaintRollerSupport(e, TempEL.Epoint, ENdetails.Supportinclination, 0.5)
        End If
    End Sub

    Private Sub LoadAreaPaintLoad(ByRef e As System.Windows.Forms.PaintEventArgs)
        If SNdetails.Loadintensity <> 0 Then
            PaintExtLoad(e, TempEL.Spoint, SNdetails.Loadintensity, SNdetails.Loadinclination)
        End If
        If ENdetails.Loadintensity <> 0 Then
            PaintExtLoad(e, TempEL.Epoint, ENdetails.Loadintensity, ENdetails.Loadinclination)
        End If
    End Sub

    Private Sub tempPaintLoad(ByRef e As System.Windows.Forms.PaintEventArgs)
        Try
            If _NodeTab.SelectedIndex = 0 Then
                PaintTempLoad(e, TempEL.Spoint, Val(_SNLoad_txt.Text), SNdetails.tempinclination)
            ElseIf _NodeTab.SelectedIndex = 1 Then
                PaintTempLoad(e, TempEL.Epoint, Val(_ENLoad_txt.Text), ENdetails.tempinclination)
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "Paint Start Node & End Node Miniature Load Pic Events"
    Private Sub _SNSupportPic_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles _SNSupportPic.MouseClick
        'If SNdetails.Supportinclination = (45 * (Math.PI / 180)) Then
        '    SNdetails.Supportinclination = (90 * (Math.PI / 180))
        'ElseIf SNdetails.Supportinclination = (90 * (Math.PI / 180)) Then
        '    SNdetails.Supportinclination = (135 * (Math.PI / 180))
        'ElseIf SNdetails.Supportinclination = (135 * (Math.PI / 180)) Then
        '    SNdetails.Supportinclination = (180 * (Math.PI / 180))
        'Else
        '    SNdetails.Supportinclination = (45 * (Math.PI / 180))
        'End If
        _SNSupportPic.Refresh()
    End Sub

    Private Sub _SNSupportPic_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles _SNSupportPic.MouseDown
        _DrgFlg = True
        _SNSupportPic.Refresh()
        _LoadPic.Refresh()
    End Sub

    Private Sub _SNSupportPic_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles _SNSupportPic.MouseMove
        If _DrgFlg = True Then
            If _NodeTab.SelectedIndex = 0 Then
                SNdetails.tempinclination = If((e.Y - 80) < 0, -1 * Math.Acos((e.X - 80) / (Math.Sqrt(Math.Pow((e.X - 80), 2) + Math.Pow((e.Y - 80), 2)))), _
                                                                    Math.Acos((e.X - 80) / (Math.Sqrt(Math.Pow((e.X - 80), 2) + Math.Pow((e.Y - 80), 2)))))
            ElseIf _NodeTab.SelectedIndex = 1 Then
                ENdetails.tempinclination = If((e.Y - 80) < 0, -1 * Math.Acos((e.X - 80) / (Math.Sqrt(Math.Pow((e.X - 80), 2) + Math.Pow((e.Y - 80), 2)))), _
                                                                    Math.Acos((e.X - 80) / (Math.Sqrt(Math.Pow((e.X - 80), 2) + Math.Pow((e.Y - 80), 2)))))
            End If
        End If
        _SNSupportPic.Refresh()
        _LoadPic.Refresh()
    End Sub

    Private Sub _SNSupportPic_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles _SNSupportPic.MouseUp
        _DrgFlg = False
        _SNSupportPic.Refresh()
        _LoadPic.Refresh()
    End Sub

    Private Sub _SNSupportPic_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles _SNSupportPic.Paint
        If _NodeTab.SelectedIndex = 0 Then
            e.Graphics.DrawLine(Pens.BurlyWood, 80, 0, 80, 160)
            e.Graphics.DrawLine(Pens.BurlyWood, 0, 80, 160, 80)

            e.Graphics.DrawLine(New Pen(OptionD.Mc, 4), New Point(80, 80), New Point(80 + (TempEL.Epoint.X - TempEL.Spoint.X), 80 + (TempEL.Epoint.Y - TempEL.Spoint.Y)))
            e.Graphics.DrawString(TempEL.SNode, New Font("Verdana", 10), New Pen(OptionD.Nc, 4).Brush, New Point(50, 50))

            PaintExtLoad(e, New Point(80, 80), SNdetails.Loadintensity, SNdetails.Loadinclination)
            PaintTempLoad(e, New Point(80, 80), Val(_SNLoad_txt.Text), SNdetails.tempinclination)
            _SNinclination_txt.Text = Math.Round((SNdetails.tempinclination * (180 / Math.PI)), 0)
            _SNinclination_txt.Refresh()

            If SNdetails.PJ = True Then
                SupportPicPaintPinJoint(e, New Point(80, 80), SNdetails.Supportinclination)
            ElseIf SNdetails.PS = True Then
                SupportPicPaintPinSupport(e, New Point(80, 80), SNdetails.Supportinclination, 1)
            ElseIf SNdetails.RS = True Then
                SupportPicPaintRollerSupport(e, New Point(80, 80), SNdetails.Supportinclination, 1)
            End If
        ElseIf _NodeTab.SelectedIndex = 1 Then
            e.Graphics.DrawLine(Pens.BurlyWood, 80, 0, 80, 160)
            e.Graphics.DrawLine(Pens.BurlyWood, 0, 80, 160, 80)

            e.Graphics.DrawLine(New Pen(OptionD.Mc, 4), New Point(80, 80), New Point((80 + (TempEL.Spoint.X - TempEL.Epoint.X)), (80 + (TempEL.Spoint.Y - TempEL.Epoint.Y))))
            e.Graphics.DrawString(TempEL.ENode, New Font("Verdana", 10), New Pen(OptionD.Nc, 4).Brush, New Point(50, 50))

            PaintExtLoad(e, New Point(80, 80), ENdetails.Loadintensity, ENdetails.Loadinclination)
            PaintTempLoad(e, New Point(80, 80), Val(_ENLoad_txt.Text), ENdetails.tempinclination)
            _ENinclination_txt.Text = Math.Round((ENdetails.tempinclination * (180 / Math.PI)), 0)
            _ENinclination_txt.Refresh()

            If ENdetails.PJ = True Then
                SupportPicPaintPinJoint(e, New Point(80, 80), ENdetails.Supportinclination)
            ElseIf ENdetails.PS = True Then
                SupportPicPaintPinSupport(e, New Point(80, 80), ENdetails.Supportinclination, 1)
            ElseIf ENdetails.RS = True Then
                SupportPicPaintRollerSupport(e, New Point(80, 80), ENdetails.Supportinclination, 1)
            End If
        End If
    End Sub

    Private Sub PaintExtLoad(ByVal e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Loadintensity As Double, ByVal LoadInclination As Double)
        If Loadintensity <> 0 Then
            Dim PPt1 As New Point(M.X, M.Y)
            Dim PPt2 As New Point
            PPt2 = New Point(PPt1.X + (Math.Cos(LoadInclination) * (50 * (-Loadintensity / TempEL.maxload))), PPt1.Y + (Math.Sin(LoadInclination) * (50 * (-Loadintensity / TempEL.maxload))))
            Dim loadpen As New System.Drawing.Pen(Color.Green, 2)
            loadpen.CustomStartCap = New System.Drawing.Drawing2D.AdjustableArrowCap(3, 5)
            e.Graphics.DrawLine(loadpen, PPt1.X, PPt1.Y, PPt2.X, PPt2.Y)
            e.Graphics.DrawString(Loadintensity, New Font("Verdana", (8)), New Pen(Color.Green, 2).Brush, PPt2.X + (2), PPt2.Y + (2))
        End If
    End Sub

    Private Sub PaintTempLoad(ByVal e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Loadintensity As Double, ByVal LoadInclination As Double)
        Dim PPt1 As New Point(M.X, M.Y)
        Dim PPt2 As New Point
        PPt2 = New Point(PPt1.X + (Math.Cos(LoadInclination) * (50 * (-Loadintensity / TempEL.maxload))), PPt1.Y + (Math.Sin(LoadInclination) * (50 * (-Loadintensity / TempEL.maxload))))
        Dim loadpen As New System.Drawing.Pen(Color.Green, 2)
        loadpen.DashStyle = Drawing2D.DashStyle.Dot
        loadpen.CustomStartCap = New System.Drawing.Drawing2D.AdjustableArrowCap(3, 5)
        e.Graphics.DrawLine(loadpen, PPt1.X, PPt1.Y, PPt2.X, PPt2.Y)
        e.Graphics.DrawString(Loadintensity, New Font("Verdana", (8)), New Pen(Color.Green, 2).Brush, PPt2.X + (2), PPt2.Y + (2))
        If _DrgFlg = True Then
            e.Graphics.DrawLine(Pens.DarkViolet, New Point(PPt1.X, PPt1.Y), New Point(PPt1.X + (TempEL.CurrentLength * Math.Cos(LoadInclination)), PPt1.Y + (TempEL.CurrentLength * Math.Sin(LoadInclination))))
            Dim a As Integer = (LoadInclination) * (180 / Math.PI)
            e.Graphics.DrawArc(New Pen(Color.DarkViolet, 2), PPt1.X - 40, PPt1.Y - 40, 80, 80, 0, a)
            e.Graphics.DrawString(a, New Font("Verdana", 8), Brushes.DarkViolet, New Point(PPt1.X + (40 * Math.Cos((a / 2) * (Math.PI / 180))), PPt1.Y + (40 * Math.Sin((a / 2) * (Math.PI / 180)))))
        End If
    End Sub
#End Region

#Region "Support Paint Events"
    Private Sub SupportPicPaintPinJoint(ByVal e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double)
        e.Graphics.DrawEllipse(New Pen(OptionD.Nc, 2), M.X - 2, M.Y - 2, 4, 4)
    End Sub

    Private Sub SupportPicPaintPinSupport(ByVal e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double, ByVal ZMfactor As Double)
        '----Triangle
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + ((Math.Cos(-Inclination) + Math.Sin(-Inclination)) * ZMfactor), M.Y + ((-Math.Sin(-Inclination) + Math.Cos(-Inclination))) * ZMfactor), New Point(M.X + ((20 * Math.Cos(-Inclination) + (12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + ((20 * -Math.Sin(-Inclination) + (12 * Math.Cos(-Inclination)))) * ZMfactor))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + ((Math.Cos(-Inclination) + Math.Sin(-Inclination)) * ZMfactor), M.Y + ((-Math.Sin(-Inclination) + Math.Cos(-Inclination))) * ZMfactor), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination)))) * ZMfactor))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (12 * Math.Cos(-Inclination)))) * ZMfactor), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination))) * ZMfactor)))

        '----Bottom Rectangle
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + ((20 * -Math.Sin(-Inclination) + (-20 * Math.Cos(-Inclination)))) * ZMfactor))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((30 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination)))) * ZMfactor))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((30 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((30 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination)))) * ZMfactor))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((30 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination)))) * ZMfactor))

        If _DrgFlg = True Then
            e.Graphics.DrawLine(Pens.DarkViolet, New Point(M.X, M.Y), New Point(M.X + (TempEL.CurrentLength * Math.Cos(Inclination)), M.Y + (TempEL.CurrentLength * Math.Sin(Inclination))))
            Dim a As Integer = (Inclination) * (180 / Math.PI)
            e.Graphics.DrawArc(New Pen(Color.DarkViolet, 2), M.X - 40, M.Y - 40, 80, 80, 0, a)
            e.Graphics.DrawString(a, New Font("Verdana", 8), Brushes.DarkViolet, New Point(M.X + (40 * Math.Cos((a / 2) * (Math.PI / 180))), M.Y + (40 * Math.Sin((a / 2) * (Math.PI / 180)))))
        End If
    End Sub

    Private Sub SupportPicPaintRollerSupport(ByVal e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double, ByVal ZMfactor As Double)
        '----Triangle
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + ((Math.Cos(-Inclination) + Math.Sin(-Inclination)) * ZMfactor), M.Y + ((-Math.Sin(-Inclination) + Math.Cos(-Inclination))) * ZMfactor), New Point(M.X + ((20 * Math.Cos(-Inclination) + (12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + ((20 * -Math.Sin(-Inclination) + (12 * Math.Cos(-Inclination)))) * ZMfactor))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + ((Math.Cos(-Inclination) + Math.Sin(-Inclination)) * ZMfactor), M.Y + ((-Math.Sin(-Inclination) + Math.Cos(-Inclination))) * ZMfactor), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination)))) * ZMfactor))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (12 * Math.Cos(-Inclination)))) * ZMfactor), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination))) * ZMfactor)))

        '----Bottom Circle
        e.Graphics.DrawEllipse(New Pen(OptionD.Nc, 2), New Rectangle(New Point((M.X - (5 * ZMfactor)) + (((25 * Math.Cos(-Inclination)) + (6 * Math.Sin(-Inclination))) * ZMfactor), (M.Y - (5 * ZMfactor)) + ((25 * (-Math.Sin(-Inclination)) + (6 * Math.Cos(-Inclination))) * ZMfactor)), New Size((10 * ZMfactor), (10 * ZMfactor))))
        e.Graphics.DrawEllipse(New Pen(OptionD.Nc, 2), New Rectangle(New Point((M.X - (5 * ZMfactor)) + (((25 * Math.Cos(-Inclination)) + (-6 * Math.Sin(-Inclination))) * ZMfactor), (M.Y - (5 * ZMfactor)) + ((25 * (-Math.Sin(-Inclination)) + (-6 * Math.Cos(-Inclination))) * ZMfactor)), New Size((10 * ZMfactor), (10 * ZMfactor))))

        If _DrgFlg = True Then
            e.Graphics.DrawLine(Pens.DarkViolet, New Point(M.X, M.Y), New Point(M.X + (TempEL.CurrentLength * Math.Cos(Inclination)), M.Y + (TempEL.CurrentLength * Math.Sin(Inclination))))
            Dim a As Integer = (Inclination) * (180 / Math.PI)
            e.Graphics.DrawArc(New Pen(Color.DarkViolet, 2), M.X - 40, M.Y - 40, 80, 80, 0, a)
            e.Graphics.DrawString(a, New Font("Verdana", 8), Brushes.DarkViolet, New Point(M.X + (40 * Math.Cos((a / 2) * (Math.PI / 180))), M.Y + (40 * Math.Sin((a / 2) * (Math.PI / 180)))))
        End If
    End Sub
#End Region

#Region "Load Adding Zone"
#Region "Start Node _ Load Adding Zone"
    Private Sub _SNAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _SNAdd.Click
        Dim intensity As Double = Val(_SNLoad_txt.Text)
        Dim inclination As Double = SNdetails.tempinclination
        If intensity <> 0 Then
            SNdetails.Loadintensity = intensity
            SNdetails.Loadinclination = inclination
            WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.Load = New Line2DT.Node.LoadCondition(intensity, inclination)
            Dim A As New AddPortal2DT(intensity)
            CorrectNodeLoadError(WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN)
        End If
        FixMaxLoad()
        WA2dt.MainPic.Refresh()
    End Sub

    Private Sub _SNDel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _SNDel.Click
        SNdetails.Loadintensity = 0
        SNdetails.Loadinclination = 0
        WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.Load = New Line2DT.Node.LoadCondition(0, 0)
        Dim A As New AddPortal2DT(0)
        CorrectNodeLoadError(WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN)
        FixMaxLoad()
        WA2dt.MainPic.Refresh()
    End Sub

    Private Sub _SNLoad_txt_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _SNLoad_txt.TextChanged
        If IsNumeric(_SNLoad_txt.Text) Then
            If Val(_SNLoad_txt.Text) = 0 Then
                _SNLoad_txt.Text = "10"
            End If
        ElseIf _SNLoad_txt.Text = "-" Then
            _SNLoad_txt.Text = ""
        Else
            _SNLoad_txt.Text = ""
        End If
        If Val(_SNLoad_txt.Text) < 0 Then
            _SNLoad_txt.Text = ""
        End If
        FixMaxLoad()
    End Sub
#End Region

#Region "End Node _ Load Adding Zone"
    Private Sub _ENAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _ENAdd.Click
        Dim intensity As Double = Val(_ENLoad_txt.Text)
        Dim inclination As Double = ENdetails.tempinclination
        If intensity <> 0 Then
            ENdetails.Loadintensity = intensity
            ENdetails.Loadinclination = inclination
            WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.Load = New Line2DT.Node.LoadCondition(intensity, inclination)
            Dim A As New AddPortal2DT(intensity)
            CorrectNodeLoadError(WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN)
        End If
        FixMaxLoad()
        WA2dt.MainPic.Refresh()
    End Sub

    Private Sub _ENDel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _ENDel.Click
        ENdetails.Loadintensity = 0
        ENdetails.Loadinclination = 0
        WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.Load = New Line2DT.Node.LoadCondition(0, 0)
        Dim A As New AddPortal2DT(0)
        CorrectNodeLoadError(WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN)
        FixMaxLoad()
        WA2dt.MainPic.Refresh()
    End Sub

    Private Sub _ENLoad_txt_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _ENLoad_txt.TextChanged
        If IsNumeric(_ENLoad_txt.Text) Then
            If Val(_ENLoad_txt.Text) = 0 Then
                _ENLoad_txt.Text = "10"
            End If
        ElseIf _ENLoad_txt.Text = "-" Then
            _ENLoad_txt.Text = ""
        Else
            _ENLoad_txt.Text = ""
        End If
        If Val(_ENLoad_txt.Text) < 0 Then
            _ENLoad_txt.Text = ""
        End If
        FixMaxLoad()
    End Sub
#End Region

#Region "Correct Node Load Error"
    Private Sub CorrectNodeLoadError(ByVal Nd As Line2DT.Node)
        '----Sort nodes based on Repetation count
        Dim intensity As Double = Nd.Load.Loadintensity
        Dim inclination As Double = Nd.Load.Loadinclination

        For Each El In WA2dt.Mem
            If Nd.index = El.SN.index Then
                WA2dt.Mem(WA2dt.Mem.IndexOf(El)).SN.Load = New Line2DT.Node.LoadCondition(intensity, inclination)
                Continue For
            End If
            If Nd.index = El.EN.index Then
                WA2dt.Mem(WA2dt.Mem.IndexOf(El)).EN.Load = New Line2DT.Node.LoadCondition(intensity, inclination)
                Continue For
            End If
        Next

        For Each Itm In WA2dt.Bob
            If Nd.index = Itm.index Then
                WA2dt.Bob(WA2dt.Bob.IndexOf(Itm)).Load = New Line2DT.Node.LoadCondition(intensity, inclination)
                Exit For
            End If
        Next
    End Sub
#End Region

    Private Sub LoadPicPaintLoad(ByVal e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double)
        e.Graphics.DrawEllipse(New Pen(OptionD.Nc, 2), M.X - 2, M.Y - 2, 4, 4)
    End Sub
#End Region
End Class
