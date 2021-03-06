﻿Imports System.Windows.Forms
Imports DBManager
Imports ITSBase
Imports UIControlLib
Imports ITSProcess

Public Class FrmWareHouseStock
    Private m_dtStock As DataTable
    Private m_dtStockDetail As DataTable
    Private m_dtPackage As DataTable
    Private m_dtPackageDetail As DataTable
    Private m_dtDrug As DataTable
    Private m_dtDrugDetail As DataTable
    Private m_oDBWarehouseStock As DBWareHouseManager

    Private Sub FrmWareHouseStock_Load(sender As Object, e As EventArgs) Handles Me.Load
        InitialTable()
        m_oDBWarehouseStock = New DBWareHouseManager
        Reset()
        btnTaking.Enabled = False
        btnChange.Enabled = False
        cmbINSTYPE.Items.Add(TEXT_WAREHOUSE_IN_REG_TYPE_WAREHOUSE_INS)
        cmbINSTYPE.Items.Add(TEXT_WAREHOUSE_IN_REG_TYPE_DRUG)
        cmbINSTYPE.Items.Add(TEXT_WAREHOUSE_IN_REG_TYPE_HIGN_VALUE)
        cmbINSTYPE.Items.Add(TEXT_WAREHOUSE_IN_REG_TYPE_PACKAGE)
        cmbINSTYPE.Text = TEXT_WAREHOUSE_IN_REG_TYPE_WAREHOUSE_INS
        Binding()
        'Binding()
        RefreshData()
    End Sub
    Private Sub InitialTable()
        TableConstructor.CreateBalanceStock(m_dtStock)
        TableConstructor.CreateWarerhouseStock(m_dtStockDetail)
        TableConstructor.CreateBalanceStock(m_dtPackage)
        TableConstructor.CreatePackageExpried(m_dtPackageDetail)
        TableConstructor.CreateDrugBalanceStock(m_dtDrug)
        TableConstructor.CreateDrugStockDetail(m_dtDrugDetail)
    End Sub
    Private Sub Binding()
        dgv.AllowUserToResizeColumns = True
        Dim arrWith() As Short
        If cmbINSTYPE.Text.Equals(TEXT_WAREHOUSE_IN_REG_TYPE_WAREHOUSE_INS) Then
            If btnDetail.Text = TEXT_DETAIL Then
                dgv.DataSource = m_dtStock
            Else
                arrWith = {0, 150, 250, 80, 80, 0, 250, 150, 150, 100, 120}
                dgv.DataSource = m_dtStockDetail
                dgv.Columns(0).Visible = False
                dgv.Columns(5).Visible = False
                dgv.Columns(1).Width = 150
            End If
        ElseIf cmbINSTYPE.Text.Equals(TEXT_WAREHOUSE_IN_REG_TYPE_PACKAGE) Then
            If btnDetail.Text = TEXT_DETAIL Then
                arrWith = {25, 25, 25, 25, 25}
                dgv.DataSource = m_dtPackage
            Else
                arrWith = {0, 15, 15, 15, 20, 10, 10, 15}
                dgv.DataSource = m_dtPackageDetail
                dgv.Columns(0).Visible = False
            End If
        ElseIf cmbINSTYPE.Text.Equals(TEXT_WAREHOUSE_IN_REG_TYPE_DRUG) Then
            If btnDetail.Text = TEXT_DETAIL Then
                dgv.DataSource = m_dtDrug
            Else
                dgv.DataSource = m_dtDrugDetail
                dgv.Columns(0).Visible = False
            End If
        End If
    End Sub
    Private Sub RefreshData()
        If cmbINSTYPE.Text.Equals(TEXT_WAREHOUSE_IN_REG_TYPE_WAREHOUSE_INS) Then
            If btnDetail.Text = TEXT_TOTAL Then
                btnTaking.Enabled = True
                If Not m_oDBWarehouseStock.QueryWareHouseStockDetail(m_dtStockDetail) = DBMEDITS_RESULT.SUCCESS Then
                    ZhiFa.Base.MessageControl.BaseMessageBox.ShowCustomerMessage(MessageBoxIcon.Error, "", MSG_DBERROR_EXCEPTION)
                End If
            Else
                btnTaking.Enabled = False
                If Not m_oDBWarehouseStock.QueryWareHouseStockTotal(m_dtStock) = DBMEDITS_RESULT.SUCCESS Then
                    ZhiFa.Base.MessageControl.BaseMessageBox.ShowCustomerMessage(MessageBoxIcon.Error, "", MSG_DBERROR_EXCEPTION)
                End If
            End If
        ElseIf cmbINSTYPE.Text.Equals(TEXT_WAREHOUSE_IN_REG_TYPE_PACKAGE) Then
            If btnDetail.Text = TEXT_TOTAL Then
                btnTaking.Enabled = True
                If Not m_oDBWarehouseStock.QueryPackageStockDetail(m_dtPackageDetail) = DBMEDITS_RESULT.SUCCESS Then
                    ZhiFa.Base.MessageControl.BaseMessageBox.ShowCustomerMessage(MessageBoxIcon.Error, "", MSG_DBERROR_EXCEPTION)
                End If
            Else
                btnTaking.Enabled = False
                If Not m_oDBWarehouseStock.QueryPackageStockTotal(m_dtPackage) = DBMEDITS_RESULT.SUCCESS Then
                    ZhiFa.Base.MessageControl.BaseMessageBox.ShowCustomerMessage(MessageBoxIcon.Error, "", MSG_DBERROR_EXCEPTION)
                End If

            End If
        ElseIf cmbINSTYPE.Text.Equals(TEXT_WAREHOUSE_IN_REG_TYPE_DRUG) Then
            If btnDetail.Text = TEXT_TOTAL Then
                btnTaking.Enabled = True
                If Not m_oDBWarehouseStock.QueryDrugStockDetail(m_dtDrugDetail) = DBMEDITS_RESULT.SUCCESS Then
                                        ZhiFa.Base.MessageControl.BaseMessageBox.ShowCustomerMessage(MessageBoxIcon.Error, "", MSG_DBERROR_EXCEPTION)
                End If
            Else
                btnTaking.Enabled = False
                If Not m_oDBWarehouseStock.QueryDrugStockTotal(m_dtDrug) = DBMEDITS_RESULT.SUCCESS Then
                    ZhiFa.Base.MessageControl.BaseMessageBox.ShowCustomerMessage(MessageBoxIcon.Error, "", MSG_DBERROR_EXCEPTION)
                End If
            End If
        End If
        Binding()
    End Sub
    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        RefreshData()
    End Sub
    Private Sub btnDetail_Click(sender As Object, e As EventArgs) Handles btnDetail.Click
        If btnDetail.Text = TEXT_DETAIL Then
            btnDetail.Text = TEXT_TOTAL
        Else
            btnDetail.Text = TEXT_DETAIL
        End If
        If dgv.DataSource Is m_dtPackage Then
            btnChange.Enabled = True
        End If
        RefreshData()
    End Sub

    Private Sub btnTaking_Click(sender As Object, e As EventArgs) Handles btnTaking.Click
        Dim dr As DataGridViewRow = dgv.CurrentRow
        If dr Is Nothing Then Exit Sub
        If cmbINSTYPE.Text.Equals(TEXT_WAREHOUSE_IN_REG_TYPE_WAREHOUSE_INS) Then
            Dim oFrmStocking As FrmWareHouseStocking = New FrmWareHouseStocking(dr)
            oFrmStocking.ShowDialog()
            btnRefresh.PerformClick()
        ElseIf cmbINSTYPE.Text.Equals(TEXT_WAREHOUSE_IN_REG_TYPE_DRUG) Then
            Dim oFrmStocking As FrmDrugStocking = New FrmDrugStocking(dr.Cells(DRS_ID).Value)
            oFrmStocking.ShowDialog()
            btnRefresh.PerformClick()
        End If
        
      
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        Dim oPrint As ReportPrinter = ReportPrinter.GetInstanse
        Dim strName As String = "Please enter the name"
        If dgv.DataSource Is m_dtStock Then
            If m_dtStock.Rows.Count < 1 Then
                Exit Sub
            End If
            If Not oPrint.PrintWareHouseStock(m_dtStock, strName) Then
                ZhiFa.Base.MessageControl.BaseMessageBox.ShowCustomerMessage(MessageBoxIcon.Error, "", oPrint.ErrorMessage)
            End If
        Else
            If m_dtStockDetail.Rows.Count < 1 Then
                Exit Sub
            End If
            If Not oPrint.PrintWareHouseStockDetail(m_dtStockDetail, strName) Then
                ZhiFa.Base.MessageControl.BaseMessageBox.ShowCustomerMessage(MessageBoxIcon.Error, "", oPrint.ErrorMessage)
            End If
        End If
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Dim oExport As ExportManager = ExportManager.GetInstanse
        Dim strName As String = "Please enter the name"
        If dgv.DataSource Is m_dtStock Then
            If m_dtStock.Rows.Count < 1 Then
                Exit Sub
            End If
            If Not oExport.ExportWareHouseStock(m_dtStock, strName) Then
                ZhiFa.Base.MessageControl.BaseMessageBox.ShowCustomerMessage(MessageBoxIcon.Error, "", oExport.ErrorMessage)
            End If
        Else
            If m_dtStockDetail.Rows.Count < 1 Then
                Exit Sub
            End If
            If Not oExport.ExportWareHouseStockDetail(m_dtStockDetail, strName) Then
                ZhiFa.Base.MessageControl.BaseMessageBox.ShowCustomerMessage(MessageBoxIcon.Error, "", oExport.ErrorMessage)
            End If
        End If
    End Sub

    Private Sub btnExpried_Click(sender As Object, e As EventArgs) Handles btnExpried.Click
        Dim oFrmExpried As FrmExpried = New FrmExpried
        oFrmExpried.ShowDialog()
    End Sub

    Private Sub cmbINSTYPE_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbINSTYPE.SelectedIndexChanged
        RefreshData()
    End Sub

    Private Sub btnChange_Click(sender As Object, e As EventArgs) Handles btnChange.Click
        Dim dr As DataGridViewRow = dgv.CurrentRow
        If dr Is Nothing Then Exit Sub
        Dim lPackageID As Long = dr.Cells(TEXT_PACKAGE_ID).Value
        Dim oFrmChange As FrmPackageChange = New FrmPackageChange(lPackageID)
        oFrmChange.ShowDialog()
        RefreshData()
    End Sub
End Class
