<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Loadwindow
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Loadwindow))
        Me._memberBindingNavigator = New System.Windows.Forms.BindingNavigator(Me.components)
        Me._memberBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.BindingNavigatorCountItem = New System.Windows.Forms.ToolStripLabel
        Me.BindingNavigatorMoveFirstItem = New System.Windows.Forms.ToolStripButton
        Me.BindingNavigatorMovePreviousItem = New System.Windows.Forms.ToolStripButton
        Me.BindingNavigatorSeparator = New System.Windows.Forms.ToolStripSeparator
        Me.BindingNavigatorPositionItem = New System.Windows.Forms.ToolStripTextBox
        Me.BindingNavigatorSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.BindingNavigatorMoveNextItem = New System.Windows.Forms.ToolStripButton
        Me.BindingNavigatorMoveLastItem = New System.Windows.Forms.ToolStripButton
        Me.BindingNavigatorSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.SNodeTabpage = New System.Windows.Forms.TabPage
        Me._SNinclination_txt = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me._SNLoad_txt = New System.Windows.Forms.TextBox
        Me._SNDel = New System.Windows.Forms.Button
        Me._SNAdd = New System.Windows.Forms.Button
        Me._NodeTab = New System.Windows.Forms.TabControl
        Me.ENodeTabpage = New System.Windows.Forms.TabPage
        Me._ENinclination_txt = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me._ENLoad_txt = New System.Windows.Forms.TextBox
        Me._ENDel = New System.Windows.Forms.Button
        Me._ENAdd = New System.Windows.Forms.Button
        Me._SNSupportPic = New System.Windows.Forms.PictureBox
        Me._LoadPic = New System.Windows.Forms.PictureBox
        CType(Me._memberBindingNavigator, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._memberBindingNavigator.SuspendLayout()
        CType(Me._memberBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SNodeTabpage.SuspendLayout()
        Me._NodeTab.SuspendLayout()
        Me.ENodeTabpage.SuspendLayout()
        CType(Me._SNSupportPic, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._LoadPic, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '_memberBindingNavigator
        '
        Me._memberBindingNavigator.AddNewItem = Nothing
        Me._memberBindingNavigator.BindingSource = Me._memberBindingSource
        Me._memberBindingNavigator.CountItem = Me.BindingNavigatorCountItem
        Me._memberBindingNavigator.DeleteItem = Nothing
        Me._memberBindingNavigator.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BindingNavigatorMoveFirstItem, Me.BindingNavigatorMovePreviousItem, Me.BindingNavigatorSeparator, Me.BindingNavigatorPositionItem, Me.BindingNavigatorCountItem, Me.BindingNavigatorSeparator1, Me.BindingNavigatorMoveNextItem, Me.BindingNavigatorMoveLastItem, Me.BindingNavigatorSeparator2})
        Me._memberBindingNavigator.Location = New System.Drawing.Point(0, 0)
        Me._memberBindingNavigator.MoveFirstItem = Me.BindingNavigatorMoveFirstItem
        Me._memberBindingNavigator.MoveLastItem = Me.BindingNavigatorMoveLastItem
        Me._memberBindingNavigator.MoveNextItem = Me.BindingNavigatorMoveNextItem
        Me._memberBindingNavigator.MovePreviousItem = Me.BindingNavigatorMovePreviousItem
        Me._memberBindingNavigator.Name = "_memberBindingNavigator"
        Me._memberBindingNavigator.PositionItem = Me.BindingNavigatorPositionItem
        Me._memberBindingNavigator.Size = New System.Drawing.Size(444, 27)
        Me._memberBindingNavigator.TabIndex = 6
        Me._memberBindingNavigator.Text = "BindingNavigator1"
        '
        '_memberBindingSource
        '
        '
        'BindingNavigatorCountItem
        '
        Me.BindingNavigatorCountItem.Name = "BindingNavigatorCountItem"
        Me.BindingNavigatorCountItem.Size = New System.Drawing.Size(45, 24)
        Me.BindingNavigatorCountItem.Text = "of {0}"
        Me.BindingNavigatorCountItem.ToolTipText = "Total number of items"
        '
        'BindingNavigatorMoveFirstItem
        '
        Me.BindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveFirstItem.Image = CType(resources.GetObject("BindingNavigatorMoveFirstItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveFirstItem.Name = "BindingNavigatorMoveFirstItem"
        Me.BindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveFirstItem.Size = New System.Drawing.Size(23, 24)
        Me.BindingNavigatorMoveFirstItem.Text = "Move first"
        '
        'BindingNavigatorMovePreviousItem
        '
        Me.BindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMovePreviousItem.Image = CType(resources.GetObject("BindingNavigatorMovePreviousItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMovePreviousItem.Name = "BindingNavigatorMovePreviousItem"
        Me.BindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMovePreviousItem.Size = New System.Drawing.Size(23, 24)
        Me.BindingNavigatorMovePreviousItem.Text = "Move previous"
        '
        'BindingNavigatorSeparator
        '
        Me.BindingNavigatorSeparator.Name = "BindingNavigatorSeparator"
        Me.BindingNavigatorSeparator.Size = New System.Drawing.Size(6, 27)
        '
        'BindingNavigatorPositionItem
        '
        Me.BindingNavigatorPositionItem.AccessibleName = "Position"
        Me.BindingNavigatorPositionItem.AutoSize = False
        Me.BindingNavigatorPositionItem.Name = "BindingNavigatorPositionItem"
        Me.BindingNavigatorPositionItem.Size = New System.Drawing.Size(50, 27)
        Me.BindingNavigatorPositionItem.Text = "0"
        Me.BindingNavigatorPositionItem.ToolTipText = "Current position"
        '
        'BindingNavigatorSeparator1
        '
        Me.BindingNavigatorSeparator1.Name = "BindingNavigatorSeparator1"
        Me.BindingNavigatorSeparator1.Size = New System.Drawing.Size(6, 27)
        '
        'BindingNavigatorMoveNextItem
        '
        Me.BindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveNextItem.Image = CType(resources.GetObject("BindingNavigatorMoveNextItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveNextItem.Name = "BindingNavigatorMoveNextItem"
        Me.BindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveNextItem.Size = New System.Drawing.Size(23, 24)
        Me.BindingNavigatorMoveNextItem.Text = "Move next"
        '
        'BindingNavigatorMoveLastItem
        '
        Me.BindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveLastItem.Image = CType(resources.GetObject("BindingNavigatorMoveLastItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveLastItem.Name = "BindingNavigatorMoveLastItem"
        Me.BindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveLastItem.Size = New System.Drawing.Size(23, 24)
        Me.BindingNavigatorMoveLastItem.Text = "Move last"
        '
        'BindingNavigatorSeparator2
        '
        Me.BindingNavigatorSeparator2.Name = "BindingNavigatorSeparator2"
        Me.BindingNavigatorSeparator2.Size = New System.Drawing.Size(6, 27)
        '
        'SNodeTabpage
        '
        Me.SNodeTabpage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SNodeTabpage.Controls.Add(Me._SNinclination_txt)
        Me.SNodeTabpage.Controls.Add(Me.Label2)
        Me.SNodeTabpage.Controls.Add(Me.Label1)
        Me.SNodeTabpage.Controls.Add(Me._SNLoad_txt)
        Me.SNodeTabpage.Controls.Add(Me._SNDel)
        Me.SNodeTabpage.Controls.Add(Me._SNAdd)
        Me.SNodeTabpage.Location = New System.Drawing.Point(4, 28)
        Me.SNodeTabpage.Name = "SNodeTabpage"
        Me.SNodeTabpage.Padding = New System.Windows.Forms.Padding(3)
        Me.SNodeTabpage.Size = New System.Drawing.Size(267, 132)
        Me.SNodeTabpage.TabIndex = 0
        Me.SNodeTabpage.Text = "Node - 1"
        Me.SNodeTabpage.UseVisualStyleBackColor = True
        '
        '_SNinclination_txt
        '
        Me._SNinclination_txt.Enabled = False
        Me._SNinclination_txt.Location = New System.Drawing.Point(160, 42)
        Me._SNinclination_txt.Name = "_SNinclination_txt"
        Me._SNinclination_txt.Size = New System.Drawing.Size(54, 22)
        Me._SNinclination_txt.TabIndex = 5
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(28, 45)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(115, 17)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Load Inclination :"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(28, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(104, 17)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Load Intensity :"
        '
        '_SNLoad_txt
        '
        Me._SNLoad_txt.Location = New System.Drawing.Point(160, 10)
        Me._SNLoad_txt.Name = "_SNLoad_txt"
        Me._SNLoad_txt.Size = New System.Drawing.Size(54, 22)
        Me._SNLoad_txt.TabIndex = 2
        Me._SNLoad_txt.Text = "10"
        '
        '_SNDel
        '
        Me._SNDel.Image = Global.TRUSSANS2D.My.Resources.Resources.del
        Me._SNDel.Location = New System.Drawing.Point(179, 85)
        Me._SNDel.Name = "_SNDel"
        Me._SNDel.Size = New System.Drawing.Size(35, 30)
        Me._SNDel.TabIndex = 1
        Me._SNDel.Text = "  "
        Me._SNDel.UseVisualStyleBackColor = True
        '
        '_SNAdd
        '
        Me._SNAdd.Image = Global.TRUSSANS2D.My.Resources.Resources.Add
        Me._SNAdd.Location = New System.Drawing.Point(138, 85)
        Me._SNAdd.Name = "_SNAdd"
        Me._SNAdd.Size = New System.Drawing.Size(35, 30)
        Me._SNAdd.TabIndex = 0
        Me._SNAdd.Text = "   "
        Me._SNAdd.UseVisualStyleBackColor = True
        '
        '_NodeTab
        '
        Me._NodeTab.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._NodeTab.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me._NodeTab.Controls.Add(Me.SNodeTabpage)
        Me._NodeTab.Controls.Add(Me.ENodeTabpage)
        Me._NodeTab.ItemSize = New System.Drawing.Size(230, 24)
        Me._NodeTab.Location = New System.Drawing.Point(4, 239)
        Me._NodeTab.Multiline = True
        Me._NodeTab.Name = "_NodeTab"
        Me._NodeTab.SelectedIndex = 0
        Me._NodeTab.Size = New System.Drawing.Size(275, 164)
        Me._NodeTab.TabIndex = 7
        '
        'ENodeTabpage
        '
        Me.ENodeTabpage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ENodeTabpage.Controls.Add(Me._ENinclination_txt)
        Me.ENodeTabpage.Controls.Add(Me.Label3)
        Me.ENodeTabpage.Controls.Add(Me.Label4)
        Me.ENodeTabpage.Controls.Add(Me._ENLoad_txt)
        Me.ENodeTabpage.Controls.Add(Me._ENDel)
        Me.ENodeTabpage.Controls.Add(Me._ENAdd)
        Me.ENodeTabpage.Location = New System.Drawing.Point(4, 28)
        Me.ENodeTabpage.Name = "ENodeTabpage"
        Me.ENodeTabpage.Padding = New System.Windows.Forms.Padding(3)
        Me.ENodeTabpage.Size = New System.Drawing.Size(267, 132)
        Me.ENodeTabpage.TabIndex = 1
        Me.ENodeTabpage.Text = "Node - 2"
        Me.ENodeTabpage.UseVisualStyleBackColor = True
        '
        '_ENinclination_txt
        '
        Me._ENinclination_txt.Enabled = False
        Me._ENinclination_txt.Location = New System.Drawing.Point(160, 42)
        Me._ENinclination_txt.Name = "_ENinclination_txt"
        Me._ENinclination_txt.Size = New System.Drawing.Size(54, 22)
        Me._ENinclination_txt.TabIndex = 11
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(28, 45)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(115, 17)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Load Inclination :"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(28, 13)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(104, 17)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Load Intensity :"
        '
        '_ENLoad_txt
        '
        Me._ENLoad_txt.Location = New System.Drawing.Point(160, 10)
        Me._ENLoad_txt.Name = "_ENLoad_txt"
        Me._ENLoad_txt.Size = New System.Drawing.Size(54, 22)
        Me._ENLoad_txt.TabIndex = 8
        Me._ENLoad_txt.Text = "10"
        '
        '_ENDel
        '
        Me._ENDel.Image = Global.TRUSSANS2D.My.Resources.Resources.del
        Me._ENDel.Location = New System.Drawing.Point(179, 85)
        Me._ENDel.Name = "_ENDel"
        Me._ENDel.Size = New System.Drawing.Size(35, 30)
        Me._ENDel.TabIndex = 7
        Me._ENDel.Text = "     "
        Me._ENDel.UseVisualStyleBackColor = True
        '
        '_ENAdd
        '
        Me._ENAdd.Image = Global.TRUSSANS2D.My.Resources.Resources.Add
        Me._ENAdd.Location = New System.Drawing.Point(138, 85)
        Me._ENAdd.Name = "_ENAdd"
        Me._ENAdd.Size = New System.Drawing.Size(35, 30)
        Me._ENAdd.TabIndex = 6
        Me._ENAdd.Text = "     "
        Me._ENAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._ENAdd.UseVisualStyleBackColor = True
        '
        '_SNSupportPic
        '
        Me._SNSupportPic.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._SNSupportPic.BackColor = System.Drawing.Color.White
        Me._SNSupportPic.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._SNSupportPic.Location = New System.Drawing.Point(282, 239)
        Me._SNSupportPic.Name = "_SNSupportPic"
        Me._SNSupportPic.Size = New System.Drawing.Size(156, 160)
        Me._SNSupportPic.TabIndex = 6
        Me._SNSupportPic.TabStop = False
        '
        '_LoadPic
        '
        Me._LoadPic.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._LoadPic.BackColor = System.Drawing.Color.White
        Me._LoadPic.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me._LoadPic.Location = New System.Drawing.Point(4, 30)
        Me._LoadPic.Name = "_LoadPic"
        Me._LoadPic.Size = New System.Drawing.Size(435, 203)
        Me._LoadPic.TabIndex = 5
        Me._LoadPic.TabStop = False
        '
        'Loadwindow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(444, 406)
        Me.Controls.Add(Me._SNSupportPic)
        Me.Controls.Add(Me._memberBindingNavigator)
        Me.Controls.Add(Me._NodeTab)
        Me.Controls.Add(Me._LoadPic)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Loadwindow"
        Me.Opacity = 0.8
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Loadwindow"
        CType(Me._memberBindingNavigator, System.ComponentModel.ISupportInitialize).EndInit()
        Me._memberBindingNavigator.ResumeLayout(False)
        Me._memberBindingNavigator.PerformLayout()
        CType(Me._memberBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SNodeTabpage.ResumeLayout(False)
        Me.SNodeTabpage.PerformLayout()
        Me._NodeTab.ResumeLayout(False)
        Me.ENodeTabpage.ResumeLayout(False)
        Me.ENodeTabpage.PerformLayout()
        CType(Me._SNSupportPic, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._LoadPic, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents _memberBindingNavigator As System.Windows.Forms.BindingNavigator
    Friend WithEvents _memberBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents BindingNavigatorCountItem As System.Windows.Forms.ToolStripLabel
    Friend WithEvents BindingNavigatorMoveFirstItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorMovePreviousItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorSeparator As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents BindingNavigatorPositionItem As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents BindingNavigatorSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents BindingNavigatorMoveNextItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorMoveLastItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents _SNSupportPic As System.Windows.Forms.PictureBox
    Friend WithEvents SNodeTabpage As System.Windows.Forms.TabPage
    Friend WithEvents _NodeTab As System.Windows.Forms.TabControl
    Friend WithEvents ENodeTabpage As System.Windows.Forms.TabPage
    Friend WithEvents _LoadPic As System.Windows.Forms.PictureBox
    Friend WithEvents _SNAdd As System.Windows.Forms.Button
    Friend WithEvents _SNinclination_txt As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents _SNLoad_txt As System.Windows.Forms.TextBox
    Friend WithEvents _SNDel As System.Windows.Forms.Button
    Friend WithEvents _ENinclination_txt As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents _ENLoad_txt As System.Windows.Forms.TextBox
    Friend WithEvents _ENDel As System.Windows.Forms.Button
    Friend WithEvents _ENAdd As System.Windows.Forms.Button
End Class
