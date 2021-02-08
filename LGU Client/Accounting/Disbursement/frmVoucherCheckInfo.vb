﻿Imports MySql.Data.MySqlClient ' this is to import MySQL.NET
Imports System
Imports System.IO
Imports DevExpress.XtraEditors

Public Class frmVoucherCheckInfo
    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, ByVal keyData As Keys) As Boolean
        If keyData = (Keys.Escape) Then
            Me.Close()
        End If
        Return ProcessCmdKey
    End Function

    Private Sub frmVoucherCheckInfo_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Me.Dispose()
    End Sub
    Private Sub frmVoucherCheckInfo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Icon = ico
        ViewInfo()
    End Sub

    Public Sub ViewInfo()
        com.CommandText = "select * from tbldisbursementvoucher as a where id='" & id.Text & "'" : rst = com.ExecuteReader
        While rst.Read
            voucherno.Text = rst("voucherno").ToString
            txtCheckNo.Text = rst("checkno").ToString
            txtCheckBankName.Text = rst("checkbank").ToString
            txtCheckDate.Text = If(rst("checkdate").ToString = "", "", CDate(rst("checkdate").ToString))
        End While
        rst.Close()
    End Sub
    Private Sub cmdSave_Click(sender As Object, e As EventArgs) Handles cmdSave.Click
        If txtCheckNo.Text = "" Then
            XtraMessageBox.Show("Please enter check number", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtCheckNo.Focus()
            Exit Sub
        ElseIf txtCheckBankName.Text = "" Then
            XtraMessageBox.Show("Please enter bank name", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtCheckBankName.Focus()
            Exit Sub
        ElseIf txtCheckDate.Text = "" Then
            XtraMessageBox.Show("Please select check date", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtCheckDate.Focus()
            Exit Sub
        End If
        If XtraMessageBox.Show("Are you sure you want to continue? ", GlobalOrganizationName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = vbYes Then
            com.CommandText = "UPDATE tbldisbursementvoucher set  " _
               + " checkno='" & txtCheckNo.Text & "', " _
               + " checkbank='" & txtCheckBankName.Text & "', " _
               + If(txtCheckDate.Text = "", "checkdate=null ", " checkdate='" & ConvertDate(txtCheckDate.EditValue) & "' ") _
               + " where id='" & id.Text & "'" : com.ExecuteNonQuery()
            frmDisbursementList.ViewList()
            XtraMessageBox.Show("Disbursement check successfully saved", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()
        End If
    End Sub

End Class