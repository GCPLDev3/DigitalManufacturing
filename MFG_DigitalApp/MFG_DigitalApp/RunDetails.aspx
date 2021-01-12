<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="RunDetails.aspx.cs" Inherits="MFG_DigitalApp.RunDetails" %>

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

        #myModalAlertSchedulePO, #myModalAlertStartPO, #myModalAlertStopPO, #myModalAlertAssignOperator {
            width: 600px;
            top: 0 !important;
        }

        #myModalAlertProdDetail {
            width: 700px;
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
                                <asp:Button runat="server" ID="btnStartRun" CssClass="btn btn-success" Text="Start Run" OnClick="btnStartRun_Click" />
                                <asp:Button runat="server" ID="btnAssignOperator" CssClass="btn btn-success" Text="Assign Operator" OnClick="btnAssignOperator_Click" />
                                <asp:Button runat="server" ID="btnSchedule" CssClass="btn btn-success" Text="Schedule PO" OnClick="btnSchedule_Click" />
                                <asp:Button runat="server" ID="btnStop" CssClass="btn btn-danger" Text="Stop Run" OnClick="btnStop_Click" />
                            </div>
                            <div class="col-md-5 col-sm-5 col-xs-5" align="Right">
                                <asp:Button runat="server" ID="btnParameter" CssClass="btn btn-primary" Text="Parameter" OnClick="btnParameter_Click" />
                                <asp:Button runat="server" ID="btnstoppages" CssClass="btn btn-primary" Text="Stoppages" OnClick="btnstoppages_Click" />
                                <asp:Button runat="server" ID="btnProductions" CssClass="btn btn-primary" Text="Productions" OnClick="btnProductions_Click" />
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
                                            Width="100%" CssClass="table table-striped table-bordered table-hover" ShowHeaderWhenEmpty="true" AllowPaging="true" PageSize="15"
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
                            </div>
                            <!-- ModalPopupExtender Start PO -->
                            <asp:Button ID="btnPopupStartPO" runat="server" Style="display: none" />
                            <asp:ModalPopupExtender ID="MPStartPO" runat="server" TargetControlID="btnPopupStartPO"
                                PopupControlID="myModalAlertStartPO" CancelControlID="btnOkStartPO" BackgroundCssClass="modalBackground">
                            </asp:ModalPopupExtender>
                            <div id="myModalAlertStartPO">
                                <div class="modal-dialog" style="width: auto;">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h4>
                                                <label>Start Process Order</label></h4>
                                            <asp:Label runat="server" ID="lblStartMessage" ForeColor="Red" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="modal-body">
                                            <div style="width: 100%;">
                                                <div class="row">
                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                        <label>Process Order</label>
                                                    </div>
                                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                                        <div class="form-group has-feedback">
                                                            <asp:DropDownList ID="drpStartPO" runat="server" CssClass="form-control select2" OnSelectedIndexChanged="drpStartPO_SelectedIndexChanged" AutoPostBack="true">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                        <label>Material</label>
                                                    </div>
                                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                                        <div class="form-group has-feedback">
                                                            <asp:TextBox ID="txtMaterialStartPO" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
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
                                                            <asp:DropDownList ID="drpTimeStartPO" runat="server" CssClass="form-control select2">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                        <label>Gram</label>
                                                    </div>
                                                    <div class="col-md-3 col-sm-3 col-xs-3">
                                                        <div class="form-group has-feedback">
                                                            <asp:TextBox ID="txtGramStart" runat="server" CssClass="form-control" Width="100px" onkeypress="return isNumberKey(event)" onpaste="return false;"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2 col-sm-2 col-xs-2">
                                                        <label>Case Size</label>
                                                    </div>
                                                    <div class="col-md-3 col-sm-3 col-xs-3">
                                                        <div class="form-group has-feedback">
                                                            <asp:TextBox ID="txtCaseSizeStart" runat="server" CssClass="form-control" Width="100px" onkeypress="return isNumberKey(event)" onpaste="return false;"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <asp:Button runat="server" ID="btnStartPO" CssClass="btn btn-primary" Text="Start Run" OnClick="btnStartPO_Click" />
                                            <asp:Button runat="server" ID="btnOkStartPO" CssClass="btn btn-default" Text="Close" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- ModalPopupExtender Start PO -->
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
                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                        <div class="form-group has-feedback">
                                                            <label>Type</label>
                                                            <asp:DropDownList ID="drpType" runat="server" CssClass="form-control select2">
                                                                <asp:ListItem Text="Select Type" Value=""></asp:ListItem>
                                                                <asp:ListItem Text="Operator" Value="Operator"></asp:ListItem>
                                                                <asp:ListItem Text="Shift Supervisor" Value="Shift Supervisor"></asp:ListItem>
                                                                <asp:ListItem Text="QC Supervisor" Value="QC Supervisor"></asp:ListItem>
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
                            <!-- ModalPopupExtender Schedule PO -->
                            <asp:Button ID="btnPopupSchedulePO" runat="server" Style="display: none" />
                            <asp:ModalPopupExtender ID="MPSchedulePO" runat="server" TargetControlID="btnPopupSchedulePO"
                                PopupControlID="myModalAlertSchedulePO" CancelControlID="btnOkSchedulePo" BackgroundCssClass="modalBackground">
                            </asp:ModalPopupExtender>
                            <div id="myModalAlertSchedulePO">
                                <div class="modal-dialog" style="width: auto;">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h4>
                                                <label>Schedule Process Order</label></h4>
                                            <asp:Label runat="server" ID="lblSchMessage" ForeColor="Red" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="modal-body">
                                            <div style="width: 100%;">
                                                <div class="row">
                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                        <label>Process Order</label>
                                                    </div>
                                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                                        <div class="form-group has-feedback">
                                                            <asp:DropDownList ID="drpSchedulePO" runat="server" CssClass="form-control select2"
                                                                OnSelectedIndexChanged="drpSchedulePO_SelectedIndexChanged" AutoPostBack="true">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                        <label>Material</label>
                                                    </div>
                                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                                        <div class="form-group has-feedback">
                                                            <asp:TextBox ID="txtMaterialSchedulePO" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                            <asp:Label ID="lblMaterialDescSchedulePO" runat="server" Visible="false"></asp:Label>
                                                            <asp:Label ID="lblOrderQuantitySchedulePO" runat="server" Visible="false"></asp:Label>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                        <label>Schedule Date & Time</label>
                                                    </div>
                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                        <div class="form-group has-feedback">
                                                            <asp:TextBox ID="txtScheduleStart" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <asp:CalendarExtender ID="calScheduleStart" runat="server" Format="yyyy-MM-dd" Enabled="True"
                                                                TargetControlID="txtScheduleStart">
                                                            </asp:CalendarExtender>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                        <div class="form-group has-feedback">
                                                            <asp:DropDownList ID="drpTimeSchedulePO" runat="server" CssClass="form-control select2">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                        <label>Gram</label>
                                                    </div>
                                                    <div class="col-md-3 col-sm-3 col-xs-3">
                                                        <div class="form-group has-feedback">
                                                            <asp:TextBox ID="txtGramSch" runat="server" CssClass="form-control" Width="100px" onkeypress="return isNumberKey(event)" onpaste="return false;"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-2 col-sm-2 col-xs-2">
                                                        <label>Case Size</label>
                                                    </div>
                                                    <div class="col-md-3 col-sm-3 col-xs-3">
                                                        <div class="form-group has-feedback">
                                                            <asp:TextBox ID="txtCaseSizeSch" runat="server" CssClass="form-control" Width="100px" onkeypress="return isNumberKey(event)" onpaste="return false;"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <asp:Button runat="server" ID="btnSchedulePo" CssClass="btn btn-primary" Text="Schedule PO" OnClick="btnSchedulePo_Click" />
                                            <asp:Button runat="server" ID="btnOkSchedulePo" CssClass="btn btn-default" Text="Close" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- ModalPopupExtender Schedule PO -->
                            <!-- ModalPopupExtender Stop PO -->
                            <asp:Button ID="btnPopupStopPO" runat="server" Style="display: none" />
                            <asp:ModalPopupExtender ID="MPStopPO" runat="server" TargetControlID="btnPopupStopPO"
                                PopupControlID="myModalAlertStopPO" CancelControlID="btnOkStopPO" BackgroundCssClass="modalBackground">
                            </asp:ModalPopupExtender>
                            <div id="myModalAlertStopPO">
                                <div class="modal-dialog" style="width: auto;">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h4>
                                                <label>Stop Process Order</label></h4>
                                            <asp:Label runat="server" ID="lblStopMessage" ForeColor="Red" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="modal-body">
                                            <div style="width: 100%;">
                                                <div class="row">
                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                        <label>Process Order</label>
                                                    </div>
                                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                                        <div class="form-group has-feedback">
                                                            <asp:DropDownList ID="drpStopPO" runat="server" CssClass="form-control select2" OnSelectedIndexChanged="drpStopPO_SelectedIndexChanged" AutoPostBack="true">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                        <label>Material</label>
                                                    </div>
                                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                                        <div class="form-group has-feedback">
                                                            <asp:TextBox ID="txtMaterialStopPO" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
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
                                                            <asp:DropDownList ID="drpTimeStopPO" runat="server" CssClass="form-control select2">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <asp:Button runat="server" ID="btnStopPO" CssClass="btn btn-primary" Text="Stop Run" OnClick="btnStopPO_Click" OnClientClick="return confirm('Are you sure you want Stop the PO?');" />
                                            <asp:Button runat="server" ID="btnOkStopPO" CssClass="btn btn-default" Text="Close" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- ModalPopupExtender Stop PO -->
                            <!-- ModalPopupExtender PO Production Details -->
                            <asp:Button ID="btnPopupProdDetail" runat="server" Style="display: none" />
                            <asp:ModalPopupExtender ID="MPProdDetail" runat="server" TargetControlID="btnPopupProdDetail"
                                PopupControlID="myModalAlertProdDetail" CancelControlID="btnOkProdDetail" BackgroundCssClass="modalBackground">
                            </asp:ModalPopupExtender>
                            <div id="myModalAlertProdDetail">
                                <div class="modal-dialog" style="width: auto;">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h4>
                                                <label>Production Details - </label>
                                                <asp:Label runat="server" ID="lblPoNumber" Font-Bold="true"></asp:Label>
                                            </h4>
                                        </div>
                                        <div class="modal-body table-responsive" style="height: 300px; overflow-y: scroll;">
                                            <div style="width: 100%;">
                                                <div class="row">
                                                    <asp:GridView runat="server" ID="GrdPOProductionDetails" AutoGenerateColumns="false" EmptyDataText="No Records Found!!"
                                                        Width="100%" CssClass="table table-striped table-bordered table-hover" ShowHeaderWhenEmpty="true">
                                                        <Columns>
                                                            <%--<asp:TemplateField HeaderText="S.No">
                                                                <ItemTemplate>
                                                                    <%# Container.DataItemIndex +1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>--%>
                                                            <asp:BoundField DataField="WRPLine" HeaderText="WRP" />
                                                            <asp:BoundField DataField="Date" HeaderText="Date" />
                                                            <asp:BoundField DataField="ShiftCode" HeaderText="Shift" />
                                                            <asp:BoundField DataField="StartTime" HeaderText="Start Time" />
                                                            <asp:BoundField DataField="EndTime" HeaderText="End Time" />
                                                            <asp:BoundField DataField="SensorQty" HeaderText="Sensor Qty" ItemStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField DataField="ActualQty" HeaderText="Actual Qty" ItemStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField DataField="Comments" HeaderText="Comments" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <asp:Button runat="server" ID="btnOkProdDetail" CssClass="btn btn-default" Text="Close" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- ModalPopupExtender PO Production Details -->
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
