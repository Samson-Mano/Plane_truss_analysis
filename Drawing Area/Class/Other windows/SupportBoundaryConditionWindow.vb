Public Class SupportBoundaryConditionWindow
    Dim TempEL As SelectedElement
    Dim SNsupport As SupportDetails
    Dim ENsupport As SupportDetails
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

    Private Class SupportDetails
        Dim _Supportinclination As Double = 90 * (Math.PI / 180)
        Dim _PJ As Boolean = True
        Dim _PS As Boolean
        Dim _RS As Boolean
        Dim _dxsettlement As Double
        Dim _dysettlement As Double

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

        Public Sub New(ByVal Pjoint As Boolean, ByVal Psupport As Boolean, ByVal Rsupport As Boolean, ByVal supincln As Double, ByVal dx As Double, ByVal dy As Double)
            _PJ = Pjoint
            _PS = Psupport
            _RS = Rsupport

            If _PS = True Or _RS = True Then
                _Supportinclination = supincln
                _dxsettlement = If(dx <> 0, dx, Nothing)
                _dysettlement = If(dy <> 0, dy, Nothing)
            End If
        End Sub
    End Class

    Public Sub New(ByVal El As Line2DT)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        '---------Fix Size of Window
        If MDIMain.Width > MDIMain.Height Then
            Me.Width = 520 '(MDIMain.Width / 3)
            Me.Height = (520 * (MDIMain.Height / MDIMain.Width)) + 235 + 25
        Else
            Me.Height = 520 + 235 + 25 '(MDIMain.Width / 3)
            Me.Width = (520 * (MDIMain.Width / MDIMain.Height))
        End If
        _NodeTab.ItemSize = New Size((Me.Width / 2) - 11, 25)

        CoordFix(El)
        For Each itm In WA2dt.Mem
            _memberBindingSource.Add(itm)
            If itm.Equals(El) Then
                _memberBindingSource.Position = WA2dt.Mem.IndexOf(itm) + 1
            End If
        Next
        SNSupportOptionchange()
        ENSupportOptionchange()
    End Sub

    Private Sub CoordFix(ByVal El As Line2DT)
        Dim Tx As Double
        Dim Ty As Double
        Dim Lcos As Double
        Dim Msin As Double
        Dim theta As Double
        Dim MinSide As Double

        If _SupportPic.Width <= _SupportPic.Height Then
            MinSide = _SupportPic.Width - 40
        Else
            MinSide = _SupportPic.Height - 40
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

        FixMaxLoad()
        TempEL.Mpoint = New Point((Me._SupportPic.Width / 2), (Me._SupportPic.Height / 2))
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

        SNsupport = New SupportDetails(El.SN.Support.PJ, _
                                       El.SN.Support.PS, _
                                       El.SN.Support.RS, _
                                       El.SN.Support.supportinclination, _
                                       El.SN.Support.settlementdx, _
                                       El.SN.Support.settlementdy)
        ENsupport = New SupportDetails(El.EN.Support.PJ, _
                                       El.EN.Support.PS, _
                                       El.EN.Support.RS, _
                                       El.EN.Support.supportinclination, _
                                       El.EN.Support.settlementdx, _
                                       El.EN.Support.settlementdy)

        _SNPJoption.Checked = El.SN.Support.PJ
        _SNPSoption.Checked = El.SN.Support.PS
        _SNRSoption.Checked = El.SN.Support.RS
        _SNdxtxtbox.Text = El.SN.Support.settlementdx
        _SNdytxtbox.Text = El.SN.Support.settlementdy

        _ENPJoption.Checked = El.EN.Support.PJ
        _ENPSoption.Checked = El.EN.Support.PS
        _ENRSoption.Checked = El.EN.Support.RS
        _ENdxtxtbox.Text = El.EN.Support.settlementdx
        _ENdytxtbox.Text = El.EN.Support.settlementdy

        _SupportPic.Refresh()
        _SNSupportPic.Refresh()
        _ENSupportPic.Refresh()

    End Sub

    Private Sub SupportBoundaryConditionWindow_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        WA2dt.selLine.Clear()
        MDIMain._DMCMenable()
        WA2dt.MainPic.Refresh()
    End Sub

#Region "Correct Node Support Error"
    Private Sub SupportBoundaryConditionWindow_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        CorrectNodeSupportError()
    End Sub

    Private Structure Repetation
        Dim NdNumber As Integer
        Dim ReOccurMember As List(Of Integer)
    End Structure

    Private Sub CorrectNodeSupportError()
        '----Sort nodes based on Repetation count
        Dim RCount As New List(Of Repetation)
        For Each Nd In WA2dt.Bob
            Dim TReap As New Repetation
            TReap.ReOccurMember = New List(Of Integer)
            For Each El In WA2dt.Mem
                If Nd.index = El.SN.index Then
                    TReap.ReOccurMember.Add(WA2dt.Mem.IndexOf(El))
                    Continue For
                End If
                If Nd.index = El.EN.index Then
                    TReap.ReOccurMember.Add(WA2dt.Mem.IndexOf(El))
                    Continue For
                End If
            Next
            TReap.NdNumber = WA2dt.Bob.IndexOf(Nd)
            RCount.Add(TReap)
        Next
        Dim PooF As New List(Of Integer)
        For Each Rc In RCount
            Dim SupportRevised As Line2DT.Node.SupportCondition
            For Each ReO In Rc.ReOccurMember
                If WA2dt.Mem(ReO).SN.index = WA2dt.Bob(Rc.NdNumber).index Then
                    If WA2dt.Mem(ReO).SN.Support.PS = True Or _
                        WA2dt.Mem(ReO).SN.Support.RS = True Then
                        SupportRevised = New  _
                                    Line2DT.Node.SupportCondition(False, _
                                                WA2dt.Mem(ReO).SN.Support.PS, _
                                                WA2dt.Mem(ReO).SN.Support.RS, _
                                                WA2dt.Mem(ReO).SN.Support.supportinclination, _
                                                WA2dt.Mem(ReO).SN.Support.settlementdx, _
                                                WA2dt.Mem(ReO).SN.Support.settlementdy)
                        Exit For
                    ElseIf WA2dt.Mem(ReO).SN.Support.PJ = True Then
                        SupportRevised = New  _
                                    Line2DT.Node.SupportCondition( _
                                                WA2dt.Mem(ReO).SN.Support.PJ, _
                                                False, _
                                                False, _
                                                Nothing, _
                                                Nothing, _
                                                Nothing)
                        Continue For
                    Else
                        PooF.Add(ReO)
                        Continue For
                    End If
                ElseIf WA2dt.Mem(ReO).EN.index = WA2dt.Bob(Rc.NdNumber).index Then
                    If WA2dt.Mem(ReO).EN.Support.PS = True Or _
                        WA2dt.Mem(ReO).EN.Support.RS = True Then
                        SupportRevised = New  _
                                    Line2DT.Node.SupportCondition(False, _
                                                WA2dt.Mem(ReO).EN.Support.PS, _
                                                WA2dt.Mem(ReO).EN.Support.RS, _
                                                WA2dt.Mem(ReO).EN.Support.supportinclination, _
                                                WA2dt.Mem(ReO).EN.Support.settlementdx, _
                                                WA2dt.Mem(ReO).EN.Support.settlementdy)
                        Exit For
                    ElseIf WA2dt.Mem(ReO).EN.Support.PJ = True Then
                        SupportRevised = New  _
                                    Line2DT.Node.SupportCondition( _
                                                WA2dt.Mem(ReO).EN.Support.PJ, _
                                                False, _
                                                False, _
                                                Nothing, _
                                                Nothing, _
                                                Nothing)
                        Continue For
                    Else
                        PooF.Add(ReO)
                        Continue For
                    End If
                End If
            Next

            For Each ReO In Rc.ReOccurMember
                If WA2dt.Mem(ReO).SN.index = WA2dt.Bob(Rc.NdNumber).index Then
                    WA2dt.Mem(ReO).SN.Support = SupportRevised
                ElseIf WA2dt.Mem(ReO).EN.index = WA2dt.Bob(Rc.NdNumber).index Then
                    WA2dt.Mem(ReO).EN.Support = SupportRevised
                End If
            Next
            WA2dt.Bob(Rc.NdNumber).Support = SupportRevised
        Next

        For Each leftOut In PooF
            If WA2dt.Mem(leftOut).SN.Support.PJ = False And _
                WA2dt.Mem(leftOut).SN.Support.PS = False And _
                WA2dt.Mem(leftOut).SN.Support.RS = False Then

                MsgBox("Something gone Wrong !!!!! Its My Fault", MsgBoxStyle.Critical, "TrussANS")
            ElseIf WA2dt.Mem(leftOut).EN.Support.PJ = False And _
                   WA2dt.Mem(leftOut).EN.Support.PS = False And _
                   WA2dt.Mem(leftOut).EN.Support.RS = False Then

                MsgBox("Something gone Wrong !!!!! Its My Fault", MsgBoxStyle.Critical, "TrussANS")
            End If
        Next
    End Sub
#End Region

    Private Sub SupportBoundaryConditionWindow_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        AddHandler _SNPJoption.CheckedChanged, AddressOf SNSupportOptionchange
        AddHandler _SNPSoption.CheckedChanged, AddressOf SNSupportOptionchange
        AddHandler _SNRSoption.CheckedChanged, AddressOf SNSupportOptionchange

        AddHandler _ENPJoption.CheckedChanged, AddressOf ENSupportOptionchange
        AddHandler _ENPSoption.CheckedChanged, AddressOf ENSupportOptionchange
        AddHandler _ENRSoption.CheckedChanged, AddressOf ENSupportOptionchange
    End Sub

    Private Sub _memberNavigator_RefreshItems(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _memberBindingSource.CurrentChanged
        If _memberBindingsource.Count <> 0 Then
            CoordFix(_memberBindingsource.Current)
            WA2dt.selLine(0) = _memberBindingSource.IndexOf(_memberBindingSource.Current)
            _SupportPic.Refresh()
            WA2dt.MainPic.Refresh()
        End If
    End Sub

    Private Sub _NodeTab_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _NodeTab.SelectedIndexChanged
        _SupportPic.Refresh()
    End Sub

    Private Sub SNSupportOptionchange()
        If _SNPJoption.Checked = True Then
            SNsupport.PJ = True
            SNsupport.PS = False
            SNsupport.RS = False
            Label2.Visible = False
            Label3.Visible = False
            _SNdxtxtbox.Visible = False
            _SNdytxtbox.Visible = False
            _SNDescriptionLabel.Text = " Pin Joint allows displacement on all directions "
        ElseIf _SNPSoption.Checked = True Then
            SNsupport.PJ = False
            SNsupport.PS = True
            SNsupport.RS = False
            Label2.Visible = True
            Label3.Visible = True
            _SNdxtxtbox.Visible = True
            _SNdytxtbox.Visible = True
            _SNDescriptionLabel.Text = " Pin Support restricts displacement in all directions but it will not restricts moment "
        ElseIf _SNRSoption.Checked = True Then
            SNsupport.PJ = False
            SNsupport.PS = False
            SNsupport.RS = True
            Label2.Visible = False
            Label3.Visible = True
            _SNdxtxtbox.Visible = False
            _SNdytxtbox.Visible = True
            _SNDescriptionLabel.Text = " Roller Support restricts displacement in Double plane but it will not restricts moment "
        End If
        'WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.Support = _
        '                                New Line2DT.Node.SupportCondition(SNsupport.PJ, _
        '                                                                  SNsupport.PS, _
        '                                                                  SNsupport.RS, _
        '                                                                  SNsupport.Supportinclination, _
        '                                                                  dx:=If(Val(_SNdxtxtbox.Text) <> 0, Val(_SNdxtxtbox.Text), Nothing), _
        '                                                                  dy:=If(Val(_SNdytxtbox.Text) <> 0, Val(_SNdytxtbox.Text), Nothing))
        _SNSupportPic.Refresh()
        _SupportPic.Refresh()
        WA2dt.MainPic.Refresh()
    End Sub

    Private Sub ENSupportOptionchange()
        If _ENPJoption.Checked = True Then
            ENsupport.PJ = True
            ENsupport.PS = False
            ENsupport.RS = False
            Label4.Visible = False
            Label5.Visible = False
            _ENdxtxtbox.Visible = False
            _ENdytxtbox.Visible = False
            _ENDescriptionLabel.Text = " Pin Joint allows displacement on all directions "
        ElseIf _ENPSoption.Checked = True Then
            ENsupport.PJ = False
            ENsupport.PS = True
            ENsupport.RS = False
            Label4.Visible = True
            Label5.Visible = True
            _ENdxtxtbox.Visible = True
            _ENdytxtbox.Visible = True
            _ENDescriptionLabel.Text = " Pin Support restricts displacement in all directions but it will not restricts moment "
        ElseIf _ENRSoption.Checked = True Then
            ENsupport.PJ = False
            ENsupport.PS = False
            ENsupport.RS = True
            Label4.Visible = True
            Label5.Visible = False
            _ENdxtxtbox.Visible = False
            _ENdytxtbox.Visible = True
            _ENDescriptionLabel.Text = " Roller Support restricts displacement in Double plane but it will not restricts moment "
        End If
        'WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.Support = _
        '                                    New Line2DT.Node.SupportCondition(ENsupport.PJ, _
        '                                                                      ENsupport.PS, _
        '                                                                      ENsupport.RS, _
        '                                                                      ENsupport.Supportinclination, _
        '                                                                      dx:=If(Val(_ENdxtxtbox.Text) <> 0, Val(_ENdxtxtbox.Text), Nothing), _
        '                                                                      dy:=If(Val(_ENdytxtbox.Text) <> 0, Val(_ENdytxtbox.Text), Nothing))
        _ENSupportPic.Refresh()
        _SupportPic.Refresh()
        WA2dt.MainPic.Refresh()
    End Sub

#Region "Paint Support Pic"
    Private Sub _SupportPic_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles _SupportPic.Paint
        Try
            SupportAreaPaintMember(e)
            SupportAreaPaintNode(e)
            SupportAreaPaintLoad(e)
            tempSUPPORTPaint(e)
        Catch ex As Exception
            'MsgBox("Zzzzzzzzsad")
        End Try
    End Sub

    Private Sub SupportAreaPaintMember(ByRef e As System.Windows.Forms.PaintEventArgs)
        e.Graphics.DrawLine(New Pen(OptionD.Mc, 2), TempEL.Spoint, TempEL.Epoint)
        e.Graphics.DrawString(Math.Round(TempEL.ActualLength, OptionD.Prec), New Font("Verdana", (8)), New Pen(OptionD.Lc, 2 / WA2dt.Zm).Brush, TempEL.Mpoint.X, TempEL.Mpoint.Y)
    End Sub

    Private Sub SupportAreaPaintNode(ByRef e As System.Windows.Forms.PaintEventArgs)
        ' Start Node
        'e.Graphics.FillEllipse(New Pen(OptionD.Nc, 4).Brush, TempEL.Spoint.X - (2), TempEL.Spoint.Y - (2), 4, 4)
        e.Graphics.DrawString(TempEL.SNode, New Font("Verdana", (8)), New Pen(OptionD.NNc, 2).Brush, TempEL.Spoint.X - (14), TempEL.Spoint.Y - (14))
        ' End Node
        'e.Graphics.FillEllipse(New Pen(OptionD.Nc, 4).Brush, TempEL.Epoint.X - (2), TempEL.Epoint.Y - (2), 4, 4)
        e.Graphics.DrawString(TempEL.ENode, New Font("Verdana", (8)), New Pen(OptionD.NNc, 2).Brush, TempEL.Epoint.X - (14), TempEL.Epoint.Y - (14))
    End Sub

    Private Sub SupportAreaPaintLoad(ByRef e As System.Windows.Forms.PaintEventArgs)
        If WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.Load.Loadintensity <> 0 Then
            PaintExtLoad(e, TempEL.Spoint, WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.Load.Loadintensity, WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.Load.Loadinclination)
        End If
        If WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.Load.Loadintensity <> 0 Then
            PaintExtLoad(e, TempEL.Epoint, WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.Load.Loadintensity, WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.Load.Loadinclination)
        End If
    End Sub

    Private Sub tempSUPPORTPaint(ByRef e As System.Windows.Forms.PaintEventArgs)
        If _NodeTab.SelectedIndex = 0 Then
            e.Graphics.DrawRectangle(Pens.Black, TempEL.Spoint.X - 30, TempEL.Spoint.Y - 30, 60, 60)

        ElseIf _NodeTab.SelectedIndex = 1 Then
            e.Graphics.DrawRectangle(Pens.Black, TempEL.Epoint.X - 30, TempEL.Epoint.Y - 30, 60, 60)
        End If
        '--- Start Node
        If WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.Support.PJ = True Then
            SupportPicPaintPinJoint(e, TempEL.Spoint, WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.Support.supportinclination)
        ElseIf WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.Support.PS = True Then
            SupportPicPaintPinSupport(e, TempEL.Spoint, WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.Support.supportinclination, 0.5, False)
        ElseIf WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.Support.RS = True Then
            SupportPicPaintRollerSupport(e, TempEL.Spoint, WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.Support.supportinclination, 0.5, False)
        End If

        '--- End Node
        If WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.Support.PJ = True Then
            SupportPicPaintPinJoint(e, TempEL.Epoint, WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.Support.supportinclination)
        ElseIf WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.Support.PS = True Then
            SupportPicPaintPinSupport(e, TempEL.Epoint, WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.Support.supportinclination, 0.5, False)
        ElseIf WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.Support.RS = True Then
            SupportPicPaintRollerSupport(e, TempEL.Epoint, WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.Support.supportinclination, 0.5, False)
        End If
    End Sub
#End Region

#Region "Paint Start Node Support Pic Events"
    Private Sub _SNSupportPic_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles _SNSupportPic.MouseClick
        'If SNsupport.Supportinclination = (45 * (Math.PI / 180)) Then
        '    SNsupport.Supportinclination = (90 * (Math.PI / 180))
        'ElseIf SNsupport.Supportinclination = (90 * (Math.PI / 180)) Then
        '    SNsupport.Supportinclination = (135 * (Math.PI / 180))
        'ElseIf SNsupport.Supportinclination = (135 * (Math.PI / 180)) Then
        '    SNsupport.Supportinclination = (180 * (Math.PI / 180))
        'Else
        '    SNsupport.Supportinclination = (45 * (Math.PI / 180))
        'End If
        _SNSupportPic.Refresh()
    End Sub

    Private Sub _SNSupportPic_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles _SNSupportPic.MouseDown
        _DrgFlg = True
        _SNSupportPic.Refresh()
        _SupportPic.Refresh()
    End Sub

    Private Sub _SNSupportPic_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles _SNSupportPic.MouseMove
        If _DrgFlg = True And (_SNPSoption.Checked = True Or _SNRSoption.Checked = True) Then
            SNsupport.Supportinclination = If((e.Y - 80) < 0, -1 * Math.Acos((e.X - 80) / (Math.Sqrt(Math.Pow((e.X - 80), 2) + Math.Pow((e.Y - 80), 2)))), Math.Acos((e.X - 80) / (Math.Sqrt(Math.Pow((e.X - 80), 2) + Math.Pow((e.Y - 80), 2)))))
            _SNSupportPic.Refresh()
            _SupportPic.Refresh()
            SNSupportOptionchange()
        End If
    End Sub

    Private Sub _SNSupportPic_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles _SNSupportPic.MouseUp
        _DrgFlg = False
        _SNSupportPic.Refresh()
        _SupportPic.Refresh()
    End Sub

    Private Sub _SNSupportPic_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles _SNSupportPic.Paint
        e.Graphics.DrawLine(Pens.BurlyWood, 80, 0, 80, 160)
        e.Graphics.DrawLine(Pens.BurlyWood, 0, 80, 160, 80)

        e.Graphics.DrawLine(New Pen(OptionD.Mc, 4), New Point(80, 80), New Point(80 + (TempEL.Epoint.X - TempEL.Spoint.X), 80 + (TempEL.Epoint.Y - TempEL.Spoint.Y)))
        e.Graphics.DrawString(TempEL.SNode, New Font("Verdana", 10), New Pen(OptionD.Nc, 4).Brush, New Point(50, 50))

        If SNsupport.PJ = True Then
            SupportPicPaintPinJoint(e, New Point(80, 80), SNsupport.Supportinclination)
        ElseIf SNsupport.PS = True Then
            SupportPicPaintPinSupport(e, New Point(80, 80), SNsupport.Supportinclination, 1, _DrgFlg)
        ElseIf SNsupport.RS = True Then
            SupportPicPaintRollerSupport(e, New Point(80, 80), SNsupport.Supportinclination, 1, _DrgFlg)
        End If

        If WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.Load.Loadintensity <> 0 Then
            PaintExtLoad(e, New Point(80, 80), WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.Load.Loadintensity, WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.Load.Loadinclination)
        End If
    End Sub
#End Region

#Region "Paint End Node Support Pic Events"
    Private Sub _ENSupportPic_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles _ENSupportPic.MouseClick
        'If SNsupport.Supportinclination = (45 * (Math.PI / 180)) Then
        '    SNsupport.Supportinclination = (90 * (Math.PI / 180))
        'ElseIf SNsupport.Supportinclination = (90 * (Math.PI / 180)) Then
        '    SNsupport.Supportinclination = (135 * (Math.PI / 180))
        'ElseIf SNsupport.Supportinclination = (135 * (Math.PI / 180)) Then
        '    SNsupport.Supportinclination = (180 * (Math.PI / 180))
        'Else
        '    SNsupport.Supportinclination = (45 * (Math.PI / 180))
        'End If
        _ENSupportPic.Refresh()
    End Sub

    Private Sub _ENSupportPic_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles _ENSupportPic.MouseDown
        _DrgFlg = True
        _ENSupportPic.Refresh()
        _SupportPic.Refresh()
    End Sub

    Private Sub _ENSupportPic_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles _ENSupportPic.MouseMove
        If _DrgFlg = True And (_ENPSoption.Checked = True Or _ENRSoption.Checked = True) Then
            ENsupport.Supportinclination = If((e.Y - 80) < 0, -1 * Math.Acos((e.X - 80) / (Math.Sqrt(Math.Pow((e.X - 80), 2) + Math.Pow((e.Y - 80), 2)))), Math.Acos((e.X - 80) / (Math.Sqrt(Math.Pow((e.X - 80), 2) + Math.Pow((e.Y - 80), 2)))))
            _ENSupportPic.Refresh()
            _SupportPic.Refresh()
            ENSupportOptionchange()
        End If
    End Sub

    Private Sub _ENSupportPic_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles _ENSupportPic.MouseUp
        _DrgFlg = False
        _ENSupportPic.Refresh()
        _SupportPic.Refresh()
    End Sub

    Private Sub _ENSupportPic_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles _ENSupportPic.Paint
        e.Graphics.DrawLine(Pens.BurlyWood, 80, 0, 80, 160)
        e.Graphics.DrawLine(Pens.BurlyWood, 0, 80, 160, 80)

        e.Graphics.DrawLine(New Pen(OptionD.Mc, 4), New Point(80, 80), New Point((80 + (TempEL.Spoint.X - TempEL.Epoint.X)), (80 + (TempEL.Spoint.Y - TempEL.Epoint.Y))))
        e.Graphics.DrawString(TempEL.ENode, New Font("Verdana", 10), New Pen(OptionD.Nc, 4).Brush, New Point(50, 50))

        If ENsupport.PJ = True Then
            SupportPicPaintPinJoint(e, New Point(80, 80), ENsupport.Supportinclination)
        ElseIf ENsupport.PS = True Then
            SupportPicPaintPinSupport(e, New Point(80, 80), ENsupport.Supportinclination, 1, _DrgFlg)
        ElseIf ENsupport.RS = True Then
            SupportPicPaintRollerSupport(e, New Point(80, 80), ENsupport.Supportinclination, 1, _DrgFlg)
        End If

        If WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.Load.Loadintensity <> 0 Then
            PaintExtLoad(e, New Point(80, 80), WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.Load.Loadintensity, WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.Load.Loadinclination)
        End If
    End Sub
#End Region

#Region "Support Pic Paint Events _ Load & Support"
    Private Sub SupportPicPaintPinJoint(ByVal e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double)
        e.Graphics.DrawEllipse(New Pen(OptionD.Nc, 2), M.X - 2, M.Y - 2, 4, 4)
    End Sub

    Private Sub SupportPicPaintPinSupport(ByVal e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double, ByVal ZMfactor As Double, ByVal Df As Boolean)
        '----Triangle
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + ((Math.Cos(-Inclination) + Math.Sin(-Inclination)) * ZMfactor), M.Y + ((-Math.Sin(-Inclination) + Math.Cos(-Inclination))) * ZMfactor), New Point(M.X + ((20 * Math.Cos(-Inclination) + (12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + ((20 * -Math.Sin(-Inclination) + (12 * Math.Cos(-Inclination)))) * ZMfactor))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + ((Math.Cos(-Inclination) + Math.Sin(-Inclination)) * ZMfactor), M.Y + ((-Math.Sin(-Inclination) + Math.Cos(-Inclination))) * ZMfactor), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination)))) * ZMfactor))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (12 * Math.Cos(-Inclination)))) * ZMfactor), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination))) * ZMfactor)))

        '----Bottom Rectangle
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + ((20 * -Math.Sin(-Inclination) + (-20 * Math.Cos(-Inclination)))) * ZMfactor))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((30 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination)))) * ZMfactor))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((30 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((30 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination)))) * ZMfactor))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination))) * ZMfactor)), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((30 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination)))) * ZMfactor))

        If Df = True Then
            e.Graphics.DrawLine(Pens.DarkViolet, New Point(M.X, M.Y), New Point(M.X + (TempEL.CurrentLength * Math.Cos(Inclination)), M.Y + (TempEL.CurrentLength * Math.Sin(Inclination))))
            Dim a As Integer = (Inclination) * (180 / Math.PI)
            e.Graphics.DrawArc(New Pen(Color.DarkViolet, 2), M.X - 40, M.Y - 40, 80, 80, 0, a)
            e.Graphics.DrawString(a, New Font("Verdana", 8), Brushes.DarkViolet, New Point(M.X + (40 * Math.Cos((a / 2) * (Math.PI / 180))), M.Y + (40 * Math.Sin((a / 2) * (Math.PI / 180)))))
        End If
    End Sub

    Private Sub SupportPicPaintRollerSupport(ByVal e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double, ByVal ZMfactor As Double, ByVal Df As Boolean)
        '----Triangle
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + ((Math.Cos(-Inclination) + Math.Sin(-Inclination)) * ZMfactor), M.Y + ((-Math.Sin(-Inclination) + Math.Cos(-Inclination))) * ZMfactor), New Point(M.X + ((20 * Math.Cos(-Inclination) + (12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + ((20 * -Math.Sin(-Inclination) + (12 * Math.Cos(-Inclination)))) * ZMfactor))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + ((Math.Cos(-Inclination) + Math.Sin(-Inclination)) * ZMfactor), M.Y + ((-Math.Sin(-Inclination) + Math.Cos(-Inclination))) * ZMfactor), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination)))) * ZMfactor))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (12 * Math.Cos(-Inclination)))) * ZMfactor), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination))) * ZMfactor)))

        '----Bottom Circle
        e.Graphics.DrawEllipse(New Pen(OptionD.Nc, 2), New Rectangle(New Point((M.X - (5 * ZMfactor)) + (((25 * Math.Cos(-Inclination)) + (6 * Math.Sin(-Inclination))) * ZMfactor), (M.Y - (5 * ZMfactor)) + ((25 * (-Math.Sin(-Inclination)) + (6 * Math.Cos(-Inclination))) * ZMfactor)), New Size((10 * ZMfactor), (10 * ZMfactor))))
        e.Graphics.DrawEllipse(New Pen(OptionD.Nc, 2), New Rectangle(New Point((M.X - (5 * ZMfactor)) + (((25 * Math.Cos(-Inclination)) + (-6 * Math.Sin(-Inclination))) * ZMfactor), (M.Y - (5 * ZMfactor)) + ((25 * (-Math.Sin(-Inclination)) + (-6 * Math.Cos(-Inclination))) * ZMfactor)), New Size((10 * ZMfactor), (10 * ZMfactor))))

        If Df = True Then
            e.Graphics.DrawLine(Pens.DarkViolet, New Point(M.X, M.Y), New Point(M.X + (TempEL.CurrentLength * Math.Cos(Inclination)), M.Y + (TempEL.CurrentLength * Math.Sin(Inclination))))
            Dim a As Integer = (Inclination) * (180 / Math.PI)
            e.Graphics.DrawArc(New Pen(Color.DarkViolet, 2), M.X - 40, M.Y - 40, 80, 80, 0, a)
            e.Graphics.DrawString(a, New Font("Verdana", 8), Brushes.DarkViolet, New Point(M.X + (40 * Math.Cos((a / 2) * (Math.PI / 180))), M.Y + (40 * Math.Sin((a / 2) * (Math.PI / 180)))))
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
#End Region

    Private Sub _ENAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _ENAdd.Click
        If _ENPJoption.Checked = True Then
            ENsupport.PJ = True
            ENsupport.PS = False
            ENsupport.RS = False
        ElseIf _ENPSoption.Checked = True Then
            ENsupport.PJ = False
            ENsupport.PS = True
            ENsupport.RS = False
        ElseIf _ENRSoption.Checked = True Then
            ENsupport.PJ = False
            ENsupport.PS = False
            ENsupport.RS = True
        End If
        Dim NoteIndex As Integer = WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.index
        WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.Support = _
                                          New Line2DT.Node.SupportCondition(ENsupport.PJ, _
                                                                            ENsupport.PS, _
                                                                            ENsupport.RS, _
                                                                            ENsupport.Supportinclination, _
                                                                            If(Val(_ENdxtxtbox.Text) <> 0, Val(_ENdxtxtbox.Text), Nothing), _
                                                                            If(Val(_ENdytxtbox.Text) <> 0, Val(_ENdytxtbox.Text), Nothing))
        For Each El In WA2dt.Mem
            If El.SN.index = NoteIndex Then
                WA2dt.Mem(WA2dt.Mem.IndexOf(El)).SN.Support = _
                                          New Line2DT.Node.SupportCondition(ENsupport.PJ, _
                                                                            ENsupport.PS, _
                                                                            ENsupport.RS, _
                                                                            ENsupport.Supportinclination, _
                                                                            If(Val(_ENdxtxtbox.Text) <> 0, Val(_ENdxtxtbox.Text), Nothing), _
                                                                            If(Val(_ENdytxtbox.Text) <> 0, Val(_ENdytxtbox.Text), Nothing))
            ElseIf El.EN.index = NoteIndex Then
                WA2dt.Mem(WA2dt.Mem.IndexOf(El)).EN.Support = _
                                          New Line2DT.Node.SupportCondition(ENsupport.PJ, _
                                                                            ENsupport.PS, _
                                                                            ENsupport.RS, _
                                                                            ENsupport.Supportinclination, _
                                                                            If(Val(_ENdxtxtbox.Text) <> 0, Val(_ENdxtxtbox.Text), Nothing), _
                                                                            If(Val(_ENdytxtbox.Text) <> 0, Val(_ENdytxtbox.Text), Nothing))
            End If
        Next
        For Each Nd In WA2dt.Bob
            If Nd.index = NoteIndex Then
                WA2dt.Bob(WA2dt.Bob.IndexOf(Nd)).Support = _
                                          New Line2DT.Node.SupportCondition(ENsupport.PJ, _
                                                                            ENsupport.PS, _
                                                                            ENsupport.RS, _
                                                                            ENsupport.Supportinclination, _
                                                                            If(Val(_ENdxtxtbox.Text) <> 0, Val(_ENdxtxtbox.Text), Nothing), _
                                                                            If(Val(_ENdytxtbox.Text) <> 0, Val(_ENdytxtbox.Text), Nothing))
            End If
        Next
        _ENSupportPic.Refresh()
        _SupportPic.Refresh()
        WA2dt.MainPic.Refresh()
    End Sub

    Private Sub _SNAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _SNAdd.Click
        If _SNPJoption.Checked = True Then
            SNsupport.PJ = True
            SNsupport.PS = False
            SNsupport.RS = False
        ElseIf _SNPSoption.Checked = True Then
            SNsupport.PJ = False
            SNsupport.PS = True
            SNsupport.RS = False
        ElseIf _SNRSoption.Checked = True Then
            SNsupport.PJ = False
            SNsupport.PS = False
            SNsupport.RS = True
        End If
        Dim NoteIndex As Integer = WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.index
        WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.Support = _
                                      New Line2DT.Node.SupportCondition(SNsupport.PJ, _
                                                                        SNsupport.PS, _
                                                                        SNsupport.RS, _
                                                                        SNsupport.Supportinclination, _
                                                                        If(Val(_SNdxtxtbox.Text) <> 0, Val(_SNdxtxtbox.Text), Nothing), _
                                                                        If(Val(_SNdytxtbox.Text) <> 0, Val(_SNdytxtbox.Text), Nothing))
        For Each El In WA2dt.Mem
            If El.SN.index = NoteIndex Then
                WA2dt.Mem(WA2dt.Mem.IndexOf(El)).SN.Support = _
                                                      New Line2DT.Node.SupportCondition(SNsupport.PJ, _
                                                                                        SNsupport.PS, _
                                                                                        SNsupport.RS, _
                                                                                        SNsupport.Supportinclination, _
                                                                                        If(Val(_SNdxtxtbox.Text) <> 0, Val(_SNdxtxtbox.Text), Nothing), _
                                                                                        If(Val(_SNdytxtbox.Text) <> 0, Val(_SNdytxtbox.Text), Nothing))
            ElseIf El.EN.index = NoteIndex Then
                WA2dt.Mem(WA2dt.Mem.IndexOf(El)).EN.Support = _
                                                      New Line2DT.Node.SupportCondition(SNsupport.PJ, _
                                                                                        SNsupport.PS, _
                                                                                        SNsupport.RS, _
                                                                                        SNsupport.Supportinclination, _
                                                                                        If(Val(_SNdxtxtbox.Text) <> 0, Val(_SNdxtxtbox.Text), Nothing), _
                                                                                        If(Val(_SNdytxtbox.Text) <> 0, Val(_SNdytxtbox.Text), Nothing))
            End If
        Next
        For Each Nd In WA2dt.Bob
            If Nd.index = NoteIndex Then
                WA2dt.Bob(WA2dt.Bob.IndexOf(Nd)).Support = _
                                                      New Line2DT.Node.SupportCondition(SNsupport.PJ, _
                                                                                        SNsupport.PS, _
                                                                                        SNsupport.RS, _
                                                                                        SNsupport.Supportinclination, _
                                                                                        If(Val(_SNdxtxtbox.Text) <> 0, Val(_SNdxtxtbox.Text), Nothing), _
                                                                                        If(Val(_SNdytxtbox.Text) <> 0, Val(_SNdytxtbox.Text), Nothing))
            End If
        Next
        _SNSupportPic.Refresh()
        _SupportPic.Refresh()
        WA2dt.MainPic.Refresh()
    End Sub

    Private Sub FixMaxLoad()
        Try
            TempEL.maxload = 0.000001
            If WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.Load.Loadintensity > TempEL.maxload Then
                TempEL.maxload = WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).SN.Load.Loadintensity
            End If
            If WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.Load.Loadintensity > TempEL.maxload Then
                TempEL.maxload = WA2dt.Mem(WA2dt.Mem.IndexOf(_memberBindingSource.Current)).EN.Load.Loadintensity
            End If
            _SupportPic.Refresh()
            _SNSupportPic.Refresh()
        Catch ex As Exception

        End Try
    End Sub
End Class