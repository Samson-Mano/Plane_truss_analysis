Public Class WorkArea2DT
#Region "Main Variables"
    Dim _Mem As New List(Of Line2DT)
    Dim _Bob As New List(Of Line2DT.Node)
    Dim _selLine As New List(Of Integer)
    Dim _PD As SnapPredicate2DT
    Dim _Zm As Double = 1
    Dim _mpoint As Point
    Dim _Spoint As Point
    Dim _Epoint As Point
    Dim _Cpoint As Point
    Dim _TempExtraPoint As Point
    Dim Tpaint As PTempShapes2DT
    Dim Ppaint As PermPaint2DT
    Dim _Linflg As Boolean = False
    Dim _Arcflg As Boolean = False
    Dim _Panflg As Boolean = False
    Dim _AP As AddPortal2DT
    Dim _HistU As New List(Of History2DT)
    Dim _HistR As New List(Of History2DT)
    Dim Apts() As Point
    Dim _PaintNULL As Boolean = False
    Dim _PredictNULL As Boolean = False
    Dim _MaxLoad As Double
    Dim _MaxLength As Double
    Dim _MinLength As Double
    Dim _UstreamGap As Integer
    Dim _Loadhtfactor As Double
#End Region

#Region "Get Set Property"
    Public Property PD() As SnapPredicate2DT
        Get
            Return _PD
        End Get
        Set(ByVal value As SnapPredicate2DT)
            _PD = value
        End Set
    End Property

    Public Property Mem() As List(Of Line2DT)
        Get
            Return _Mem
        End Get
        Set(ByVal value As List(Of Line2DT))
            _Mem = value
        End Set
    End Property

    Public Property Bob() As List(Of Line2DT.Node)
        Get
            Return _Bob
        End Get
        Set(ByVal value As List(Of Line2DT.Node))
            _Bob = value
        End Set
    End Property

    Public Property HistU() As List(Of History2DT)
        Get
            Return _HistU
        End Get
        Set(ByVal value As List(Of History2DT))
            _HistU = value
        End Set
    End Property

    Public Property HistR() As List(Of History2DT)
        Get
            Return _HistR
        End Get
        Set(ByVal value As List(Of History2DT))
            _HistR = value
        End Set
    End Property

    Public Property Mpoint() As Point
        Get
            Return _mpoint
        End Get
        Set(ByVal value As Point)
            _mpoint = value
        End Set
    End Property

    Public Property Spoint() As Point
        Get
            Return _Spoint
        End Get
        Set(ByVal value As Point)

        End Set
    End Property

    Public Property Cpoint() As Point
        Get
            Return _Cpoint
        End Get
        Set(ByVal value As Point)

        End Set
    End Property

    Public Property Epoint() As Point
        Get
            Return _Epoint
        End Get
        Set(ByVal value As Point)
            _Epoint = value
        End Set
    End Property '--- Sets from MDImain.commandtxtbox

    Public Property TempExtraPoint() As Point
        Get
            Return _TempExtraPoint
        End Get
        Set(ByVal value As Point)
            _TempExtraPoint = value
        End Set
    End Property

    Public Property Zm() As Double
        Get
            Return _Zm
        End Get
        Set(ByVal value As Double)
            _Zm = value
        End Set
    End Property

    Public Property Linflg() As Boolean
        Get
            Return _Linflg
        End Get
        Set(ByVal value As Boolean)
            _Linflg = value
        End Set
    End Property '--- Sets from MDImain.commandtxtbox

    Public Property Arcflg() As Boolean
        Get
            Return _Arcflg
        End Get
        Set(ByVal value As Boolean)
            _Arcflg = value
        End Set
    End Property '--- Sets from MDImain.commandtxtbox

    Public Property PaintNULL() As Boolean
        Get
            Return _PaintNULL
        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property

    Public Property PredictNULL() As Boolean
        Get
            Return _PredictNULL
        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property

    Public Property selLine() As List(Of Integer)
        Get
            Return _selLine
        End Get
        Set(ByVal value As List(Of Integer))
            _selLine = value
        End Set
    End Property

    Public Property AP() As AddPortal2DT
        Get
            Return _AP
        End Get
        Set(ByVal value As AddPortal2DT)
            _AP = value
        End Set
    End Property

    Public Property Maxload()
        Get
            Return _MaxLoad
        End Get
        Set(ByVal value)
            _MaxLoad = value
        End Set
    End Property

    Public Property MaxLength() As Double
        Get
            Return _MaxLength
        End Get
        Set(ByVal value As Double)
            _MaxLength = value
        End Set
    End Property

    Public Property MinLength() As Double
        Get
            Return _MinLength
        End Get
        Set(ByVal value As Double)
            _MinLength = value
        End Set
    End Property

    Public Property UStreamGap() As Integer
        Get
            Return _UstreamGap
        End Get
        Set(ByVal value As Integer)
            _UstreamGap = value
        End Set
    End Property

    Public Property Loadhtfactor() As Double
        Get
            Return _Loadhtfactor
        End Get
        Set(ByVal value As Double)
            _Loadhtfactor = value
        End Set
    End Property
#End Region

    Private Sub WorkArea2DT_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        WA2dt.PD = New SnapPredicate2DT
        _mpoint = New Point(MainPic.Width / 2, MainPic.Height / 2)
    End Sub

#Region "Zoom Handler"

    Private Sub MainPic_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MainPic.MouseDoubleClick
        If e.Button = Windows.Forms.MouseButtons.Middle Then
            _AP = New AddPortal2DT(New Point((e.X / Zm) - WA2dt.Mpoint.X, (e.Y / Zm) - WA2dt.Mpoint.Y), New Point(0, 0), 7)
            'MTPic.Refresh()
            MainPic.Refresh()
        End If
    End Sub

    Private Sub MainPic_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles MainPic.MouseEnter
        MainPic.Focus()
        _PD = New SnapPredicate2DT
        Windows.Forms.Cursor.Show()
    End Sub

    Private Sub MainPic_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles MainPic.MouseLeave
        _PD = New SnapPredicate2DT
        Windows.Forms.Cursor.Show()
    End Sub

    Private Sub MainPic_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MainPic.MouseWheel
        Dim xw, yw As Double
        MainPic.Focus()
        xw = (e.X / Zm) - _mpoint.X
        yw = (e.Y / Zm) - _mpoint.Y
        If e.Delta > 0 Then
            If Zm < 100 Then
                Zm = Zm + 0.1
            End If
        ElseIf e.Delta < 0 Then
            If Zm > 0.101 Then
                Zm = Zm - 0.1
                'ElseIf Zm <= 0.1 Then
                '    If Zm > 0.0101 Then
                '        Zm = Zm - 0.01
                '    ElseIf Zm <= 0.01 Then
                '        If Zm > 0.00101 Then
                '            Zm = Zm - 0.001
                '        End If
                '    End If
            End If
        End If
        _mpoint.X = (e.X / Zm) - xw
        _mpoint.Y = (e.Y / Zm) - yw
        MTPic.Refresh()
        _PD = New SnapPredicate2DT
    End Sub
#End Region

#Region "Move Handler"
    Private Sub MainPic_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MainPic.MouseMove
        _Cpoint = New Point((e.X), (e.Y))
        MTPic.Refresh()
    End Sub
#End Region

#Region "Click Handler"
    Private Sub MainPic_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MainPic.MouseClick
        MainPic.Focus()
        If e.Button <> Windows.Forms.MouseButtons.Middle Then
            MDIMain._Commandtxtbox.Visible = False
            MDIMain._commandtxtlabel.Visible = False
        End If
        If e.Button = Windows.Forms.MouseButtons.Right And (Linflg = False And Arcflg = False) Then
            MDIMain.ContextMenuStrip2.Show(MainPic.PointToScreen(e.Location))
        End If
        If MDIMain._line.Checked = True Then
            If e.Button = Windows.Forms.MouseButtons.Left Then
                If _Linflg = False Then
                    _Spoint = New Point((e.X / _Zm), (e.Y / _Zm))
                    Dim A As New PTempShapes2DT(_Spoint)
                    '_Spoint = New Point((_Spoint.X - Mpoint.X), (_Spoint.Y - Mpoint.Y))
                    _AP = New AddPortal2DT(_Spoint, _Epoint, 0)
                    _Linflg = True
                    MDIMain._Commandtxtbox.Visible = True
                    MDIMain._commandtxtlabel.Visible = True
                Else
                    _Epoint = New Point(((e.X / _Zm) - Mpoint.X), ((e.Y / _Zm) - Mpoint.Y))
                    _AP = New AddPortal2DT(_Spoint, _Epoint, 1)
                    _Linflg = False
                    MDIMain._Commandtxtbox.Visible = False
                    MDIMain._commandtxtlabel.Visible = False
                End If
            ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
                _Linflg = False
                Exit Sub
            End If
        ElseIf MDIMain._arc.Checked = True Then
            If e.Button = Windows.Forms.MouseButtons.Left Then
                If _Linflg = False And _Arcflg = False Then
                    _Spoint = New Point(e.X / _Zm, e.Y / _Zm)
                    Dim A As New PTempShapes2DT(_Spoint)
                    '_Spoint = New Point((_Spoint.X - Mpoint.X), (_Spoint.Y - Mpoint.Y))
                    _Arcflg = True
                    _Linflg = False
                    MDIMain._Commandtxtbox.Visible = True
                    MDIMain._commandtxtlabel.Visible = True
                ElseIf _Linflg = False And _Arcflg = True Then
                    _Epoint = New Point((e.X / _Zm), (e.Y / _Zm))
                    Dim A As New PTempShapes2DT(_Epoint)
                    '_Epoint = New Point((_Epoint.X), (_Epoint.Y))
                    If _Spoint = _Epoint Then
                        _Arcflg = False
                        Exit Sub
                    End If
                    _Linflg = True
                    _Arcflg = False
                    MDIMain._Commandtxtbox.Visible = False
                    MDIMain._commandtxtlabel.Visible = False
                ElseIf _Linflg = True And _Arcflg = False Then
                    If e.Button = Windows.Forms.MouseButtons.Right Then
                        _Linflg = False
                        Exit Sub
                    End If
                    Dim seg As Integer
                    seg = Val(InputBox("No., Of Segments ?", "Arc Segments", "2"))
                    If seg >= 2 Then
                        Dim P As New PTempShapes2DT(Apts, _Spoint, _Epoint)
                        _AP = New AddPortal2DT(Apts, seg)
                    End If
                    _Arcflg = False
                    _Linflg = False
                End If
            ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
                _Linflg = False
                _Arcflg = False
                Exit Sub
            End If
        ElseIf MDIMain._selectM.Checked = True Then
            If MDIMain._move.Checked = True Then
                If e.Button = Windows.Forms.MouseButtons.Left Then
                    If _Linflg = False Then
                        _Spoint = New Point(e.X / _Zm, e.Y / _Zm)
                        Dim A As New PTempShapes2DT(_Spoint)
                        '_Spoint = New Point((_Spoint.X - Mpoint.X), (_Spoint.Y - Mpoint.Y))
                        _Linflg = True
                    Else
                        _Epoint = New Point(((e.X / _Zm) - Mpoint.X), ((e.Y / _Zm) - Mpoint.Y))
                        _AP = New AddPortal2DT(_Spoint, _Epoint, 4)
                        _Linflg = False
                    End If
                ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
                    _selLine.Clear()
                    MDIMain._DMCMenable()
                    _Linflg = False
                    Exit Sub
                End If
            ElseIf MDIMain._clone.Checked = True Then
                If e.Button = Windows.Forms.MouseButtons.Left Then
                    If _Linflg = False Then
                        _Spoint = New Point(e.X / _Zm, e.Y / _Zm)
                        Dim A As New PTempShapes2DT(_Spoint)
                        '_Spoint = New Point((_Spoint.X - Mpoint.X), (_Spoint.Y - Mpoint.Y))
                        _Linflg = True
                    Else
                        _Epoint = New Point(((e.X / _Zm) - Mpoint.X), ((e.Y / _Zm) - Mpoint.Y))
                        _AP = New AddPortal2DT(_Spoint, _Epoint, 5)
                        _Linflg = False
                    End If
                ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
                    _selLine.Clear()
                    MDIMain._DMCMenable()
                    _Linflg = False
                    Exit Sub
                End If
            ElseIf MDIMain._mirror.Checked = True Then
                If e.Button = Windows.Forms.MouseButtons.Left Then
                    If _Linflg = False Then
                        _Spoint = New Point(e.X / _Zm, e.Y / _Zm)
                        Dim A As New PTempShapes2DT(_Spoint)
                        '_Spoint = New Point((_Spoint.X - Mpoint.X), (_Spoint.Y - Mpoint.Y))
                        _Linflg = True
                    Else
                        _Epoint = New Point(((e.X / _Zm) - Mpoint.X), ((e.Y / _Zm) - Mpoint.Y))
                        _AP = New AddPortal2DT(_Spoint, _Epoint, 6)
                        _Linflg = False
                    End If
                ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
                    _selLine.Clear()
                    MDIMain._DMCMenable()
                    _Linflg = False
                    Exit Sub
                End If
            Else
                If e.Button = Windows.Forms.MouseButtons.Left Then
                    If My.Computer.Keyboard.CtrlKeyDown = True Then
                        _Spoint = New Point(((e.X / _Zm) - Mpoint.X), ((e.Y / _Zm) - Mpoint.Y))
                        _AP = New AddPortal2DT(_Spoint)
                        MDIMain._DMCMenable()
                    End If
                    'ElseIf e.Button = Windows.Forms.MouseButtons.Right AndAlso (MDIMain._move.Checked = False And MDIMain._clone.Checked = False And MDIMain._mirror.Checked = False) Then
                    '    _selLine.Clear()
                    '    MDIMain._DMCMenable()
                End If
            End If
        End If
        MTPic.Refresh()
        MDIMain._Commandtxtbox.Focus()
    End Sub
#End Region

#Region "Drag Handler"
    Private Sub MainPic_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MainPic.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Middle Then
            If _Linflg = True And MDIMain._line.Checked = True Then
                _TempExtraPoint = _Spoint
                _Panflg = True
            End If
            _Spoint = New Point((e.X / _Zm) - Mpoint.X, (e.Y / _Zm) - Mpoint.Y)
            _PredictNULL = True
            _PaintNULL = True
            _Linflg = True
            MDIMain._pan.Checked = True
        End If
        If MDIMain._selectM.Checked = True AndAlso (MDIMain._move.Checked = False And MDIMain._clone.Checked = False And MDIMain._mirror.Checked = False) Then
            If e.Button = Windows.Forms.MouseButtons.Left Then
                If _Linflg = False Then
                    _Spoint = New Point((e.X / _Zm) - Mpoint.X, (e.Y / _Zm) - Mpoint.Y)
                    _PredictNULL = True
                    _Linflg = True
                End If
            End If
        End If
    End Sub

    Private Sub MainPic_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MainPic.MouseUp
        If e.Button = Windows.Forms.MouseButtons.Middle Then
            _Epoint = New Point((e.X / _Zm) - Mpoint.X, (e.Y / _Zm) - Mpoint.Y)
            _AP = New AddPortal2DT(_Spoint, _Epoint, 2)
            _PredictNULL = False
            _PaintNULL = False
            _Linflg = False
            If MDIMain._line.Checked = True And _Panflg = True Then
                _Spoint = _TempExtraPoint
                _Linflg = True
                _Panflg = False
            End If
            MDIMain._pan.Checked = False
        End If
        If MDIMain._selectM.Checked = True AndAlso (MDIMain._move.Checked = False And MDIMain._clone.Checked = False And MDIMain._mirror.Checked = False) Then
            If e.Button = Windows.Forms.MouseButtons.Left Then
                If _Linflg = True Then
                    If My.Computer.Keyboard.CtrlKeyDown = False Then
                        _selLine.Clear()
                    End If
                    _Epoint = New Point((e.X / _Zm) - Mpoint.X, (e.Y / _Zm) - Mpoint.Y)
                    _AP = New AddPortal2DT(_Spoint, _Epoint, 3)
                    MDIMain._DMCMenable()
                    _PredictNULL = False
                    _Linflg = False
                End If
            End If
        End If
    End Sub
#End Region

#Region "Paint Events"
    Private Sub MainPic_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MainPic.Paint
        e.Graphics.ScaleTransform(_Zm, _Zm)
        e.Graphics.TranslateTransform(_mpoint.X, _mpoint.Y)
        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        Ppaint = New PermPaint2DT(e)
    End Sub

    Private Sub MTPic_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MTPic.Paint
        e.Graphics.DrawImage(MDIMain.Cursorimg, New Point(_Cpoint.X + 10, _Cpoint.Y + 10))
        e.Graphics.ScaleTransform(_Zm, _Zm)
        e.Graphics.TranslateTransform(_mpoint.X, _mpoint.Y)
        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        Tpaint = New PTempShapes2DT(e)
    End Sub
#End Region

#Region "ShortCut Keys"
    Private Sub UndoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UndoToolStripMenuItem.Click
        '--- Ctrl + Z
        If HistU.Count <> 0 Then
            HistR.Add(New History2DT(Mem, Bob, _Zm, _mpoint))
            Mem.Clear()
            Bob.Clear()

            Mem.AddRange(HistU(HistU.Count - 1).HMem)
            Bob.AddRange(HistU(HistU.Count - 1).HBob)
            _Zm = HistU(HistU.Count - 1).HZm
            _mpoint = HistU(HistU.Count - 1).HMpoint
            HistU.Remove(HistU(HistU.Count - 1))
            Dim A As New AddPortal2DT(0)
        End If
        MTPic.Refresh()
        _PD = New SnapPredicate2DT
    End Sub

    Private Sub RedoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RedoToolStripMenuItem.Click
        '--- Ctrl + R
        If HistR.Count <> 0 Then
            HistU.Add(New History2DT(Mem, Bob, _Zm, _mpoint))
            Mem.Clear()
            Bob.Clear()

            _Mem.AddRange(HistR(HistR.Count - 1).HMem)
            _Bob.AddRange(HistR(HistR.Count - 1).HBob)
            _Zm = HistR(HistR.Count - 1).HZm
            _mpoint = HistR(HistR.Count - 1).HMpoint
            HistR.Remove(HistR(HistR.Count - 1))
            Dim A As New AddPortal2DT(0)
        End If
        MTPic.Refresh()
        _PD = New SnapPredicate2DT
    End Sub

    Private Sub DelToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DelToolStripMenuItem.Click
        '--- Del
        If WA2dt.selLine.Count <> 0 Then
            WA2dt.AP = New AddPortal2DT(WA2dt.selLine)
            WA2dt.selLine.Clear()
        End If
        MDIMain._DMCMenable()
    End Sub

    Private Sub MoveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveToolStripMenuItem.Click
        '--- Ctrl + X
        '--Tool Bar
        MDIMain._move.Checked = True
        MDIMain._clone.Checked = False
        MDIMain._mirror.Checked = False
        '--Context Menu
        MDIMain.MoveToolStripMenuItem.Enabled = True
        MDIMain.CloneToolStripMenuItem.Enabled = False
        MDIMain.MirrorToolStripMenuItem.Enabled = False
        MDIMain.Cursorimg = My.Resources.move
        MDIMain._DMCMenable()
    End Sub

    Private Sub CloneToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloneToolStripMenuItem.Click
        '--- Ctrl + C
        '--Tool Bar
        MDIMain._move.Checked = False
        MDIMain._clone.Checked = True
        MDIMain._mirror.Checked = False
        '--Context Menu
        MDIMain.MoveToolStripMenuItem.Enabled = False
        MDIMain.CloneToolStripMenuItem.Enabled = True
        MDIMain.MirrorToolStripMenuItem.Enabled = False
        MDIMain.Cursorimg = My.Resources.clone
        MDIMain._DMCMenable()
    End Sub

    Private Sub MirrorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MirrorToolStripMenuItem.Click
        '--- Ctrl + V
        MDIMain._move.Checked = False
        MDIMain._clone.Checked = False
        MDIMain._mirror.Checked = True
        '--Context Menu
        MDIMain.MoveToolStripMenuItem.Enabled = False
        MDIMain.CloneToolStripMenuItem.Enabled = False
        MDIMain.MirrorToolStripMenuItem.Enabled = True
        MDIMain.Cursorimg = My.Resources.Mirror
        MDIMain._DMCMenable()
    End Sub

    Private Sub LineToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LineToolStripMenuItem.Click
        '--- Ctrl + L
        '--Tool Bar
        MDIMain._line.Checked = True
        MDIMain._arc.Checked = False
        MDIMain._selectM.Checked = False
        '--Context Menu
        MDIMain.LineToolStripMenuItem.Checked = True
        MDIMain.ArcToolStripMenuItem.Checked = False
        MDIMain.SelectMToolStripMenuItem.Checked = False
        MDIMain.Cursorimg = My.Resources.line
        MDIMain._DMCMenable()
    End Sub

    Private Sub LoadToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LoadToolStripMenuItem.Click
        '--- Ctrl + A
        Try
            If WA2dt.selLine.Count = 1 Then
                Dim A As New LoadWindow(WA2dt.Mem(WA2dt.selLine(0)))
                A.ShowDialog()
            End If
        Catch ex As Exception
            MsgBox("AddLoad Window Dialog Error", MsgBoxStyle.Critical, "TrussANS")
        End Try
    End Sub

    Private Sub SupportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SupportToolStripMenuItem.Click
        '--- Ctrl + S
        Try
            If WA2dt.selLine.Count = 1 Then
                Dim A As New SupportBoundaryConditionWindow(WA2dt.Mem(WA2dt.selLine(0)))
                A.ShowDialog()
            End If
        Catch ex As Exception
            MsgBox("Support - Boundary Condition Window Dialog Error", MsgBoxStyle.Critical, "TrussANS")
        End Try
    End Sub

    Private Sub PropertiesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PropertiesToolStripMenuItem.Click
        '--- Ctrl + D
        Try
            If WA2dt.selLine.Count = 1 Then
                Dim A As New Member_Properties(WA2dt.Mem(WA2dt.selLine(0)))
                A.ShowDialog()
            End If
        Catch ex As Exception
            MsgBox("Member Properties Window Dialog Error", MsgBoxStyle.Critical, "TrussANS")
        End Try
    End Sub

    Private Sub AnalyzeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AnalyzeToolStripMenuItem.Click
        '--- F5
    End Sub

#End Region
End Class