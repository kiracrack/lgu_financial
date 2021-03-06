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
        LoadBankAccount()
    End Sub

    Public Sub LoadBankAccount()
        LoadXgridLookupSearch("select code as 'Account No.', description as 'Bank Name' from  tblbankaccounts where fundcode='" & fundcode.Text & "'  order by description asc", "tblbankaccounts", txtCheckBankName, gridBank)
        XgridColAlign({"Account No."}, gridBank, DevExpress.Utils.HorzAlignment.Center)
        XgridColWidth({"Account No."}, gridBank, 140)
    End Sub

    Public Sub ViewInfo()
        com.CommandText = "select * from tbldisbursementvoucher as a where voucherid='" & id.Text & "'" : rst = com.ExecuteReader
        While rst.Read
            voucherno.Text = rst("voucherno").ToString
            txtCheckNo.Text = rst("checkno").ToString
            txtCheckBankName.EditValue = rst("checkbank").ToString
            txtCheckDate.Text = If(rst("checkdate").ToString = "", "", CDate(rst("checkdate").ToString))
            ckCheckIssued.Checked = rst("checkissued")
            fundcode.Text = rst("fundcode").ToString
            pid.Text = rst("pid").ToString
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
        ElseIf countqry("tbldisbursementvoucher", "checkno='" & txtCheckNo.Text & "' and voucherid<>'" & id.Text & "' and cancelled=0") > 0 Then
            XtraMessageBox.Show("Check number is already exists", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtCheckNo.Focus()
            Exit Sub
        End If
        If XtraMessageBox.Show("Are you sure you want to continue? ", GlobalOrganizationName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = vbYes Then

            com.CommandText = "update tblrequisition set paid=1 where pid='" & pid.Text & "'" : com.ExecuteNonQuery()
            If ckCheckIssued.Checked = True Then
                com.CommandText = "UPDATE tbldisbursementvoucher set  " _
                              + " checkissued=1, " _
                              + " checkno='" & txtCheckNo.Text & "', " _
                              + " checkbank='" & txtCheckBankName.EditValue & "', " _
                              + If(txtCheckDate.Text = "", "checkdate=null ", " checkdate='" & ConvertDate(txtCheckDate.EditValue) & "' ") _
                              + " where voucherid='" & id.Text & "'" : com.ExecuteNonQuery()
            Else
                Dim yeartrn = CDate(txtCheckDate.EditValue).ToString("yyyy")
                Dim vno As String = getVoucherNumber(yeartrn, fundcode.Text, "tbldisbursementvoucher")
                voucherno.Text = fundcode.Text & "-" & yeartrn & "-" & CDate(txtCheckDate.EditValue).ToString("MM") & "-" & vno
                seriesno.Text = vno

                com.CommandText = "UPDATE tbldisbursementvoucher set  " _
                             + " checkissued=1, " _
                             + " voucherno='" & voucherno.Text & "', " _
                             + " seriesno='" & seriesno.Text & "', " _
                             + " checkno='" & txtCheckNo.Text & "', " _
                             + " checkbank='" & txtCheckBankName.EditValue & "', " _
                             + If(txtCheckDate.Text = "", "checkdate=null ", " checkdate='" & ConvertDate(txtCheckDate.EditValue) & "' ") _
                             + " where voucherid='" & id.Text & "'" : com.ExecuteNonQuery()
            End If
            frmDisbursementList.ViewList()
            XtraMessageBox.Show("Disbursement check successfully saved", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()
        End If
    End Sub

End Class