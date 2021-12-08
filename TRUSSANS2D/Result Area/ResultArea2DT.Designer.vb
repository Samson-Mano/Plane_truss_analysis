<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ResultArea2DT
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Coverpanel = New System.Windows.Forms.Panel
        Me.MainPic = New System.Windows.Forms.Panel
        Me.MTPic = New System.Windows.Forms.PictureBox
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.UndoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RedoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DelToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MoveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CloneToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MirrorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.LineToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.LoadToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SupportToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PropertiesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AnalyzeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Coverpanel.SuspendLayout()
        Me.MainPic.SuspendLayout()
        CType(Me.MTPic, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Coverpanel
        '
        Me.Coverpanel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Coverpanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Coverpanel.Controls.Add(Me.MainPic)
        Me.Coverpanel.Location = New System.Drawing.Point(0, 0)
        Me.Coverpanel.Margin = New System.Windows.Forms.Padding(4)
        Me.Coverpanel.Name = "Coverpanel"
        Me.Coverpanel.Size = New System.Drawing.Size(803, 516)
        Me.Coverpanel.TabIndex = 2
        '
        'MainPic
        '
        Me.MainPic.BackColor = System.Drawing.Color.White
        Me.MainPic.Controls.Add(Me.MTPic)
        Me.MainPic.Controls.Add(Me.MenuStrip1)
        Me.MainPic.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MainPic.Location = New System.Drawing.Point(0, 0)
        Me.MainPic.Margin = New System.Windows.Forms.Padding(4)
        Me.MainPic.Name = "MainPic"
        Me.MainPic.Size = New System.Drawing.Size(799, 512)
        Me.MainPic.TabIndex = 0
        '
        'MTPic
        '
        Me.MTPic.BackColor = System.Drawing.Color.Transparent
        Me.MTPic.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MTPic.Enabled = False
        Me.MTPic.Location = New System.Drawing.Point(0, 0)
        Me.MTPic.Margin = New System.Windows.Forms.Padding(4)
        Me.MTPic.Name = "MTPic"
        Me.MTPic.Size = New System.Drawing.Size(799, 512)
        Me.MTPic.TabIndex = 0
        Me.MTPic.TabStop = False
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.UndoToolStripMenuItem, Me.RedoToolStripMenuItem, Me.DelToolStripMenuItem, Me.MoveToolStripMenuItem, Me.CloneToolStripMenuItem, Me.MirrorToolStripMenuItem, Me.LineToolStripMenuItem, Me.LoadToolStripMenuItem, Me.SupportToolStripMenuItem, Me.PropertiesToolStripMenuItem, Me.AnalyzeToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Padding = New System.Windows.Forms.Padding(8, 2, 0, 2)
        Me.MenuStrip1.Size = New System.Drawing.Size(799, 28)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        Me.MenuStrip1.Visible = False
        '
        'UndoToolStripMenuItem
        '
        Me.UndoToolStripMenuItem.Name = "UndoToolStripMenuItem"
        Me.UndoToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Z), System.Windows.Forms.Keys)
        Me.UndoToolStripMenuItem.Size = New System.Drawing.Size(57, 24)
        Me.UndoToolStripMenuItem.Text = "Undo"
        '
        'RedoToolStripMenuItem
        '
        Me.RedoToolStripMenuItem.Name = "RedoToolStripMenuItem"
        Me.RedoToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.R), System.Windows.Forms.Keys)
        Me.RedoToolStripMenuItem.Size = New System.Drawing.Size(56, 24)
        Me.RedoToolStripMenuItem.Text = "Redo"
        '
        'DelToolStripMenuItem
        '
        Me.DelToolStripMenuItem.Name = "DelToolStripMenuItem"
        Me.DelToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete
        Me.DelToolStripMenuItem.Size = New System.Drawing.Size(44, 24)
        Me.DelToolStripMenuItem.Text = "Del"
        '
        'MoveToolStripMenuItem
        '
        Me.MoveToolStripMenuItem.Name = "MoveToolStripMenuItem"
        Me.MoveToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
        Me.MoveToolStripMenuItem.Size = New System.Drawing.Size(58, 24)
        Me.MoveToolStripMenuItem.Text = "move"
        '
        'CloneToolStripMenuItem
        '
        Me.CloneToolStripMenuItem.Name = "CloneToolStripMenuItem"
        Me.CloneToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.CloneToolStripMenuItem.Size = New System.Drawing.Size(57, 24)
        Me.CloneToolStripMenuItem.Text = "clone"
        '
        'MirrorToolStripMenuItem
        '
        Me.MirrorToolStripMenuItem.Name = "MirrorToolStripMenuItem"
        Me.MirrorToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
        Me.MirrorToolStripMenuItem.Size = New System.Drawing.Size(62, 24)
        Me.MirrorToolStripMenuItem.Text = "mirror"
        '
        'LineToolStripMenuItem
        '
        Me.LineToolStripMenuItem.Name = "LineToolStripMenuItem"
        Me.LineToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.L), System.Windows.Forms.Keys)
        Me.LineToolStripMenuItem.Size = New System.Drawing.Size(45, 24)
        Me.LineToolStripMenuItem.Text = "line"
        '
        'LoadToolStripMenuItem
        '
        Me.LoadToolStripMenuItem.Name = "LoadToolStripMenuItem"
        Me.LoadToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.A), System.Windows.Forms.Keys)
        Me.LoadToolStripMenuItem.Size = New System.Drawing.Size(51, 24)
        Me.LoadToolStripMenuItem.Text = "load"
        '
        'SupportToolStripMenuItem
        '
        Me.SupportToolStripMenuItem.Name = "SupportToolStripMenuItem"
        Me.SupportToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.SupportToolStripMenuItem.Size = New System.Drawing.Size(72, 24)
        Me.SupportToolStripMenuItem.Text = "support"
        '
        'PropertiesToolStripMenuItem
        '
        Me.PropertiesToolStripMenuItem.Name = "PropertiesToolStripMenuItem"
        Me.PropertiesToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D), System.Windows.Forms.Keys)
        Me.PropertiesToolStripMenuItem.Size = New System.Drawing.Size(89, 24)
        Me.PropertiesToolStripMenuItem.Text = "properties"
        '
        'AnalyzeToolStripMenuItem
        '
        Me.AnalyzeToolStripMenuItem.Name = "AnalyzeToolStripMenuItem"
        Me.AnalyzeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5
        Me.AnalyzeToolStripMenuItem.Size = New System.Drawing.Size(73, 24)
        Me.AnalyzeToolStripMenuItem.Text = "Analyze"
        '
        'ResultArea2DT
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(804, 517)
        Me.Controls.Add(Me.Coverpanel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "ResultArea2DT"
        Me.Text = "WorkArea2DT"
        Me.Coverpanel.ResumeLayout(False)
        Me.MainPic.ResumeLayout(False)
        Me.MainPic.PerformLayout()
        CType(Me.MTPic, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Coverpanel As System.Windows.Forms.Panel
    Friend WithEvents MainPic As System.Windows.Forms.Panel
    Friend WithEvents MTPic As System.Windows.Forms.PictureBox
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents UndoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RedoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DelToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MoveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CloneToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MirrorToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LineToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LoadToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SupportToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PropertiesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AnalyzeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
