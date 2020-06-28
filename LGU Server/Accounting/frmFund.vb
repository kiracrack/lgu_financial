﻿Imports DevExpress.XtraEditors
Imports DevExpress.Skins

Public Class frmFund
    Private Sub frmFund_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SkinManager.EnableMdiFormSkins() : SetIcon(Me)
        SetIcon(Me)
        filter()
    End Sub

    Public Sub filter()
        LoadXgrid("Select  Code, CodeName, Description  from tblfund order by code asc", "tblfund", Em, GridView1, Me)
        XgridColAlign({"Code", "CodeName"}, GridView1, DevExpress.Utils.HorzAlignment.Center)
        GridView1.BestFitColumns()
    End Sub

    Private Sub cmdSaveButton_Click(sender As Object, e As EventArgs) Handles cmdSaveButton.Click
        If countqry("tblfund", "code='" & code.Text & "'") > 0 And mode.Text <> "edit" Then
            XtraMessageBox.Show("Code already exists!", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            code.Focus()
            Exit Sub
        ElseIf txtcodename.Text = "" Then
            XtraMessageBox.Show("Please enter code name!", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            txtcodename.Focus()
            Exit Sub
        ElseIf txtDescription.Text = "" Then
            XtraMessageBox.Show("Please enter description!", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            txtDescription.Focus()
            Exit Sub
        End If
        If mode.Text = "edit" Then
            com.CommandText = "update tblfund set  codename='" & txtcodename.Text & "',description='" & rchar(txtDescription.Text) & "' where code='" & code.Text & "'" : com.ExecuteNonQuery()
        Else
            com.CommandText = "insert into tblfund set code='" & code.Text & "',codename='" & txtcodename.Text & "',description='" & rchar(txtDescription.Text) & "'" : com.ExecuteNonQuery()
        End If
        code.Text = "" : mode.Text = "" : code.Enabled = True : txtcodename.Text = "" : txtDescription.Text = "" : code.Focus() : filter()
        XtraMessageBox.Show("fund successfully saved", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
    Public Sub showInfo()
        If code.Text = "" Then Exit Sub
        com.CommandText = "select * from tblfund where code='" & code.Text & "'" : rst = com.ExecuteReader
        While rst.Read
            txtcodename.Text = rst("codename").ToString
            txtDescription.Text = rst("description").ToString
        End While
        rst.Close()
    End Sub

    Private Sub cmdEdit_Click(sender As Object, e As EventArgs) Handles cmdEdit.Click
        mode.Text = "" : code.Enabled = False
        code.Text = GridView1.GetFocusedRowCellValue("Code").ToString
        mode.Text = "edit"
        showInfo()
    End Sub

    Private Sub cmdDelete_Click(sender As Object, e As EventArgs) Handles cmdDelete.Click
        If XtraMessageBox.Show("Are you sure you want to permanently remove selected item? ", compname, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = vbYes Then
            Dim I As Integer = 0
            For I = 0 To GridView1.SelectedRowsCount - 1
                com.CommandText = "delete from tblfund where code='" & GridView1.GetRowCellValue(GridView1.GetSelectedRows(I), "Code") & "' " : com.ExecuteNonQuery()
            Next
            filter()
        End If
    End Sub
End Class