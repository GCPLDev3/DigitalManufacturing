<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" EnableEventValidation="false" AutoEventWireup="True" CodeBehind="QualityCheck.aspx.cs" Inherits="MFG_DigitalApp.QualityCheck" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .rbtnlbl {
            margin: 5px 0px;
        }

            .rbtnlbl label {
                padding-left: 5px;
                padding-right: 10px;
            }

        .modalBackground {
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            z-index: 1040;
            background-color: #000;
            opacity: 0.8;
        }

        #myModalAlertActiveSchedule, #myModalAlertStopSchedule, #myModalAlertAssignOperator {
            width: 600px;
            top: 0 !important;
        }

        .btn {
            font-weight: bold !important;
            font-size: 12px !important;
        }
    </style>
    <script>
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="content-wrapper" style="min-height: 500px;">
                <div class="container">
                    <section class="content">
                        <ol class="breadcrumb">
                            <li class="breadcrumb-item">
                                <asp:Label runat="server" ID="lblplantcode" Text=""></asp:Label>
                            </li>
                            <li class="breadcrumb-item">
                                <asp:Label runat="server" ID="lblline" Text=""></asp:Label></li>
                            <li class="breadcrumb-item">
                                <asp:Label runat="server" ID="lblshift" Text=""></asp:Label></li>
                            <li class="breadcrumb-item">
                                <asp:Label runat="server" ID="lbldate" Text=""></asp:Label></li>
                        </ol>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:RadioButtonList ID="drpStatus" runat="server" RepeatDirection="Horizontal" CssClass="rbtnlbl"
                                    OnSelectedIndexChanged="drpStatus_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Selected="True">Active & Scheduled </asp:ListItem>
                                    <asp:ListItem>Completed</asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class="row" runat="server" id="HeaderButtons">
                            <div class="col-md-7 col-sm-7 col-xs-7">
                                <asp:Button runat="server" ID="btnActivateSchedule" CssClass="btn btn-success" Text="Activate Schedule" OnClick="btnActivateSchedule_Click" />
                                <asp:Button runat="server" ID="btnAssignOperator" CssClass="btn btn-success" Text="Assign Operator" OnClick="btnAssignOperator_Click" />
                                <asp:Button runat="server" ID="btnStopSchedule" CssClass="btn btn-danger" Text="Stop Schedule" OnClick="btnStopSchedule_Click" />
                            </div>
                            <div class="col-md-5 col-sm-5 col-xs-5" align="Right">
                                <asp:Button runat="server" ID="btnProcessParameter" CssClass="btn btn-primary" Text="Process" OnClick="btnProcessParameter_Click" />
                                <asp:Button runat="server" ID="btnWeight" CssClass="btn btn-primary" Text="Weight" OnClick="btnWeight_Click" />
                                <asp:Button runat="server" ID="btnInProcessParameter" CssClass="btn btn-primary" Text="In-Process" OnClick="btnInProcessParameter_Click" />
                                <asp:Button runat="server" ID="btnSQC" CssClass="btn btn-primary" Text="SQC" OnClick="btnSQC_Click" />
                            </div>
                        </div>
                        <div class="box box-default" style="margin-top: 7px;">
                            <div class="box-body">
                                <div class="row">
                                    <div class="col-md-12 table-responsive">
                                        <asp:GridView runat="server" ID="GrdActiveAndScheduled" AutoGenerateColumns="false" EmptyDataText="No Records Found!!"
                                            Width="100%" CssClass="table table-striped table-bordered table-hover" ShowHeaderWhenEmpty="true" OnRowDataBound="GrdActiveAndScheduled_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="CurrentStatus" HeaderText="Status" />
                                                <asp:BoundField DataField="PONumber" HeaderText="PO" />
                                                <asp:BoundField DataField="MaterialCode" HeaderText="Material" />
                                                <asp:BoundField DataField="MaterialDescrip" HeaderText="Material Desc" />
                                                <asp:BoundField DataField="ScheduleDateTime" HeaderText="Sch. Date Time" />
                                                <asp:BoundField DataField="StartTime" HeaderText="Start Time" />
                                                <asp:BoundField DataField="OrderQuantity" HeaderText="Order Qty" ItemStyle-HorizontalAlign="Right" />
                                                <asp:BoundField DataField="ProducedQuanity" HeaderText="Prod Qty" ItemStyle-HorizontalAlign="Right" />
                                                <asp:BoundField DataField="ShiftQuantity" HeaderText="Shift Qty" ItemStyle-HorizontalAlign="Right" />
                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:LinkButton Text="Delete" runat="server" CommandArgument='<%#Eval("PONumber") %>' ID="lnkDeletePO" OnClick="lnkDeletePO_Click" OnClientClick="return confirm('Are you sure you want Delete the PO?');" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle CssClass="GridPager" />
                                        </asp:GridView>
                                        <asp:GridView runat="server" ID="GrdCompleted" AutoGenerateColumns="false" EmptyDataText="No Records Found!!"
                                            Width="100%" CssClass="table table-striped table-bordered table-hover" ShowHeaderWhenEmpty="true" AllowPaging="true" PageSize="10"
                                            OnPageIndexChanging="GrdCompleted_PageIndexChanging">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="CurrentStatus" HeaderText="Status" Visible="false" />
                                                <asp:TemplateField HeaderText="PO">
                                                    <ItemTemplate>
                                                        <asp:LinkButton Text='<%# Eval("PONumber") %>' runat="server" Font-Underline="true" CommandArgument="<%# Container.DataItemIndex %>" ID="lnkPONumber" OnClick="lnkPONumber_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="MaterialCode" HeaderText="Material" />
                                                <asp:BoundField DataField="MaterialDescrip" HeaderText="Material Desc" />
                                                <asp:BoundField DataField="StoppageTime" HeaderText="Stoppage Time" />
                                                <asp:BoundField DataField="OrderQuantity" HeaderText="Order Qty" ItemStyle-HorizontalAlign="Right" />
                                                <asp:BoundField DataField="ProducedQuanity" HeaderText="Produced Qty" ItemStyle-HorizontalAlign="Right" />
                                            </Columns>
                                            <PagerStyle CssClass="GridPager" />
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12 table-responsive">
                                        <asp:GridView runat="server" ID="GrdScheduleStatus" AutoGenerateColumns="false" EmptyDataText="No Records Found!!"
                                            Width="100%" CssClass="table table-striped table-bordered table-hover" ShowHeaderWhenEmpty="true" >
                                            <Columns>
                                                <asp:BoundField DataField="Status" HeaderText="Status" />
                                                <asp:BoundField DataField="ScheduleTypeName" HeaderText="Schedule" />
                                                <asp:BoundField DataField="StartTime" HeaderText="Start Time" />
                                                <asp:BoundField DataField="TotalParameter" HeaderText="Total Parameter" />
                                                <asp:BoundField DataField="TestedParameter" HeaderText="Tested Parameter" />
                                                <asp:BoundField DataField="DueForTesting" HeaderText="Due For Testing" />
                                             </Columns>
                                            <PagerStyle CssClass="GridPager" />
                                        </asp:GridView>
                                        <asp:GridView runat="server" ID="GrdCompletedStatus"  DataKeyNames="Id, ScheduleTypeId"  AutoGenerateColumns="false" EmptyDataText="No Records Found!!"
                                            Width="100%" CssClass="table table-striped table-bordered table-hover" ShowHeaderWhenEmpty="true" >
                                            <Columns>
                                               <asp:TemplateField HeaderText="S.No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Schedule Name">
                                                    <ItemTemplate>
                                                        <asp:LinkButton Text='<%# Eval("ScheduleName") %>' runat="server" Font-Underline="true" CommandArgument="<%# Container.DataItemIndex %>" ID="lnkScheduleName" OnClick="lnkScheduleName_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ScheduleTypeName" HeaderText="Schedule Type Name" />
                                                <asp:BoundField DataField="StoppageTime" HeaderText="Stoppage Time" />
                                                <asp:BoundField DataField="TotalParameter" HeaderText="Total Parameter" />
                                                <asp:BoundField DataField="TestedParameter" HeaderText="Tested Parameter" />
                                             </Columns>
                                            <PagerStyle CssClass="GridPager" />
                                        </asp:GridView>
                                        </div>
                                    </div>
                            </div>
                            <!-- ModalPopupExtender Start Schedule -->
                            <asp:Button ID="btnPopupActiveSchedule" runat="server" Style="display: none" />
                            <asp:ModalPopupExtender ID="MPActiveSchedule" runat="server" TargetControlID="btnActivate"
                                PopupControlID="myModalAlertActiveSchedule" CancelControlID="btnOkActiveSchedule" BackgroundCssClass="modalBackground">
                            </asp:ModalPopupExtender>
                            <div id="myModalAlertActiveSchedule">
                                <div class="modal-dialog" style="width: auto;">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h4>
                                                <label>Activate QC Testing Schedule</label></h4>
                                            <asp:Label runat="server" ID="lblStartMessage" ForeColor="Red" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="modal-body">
                                            <div style="width: 100%;">
                                                <div class="row">
                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                        <label>Schedule Type</label>
                                                    </div>
                                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                                        <div class="form-group has-feedback">
                                                            <asp:DropDownList ID="drpScheduleType" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="LoadSchedule">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                        <label>Schedule</label>
                                                    </div>
                                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                                         <div class="form-group has-feedback">
                                                            <asp:DropDownList ID="drpSchedule" runat="server" CssClass="form-control select2" Enabled="false">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                        <label>Start Date & Time</label>
                                                    </div>
                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                        <div class="form-group has-feedback">
                                                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalStartDate" runat="server" Format="yyyy-MM-dd" Enabled="True"
                                                                TargetControlID="txtStartDate">
                                                            </asp:CalendarExtender>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                        <div class="form-group has-feedback">
                                                            <asp:DropDownList ID="drpTimeStartSchedule" runat="server" CssClass="form-control select2">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                        <label>Grammage</label>
                                                    </div>
                                                    <div class="col-md-3 col-sm-3 col-xs-3">
                                                        <div class="form-group has-feedback">
                                                            <asp:TextBox ID="txtGramStart" runat="server" CssClass="form-control" Width="100px" onkeypress="return isNumberKey(event)" onpaste="return false;"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                 </div>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <asp:Button runat="server" ID="btnActivate" CssClass="btn btn-primary" Text="Activate" OnClick="btnActivate_Click" UseSubmitBehavior="false" />
                                            <asp:Button runat="server" ID="btnOkActiveSchedule" CssClass="btn btn-default" Text="Close" OnClick="btnClearSchedule_Click"/>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                           <!-- ModalPopupExtender Assign Operator -->
                            <asp:Button ID="btnPopupAssignOperator" runat="server" Style="display: none" />
                            <asp:ModalPopupExtender ID="MPAssignOperator" runat="server" TargetControlID="btnPopupAssignOperator"
                                PopupControlID="myModalAlertAssignOperator" CancelControlID="btnOkAssignOperator" BackgroundCssClass="modalBackground">
                            </asp:ModalPopupExtender>
                            <div id="myModalAlertAssignOperator">
                                <div class="modal-dialog" style="width: auto;">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h4>
                                                <label>Assign Operators to Line</label></h4>
                                            <asp:Label runat="server" ID="lblOperatorMessage" ForeColor="Red" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="modal-body">
                                            <div style="width: 100%;">
                                                <div class="row">
                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                        <div class="form-group has-feedback">
                                                            <label>Station Name</label>
                                                            <asp:DropDownList ID="drpStation" runat="server" CssClass="form-control select2">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                        <div class="form-group has-feedback">
                                                            <label>Name</label>
                                                            <asp:DropDownList ID="drpOperator" runat="server" CssClass="form-control select2">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12 col-sm-12 col-xs-12">
                                                        <div class="form-group has-feedback" style="float: right;">
                                                            <asp:Button runat="server" ID="btnAddOperator" CssClass="btn btn-primary" Text="Add" OnClick="btnAddOperator_Click" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <asp:GridView runat="server" ID="GrdOperators" AutoGenerateColumns="false" EmptyDataText="No Records Found!!"
                                                            Width="100%" CssClass="table table-striped table-bordered table-hover" ShowHeaderWhenEmpty="true">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="S.No">
                                                                    <ItemTemplate>
                                                                        <%# Container.DataItemIndex +1 %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="Line" HeaderText="Line" />
                                                                <asp:BoundField DataField="StationDescription" HeaderText="Station Name" />
                                                                <asp:BoundField DataField="OperatorName" HeaderText="Name" />
                                                                <asp:BoundField DataField="AOType" HeaderText="Type" />
                                                                <asp:TemplateField HeaderText="">
                                                                    <ItemTemplate>
                                                                        <%--<asp:LinkButton Text="Delete" runat="server" CommandArgument='<%#Eval("ID") %>' ID="lnkDeleteAO" OnClick="lnkDeleteAO_Click" OnClientClick="return confirm('Are you sure you want Delete?');" />--%>
                                                                        <asp:ImageButton runat="server" ID="lnkDeleteAO" ImageUrl="~/images/delete.png" CommandArgument='<%#Eval("ID") %>' OnClick="lnkDeleteAO_Click" OnClientClick="return confirm('Are you sure you want Delete?');" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <asp:Button runat="server" ID="btnOkAssignOperator" CssClass="btn btn-default" Text="Close" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- ModalPopupExtender Assign Operator -->

                        </div>

                        	<asp:Button ID="btnPopupStopSchedule" runat="server" Style="display: none" />
                            <asp:ModalPopupExtender ID="MPStopSchedule" runat="server" TargetControlID="btnMPPopupStopSchedule"
                                PopupControlID="myModalAlertStopSchedule" CancelControlID="btnOkStopSchedule" BackgroundCssClass="modalBackground">
                            </asp:ModalPopupExtender>
                            <div id="myModalAlertStopSchedule">
                                <div class="modal-dialog" style="width: auto;">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h4>
                                                <label>Stop Schedule</label></h4>
                                            <asp:Label runat="server" ID="lblStopMessage" ForeColor="Red" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="modal-body">
                                            <div style="width: 100%;">
                                                <div class="row">
                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                        <label>Schedule Type</label>
                                                    </div>
                                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                                        <div class="form-group has-feedback">
                                                            <asp:DropDownList ID="drpStopScheduleType" runat="server" CssClass="form-control select2" >
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                        <label>Stop Date & Time</label>
                                                    </div>
                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                        <div class="form-group has-feedback">
                                                            <asp:TextBox ID="txtStopDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CaltxtStopDate" runat="server" Format="yyyy-MM-dd" Enabled="True"
                                                                TargetControlID="txtStopDate">
                                                            </asp:CalendarExtender>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                        <div class="form-group has-feedback">
                                                            <asp:DropDownList ID="drpTimeStopSchedule" runat="server" CssClass="form-control select2">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                             <asp:Button runat="server" ID="btnMPPopupStopSchedule" CssClass="btn btn-primary" Text="Stop Schedule" OnClick="btnStopScheduleType_Click" UseSubmitBehavior="false" />
                                            <asp:Button runat="server" ID="btnOkStopSchedule" CssClass="btn btn-default" Text="Close" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- ModalPopupExtender Show Schedule Parameter -->
                            <asp:Button ID="btnPopupCompletedSchedule" runat="server" Style="display: none" />
                            <asp:ModalPopupExtender ID="MPCompletedSchedule" runat="server" TargetControlID="btnCompletedSchedule"
                                PopupControlID="myModalCompletedSchedule" CancelControlID="btnOkCompletedSchedule"  BackgroundCssClass="modalBackground">
                            </asp:ModalPopupExtender>
                           <div id="myModalCompletedSchedule">
                                <div class="modal-dialog" style="width: auto;">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <h4>
                                                        <label>Tested Parameter List </label>
                                                    </h4>
                                                </div>
                                                <div class="modal-body">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <div class="row" runat="server" id="ExportButton">
                                                                <div class="col-md-12" align="Right">
                                                                    <asp:Button runat="server" ID="btnExport" CssClass="btn btn-primary" Text="Export to Excel" OnClick="btnExport_Click" />
                                                                </div>
                                                            </div>
                                                            <div id="dvGrdCompletedScheduleStatus" runat="server">
                                                                 <asp:GridView runat="server" ID="GrdCompletedScheduleStatus" AutoGenerateColumns="false" EmptyDataText="No Records Found!!"
                                                                    Width="100%" CssClass="table table-striped table-bordered table-hover" ShowHeaderWhenEmpty="true" AllowPaging="true" PageSize="10"
                                                                    OnPageIndexChanging="GrdCompletedSchedule_PageIndexChanging">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="Station" HeaderText="Station" />
                                                                        <asp:BoundField DataField="Parameter" HeaderText="Parameter" />
                                                                        <asp:BoundField DataField="DueTime" HeaderText="Due Time" />
                                                                        <asp:BoundField DataField="Tool" HeaderText="Tool Required" />
                                                                        <asp:BoundField DataField="Sample" HeaderText="Sample" />
                                                                        <asp:BoundField DataField="ToleranceLCL" HeaderText="Tolerance LCL" />
                                                                        <asp:BoundField DataField="ToleranceUCL" HeaderText="Tolerance UCL" />
                                                                        <asp:BoundField DataField="UOM" HeaderText="UOM" />
                                                                        <asp:BoundField DataField="Result" HeaderText="Result" />
                                                                        <asp:BoundField DataField="ExpectedResult" HeaderText="Expected Result" />
                                                                        <asp:TemplateField HeaderText="Status" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:Image ID="Image_Status" ImageUrl='<%#Eval("Status")%>' runat="server" Height="15" Width="15" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <PagerStyle CssClass="GridPager" />
                                                                </asp:GridView>
                                                            </div>
                                                            <div id="divGrdSQCCompletedScheduleStatus" runat="server">
                                                                 <asp:GridView runat="server" ID="GrdSQCCompletedScheduleStatus" AutoGenerateColumns="false" EmptyDataText="No Records Found!!"
                                                                    Width="100%" CssClass="table table-striped table-bordered table-hover" ShowHeaderWhenEmpty="true" AllowPaging="true" PageSize="10"
                                                                    OnPageIndexChanging="GrdSQCCompletedScheduleStatus_PageIndexChanging">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="Station" HeaderText="Station" />
                                                                        <asp:BoundField DataField="Parameter" HeaderText="Parameter" />
                                                                        <asp:BoundField DataField="Tool" HeaderText="Tool Required" />
																		<asp:BoundField DataField="DueTime" HeaderText="Due Time" />
                                                                        <asp:BoundField DataField="Sample" HeaderText="Sample" />
                                                                        <asp:BoundField DataField="PalletNo" HeaderText="Pallet No" />
                                                                        <asp:BoundField DataField="NCriticalD" HeaderText="No_Of_Critical_Def" />
                                                                        <asp:BoundField DataField="NMajorD" HeaderText="No_Of_Major_Def" />
																		<asp:BoundField DataField="NMinorD" HeaderText="No_Of_Minor_Def" />
																		<asp:BoundField DataField="Total" HeaderText="Total" />
                                                                        <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                                                                        <asp:TemplateField HeaderText="Status" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:Image ID="Image_Status" ImageUrl='<%#Eval("Status")%>' runat="server" Height="15" Width="15" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <PagerStyle CssClass="GridPager" />
                                                                </asp:GridView>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="modal-footer">
                                                    <asp:Button runat="server" ID="btnCompletedSchedule" Style="visibility: hidden;" />
                                                    <asp:Button runat="server" ID="btnOkCompletedSchedule" CssClass="btn btn-default" Text="Close" />
                                                </div>
                                            </div>
								</div>
                           </div>

                    </section>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="drpStatus" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
