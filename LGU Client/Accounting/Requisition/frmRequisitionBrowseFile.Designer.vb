﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRequisitionBrowseFile
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
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.MyDataGridView = New System.Windows.Forms.DataGridView()
        Me.cms_em = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.trncode = New System.Windows.Forms.TextBox()
        Me.trntype = New System.Windows.Forms.TextBox()
        Me.txtFileName = New DevExpress.XtraEditors.SearchLookUpEdit()
        Me.gridfilename = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.LabelControl8 = New DevExpress.XtraEditors.LabelControl()
        Me.docname = New System.Windows.Forms.TextBox()
        Me.filecode = New System.Windows.Forms.TextBox()
        Me.cmdBrowseFile = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.MyDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cms_em.SuspendLayout()
        CType(Me.txtFileName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridfilename, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.Button1.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Button1.Location = New System.Drawing.Point(121, 378)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(205, 32)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Confirm and Upload File"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'MyDataGridView
        '
        Me.MyDataGridView.AllowUserToAddRows = False
        Me.MyDataGridView.AllowUserToResizeColumns = False
        Me.MyDataGridView.AllowUserToResizeRows = False
        Me.MyDataGridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MyDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.MyDataGridView.BackgroundColor = System.Drawing.Color.White
        Me.MyDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.MyDataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.MyDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.MyDataGridView.ContextMenuStrip = Me.cms_em
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.MyDataGridView.DefaultCellStyle = DataGridViewCellStyle2
        Me.MyDataGridView.Location = New System.Drawing.Point(8, 60)
        Me.MyDataGridView.Margin = New System.Windows.Forms.Padding(2)
        Me.MyDataGridView.Name = "MyDataGridView"
        Me.MyDataGridView.ReadOnly = True
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.MyDataGridView.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.MyDataGridView.RowHeadersVisible = False
        Me.MyDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.CornflowerBlue
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White
        Me.MyDataGridView.RowsDefaultCellStyle = DataGridViewCellStyle4
        Me.MyDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.MyDataGridView.Size = New System.Drawing.Size(413, 313)
        Me.MyDataGridView.TabIndex = 374
        '
        'cms_em
        '
        Me.cms_em.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmdBrowseFile})
        Me.cms_em.Name = "ContextMenuStrip1"
        Me.cms_em.Size = New System.Drawing.Size(134, 26)
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'trncode
        '
        Me.trncode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.trncode.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.trncode.ForeColor = System.Drawing.Color.Black
        Me.trncode.Location = New System.Drawing.Point(13, 303)
        Me.trncode.Margin = New System.Windows.Forms.Padding(4)
        Me.trncode.Name = "trncode"
        Me.trncode.ReadOnly = True
        Me.trncode.Size = New System.Drawing.Size(35, 22)
        Me.trncode.TabIndex = 416
        Me.trncode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.trncode.Visible = False
        '
        'trntype
        '
        Me.trntype.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.trntype.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.trntype.ForeColor = System.Drawing.Color.Black
        Me.trntype.Location = New System.Drawing.Point(56, 303)
        Me.trntype.Margin = New System.Windows.Forms.Padding(4)
        Me.trntype.Name = "trntype"
        Me.trntype.ReadOnly = True
        Me.trntype.Size = New System.Drawing.Size(35, 22)
        Me.trntype.TabIndex = 419
        Me.trntype.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.trntype.Visible = False
        '
        'txtFileName
        '
        Me.txtFileName.EditValue = "sss"
        Me.txtFileName.Location = New System.Drawing.Point(8, 29)
        Me.txtFileName.Name = "txtFileName"
        Me.txtFileName.Properties.Appearance.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtFileName.Properties.Appearance.Options.UseFont = True
        Me.txtFileName.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtFileName.Properties.AppearanceFocused.Options.UseBackColor = True
        Me.txtFileName.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat
        Me.txtFileName.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.txtFileName.Properties.DisplayMember = "Select"
        Me.txtFileName.Properties.NullText = ""
        Me.txtFileName.Properties.PopupView = Me.gridfilename
        Me.txtFileName.Properties.ValueMember = "code"
        Me.txtFileName.Size = New System.Drawing.Size(413, 26)
        Me.txtFileName.TabIndex = 936
        '
        'gridfilename
        '
        Me.gridfilename.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        Me.gridfilename.Name = "gridfilename"
        Me.gridfilename.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.gridfilename.OptionsView.ShowGroupPanel = False
        '
        'LabelControl8
        '
        Me.LabelControl8.Appearance.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.LabelControl8.Appearance.Options.UseFont = True
        Me.LabelControl8.Location = New System.Drawing.Point(8, 6)
        Me.LabelControl8.Name = "LabelControl8"
        Me.LabelControl8.Size = New System.Drawing.Size(146, 17)
        Me.LabelControl8.TabIndex = 937
        Me.LabelControl8.Text = "Select document file type"
        '
        'docname
        '
        Me.docname.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.docname.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.docname.ForeColor = System.Drawing.Color.Black
        Me.docname.Location = New System.Drawing.Point(99, 303)
        Me.docname.Margin = New System.Windows.Forms.Padding(4)
        Me.docname.Name = "docname"
        Me.docname.ReadOnly = True
        Me.docname.Size = New System.Drawing.Size(35, 22)
        Me.docname.TabIndex = 938
        Me.docname.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.docname.Visible = False
        '
        'filecode
        '
        Me.filecode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.filecode.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.filecode.ForeColor = System.Drawing.Color.Black
        Me.filecode.Location = New System.Drawing.Point(142, 303)
        Me.filecode.Margin = New System.Windows.Forms.Padding(4)
        Me.filecode.Name = "filecode"
        Me.filecode.ReadOnly = True
        Me.filecode.Size = New System.Drawing.Size(35, 22)
        Me.filecode.TabIndex = 939
        Me.filecode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.filecode.Visible = False
        '
        'cmdBrowseFile
        '
        Me.cmdBrowseFile.Image = Global.LGUClient.My.Resources.Resources.inbox_upload
        Me.cmdBrowseFile.Name = "cmdBrowseFile"
        Me.cmdBrowseFile.Size = New System.Drawing.Size(133, 22)
        Me.cmdBrowseFile.Text = "Browse File"
        '
        'frmRequisitionBrowseFile
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(430, 416)
        Me.Controls.Add(Me.filecode)
        Me.Controls.Add(Me.docname)
        Me.Controls.Add(Me.txtFileName)
        Me.Controls.Add(Me.LabelControl8)
        Me.Controls.Add(Me.trntype)
        Me.Controls.Add(Me.trncode)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.MyDataGridView)
        Me.MaximizeBox = False
        Me.MinimumSize = New System.Drawing.Size(345, 434)
        Me.Name = "frmRequisitionBrowseFile"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "File Uploader"
        CType(Me.MyDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cms_em.ResumeLayout(False)
        CType(Me.txtFileName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridfilename, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents MyDataGridView As System.Windows.Forms.DataGridView
    Friend WithEvents cms_em As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents cmdBrowseFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents trncode As System.Windows.Forms.TextBox
    Friend WithEvents trntype As System.Windows.Forms.TextBox
    Friend WithEvents txtFileName As DevExpress.XtraEditors.SearchLookUpEdit
    Friend WithEvents gridfilename As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LabelControl8 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents docname As System.Windows.Forms.TextBox
    Friend WithEvents filecode As System.Windows.Forms.TextBox

End Class