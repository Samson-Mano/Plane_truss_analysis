Public Class ResultArea2DT
#Region "Main Variables"
    Dim _Rmem As New List(Of Rslt2DT)
    Dim _Rbob As New List(Of Rslt2DT.Node)
    Dim _Zm As Double = 1
    Dim _mpoint As Point
    Dim _Spoint As Point
    Dim _Epoint As Point
    Dim _Cpoint As Point
    Dim Tpaint As RTempPaint
    Dim Ppaint As BackgroundMemberPaint
    Dim Rpaint As ResultPaint
    Dim _Linflg As Boolean = False
    Dim _AP As RTempPaint
    Dim _PaintNULL As Boolean = False
    Dim _Analyzer As Truss_FE_Solver
#End Region

#Region "Get Set Property"
    Public Property Rmem() As List(Of Rslt2DT)
        Get
            Return _Rmem
        End Get
        Set(ByVal value As List(Of Rslt2DT))
            _Rmem = value
        End Set
    End Property

    Public Property Rbob() As List(Of Rslt2DT.Node)
        Get
            Return _Rbob
        End Get
        Set(ByVal value As List(Of Rslt2DT.Node))
            _Rbob = value
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
    End Property

    Public Property PaintNULL() As Boolean
        Get
            Return _PaintNULL
        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property

    Public Property Analyzer() As Truss_FE_Solver
        Get
            Return _Analyzer
        End Get
        Set(ByVal value As Truss_FE_Solver)
            _Analyzer = value
        End Set
    End Property

#End Region

    Private Sub ResultArea2DT_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _mpoint = New Point(WA2dt.Mpoint.X, WA2dt.Mpoint.Y)
        _Zm = WA2dt.Zm
    End Sub

#Region "Zoom Handler"

    Private Sub MainPic_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MainPic.MouseDoubleClick
        If e.Button = Windows.Forms.MouseButtons.Middle Then
            '_AP = New AddPortal2DT(New Point((e.X / Zm) - WA2dt.Mpoint.X, (e.Y / Zm) - WA2dt.Mpoint.Y), New Point(0, 0), 7)
            'MTPic.Refresh()
            MainPic.Refresh()
        End If
    End Sub

    Private Sub MainPic_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles MainPic.MouseEnter
        MainPic.Focus()
        '_PD = New SnapPredicate2DT
        Windows.Forms.Cursor.Show()
    End Sub

    Private Sub MainPic_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles MainPic.MouseLeave
        '_PD = New SnapPredicate2DT
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
            End If
        End If
        _mpoint.X = (e.X / Zm) - xw
        _mpoint.Y = (e.Y / Zm) - yw
        MTPic.Refresh()
        '_PD = New SnapPredicate2DT
    End Sub
#End Region

#Region "Move Handler"
    Private Sub MainPic_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MainPic.MouseMove
        _Cpoint = e.Location
        MainPic.Refresh()
    End Sub
#End Region

#Region "Click Handler"
    Private Sub MainPic_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MainPic.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Right Then
            MDIMain.ContextMenuStrip2.Show(MainPic.PointToScreen(e.Location))
        End If
    End Sub
#End Region

#Region "Drag Handler"
    Private Sub MainPic_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MainPic.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Middle Then
            _Spoint = New Point((e.X / _Zm) - Mpoint.X, (e.Y / _Zm) - Mpoint.Y)
            _PaintNULL = True
            _Linflg = True
            MDIMain._pan.Checked = True
        End If
    End Sub

    Private Sub MainPic_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MainPic.MouseUp
        If e.Button = Windows.Forms.MouseButtons.Middle Then
            _Epoint = New Point((e.X / _Zm) - Mpoint.X, (e.Y / _Zm) - Mpoint.Y)
            _AP = New RTempPaint(_Spoint, _Epoint)
            _PaintNULL = False
            _Linflg = False
            MDIMain._pan.Checked = False
        End If
    End Sub
#End Region

#Region "Paint Events"
    Private Sub MainPic_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MainPic.Paint
        e.Graphics.ScaleTransform(_Zm, _Zm)
        Ppaint = New BackgroundMemberPaint(e)
        Rpaint = New ResultPaint(e, PaintNULL, RA2dt.Mpoint)
    End Sub

    Private Sub MTPic_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MTPic.Paint
        e.Graphics.DrawImage(MDIMain.Cursorimg, New Point(Cpoint.X + 10, Cpoint.Y + 10))
        e.Graphics.ScaleTransform(_Zm, _Zm)
        Tpaint = New RTempPaint(e)
        Rpaint = New ResultPaint(e, If(PaintNULL = True, False, True), New Point(-RA2dt.Spoint.X + (Cpoint.X / Zm), -RA2dt.Spoint.Y + (Cpoint.Y / Zm)))
    End Sub
#End Region

End Class