Public Class Member_Properties
    Dim TempEL As SelectedElement
    Dim SNsupport As SupportDetails
    Dim ENsupport As SupportDetails

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
            Me.Height = (520 * (MDIMain.Height / MDIMain.Width)) + 280 + 25
        Else
            Me.Height = 520 + 280 + 25 '(MDIMain.Width / 3)
            Me.Width = (520 * (MDIMain.Width / MDIMain.Height))
        End If


        CoordFix(El)
        For Each itm In WA2dt.Mem
            _memberBindingSource.Add(itm)
            If itm.Equals(El) Then
                _memberBindingSource.Position = WA2dt.Mem.IndexOf(itm) + 1
            End If
        Next
    End Sub

#Region "Navigate Member Events"
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

        RadioButton1.Checked = MDIMain.Nappdefaults.defaultPJ


        TextBox1.Text = El.Length
        TextBox2.Text = El.E
        TextBox3.Text = El.A

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

        _SupportPic.Refresh()
    End Sub

    Private Sub _memberNavigator_RefreshItems(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _memberBindingSource.CurrentChanged
        If _memberBindingSource.Count <> 0 Then
            CoordFix(_memberBindingSource.Current)
            WA2dt.selLine(0) = _memberBindingSource.IndexOf(_memberBindingSource.Current)
            _SupportPic.Refresh()
            WA2dt.MainPic.Refresh()
        End If
    End Sub
#End Region

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
        '--- Start Node
        If SNsupport.PJ = True Then
            SupportPicPaintPinJoint(e, TempEL.Spoint, SNsupport.Supportinclination)
        ElseIf SNsupport.PS = True Then
            SupportPicPaintPinSupport(e, TempEL.Spoint, SNsupport.Supportinclination, 0.5)
        ElseIf SNsupport.RS = True Then
            SupportPicPaintRollerSupport(e, TempEL.Spoint, SNsupport.Supportinclination, 0.5)
        End If

        '--- End Node
        If ENsupport.PJ = True Then
            SupportPicPaintPinJoint(e, TempEL.Epoint, ENsupport.Supportinclination)
        ElseIf ENsupport.PS = True Then
            SupportPicPaintPinSupport(e, TempEL.Epoint, ENsupport.Supportinclination, 0.5)
        ElseIf ENsupport.RS = True Then
            SupportPicPaintRollerSupport(e, TempEL.Epoint, ENsupport.Supportinclination, 0.5)
        End If
    End Sub
#End Region

#Region "Support Pic Paint Events"
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

    End Sub

    Private Sub SupportPicPaintRollerSupport(ByVal e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double, ByVal ZMfactor As Double)
        '----Triangle
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + ((Math.Cos(-Inclination) + Math.Sin(-Inclination)) * ZMfactor), M.Y + ((-Math.Sin(-Inclination) + Math.Cos(-Inclination))) * ZMfactor), New Point(M.X + ((20 * Math.Cos(-Inclination) + (12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + ((20 * -Math.Sin(-Inclination) + (12 * Math.Cos(-Inclination)))) * ZMfactor))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + ((Math.Cos(-Inclination) + Math.Sin(-Inclination)) * ZMfactor), M.Y + ((-Math.Sin(-Inclination) + Math.Cos(-Inclination))) * ZMfactor), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination)))) * ZMfactor))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (12 * Math.Cos(-Inclination)))) * ZMfactor), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZMfactor), M.Y + (((20 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination))) * ZMfactor)))

        '----Bottom Circle
        e.Graphics.DrawEllipse(New Pen(OptionD.Nc, 2), New Rectangle(New Point((M.X - (5 * ZMfactor)) + (((25 * Math.Cos(-Inclination)) + (6 * Math.Sin(-Inclination))) * ZMfactor), (M.Y - (5 * ZMfactor)) + ((25 * (-Math.Sin(-Inclination)) + (6 * Math.Cos(-Inclination))) * ZMfactor)), New Size((10 * ZMfactor), (10 * ZMfactor))))
        e.Graphics.DrawEllipse(New Pen(OptionD.Nc, 2), New Rectangle(New Point((M.X - (5 * ZMfactor)) + (((25 * Math.Cos(-Inclination)) + (-6 * Math.Sin(-Inclination))) * ZMfactor), (M.Y - (5 * ZMfactor)) + ((25 * (-Math.Sin(-Inclination)) + (-6 * Math.Cos(-Inclination))) * ZMfactor)), New Size((10 * ZMfactor), (10 * ZMfactor))))

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

#Region "Modifying E A & I Events"
    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked = True Then
            MDIMain.Nappdefaults.defaultPJ = True
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Val(TextBox2.Text) <> 0 And Val(TextBox3.Text) <> 0 Then
            WA2dt.Mem(_memberBindingSource.IndexOf(_memberBindingSource.Current)).E = Val(TextBox2.Text)
            WA2dt.Mem(_memberBindingSource.IndexOf(_memberBindingSource.Current)).A = Val(TextBox3.Text)
            _memberBindingSource.ResetBindings(True)
        End If
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged
        If IsNumeric(TextBox2.Text) Then

        Else
            TextBox2.Text = ""
        End If
    End Sub

    Private Sub TextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox3.TextChanged
        If IsNumeric(TextBox3.Text) Then

        Else
            TextBox3.Text = ""
        End If
    End Sub
#End Region

    Private Sub Member_Properties_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        WA2dt.selLine.Clear()
        MDIMain._DMCMenable()
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
        Catch ex As Exception

        End Try
    End Sub
End Class