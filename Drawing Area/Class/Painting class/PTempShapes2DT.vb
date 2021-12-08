Public Class PTempShapes2DT
    Dim Pt As Point
    Dim SnaPT1 As New Point
    Dim SnaPT2 As New Point
    Dim Nds As Boolean = False
    Dim Lpts As Boolean = False
    Dim arcOrig As Point
    Dim arcPTS() As Point
    Dim NoAPts As Integer

    Public Sub New(ByRef arcP() As Point, ByRef SPt As Point, ByRef EPt As Point)
        Pt = New Point(WA2dt.Cpoint.X / WA2dt.Zm, WA2dt.Cpoint.Y / WA2dt.Zm)
        PredictSnap(Pt, Pt)
        CreateArcPts(arcP, WA2dt.Spoint.X, WA2dt.Spoint.Y, Pt.X, Pt.Y, WA2dt.Epoint.X, WA2dt.Epoint.Y)
    End Sub

    Public Sub New(ByRef Pt As Point) '--Predicts Spoint
        PredictSnap(Pt, Pt)
    End Sub

    Public Sub New(ByRef e As System.Windows.Forms.PaintEventArgs)
        Pt = New Point(WA2dt.Cpoint.X / WA2dt.Zm, WA2dt.Cpoint.Y / WA2dt.Zm)
        PredictSnap(Pt, Pt)
        PaintCursor(e)
        If WA2dt.Linflg = True Then
            If MDIMain._pan.Checked = True Then
                PaintTempPan(e)
            ElseIf MDIMain._line.Checked = True Then
                PaintTempLine(e)
            ElseIf MDIMain._arc.Checked = True Then
                PaintTempArc(e)
            ElseIf MDIMain._selectM.Checked = True Then
                If MDIMain._move.Checked = True Then
                    PaintMCMTempLine(e)
                    PaintTempMC(e)
                ElseIf MDIMain._clone.Checked = True Then
                    PaintMCMTempLine(e)
                    PaintTempMC(e)
                ElseIf MDIMain._mirror.Checked = True Then
                    PaintMirr(e)
                Else
                    PaintSelectRectangle(e)
                End If
            End If
        ElseIf WA2dt.Arcflg = True Then
            If MDIMain._arc.Checked = True Then
                PaintTempArcBaseLine(e)
            End If
        End If
    End Sub

#Region "Predicate for Snapping"
    Private Sub PredictSnap(ByVal Pt As Point, ByRef ChPt As Point)
        If WA2dt.PredictNULL = False Then
            Pt.X = If( _
                    (Pt.X < WA2dt.Spoint.X + (2 / WA2dt.Zm) _
                     And Pt.X > WA2dt.Spoint.X - (2 / WA2dt.Zm)), _
                        WA2dt.Spoint.X, _
                        Pt.X)
            Pt.Y = If( _
                    (Pt.Y < WA2dt.Spoint.Y + (2 / WA2dt.Zm) _
                     And Pt.Y > WA2dt.Spoint.Y - (2 / WA2dt.Zm)), _
                        WA2dt.Spoint.Y, _
                        Pt.Y)
            Pt.X = Pt.X - WA2dt.Mpoint.X
            Pt.Y = Pt.Y - WA2dt.Mpoint.Y
            Nds = False
            WA2dt.PD.Her = New Line2DT.Node(Pt)
            If OptionD.MSnap = True Then
                If WA2dt.Mem.Exists(WA2dt.PD.MidptSnapPD) = True Then
                    LineMidpt(Pt, WA2dt.Mem.Find(WA2dt.PD.MidptSnapPD))
                    ChPt.X = Pt.X
                    ChPt.Y = Pt.Y
                    Nds = True
                    Exit Sub
                End If
                If WA2dt.Mem.Exists(WA2dt.PD.LineSnapPD) = True Then
                    SnaptoLine(Pt, WA2dt.Mem.Find(WA2dt.PD.LineSnapPD))
                    ChPt.X = Pt.X
                    ChPt.Y = Pt.Y
                    Lpts = True
                    Exit Sub
                End If
            End If
            If OptionD.NSnap = True Then
                If WA2dt.Bob.Exists(WA2dt.PD.NodeSnapPD) = True Then
                    Pt = WA2dt.Bob.Find(WA2dt.PD.NodeSnapPD).Coord
                    ChPt.X = Pt.X
                    ChPt.Y = Pt.Y
                    Nds = True
                    Exit Sub
                End If
            End If
            If OptionD.HSnap = True Then
                If WA2dt.Bob.Exists(WA2dt.PD.HorizSnapPD) = True Then
                    HSnapPt(Pt, SnaPT1, SnaPT2, WA2dt.Bob.Find(WA2dt.PD.HorizSnapPD))
                End If
                If WA2dt.Bob.Exists(WA2dt.PD.VertSnapPD) = True Then
                    VSnapPt(Pt, SnaPT1, SnaPT2, WA2dt.Bob.Find(WA2dt.PD.VertSnapPD))
                End If
            End If


            ChPt.X = Pt.X '+ WA2dt.Mpoint.X
            ChPt.Y = Pt.Y '+ WA2dt.Mpoint.Y
        End If
    End Sub

    Private Sub HSnapPt(ByRef pt As Point, ByRef Spt1 As Point, ByRef Spt2 As Point, ByVal SnapND As Line2DT.Node)
        pt.Y = SnapND.Coord.Y
        SnaPT1.X = If( _
                    (SnapND.Coord.X - pt.X) < 0, _
                        pt.X - (20 / WA2dt.Zm), _
                        pt.X + (20 / WA2dt.Zm))
        SnaPT2.X = If((SnapND.Coord.X - pt.X) < 0, -WA2dt.MainPic.Width / WA2dt.Zm, WA2dt.MainPic.Width / WA2dt.Zm)
    End Sub

    Private Sub VSnapPt(ByRef pt As Point, ByRef Spt1 As Point, ByRef Spt2 As Point, ByVal SnapND As Line2DT.Node)
        pt.X = SnapND.Coord.X
        SnaPT1.Y = If( _
                    (SnapND.Coord.Y - pt.Y) < 0, _
                        pt.Y - (20 / WA2dt.Zm), _
                        pt.Y + (20 / WA2dt.Zm))
        SnaPT2.Y = If((SnapND.Coord.Y - pt.Y) < 0, -WA2dt.MainPic.Height / WA2dt.Zm, WA2dt.MainPic.Height / WA2dt.Zm)
    End Sub

    Private Sub LineMidpt(ByRef pt As Point, ByVal SnapEL As Line2DT)
        pt = New Point( _
                    (SnapEL.SN.Coord.X + SnapEL.EN.Coord.X) / 2, _
                    (SnapEL.SN.Coord.Y + SnapEL.EN.Coord.Y) / 2)
    End Sub

    Private Sub SnaptoLine(ByRef pt As Point, ByVal SnapEL As Line2DT)
        'Dim ReTx, ReTy As Double
        'ReTx = ((pt.X * (SnapEL.Lcos)) + (pt.Y * (-1 * SnapEL.Msin)))
        'ReTy = ((SnapEL.SN.Coord.Y * (SnapEL.Lcos)) + (SnapEL.SN.Coord.X * (SnapEL.Msin)))
        'pt.X = ((ReTx * (SnapEL.Lcos)) + (ReTy * (SnapEL.Msin)))
        'pt.Y = ((ReTy * (SnapEL.Lcos)) + (ReTx * (-1 * SnapEL.Msin)))
    End Sub
#End Region

#Region "Cursor Draw"
    Private Sub PaintCursor(ByRef e As System.Windows.Forms.PaintEventArgs)
        If WA2dt.PredictNULL = False Then
            Dim SNPen As New Pen(OptionD.Sc, 1 / WA2dt.Zm)
            SNPen.DashStyle = Drawing2D.DashStyle.Dash
            e.Graphics.DrawLine( _
                            New Pen(Color.DarkGray, 2 / WA2dt.Zm), _
                            CInt(Pt.X), _
                            CInt(Pt.Y - (20 / WA2dt.Zm)), _
                            CInt(Pt.X), _
                            CInt(Pt.Y + (20 / WA2dt.Zm)))
            e.Graphics.DrawLine( _
                            New Pen(Color.DarkGray, 2 / WA2dt.Zm), _
                            CInt(Pt.X - (20 / WA2dt.Zm)), _
                            CInt(Pt.Y), _
                            CInt(Pt.X + (20 / WA2dt.Zm)), _
                            CInt(Pt.Y))
            e.Graphics.DrawLine(SNPen, SnaPT1.X, Pt.Y, SnaPT2.X, Pt.Y)
            e.Graphics.DrawLine(SNPen, Pt.X, SnaPT1.Y, Pt.X, SnaPT2.Y)
            If Lpts = True Then
                e.Graphics.FillEllipse( _
                            SNPen.Brush, _
                            CInt(Pt.X - (3 / WA2dt.Zm)), _
                            CInt(Pt.Y - (3 / WA2dt.Zm)), _
                            CInt(6 / WA2dt.Zm), _
                            CInt(6 / WA2dt.Zm))
            ElseIf Nds = True Then
                e.Graphics.DrawLine( _
                            New Pen(SNPen.Color, 2 / WA2dt.Zm), _
                            CInt(Pt.X - (6 / WA2dt.Zm)), _
                            CInt(Pt.Y + (6 / WA2dt.Zm)), _
                            CInt(Pt.X + (6 / WA2dt.Zm)), _
                            CInt(Pt.Y - (6 / WA2dt.Zm)))
                e.Graphics.DrawLine( _
                            New Pen(SNPen.Color, 2 / WA2dt.Zm), _
                            CInt(Pt.X - (6 / WA2dt.Zm)), _
                            CInt(Pt.Y - (6 / WA2dt.Zm)), _
                            CInt(Pt.X + (6 / WA2dt.Zm)), _
                            CInt(Pt.Y + (6 / WA2dt.Zm)))
            End If
        End If
    End Sub
#End Region

#Region "Line Draw"
    Private Sub PaintTempLine(ByRef e As System.Windows.Forms.PaintEventArgs)
        e.Graphics.DrawLine(New Pen(OptionD.Mc, 2 / WA2dt.Zm), WA2dt.Spoint.X, WA2dt.Spoint.Y, Pt.X, Pt.Y)
        e.Graphics.DrawString(Math.Round((Math.Sqrt(Math.Pow(((WA2dt.Spoint.X) - Pt.X), 2) + Math.Pow(((WA2dt.Spoint.Y) - Pt.Y), 2)) / MDIMain.Nappdefaults.defaultScaleFactor), OptionD.Prec), New Font("Verdana", (8 / WA2dt.Zm)), New Pen(OptionD.Lc, 2 / WA2dt.Zm).Brush, ((WA2dt.Spoint.X + Pt.X) / 2), ((WA2dt.Spoint.Y + Pt.Y) / 2))
        Dim length As Double = (Math.Sqrt(Math.Pow(((WA2dt.Spoint.X) - Pt.X), 2) + Math.Pow(((WA2dt.Spoint.Y) - Pt.Y), 2))) / MDIMain.Nappdefaults.defaultScaleFactor
        Dim Angle As Double = (Math.Acos(((-(WA2dt.Spoint.X) + Pt.X)) / (length * MDIMain.Nappdefaults.defaultScaleFactor))) * (180 / Math.PI)
        Angle = If((-(WA2dt.Spoint.Y) + Pt.Y) < 1, Angle, -1 * Angle)
        MDIMain._Commandtxtbox.Text = Math.Round(length, 2) & "   " & Math.Round(Angle, 0)
        MDIMain._Commandtxtbox.SelectionLength = If(MDIMain.txtinputflg = False, MDIMain._Commandtxtbox.TextLength, Nothing)
    End Sub
#End Region

#Region "Arc Draw"
    Private Sub PaintTempArcBaseLine(ByRef e As System.Windows.Forms.PaintEventArgs)
        e.Graphics.DrawLine(New Pen(OptionD.Mc, 2 / WA2dt.Zm), WA2dt.Spoint.X, WA2dt.Spoint.Y, Pt.X, Pt.Y)
        e.Graphics.DrawString(Math.Round((Math.Sqrt(Math.Pow(((WA2dt.Spoint.X) - Pt.X), 2) + Math.Pow(((WA2dt.Spoint.Y) - Pt.Y), 2)) / MDIMain.Nappdefaults.defaultScaleFactor), OptionD.Prec), New Font("Verdana", (8 / WA2dt.Zm)), New Pen(OptionD.Lc, 2 / WA2dt.Zm).Brush, ((WA2dt.Spoint.X + Pt.X) / 2), ((WA2dt.Spoint.Y + Pt.Y) / 2))
        Dim length As Double = (Math.Sqrt(Math.Pow(((WA2dt.Spoint.X) - Pt.X), 2) + Math.Pow(((WA2dt.Spoint.Y) - Pt.Y), 2))) / MDIMain.Nappdefaults.defaultScaleFactor
        Dim Angle As Double = (Math.Acos(((-(WA2dt.Spoint.X) + Pt.X)) / (length * MDIMain.Nappdefaults.defaultScaleFactor))) * (180 / Math.PI)
        Angle = If((-(WA2dt.Spoint.Y) + Pt.Y) < 1, Angle, -1 * Angle)
        MDIMain._Commandtxtbox.Text = Math.Round(length, 2) & "   " & Math.Round(Angle, 0)
        MDIMain._Commandtxtbox.SelectionLength = If(MDIMain.txtinputflg = False, MDIMain._Commandtxtbox.TextLength, Nothing)
    End Sub

    Private Sub PaintTempArc(ByRef e As System.Windows.Forms.PaintEventArgs)
        CreateArcPts(arcPTS, WA2dt.Spoint.X, WA2dt.Spoint.Y, Pt.X, Pt.Y, WA2dt.Epoint.X, WA2dt.Epoint.Y)
        Dim ArcMidPt As New Point((WA2dt.Spoint.X + WA2dt.Epoint.X) / 2, (WA2dt.Spoint.Y + WA2dt.Epoint.Y) / 2)
        e.Graphics.DrawLine(New Pen(OptionD.Mc, (2 / WA2dt.Zm)), ArcMidPt.X, ArcMidPt.Y, Pt.X, Pt.Y)
        e.Graphics.DrawString(Math.Round((Math.Sqrt(Math.Pow(((ArcMidPt.X) - Pt.X), 2) + Math.Pow(((ArcMidPt.Y) - Pt.Y), 2)) / MDIMain.Nappdefaults.defaultScaleFactor), OptionD.Prec), New Font("Verdana", (8 / WA2dt.Zm)), New Pen(OptionD.Lc, 2 / WA2dt.Zm).Brush, ((ArcMidPt.X + Pt.X) / 2), ((ArcMidPt.Y + Pt.Y) / 2))
        If NoAPts > 1 Then
            e.Graphics.DrawLines(New Pen(OptionD.Mc, (2 / WA2dt.Zm)), arcPTS)
        End If
    End Sub

    Private Sub CreateArcPts(ByRef APts() As Point, ByRef X1 As Double, ByRef Y1 As Double, ByRef CptX As Double, ByRef CptY As Double, ByRef X2 As Double, ByRef Y2 As Double)
        ReDim APts(0)
        Dim Tm, Tn As Double
        NoAPts = 0
        Tm = ((2 * (Y1 - CptY)) - (((2 * (Y1 - Y2)) * (2 * (X1 - CptX))) / (2 * (X1 - X2))))
        Tn = ((X1 ^ 2 + Y1 ^ 2 - CptX ^ 2 - CptY ^ 2) - ((((X1 ^ 2 + Y1 ^ 2 - X2 ^ 2 - Y2 ^ 2)) * (2 * (X1 - CptX))) / (2 * (X1 - X2))))
        Dim x, y As Double
        y = Tn / Tm
        x = ((X1 ^ 2 + Y1 ^ 2 - X2 ^ 2 - Y2 ^ 2) - ((2 * (Y1 - Y2)) * y)) / (2 * (X1 - X2))
        Try
            arcOrig = New Point(x, y)
        Catch ex As Exception
            arcOrig = New Point(X1, Y1)
        End Try

        Dim theta1, theta2 As Double
        Dim lent As Double
        Dim l, m As Double
        Dim xx, yy, xz, yz As Double
        xx = CptX - X1
        yy = CptY - Y1
        lent = System.Math.Sqrt(((X2 - X1) ^ 2) + ((Y2 - Y1) ^ 2))
        l = (X2 - X1) / lent
        m = (Y2 - Y1) / lent
        xz = (l * xx) + (m * yy)
        yz = (m * xx) - (l * yy)
        'Label3.Text = xz
        'Label4.Text = yz

        If yz < 0 Then
            Dim temp As Double
            temp = X1
            X1 = X2
            X2 = temp
            temp = Y1
            Y1 = Y2
            Y2 = temp
        End If
        NoAPts = 0

        lent = System.Math.Sqrt(((X2 - x) ^ 2) + ((Y2 - y) ^ 2))
        theta2 = System.Math.Acos((X2 - x) / lent)
        theta2 = If((Y2 > y), (3.14 * 2) - theta2, theta2)

        lent = System.Math.Sqrt(((X1 - x) ^ 2) + ((Y1 - y) ^ 2))
        theta1 = System.Math.Acos((X1 - x) / lent)
        theta1 = If((Y1 > y), (3.14 * 2) - theta1, theta1)

        theta1 = If((theta1 < theta2), theta1 + (3.14 * 2), theta1)

        Dim scale As Double
        scale = (((theta1 - theta2) + 1) / (360 * 2)) '* 10
        While theta2 <= theta1
            Dim ex, ey As Double
            ReDim Preserve APts(NoAPts)
            ex = x + lent * System.Math.Cos(theta2)
            ey = y - lent * System.Math.Sin(theta2)

            Dim p As New Point(ex, ey)
            APts(NoAPts) = p
            theta2 = theta2 + scale
            NoAPts = NoAPts + 1
        End While
    End Sub
#End Region

#Region "Pan Draw"
    Private Sub PaintTempPan(ByRef e As System.Windows.Forms.PaintEventArgs)
        Dim ext As New Point(-WA2dt.Spoint.X - WA2dt.Mpoint.X + Pt.X, -WA2dt.Spoint.Y - WA2dt.Mpoint.Y + Pt.Y)
        'PaintPanAxis(e, ext)
        PaintPanMember(e, ext)
        PaintPanNode(e, ext)
        PaintPanLoad(e, ext)
        PaintPanSupport(e, ext)
    End Sub

    Private Sub PaintPanAxis(ByRef e As System.Windows.Forms.PaintEventArgs, ByRef Extent As Point)
        e.Graphics.DrawLine(New Pen(Color.FromArgb(100, Color.Red), 1 / WA2dt.Zm), CInt(Extent.X), CInt(-(WA2dt.MainPic.Height / WA2dt.Zm)), CInt(Extent.X), CInt((WA2dt.MainPic.Height / WA2dt.Zm)))
        e.Graphics.DrawLine(New Pen(Color.FromArgb(100, Color.Blue), 1 / WA2dt.Zm), CInt(-(WA2dt.MainPic.Width / WA2dt.Zm)), CInt(Extent.Y), CInt((WA2dt.MainPic.Width / WA2dt.Zm)), CInt(Extent.Y))
    End Sub

    Private Sub PaintPanMember(ByRef e As System.Windows.Forms.PaintEventArgs, ByRef Extent As Point)
        For Each El In WA2dt.Mem
            e.Graphics.DrawLine(New Pen(OptionD.Mc, 2 / WA2dt.Zm), El.SN.Coord.X + Extent.X, El.SN.Coord.Y + Extent.Y, El.EN.Coord.X + Extent.X, El.EN.Coord.Y + Extent.Y)
            If MDIMain.LengthviewoptionToolStripMenuItem1.Checked = True Then
                e.Graphics.DrawString(Math.Round(El.Length, OptionD.Prec), New Font("Verdana", (8 / WA2dt.Zm)), New Pen(OptionD.Lc, 2 / WA2dt.Zm).Brush, ((El.SN.Coord.X + El.EN.Coord.X) / 2) + Extent.X, ((El.SN.Coord.Y + El.EN.Coord.Y) / 2) + Extent.Y)
            End If
        Next
    End Sub

    Private Sub PaintPanNode(ByRef e As System.Windows.Forms.PaintEventArgs, ByRef Extent As Point)
        For Each Nd In WA2dt.Bob
            'e.Graphics.FillEllipse(New Pen(OptionD.Nc, 4 / WA2dt.Zm).Brush, Nd.Coord.X  - (2 / WA2dt.Zm) + Extent.X, Nd.Coord.Y  - (2 / WA2dt.Zm) + Extent.Y, 4 / WA2dt.Zm, 4 / WA2dt.Zm)
            If MDIMain.NodeNoviewoptionToolStripMenuItem1.Checked = True Then
                e.Graphics.DrawString(WA2dt.Bob.IndexOf(Nd), New Font("Verdana", (8 / WA2dt.Zm)), New Pen(OptionD.NNc, 2 / WA2dt.Zm).Brush, Nd.Coord.X - (14 / WA2dt.Zm) + Extent.X, Nd.Coord.Y - (14 / WA2dt.Zm) + Extent.Y)
            End If
        Next
    End Sub

    Private Sub PaintPanSupport(ByRef e As System.Windows.Forms.PaintEventArgs, ByRef Extent As Point)
        '--- Start Node
        For Each EL In WA2dt.Mem
            If EL.SN.Support.PJ = True Then
                SupportPicPaintPinJoint(e, New Point(EL.SN.Coord.X + Extent.X, EL.SN.Coord.Y + Extent.Y))
            ElseIf EL.SN.Support.PS = True Then
                SupportPicPaintPinSupport(e, New Point(EL.SN.Coord.X + Extent.X, EL.SN.Coord.Y + Extent.Y), EL.SN.Support.supportinclination, 0.5 / WA2dt.Zm)
            ElseIf EL.SN.Support.RS = True Then
                SupportPicPaintRollerSupport(e, New Point(EL.SN.Coord.X + Extent.X, EL.SN.Coord.Y + Extent.Y), EL.SN.Support.supportinclination, 0.5 / WA2dt.Zm)
            End If

            '--- End Node
            If EL.EN.Support.PJ = True Then
                SupportPicPaintPinJoint(e, New Point(EL.EN.Coord.X + Extent.X, EL.EN.Coord.Y + Extent.Y))
            ElseIf EL.EN.Support.PS = True Then
                SupportPicPaintPinSupport(e, New Point(EL.EN.Coord.X + Extent.X, EL.EN.Coord.Y + Extent.Y), EL.EN.Support.supportinclination, 0.5 / WA2dt.Zm)
            ElseIf EL.EN.Support.RS = True Then
                SupportPicPaintRollerSupport(e, New Point(EL.EN.Coord.X + Extent.X, EL.EN.Coord.Y + Extent.Y), EL.EN.Support.supportinclination, 0.5 / WA2dt.Zm)
            End If
        Next
    End Sub

    Private Sub PaintPanLoad(ByRef e As System.Windows.Forms.PaintEventArgs, ByRef Extent As Point)
        For Each Nd In WA2dt.Bob
            If Nd.Load.Loadintensity <> 0 Then
                Dim PPt1 As New Point(Nd.Coord.X + Extent.X, Nd.Coord.Y + Extent.Y)
                Dim PPt2 As New Point
                PPt2 = New Point(PPt1.X + (Math.Cos(Nd.Load.Loadinclination) * (WA2dt.Loadhtfactor * (-Nd.Load.Loadintensity / WA2dt.Maxload))), _
                                 PPt1.Y + (Math.Sin(Nd.Load.Loadinclination) * (WA2dt.Loadhtfactor * (-Nd.Load.Loadintensity / WA2dt.Maxload))))
                Dim loadpen As New System.Drawing.Pen(Color.Green, 2 / WA2dt.Zm)
                loadpen.CustomStartCap = New System.Drawing.Drawing2D.AdjustableArrowCap(3, 5)
                e.Graphics.DrawLine(loadpen, PPt1.X, PPt1.Y, PPt2.X, PPt2.Y)
                If MDIMain.LoadviewoptionToolStripMenuItem1.Checked = True Then
                    e.Graphics.DrawString(Nd.Load.Loadintensity, New Font("Verdana", (8 / WA2dt.Zm)), New Pen(Color.Green, 2 / WA2dt.Zm).Brush, PPt2.X + (2 / WA2dt.Zm), PPt2.Y + (2 / WA2dt.Zm))
                End If
            End If
        Next
    End Sub
#End Region

#Region "Select Rectangle Draw"
    Private Sub PaintSelectRectangle(ByRef e As System.Windows.Forms.PaintEventArgs)
        e.Graphics.DrawLine(New Pen(Color.IndianRed, 2 / WA2dt.Zm), WA2dt.Spoint.X, WA2dt.Spoint.Y, WA2dt.Spoint.X, Pt.Y - WA2dt.Mpoint.Y)
        e.Graphics.DrawLine(New Pen(Color.IndianRed, 2 / WA2dt.Zm), WA2dt.Spoint.X, WA2dt.Spoint.Y, Pt.X - WA2dt.Mpoint.X, WA2dt.Spoint.Y)
        e.Graphics.DrawLine(New Pen(Color.IndianRed, 2 / WA2dt.Zm), Pt.X - WA2dt.Mpoint.X, Pt.Y - WA2dt.Mpoint.Y, WA2dt.Spoint.X, Pt.Y - WA2dt.Mpoint.Y)
        e.Graphics.DrawLine(New Pen(Color.IndianRed, 2 / WA2dt.Zm), Pt.X - WA2dt.Mpoint.X, Pt.Y - WA2dt.Mpoint.Y, Pt.X - WA2dt.Mpoint.X, WA2dt.Spoint.Y)
        Dim pts() As Point = {New Point(WA2dt.Spoint.X, WA2dt.Spoint.Y), New Point(Pt.X - WA2dt.Mpoint.X, WA2dt.Spoint.Y), New Point(Pt.X - WA2dt.Mpoint.X, Pt.Y - WA2dt.Mpoint.Y), New Point(WA2dt.Spoint.X, Pt.Y - WA2dt.Mpoint.Y)}
        e.Graphics.FillPolygon(New Pen(Color.FromArgb(50, Color.Crimson)).Brush, pts)
    End Sub
#End Region

#Region "Move N Clone Draw"
    Private Sub PaintMCMTempLine(ByRef e As System.Windows.Forms.PaintEventArgs)
        'Dim Acap As New System.Drawing.Drawing2D.AdjustableArrowCap(3, 8)
        e.Graphics.DrawLine(New Pen(OptionD.Gc, 2 / WA2dt.Zm), WA2dt.Spoint.X, WA2dt.Spoint.Y, Pt.X, Pt.Y)
        e.Graphics.DrawString(Math.Round((Math.Sqrt(Math.Pow(((WA2dt.Spoint.X) - Pt.X), 2) + Math.Pow(((WA2dt.Spoint.Y) - Pt.Y), 2)) / MDIMain.Nappdefaults.defaultScaleFactor), OptionD.Prec), New Font("Verdana", (8 / WA2dt.Zm)), New Pen(OptionD.Gc, 2 / WA2dt.Zm).Brush, ((WA2dt.Spoint.X + Pt.X) / 2), ((WA2dt.Spoint.Y + Pt.Y) / 2))
    End Sub

    '---Main Move Clone _ Paint Sequence
    Private Sub PaintTempMC(ByRef e As System.Windows.Forms.PaintEventArgs)
        Dim ext As New Point(-WA2dt.Spoint.X + Pt.X, -WA2dt.Spoint.Y + Pt.Y)
        PaintMCMember(e, ext)
        PaintMCNode(e, ext)
        'PaintMCLoad(e, ext)
        'PaintMCSupport(e, ext)
    End Sub

    Private Sub PaintMCMember(ByRef e As System.Windows.Forms.PaintEventArgs, ByRef Extent As Point)
        For Each ind In WA2dt.selLine
            e.Graphics.DrawLine(New Pen(OptionD.Mc, 2 / WA2dt.Zm), WA2dt.Mem(ind).SN.Coord.X + Extent.X, WA2dt.Mem(ind).SN.Coord.Y + Extent.Y, WA2dt.Mem(ind).EN.Coord.X + Extent.X, WA2dt.Mem(ind).EN.Coord.Y + Extent.Y)
            If MDIMain.LengthviewoptionToolStripMenuItem1.Checked = True Then
                e.Graphics.DrawString(Math.Round(WA2dt.Mem(ind).Length, OptionD.Prec), New Font("Verdana", (8 / WA2dt.Zm)), New Pen(OptionD.Lc, 2 / WA2dt.Zm).Brush, ((WA2dt.Mem(ind).SN.Coord.X + WA2dt.Mem(ind).EN.Coord.X) / 2) + Extent.X, ((WA2dt.Mem(ind).SN.Coord.Y + WA2dt.Mem(ind).EN.Coord.Y) / 2) + Extent.Y)
            End If
        Next
    End Sub

    Private Sub PaintMCNode(ByRef e As System.Windows.Forms.PaintEventArgs, ByRef Extent As Point)
        Dim TempB As New List(Of Line2DT.Node)
        TempCreateBob(TempB, Extent)
        'For Each Nd In TempB
        '    e.Graphics.FillEllipse(New Pen(OptionD.Nc, 4 / WA2dt.Zm).Brush, Nd.Coord.X  - (2 / WA2dt.Zm), Nd.Coord.Y  - (2 / WA2dt.Zm), 4 / WA2dt.Zm, 4 / WA2dt.Zm)
        'Next
    End Sub

    Private Sub TempCreateBob(ByRef TB As List(Of Line2DT.Node), ByRef Extent As Point)
        For Each ind In WA2dt.selLine
            AttachNode(WA2dt.Mem(ind).SN, TB, Extent)
            AttachNode(WA2dt.Mem(ind).EN, TB, Extent)
        Next
    End Sub

    Private Sub AttachNode(ByVal Nd As Line2DT.Node, ByRef TB As List(Of Line2DT.Node), ByRef Extent As Point)
        WA2dt.PD.Her = New Line2DT.Node(Nd.Coord + Extent)
        If WA2dt.Bob.Exists(WA2dt.PD.ExistingNodePD) = False Then
            TB.Add(New Line2DT.Node(Nd.Coord + Extent))
        End If
    End Sub

    Private Sub PaintMCSupport(ByRef e As System.Windows.Forms.PaintEventArgs, ByRef Extent As Point)
        '--- Start Node
        For Each ind In WA2dt.selLine
            If WA2dt.Mem(ind).SN.Support.PJ = True Then
                SupportPicPaintPinJoint(e, New Point(WA2dt.Mem(ind).SN.Coord.X + Extent.X, WA2dt.Mem(ind).SN.Coord.Y + Extent.Y))
            ElseIf WA2dt.Mem(ind).SN.Support.PS = True Then
                SupportPicPaintPinSupport(e, New Point(WA2dt.Mem(ind).SN.Coord.X + Extent.X, WA2dt.Mem(ind).SN.Coord.Y + Extent.Y), WA2dt.Mem(ind).SN.Support.supportinclination, 0.5)
            ElseIf WA2dt.Mem(ind).SN.Support.RS = True Then
                SupportPicPaintRollerSupport(e, New Point(WA2dt.Mem(ind).SN.Coord.X + Extent.X, WA2dt.Mem(ind).SN.Coord.Y + Extent.Y), WA2dt.Mem(ind).SN.Support.supportinclination, 0.5)
            End If

            '--- End Node
            If WA2dt.Mem(ind).EN.Support.PJ = True Then
                SupportPicPaintPinJoint(e, New Point(WA2dt.Mem(ind).EN.Coord.X + Extent.X, WA2dt.Mem(ind).EN.Coord.Y + Extent.Y))
            ElseIf WA2dt.Mem(ind).EN.Support.PS = True Then
                SupportPicPaintPinSupport(e, New Point(WA2dt.Mem(ind).EN.Coord.X + Extent.X, WA2dt.Mem(ind).EN.Coord.Y + Extent.Y), WA2dt.Mem(ind).EN.Support.supportinclination, 0.5)
            ElseIf WA2dt.Mem(ind).EN.Support.RS = True Then
                SupportPicPaintRollerSupport(e, New Point(WA2dt.Mem(ind).EN.Coord.X + Extent.X, WA2dt.Mem(ind).EN.Coord.Y + Extent.Y), WA2dt.Mem(ind).EN.Support.supportinclination, 0.5)
            End If
        Next
    End Sub

    Private Sub PaintMCLoad(ByRef e As System.Windows.Forms.PaintEventArgs, ByRef Extent As Point)
        For Each Nd In WA2dt.Bob
            If Nd.Load.Loadintensity <> 0 Then
                Dim PPt1 As New Point(Nd.Coord.X + Extent.X, Nd.Coord.Y + Extent.Y)
                Dim PPt2 As New Point
                PPt2 = New Point(PPt1.X + (Math.Cos(Nd.Load.Loadinclination) * (WA2dt.Loadhtfactor * (-Nd.Load.Loadintensity / WA2dt.Maxload))), _
                                 PPt1.Y + (Math.Sin(Nd.Load.Loadinclination) * (WA2dt.Loadhtfactor * (-Nd.Load.Loadintensity / WA2dt.Maxload))))
                Dim loadpen As New System.Drawing.Pen(Color.Green, 2 / WA2dt.Zm)
                loadpen.CustomStartCap = New System.Drawing.Drawing2D.AdjustableArrowCap(3, 5)
                e.Graphics.DrawLine(loadpen, PPt1.X, PPt1.Y, PPt2.X, PPt2.Y)
                If MDIMain.LoadviewoptionToolStripMenuItem1.Checked = True Then
                    e.Graphics.DrawString(Nd.Load.Loadintensity, New Font("Verdana", (8 / WA2dt.Zm)), New Pen(Color.Green, 2 / WA2dt.Zm).Brush, PPt2.X + (2 / WA2dt.Zm), PPt2.Y + (2 / WA2dt.Zm))
                End If
            End If
        Next
    End Sub
#End Region

#Region "Support Pic Paint Events"
    Private Sub SupportPicPaintPinJoint(ByRef e As System.Windows.Forms.PaintEventArgs, ByVal M As Point)
        e.Graphics.DrawEllipse(New Pen(OptionD.Nc, 2 / WA2dt.Zm), CInt(M.X - (2 / WA2dt.Zm)), CInt(M.Y - (2 / WA2dt.Zm)), CInt((4 / WA2dt.Zm)), CInt((4 / WA2dt.Zm)))
        'e.Graphics.FillEllipse(New Pen(OptionD.Nc, 4 / WA2dt.Zm).Brush, M.X - (3 / WA2dt.Zm), M.Y - (3 / WA2dt.Zm), (6 / WA2dt.Zm), (6 / WA2dt.Zm))
    End Sub

    Private Sub SupportPicPaintPinSupport(ByRef e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double, ByVal ZM As Double)
        '----Triangle
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / WA2dt.Zm), New Point(M.X + ((Math.Cos(-Inclination) + Math.Sin(-Inclination)) * ZM), M.Y + ((-Math.Sin(-Inclination) + Math.Cos(-Inclination))) * ZM), New Point(M.X + ((20 * Math.Cos(-Inclination) + (12 * Math.Sin(-Inclination))) * ZM), M.Y + ((20 * -Math.Sin(-Inclination) + (12 * Math.Cos(-Inclination)))) * ZM))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / WA2dt.Zm), New Point(M.X + ((Math.Cos(-Inclination) + Math.Sin(-Inclination)) * ZM), M.Y + ((-Math.Sin(-Inclination) + Math.Cos(-Inclination))) * ZM), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination)))) * ZM))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / WA2dt.Zm), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (12 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (12 * Math.Cos(-Inclination)))) * ZM), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination))) * ZM)))

        '----Bottom Rectangle
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / WA2dt.Zm), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZM), M.Y + ((20 * -Math.Sin(-Inclination) + (-20 * Math.Cos(-Inclination)))) * ZM))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / WA2dt.Zm), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZM), M.Y + (((30 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination)))) * ZM))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / WA2dt.Zm), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (20 * Math.Sin(-Inclination))) * ZM), M.Y + (((30 * -Math.Sin(-Inclination)) + (20 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZM), M.Y + (((30 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination)))) * ZM))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / WA2dt.Zm), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination))) * ZM)), New Point(M.X + (((30 * Math.Cos(-Inclination)) + (-20 * Math.Sin(-Inclination))) * ZM), M.Y + (((30 * -Math.Sin(-Inclination)) + (-20 * Math.Cos(-Inclination)))) * ZM))

    End Sub

    Private Sub SupportPicPaintRollerSupport(ByRef e As System.Windows.Forms.PaintEventArgs, ByVal M As Point, ByVal Inclination As Double, ByVal ZM As Double)
        '----Triangle
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / WA2dt.Zm), New Point(M.X + ((Math.Cos(-Inclination) + Math.Sin(-Inclination)) * ZM), M.Y + ((-Math.Sin(-Inclination) + Math.Cos(-Inclination))) * ZM), New Point(M.X + ((20 * Math.Cos(-Inclination) + (12 * Math.Sin(-Inclination))) * ZM), M.Y + ((20 * -Math.Sin(-Inclination) + (12 * Math.Cos(-Inclination)))) * ZM))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / WA2dt.Zm), New Point(M.X + ((Math.Cos(-Inclination) + Math.Sin(-Inclination)) * ZM), M.Y + ((-Math.Sin(-Inclination) + Math.Cos(-Inclination))) * ZM), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination)))) * ZM))
        e.Graphics.DrawLine(New Pen(OptionD.Nc, 2 / WA2dt.Zm), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (12 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (12 * Math.Cos(-Inclination)))) * ZM), New Point(M.X + (((20 * Math.Cos(-Inclination)) + (-12 * Math.Sin(-Inclination))) * ZM), M.Y + (((20 * -Math.Sin(-Inclination)) + (-12 * Math.Cos(-Inclination))) * ZM)))

        '----Bottom Circle
        e.Graphics.DrawEllipse(New Pen(OptionD.Nc, 2 / WA2dt.Zm), New Rectangle(New Point((M.X - (5 * ZM)) + (((25 * Math.Cos(-Inclination)) + (6 * Math.Sin(-Inclination))) * ZM), (M.Y - (5 * ZM)) + ((25 * (-Math.Sin(-Inclination)) + (6 * Math.Cos(-Inclination))) * ZM)), New Size((10 * ZM), (10 * ZM))))
        e.Graphics.DrawEllipse(New Pen(OptionD.Nc, 2 / WA2dt.Zm), New Rectangle(New Point((M.X - (5 * ZM)) + (((25 * Math.Cos(-Inclination)) + (-6 * Math.Sin(-Inclination))) * ZM), (M.Y - (5 * ZM)) + ((25 * (-Math.Sin(-Inclination)) + (-6 * Math.Cos(-Inclination))) * ZM)), New Size((10 * ZM), (10 * ZM))))

    End Sub
#End Region

#Region "Mirror Draw"
    Private Sub PaintMirr(ByRef e As System.Windows.Forms.PaintEventArgs)
        'Dim Acap As New System.Drawing.Drawing2D.AdjustableArrowCap(3, 8)
        Dim Ext1 As New Point(WA2dt.Spoint.X, WA2dt.Spoint.Y)
        Dim Ext2 As New Point(Pt.X, Pt.Y)
        FlipTempLine(Ext1, Ext2)
        PaintTempMirr(e, Ext1, Ext2)
        e.Graphics.DrawLine(New Pen(OptionD.Gc, 2 / WA2dt.Zm), Ext1.X, Ext1.Y, Ext2.X, Ext2.Y)
    End Sub

    Private Sub FlipTempLine(ByRef e1 As Point, ByRef e2 As Point)
        If Math.Abs(e2.X - e1.X) > Math.Abs(e2.Y - e1.Y) Then
            e2.Y = e1.Y
        Else
            e2.X = e1.X
        End If
    End Sub

    Private Sub PaintTempMirr(ByRef e As System.Windows.Forms.PaintEventArgs, ByRef e1 As Point, ByRef e2 As Point)
        If e1.X = e2.X Then
            PaintHorizFlip(e, e1.X)
        ElseIf e1.Y = e2.Y Then
            PaintVertFlip(e, e1.Y)
        End If
    End Sub

    '---Main Horizontal Flip _ Paint Sequence
    Private Sub PaintHorizFlip(ByRef e As System.Windows.Forms.PaintEventArgs, ByRef Mirx As Double)
        For Each ind In WA2dt.selLine
            Dim Sp As New Point(-(WA2dt.Mem(ind).SN.Coord.X) + (2 * Mirx), (WA2dt.Mem(ind).SN.Coord.Y))
            Dim Ep As New Point(-(WA2dt.Mem(ind).EN.Coord.X) + (2 * Mirx), (WA2dt.Mem(ind).EN.Coord.Y))
            e.Graphics.DrawLine(New Pen(OptionD.Mc, 2 / WA2dt.Zm), Sp.X, Sp.Y, Ep.X, Ep.Y)
            If MDIMain.LengthviewoptionToolStripMenuItem1.Checked = True Then
                e.Graphics.DrawString(Math.Round(WA2dt.Mem(ind).Length, OptionD.Prec), New Font("Verdana", (8 / WA2dt.Zm)), New Pen(OptionD.Lc, 2 / WA2dt.Zm).Brush, (Sp.X + Ep.X) / 2, (Sp.Y + Ep.Y) / 2)
            End If
        Next
        PaintHorizMirrNode(e, Mirx)
        'PaintMirrSupport(e, Mirx, 1, 0, -1)
    End Sub

    Private Sub PaintHorizMirrNode(ByRef e As System.Windows.Forms.PaintEventArgs, ByRef Mirx As Double)
        Dim TempB As New List(Of Line2DT.Node)
        For Each ind In WA2dt.selLine
            AttachHorizMirrNode(WA2dt.Mem(ind).SN, TempB, Mirx)
            AttachHorizMirrNode(WA2dt.Mem(ind).EN, TempB, Mirx)
        Next
        For Each Nd In TempB
            e.Graphics.FillEllipse(New Pen(OptionD.Nc, 4 / WA2dt.Zm).Brush, CInt(Nd.Coord.X - (2 / WA2dt.Zm)), CInt(Nd.Coord.Y - (2 / WA2dt.Zm)), CInt(4 / WA2dt.Zm), CInt(4 / WA2dt.Zm))
        Next
    End Sub

    Private Sub AttachHorizMirrNode(ByVal Nd As Line2DT.Node, ByRef TB As List(Of Line2DT.Node), ByRef Mirx As Double)
        WA2dt.PD.Her = New Line2DT.Node(New Point((-(Nd.Coord.X) + (2 * Mirx)), (Nd.Coord.Y)))
        If WA2dt.Bob.Exists(WA2dt.PD.ExistingNodePD) = False Then
            TB.Add(New Line2DT.Node(New Point((-(Nd.Coord.X) + (2 * Mirx)), (Nd.Coord.Y))))
        End If
    End Sub

    '---Main Vertical Flip _ Paint Sequence
    Private Sub PaintVertFlip(ByRef e As System.Windows.Forms.PaintEventArgs, ByRef Miry As Double)
        For Each ind In WA2dt.selLine
            Dim Sp As New Point((WA2dt.Mem(ind).SN.Coord.X), -(WA2dt.Mem(ind).SN.Coord.Y) + (2 * Miry))
            Dim Ep As New Point((WA2dt.Mem(ind).EN.Coord.X), -(WA2dt.Mem(ind).EN.Coord.Y) + (2 * Miry))
            e.Graphics.DrawLine(New Pen(OptionD.Mc, 2 / WA2dt.Zm), Sp.X, Sp.Y, Ep.X, Ep.Y)
            If MDIMain.LengthviewoptionToolStripMenuItem1.Checked = True Then
                e.Graphics.DrawString(Math.Round(WA2dt.Mem(ind).Length, OptionD.Prec), New Font("Verdana", (8 / WA2dt.Zm)), New Pen(OptionD.Lc, 2 / WA2dt.Zm).Brush, (Sp.X + Ep.X) / 2, (Sp.Y + Ep.Y) / 2)
            End If
        Next
        PaintVertMirrNode(e, Miry)
        'PaintMirrSupport(e, 0, -1, Miry, 1)
    End Sub

    Private Sub PaintVertMirrNode(ByRef e As System.Windows.Forms.PaintEventArgs, ByRef Miry As Double)
        Dim TempB As New List(Of Line2DT.Node)
        For Each ind In WA2dt.selLine
            AttachVertMirrNode(WA2dt.Mem(ind).SN, TempB, Miry)
            AttachVertMirrNode(WA2dt.Mem(ind).EN, TempB, Miry)
        Next
        For Each Nd In TempB
            e.Graphics.FillEllipse(New Pen(OptionD.Nc, 4 / WA2dt.Zm).Brush, CInt(Nd.Coord.X - (2 / WA2dt.Zm)), CInt(Nd.Coord.Y - (2 / WA2dt.Zm)), CInt(4 / WA2dt.Zm), CInt(4 / WA2dt.Zm))
        Next
    End Sub

    Private Sub AttachVertMirrNode(ByVal Nd As Line2DT.Node, ByRef TB As List(Of Line2DT.Node), ByRef Miry As Double)
        WA2dt.PD.Her = New Line2DT.Node(New Point((Nd.Coord.X), (-(Nd.Coord.Y) + (2 * Miry))))
        If WA2dt.Bob.Exists(WA2dt.PD.ExistingNodePD) = False Then
            TB.Add(New Line2DT.Node(New Point((Nd.Coord.X), (-(Nd.Coord.Y) + (2 * Miry)))))
        End If
    End Sub

    Private Sub PaintMirrSupport(ByRef e As System.Windows.Forms.PaintEventArgs, ByRef MirrX As Double, ByVal XF As Integer, ByRef MirrY As Double, ByVal YF As Integer)
        '--- Start Node
        For Each ind In WA2dt.selLine
            Dim FlipPt1 As New Point((2 * MirrX) - (XF * (WA2dt.Mem(ind).SN.Coord.X)), (2 * MirrY) - (YF * (WA2dt.Mem(ind).SN.Coord.Y)))
            Dim FlipPt2 As New Point((2 * MirrX) - (XF * (WA2dt.Mem(ind).EN.Coord.X)), (2 * MirrY) - (YF * (WA2dt.Mem(ind).EN.Coord.Y)))

            If WA2dt.Mem(ind).SN.Support.PJ = True Then
                SupportPicPaintPinJoint(e, New Point(FlipPt1.X, FlipPt1.Y))
            ElseIf WA2dt.Mem(ind).SN.Support.PS = True Then
                SupportPicPaintPinSupport(e, New Point(FlipPt1.X, FlipPt1.Y), WA2dt.Mem(ind).SN.Support.supportinclination, 0.5)
            ElseIf WA2dt.Mem(ind).SN.Support.RS = True Then
                SupportPicPaintRollerSupport(e, New Point(FlipPt1.X, FlipPt1.Y), WA2dt.Mem(ind).SN.Support.supportinclination, 0.5)
            End If

            '--- End Node
            If WA2dt.Mem(ind).EN.Support.PJ = True Then
                SupportPicPaintPinJoint(e, New Point(FlipPt2.X, FlipPt2.Y))
            ElseIf WA2dt.Mem(ind).EN.Support.PS = True Then
                SupportPicPaintPinSupport(e, New Point(FlipPt2.X, FlipPt2.Y), WA2dt.Mem(ind).EN.Support.supportinclination, 0.5)
            ElseIf WA2dt.Mem(ind).EN.Support.RS = True Then
                SupportPicPaintRollerSupport(e, New Point(FlipPt2.X, FlipPt2.Y), WA2dt.Mem(ind).EN.Support.supportinclination, 0.5)
            End If
        Next
    End Sub
#End Region
End Class

