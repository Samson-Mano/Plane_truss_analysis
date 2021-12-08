Public Class RTempPaint
    Dim Pt As Point
    Dim Spoint As Point
    Dim Epoint As Point
    Dim Zmfactor As Double

    Public Sub New(ByRef Sp As Point, ByRef Ep As Point)
        Zmfactor = RA2dt.Zm
        Spoint = Sp
        Epoint = Ep
        Pt = New Point(RA2dt.Cpoint.X / Zmfactor, RA2dt.Cpoint.Y / Zmfactor)
        PanH()
    End Sub

    Public Sub New(ByRef e As System.Windows.Forms.PaintEventArgs)
        Zmfactor = RA2dt.Zm
        Pt = New Point(RA2dt.Cpoint.X / Zmfactor, RA2dt.Cpoint.Y / Zmfactor)
        If RA2dt.Linflg = True Then
            If MDIMain._pan.Checked = True Then
                PaintTempPan(e)
            End If
        End If
    End Sub

#Region "PanH"
    Private Sub PanH()
        RA2dt.Mpoint = New Point _
                            (RA2dt.Mpoint.X + (Epoint.X - Spoint.X), _
                             RA2dt.Mpoint.Y + (Epoint.Y - Spoint.Y))
    End Sub
#End Region

#Region "Pan Draw"
    Private Sub PaintTempPan(ByRef e As System.Windows.Forms.PaintEventArgs)
        Dim ext As New Point(-RA2dt.Spoint.X - RA2dt.Mpoint.X + Pt.X, -RA2dt.Spoint.Y - RA2dt.Mpoint.Y + Pt.Y)
        'PaintPanAxis(e, ext)
        PaintPanMember(e, ext)
        PaintPanNode(e, ext)
        PaintPanLoad(e, ext)
        PaintPanSupport(e, ext)
    End Sub

    Private Sub PaintPanAxis(ByRef e As System.Windows.Forms.PaintEventArgs, ByRef Extent As Point)
        e.Graphics.DrawLine(New Pen(Color.FromArgb(100, Color.Red), 1 / Zmfactor), CInt(RA2dt.Mpoint.X + Extent.X), 0, CInt(RA2dt.Mpoint.X + Extent.X), CInt(RA2dt.MainPic.Height / Zmfactor))
        e.Graphics.DrawLine(New Pen(Color.FromArgb(100, Color.Blue), 1 / Zmfactor), 0, CInt(RA2dt.Mpoint.Y + Extent.Y), CInt(RA2dt.MainPic.Width / Zmfactor), CInt(RA2dt.Mpoint.Y + Extent.Y))
    End Sub

    Private Sub PaintPanMember(ByRef e As System.Windows.Forms.PaintEventArgs, ByRef Extent As Point)
        For Each El In WA2dt.Mem
            e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / Zmfactor), El.SN.Coord.X + RA2dt.Mpoint.X + Extent.X, El.SN.Coord.Y + RA2dt.Mpoint.Y + Extent.Y, El.EN.Coord.X + RA2dt.Mpoint.X + Extent.X, El.EN.Coord.Y + RA2dt.Mpoint.Y + Extent.Y)
            'e.Graphics.DrawString(Math.Round(El.Length, OptionD.Prec), New Font("Verdana", (8 / Zmfactor)), New Pen(Color.LightGray, 2 / Zmfactor).Brush, ((El.SN.Coord.X + El.EN.Coord.X) / 2) + RA2dt.Mpoint.X + Extent.X, ((El.SN.Coord.Y + El.EN.Coord.Y) / 2) + RA2dt.Mpoint.Y + Extent.Y)
        Next
    End Sub

    Private Sub PaintPanNode(ByRef e As System.Windows.Forms.PaintEventArgs, ByRef Extent As Point)
        'For Each Nd In WA2dt.Bob
        '    'e.Graphics.FillEllipse(New Pen(Color.lightgray, 4 / Ra2dt.Zm).Brush, Nd.Coord.X + Ra2dt.Mpoint.X - (2 / Ra2dt.Zm) + Extent.X, Nd.Coord.Y + Ra2dt.Mpoint.Y - (2 / Ra2dt.Zm) + Extent.Y, 4 / Ra2dt.Zm, 4 / Ra2dt.Zm)
        '    'e.Graphics.DrawString(WA2dt.Bob.IndexOf(Nd), New Font("Verdana", (8 / RA2dt.Zm)), New Pen(Color.LightGray, 2 / RA2dt.Zm).Brush, Nd.Coord.X + RA2dt.Mpoint.X - (14 / RA2dt.Zm) + Extent.X, Nd.Coord.Y + RA2dt.Mpoint.Y - (14 / RA2dt.Zm) + Extent.Y)
        'Next
    End Sub

    Private Sub PaintPanSupport(ByRef e As System.Windows.Forms.PaintEventArgs, ByRef Extent As Point)
        '--- Start Node
        For Each EL In WA2dt.Mem
            If EL.SN.Support.PJ = True Then
                SupportPicPaintPinJoint(e, New Point(EL.SN.Coord.X + RA2dt.Mpoint.X + Extent.X, EL.SN.Coord.Y + RA2dt.Mpoint.Y + Extent.Y))
            ElseIf EL.SN.Support.PS = True Then
                SupportPicPaintPinSupport(e, New Point(EL.SN.Coord.X + RA2dt.Mpoint.X + Extent.X, EL.SN.Coord.Y + RA2dt.Mpoint.Y + Extent.Y), EL.SN.Support.supportinclination, 0.5 / Zmfactor)
            ElseIf EL.SN.Support.RS = True Then
                SupportPicPaintRollerSupport(e, New Point(EL.SN.Coord.X + RA2dt.Mpoint.X + Extent.X, EL.SN.Coord.Y + RA2dt.Mpoint.Y + Extent.Y), EL.SN.Support.supportinclination, 0.5 / Zmfactor)
            End If

            '--- End Node
            If EL.EN.Support.PJ = True Then
                SupportPicPaintPinJoint(e, New Point(EL.EN.Coord.X + RA2dt.Mpoint.X + Extent.X, EL.EN.Coord.Y + RA2dt.Mpoint.Y + Extent.Y))
            ElseIf EL.EN.Support.PS = True Then
                SupportPicPaintPinSupport(e, New Point(EL.EN.Coord.X + RA2dt.Mpoint.X + Extent.X, EL.EN.Coord.Y + RA2dt.Mpoint.Y + Extent.Y), EL.EN.Support.supportinclination, 0.5 / Zmfactor)
            ElseIf EL.EN.Support.RS = True Then
                SupportPicPaintRollerSupport(e, New Point(EL.EN.Coord.X + RA2dt.Mpoint.X + Extent.X, EL.EN.Coord.Y + RA2dt.Mpoint.Y + Extent.Y), EL.EN.Support.supportinclination, 0.5 / Zmfactor)
            End If
        Next
    End Sub

    Private Sub PaintPanLoad(ByRef e As System.Windows.Forms.PaintEventArgs, ByRef Extent As Point)
        For Each Nd In WA2dt.Bob
            If Nd.Load.Loadintensity <> 0 Then
                Dim PPt1 As New Point(Nd.Coord.X + RA2dt.Mpoint.X + Extent.X, Nd.Coord.Y + RA2dt.Mpoint.Y + Extent.Y)
                Dim PPt2 As New Point
                PPt2 = New Point(PPt1.X + (Math.Cos(Nd.Load.Loadinclination) * (WA2dt.Loadhtfactor * (-Nd.Load.Loadintensity / WA2dt.Maxload))), _
                                 PPt1.Y + (Math.Sin(Nd.Load.Loadinclination) * (WA2dt.Loadhtfactor * (-Nd.Load.Loadintensity / WA2dt.Maxload))))
                Dim loadpen As New System.Drawing.Pen(Color.LightGray, 2 / RA2dt.Zm)
                loadpen.CustomStartCap = New System.Drawing.Drawing2D.AdjustableArrowCap(3, 5)
                e.Graphics.DrawLine(loadpen, PPt1.X, PPt1.Y, PPt2.X, PPt2.Y)
                If MDIMain.LoadviewoptionToolStripMenuItem1.Checked = True Then
                    e.Graphics.DrawString(Nd.Load.Loadintensity, New Font("Verdana", (8 / RA2dt.Zm)), New Pen(Color.LightGray, 2 / RA2dt.Zm).Brush, PPt2.X + (2 / RA2dt.Zm), PPt2.Y + (2 / RA2dt.Zm))
                End If
            End If
        Next
    End Sub
#End Region

#Region "Support Pic Paint Events"
    Private Sub SupportPicPaintPinJoint(ByRef e As System.Windows.Forms.PaintEventArgs, ByVal M As Point)
        e.Graphics.DrawEllipse(New Pen(Color.LightGray, 2 / RA2dt.Zm), CInt(M.X - (2 / RA2dt.Zm)), CInt(M.Y - (2 / RA2dt.Zm)), CInt((4 / RA2dt.Zm)), CInt((4 / RA2dt.Zm)))
        'e.Graphics.FillEllipse(New Pen(Color.lightgray, 4 / Ra2dt.Zm).Brush, M.X - (3 / Ra2dt.Zm), M.Y - (3 / Ra2dt.Zm), (6 / Ra2dt.Zm), (6 / Ra2dt.Zm))
    End Sub

    Private Sub SupportPicPaintPinSupport(ByRef e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double, ByVal ZM As Double)
        '----Triangle
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + ((Math.Cos(-Inclination) + Math.Sin(-Inclination)) * ZM), M.Y + ((-Math.Sin(-Inclination) + Math.Cos(-Inclination))) * ZM), New Point(M.X + ((20 * Math.Cos(-Inclination) + (12 * Math.Sin(-Inclination))) * ZM), M.Y + ((20 * -Math.Sin(-Inclination) + (12 * Math.Cos(-Inclination)))) * ZM))
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + ((Math.Cos(-Inclination) + Math.Sin(-Inclination)) * ZM), M.Y + ((-Math.Sin(-Inclination) + Math.Cos(-Inclination))) * ZM), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination)))) * ZM))
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (12 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (12 * Math.Cos(-Inclination)))) * ZM), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination))) * ZM)))

        '----Bottom Rectangle
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZM), M.Y + ((20 * -Math.Sin(-Inclination) + (-20 * Math.Cos(-Inclination)))) * ZM))
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZM), M.Y + (((30 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination)))) * ZM))
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZM), M.Y + (((30 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZM), M.Y + (((30 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination)))) * ZM))
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZM), M.Y + (((30 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination)))) * ZM))

        '----Line showing the resultant force direction
        Dim loadpen As New System.Drawing.Pen(Color.Green, 2 / RA2dt.Zm)
        loadpen.CustomStartCap = New System.Drawing.Drawing2D.AdjustableArrowCap(3, 5)
        e.Graphics.DrawLine(loadpen, New Point(M.X + (((50 * Math.Cos(-Inclination)) + (-55 * Math.Sin(-Inclination))) * ZM), M.Y + (((50 * -Math.Sin(-Inclination)) + (-55 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((50 * Math.Cos(-Inclination)) + (0 * Math.Sin(-Inclination))) * ZM), M.Y + ((50 * -Math.Sin(-Inclination) + (0 * Math.Cos(-Inclination)))) * ZM))
        e.Graphics.DrawLine(loadpen, New Point(M.X + (((100 * Math.Cos(-Inclination)) + (0 * Math.Sin(-Inclination))) * ZM), M.Y + (((100 * -Math.Sin(-Inclination)) + (0 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((50 * Math.Cos(-Inclination)) + (0 * Math.Sin(-Inclination))) * ZM), M.Y + (((50 * -Math.Sin(-Inclination)) + (0 * Math.Cos(-Inclination)))) * ZM))
    End Sub

    Private Sub SupportPicPaintRollerSupport(ByRef e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double, ByVal ZM As Double)
        '----Triangle
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + ((Math.Cos(-Inclination) + Math.Sin(-Inclination)) * ZM), M.Y + ((-Math.Sin(-Inclination) + Math.Cos(-Inclination))) * ZM), New Point(M.X + ((20 * Math.Cos(-Inclination) + (12 * Math.Sin(-Inclination))) * ZM), M.Y + ((20 * -Math.Sin(-Inclination) + (12 * Math.Cos(-Inclination)))) * ZM))
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + ((Math.Cos(-Inclination) + Math.Sin(-Inclination)) * ZM), M.Y + ((-Math.Sin(-Inclination) + Math.Cos(-Inclination))) * ZM), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination)))) * ZM))
        e.Graphics.DrawLine(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (12 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (12 * Math.Cos(-Inclination)))) * ZM), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination))) * ZM)))

        '----Bottom Circle
        e.Graphics.DrawEllipse(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Rectangle(New Point((M.X - (5 * ZM)) + (((25 * Math.Cos(-Inclination)) + (6 * Math.Sin(-Inclination))) * ZM), (M.Y - (5 * ZM)) + ((25 * (-Math.Sin(-Inclination)) + (6 * Math.Cos(-Inclination))) * ZM)), New Size((10 * ZM), (10 * ZM))))
        e.Graphics.DrawEllipse(New Pen(Color.LightGray, 2 / RA2dt.Zm), New Rectangle(New Point((M.X - (5 * ZM)) + (((25 * Math.Cos(-Inclination)) + (-6 * Math.Sin(-Inclination))) * ZM), (M.Y - (5 * ZM)) + ((25 * (-Math.Sin(-Inclination)) + (-6 * Math.Cos(-Inclination))) * ZM)), New Size((10 * ZM), (10 * ZM))))

        '----Line showing the resultant force direction
        Dim loadpen As New System.Drawing.Pen(Color.Green, 2 / RA2dt.Zm)
        loadpen.CustomStartCap = New System.Drawing.Drawing2D.AdjustableArrowCap(3, 5)
        e.Graphics.DrawLine(loadpen, New Point(M.X + (((100 * Math.Cos(-Inclination)) + (0 * Math.Sin(-Inclination))) * ZM), M.Y + (((100 * -Math.Sin(-Inclination)) + (0 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((50 * Math.Cos(-Inclination)) + (0 * Math.Sin(-Inclination))) * ZM), M.Y + (((50 * -Math.Sin(-Inclination)) + (0 * Math.Cos(-Inclination)))) * ZM))
    End Sub
#End Region
End Class
