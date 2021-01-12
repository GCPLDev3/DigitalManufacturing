<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="UploadOperator.aspx.cs" Inherits="MFG_DigitalApp.UploadOperator" EnableEventValidation="false" %>

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
            width: 900px;
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

        .Addbtncss1 {
            margin-top: 23px;
        }
    </style>
    <script>
        function checkAll(GVAssignedOperator, colIndex) {
            var GridView = GVAssignedOperator.parentNode.parentNode.parentNode;
            for (var i = 1; i < GridView.rows.length; i++) {
                var chb = GridView.rows[i].cells[colIndex].getElementsByTagName("input")[0];
                chb.checked = GVAssignedOperator.checked;
            }
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
        function checkItem_All(objRef, colIndex) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var selectAll = GridView.rows[0].cells[colIndex].getElementsByTagName("input")[0];
            if (!objRef.checked) {
                selectAll.checked = false;
            }
            else {
                var checked = true;
                for (var i = 1; i < GridView.rows.length; i++) {
                    var chb = GridView.rows[i].cells[colIndex].getElementsByTagName("input")[0];
                    if (!chb.checked) {
                        checked = false;
                        break;
                    }
                }
                selectAll.checked = checked;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content-wrapper" style="min-height: 500px;">
        <div class="container">

            <section class="content">

                <div class="box box-default">
                    <div class="box-body">
                        <div class="row">
                            <div class="col-md-2 col-sm-4 col-xs-4">
                                <div class="form-group has-feedback">
                                    <label>Download Excel Format</label>

                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-4">
                                <div class="form-group has-feedback">
                                    <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-2 col-sm-4 col-xs-4">
                                <div class="form-group has-feedback">
                                    <label>Upload Excel File</label>


                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-4">
                                <div class="form-group has-feedback">
                                    <asp:FileUpload ID="FileUpload1" runat="server" Width="500px" />
                                    <asp:RequiredFieldValidator ID="rfvFileUpload" runat="server" ControlToValidate="FileUpload1"
                                        CssClass="failureNotification" ErrorMessage="Please Select Excel File" Display="Dynamic"
                                        ValidationGroup="grvUploadNormValidationSummary" ForeColor="Red" />
                                    <asp:RegularExpressionValidator ID="revFileUpload" runat="server" ControlToValidate="FileUpload1"
                                        CssClass="failureNotification" ErrorMessage="Please Select .xls OR .xlsx File"
                                        Display="Dynamic" ValidationExpression="^.*\.(xlsx|XLSX|xls|XLS)$" ValidationGroup="grvUploadNormValidationSummary" ForeColor="Red" />
                                    <br />
                                    <asp:Button ID="btnUpload" runat="server" Text="Upload" Width="100px" OnClick="btnUpload_Click" CssClass="btn btn-primary"
                                        ValidationGroup="grvUploadNormValidationSummary" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:GridView runat="server" ID="GrdOperators" AutoGenerateColumns="false"
                                Width="100%" CssClass="table table-striped table-bordered table-hover" ShowHeaderWhenEmpty="true">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex +1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="PlantCode" HeaderText="Plant Code" />
                                    <asp:BoundField DataField="Line" HeaderText="Line" />
                                    <asp:BoundField DataField="StationCode" HeaderText="Station Code" />
                                    <asp:BoundField DataField="EDP" HeaderText="EDP No." />
                                    <asp:BoundField DataField="Name" HeaderText="Name" />
                                    <asp:BoundField DataField="Type" HeaderText="Type" />
                                    <asp:BoundField DataField="ShiftCode" HeaderText="Shift Code" />
                                    <asp:BoundField DataField="Date" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" />
                                </Columns>
                            </asp:GridView>



                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-2 col-sm-4 col-xs-4">
                            <div class="form-group has-feedback">
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4 col-xs-4">
                            <div class="form-group has-feedback">
                                <asp:Button ID="btnSave" runat="server" Text="Save" Width="120px"
                                    OnClick="btnSave_Click" CssClass="btn btn-primary" Visible="false" />
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-2 col-sm-4 col-xs-4">
                            <div class="form-group has-feedback">
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4 col-xs-4">
                            <div class="form-group has-feedback">
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-2 col-sm-4 col-xs-4">
                            <div class="form-group has-feedback">
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4 col-xs-4">
                            <div class="form-group has-feedback">
                                <asp:Label runat="server" ID="lblOPeratorName" Text="Operator Details" Style="color: #BA075F; font-size: 18px" Font-Bold="true"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-2 col-sm-4 col-xs-4">
                            <div class="form-group has-feedback">
                                <label>Plant Code</label>
                                <asp:DropDownList ID="drpPlantCode" runat="server" CssClass="form-control select2"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-2 col-sm-4 col-xs-4">
                            <div class="form-group has-feedback">
                                <label>Line</label>
                                <asp:DropDownList ID="drpLine" runat="server" CssClass="form-control select2"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-2 col-sm-4 col-xs-4">
                            <div class="form-group has-feedback">
                                <label>Station Code</label>
                                <asp:DropDownList ID="drpStationCode" runat="server" CssClass="form-control select2"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-2 col-sm-4 col-xs-4">
                            <div class="form-group has-feedback">
                                <label>EDP Number</label>
                                <asp:TextBox ID="txtedp1" runat="server" CssClass="form-control"> </asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2 col-sm-4 col-xs-4">
                            <div class="form-group has-feedback">
                                <label>Operator Name</label>
                                <asp:TextBox ID="txtOperator" runat="server" CssClass="form-control"> </asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2 col-sm-4 col-xs-4">
                            <div class="form-group has-feedback">
                                <label>Type</label>
                                <asp:DropDownList ID="drpAOType" runat="server" CssClass="form-control select2">
                                    <asp:ListItem Text="Select Type" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Operator" Value="Operator"></asp:ListItem>
                                    <asp:ListItem Text="Shift Supervisor" Value="Shift Supervisor"></asp:ListItem>
                                    <asp:ListItem Text="QC Supervisor" Value="QC Supervisor"></asp:ListItem>
                                    <asp:ListItem Text="Line Technician" Value="Line Technician"></asp:ListItem>
                                    <asp:ListItem Text="Electrician" Value="Electrician"></asp:ListItem>
                                    <asp:ListItem Text="Utility" Value="Utility"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-2 col-sm-4 col-xs-4">
                            <div class="form-group has-feedback">
                                <label>Shift Code</label>
                                <asp:DropDownList ID="drpShift" runat="server" CssClass="form-control select2">
                                    <asp:ListItem Text="Select Shift" Value=""></asp:ListItem>
                                    <asp:ListItem Text="S1" Value="S1"></asp:ListItem>
                                    <asp:ListItem Text="S2" Value="S2"></asp:ListItem>
                                    <asp:ListItem Text="S3" Value="S3"></asp:ListItem>
                                    <asp:ListItem Text="HO" Value="HO"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-2 col-sm-4 col-xs-4">
                            <div class="form-group has-feedback">
                                <label>Shift From Date </label>
                                <asp:TextBox ID="txtShiftFromDate" runat="server" CssClass="form-control"> </asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="yyyy-MM-dd" Enabled="True"
                                    TargetControlID="txtShiftFromDate">
                                </asp:CalendarExtender>
                            </div>
                        </div>
                        <div class="col-md-2 col-sm-4 col-xs-4">
                            <div class="form-group has-feedback">
                                <label>Shift To Date</label>
                                <asp:TextBox ID="txtShiftToDate" runat="server" CssClass="form-control"> </asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="yyyy-MM-dd" Enabled="True"
                                    TargetControlID="txtShiftToDate">
                                </asp:CalendarExtender>
                            </div>
                        </div>
                        <div class="col-md-2 col-sm-4 col-xs-4 hide">
                            <div class="form-group has-feedback">
                                <label>Created From Date</label>
                                <asp:TextBox ID="txtCreatedFromDate" runat="server" CssClass="form-control"> </asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="yyyy-MM-dd" Enabled="True"
                                    TargetControlID="txtCreatedFromDate">
                                </asp:CalendarExtender>
                            </div>
                        </div>
                        <div class="col-md-2 col-sm-4 col-xs-4 hide">
                            <div class="form-group has-feedback">
                                <label>Created To Date</label>
                                <asp:TextBox ID="txtCreatedToDate" runat="server" CssClass="form-control"> </asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender5" runat="server" Format="yyyy-MM-dd" Enabled="True"
                                    TargetControlID="txtCreatedToDate">
                                </asp:CalendarExtender>
                            </div>
                        </div>
                        <div class="col-md-2 col-sm-4 col-xs-4 hide">
                            <div class="form-group has-feedback">
                                <label>Created By</label>
                                <asp:TextBox ID="CreatedBy" runat="server" CssClass="form-control"> </asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2 col-sm-4 col-xs-4">
                            <div class="form-group has-feedback">
                                <label></label>
                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" CssClass="btn btn-primary Addbtncss1" />
                                <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" CssClass="btn btn-primary Addbtncss1" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-10 col-sm-4 col-xs-4">
                            <div class="form-group has-feedback">
                            </div>
                        </div>
                        <div class="col-md-2 col-sm-4 col-xs-4">
                            <div class="form-group has-feedback">
                                <asp:Button ID="btnExporttoExcel" runat="server" Text="Export To Excel" CssClass="btn btn-primary" OnClick="btnExporttoExcel_Click" Style="text-align: left" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
<asp:Button ID="btnDeleteRecords" Text="Delete Records" OnClick="btnDeleteRecords_Click" OnClientClick="return confirm('Are you sure you want to delete this record(s)?');" CssClass="btn btn-primary"
                        runat="server" />
                            <asp:GridView runat="server" ID="GVAssignedOperator" AutoGenerateColumns="false" AllowPaging="true" PageSize="10" DataKeyNames="ID"
                                Width="100%" CssClass="table table-striped table-bordered table-hover" ShowHeaderWhenEmpty="true" ClientIDMode="Static"
                                OnRowDataBound="GVAssignedOperator_RowDataBound" OnRowCommand="GVAssignedOperator_RowCommand" OnPageIndexChanging="GVAssignedOperator_PageIndexChanging">
                                <Columns>
                                      <asp:TemplateField HeaderText="">  
                                             <HeaderTemplate>
                                                <asp:CheckBox ID="chkAll" runat="server" onclick="checkAll(this,0);" />
                                                </HeaderTemplate>
                                <ItemTemplate>  
                                    <asp:CheckBox ID="chkSelect" runat="server" onclick="checkItem_All(this,0)" />  
                                </ItemTemplate>  
                            </asp:TemplateField>  
                                   <%-- <asp:TemplateField HeaderText="S.No" HeaderStyle-Width="20px">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex +1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:BoundField DataField="PlantCode" HeaderText="Plant Code" />
                                    <asp:BoundField DataField="Line" HeaderText="Line" />
                                    <asp:BoundField DataField="StationCode" HeaderText="Station Code" />
                                    <asp:BoundField DataField="EDP" HeaderText="EDP No" />
                                    <asp:BoundField DataField="AssignedOperator" HeaderText="Name" />
                                    <asp:BoundField DataField="AOType" HeaderText="Type" />
                                    <asp:BoundField DataField="Shift" HeaderText="Shift Code" />
                                    <asp:BoundField DataField="Date" HeaderText="Shift Date" DataFormatString="{0:dd-MM-yyyy}" />
                                    <asp:BoundField DataField="CreatedOn" HeaderText="Created On" DataFormatString="{0:dd-MM-yyyy}" />
                                    <asp:BoundField DataField="CreatedBy" HeaderText="Created By" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button runat="server" ID="btnEdit" Text="Edit" CssClass="btn btn-primary" CommandName="EditOperator" CommandArgument='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button runat="server" ID="btnDelete" Text="Delete" CssClass="btn btn-primary" OnClientClick="return confirm('Are you sure you want to delete this record?');" CommandName="DeleteOperator" CommandArgument='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle CssClass="GridPager" />
                            </asp:GridView>



                        </div>
                    </div>

                    <!-- ModalPopupExtender Assign Operator -->
                    <asp:Button ID="btnPopupAssignOperator" runat="server" Style="display: none" />
                    <asp:HiddenField ID="hdID" runat="server" />
                    <asp:ModalPopupExtender ID="MPAssignOperator" runat="server" TargetControlID="btnPopupAssignOperator"
                        PopupControlID="myModalAlertAssignOperator" CancelControlID="btnOkAssignOperator" BackgroundCssClass="modalBackground">
                    </asp:ModalPopupExtender>
                    <div id="myModalAlertAssignOperator">
                        <div class="modal-dialog" style="width: auto;">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4>
                                        <label>Edit Operator</label></h4>
                                    <asp:Label runat="server" ID="lblOperatorMessage" ForeColor="Red" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="modal-body">
                                    <div style="width: 100%;">
                                        <div class="row">
                                            <div class="col-md-4 col-sm-4 col-xs-4">
                                                <div class="form-group has-feedback">
                                                    <label>Plant Code</label>
                                                  <asp:DropDownList ID="ddlPlantCode" runat="server" CssClass="form-control ">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator InitialValue="0" ID="ReqStation" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="ddlPlantCode" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-4 col-xs-4">
                                                <div class="form-group has-feedback">
                                                    <label>Line</label>
                                                   <asp:DropDownList ID="ddlLine" runat="server" CssClass="form-control ">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator InitialValue="0" ID="RequiredFieldValidator5" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="ddlLine" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-4 col-xs-4">
                                                <div class="form-group has-feedback">
                                                    <label>Station Code</label>
                                                   <asp:DropDownList ID="ddlStationCode" runat="server" CssClass="form-control ">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator InitialValue="0" ID="RequiredFieldValidator6" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="ddlStationCode" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-4 col-xs-4">
                                                <div class="form-group has-feedback">
                                                    <label>EDP</label>
                                                    <asp:TextBox ID="txtEDP" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="txtEDP" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>

                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-4 col-xs-4">
                                                <div class="form-group has-feedback">
                                                    <label>Name</label>
                                                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="txtName" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>

                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-4 col-xs-4">
                                                <div class="form-group has-feedback">
                                                    <label>Type</label>
                                                    <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control ">
                                                        <asp:ListItem Text="Select Type" Value=""></asp:ListItem>
                                                        <asp:ListItem Text="Operator" Value="Operator"></asp:ListItem>
                                                        <asp:ListItem Text="Shift Supervisor" Value="Shift Supervisor"></asp:ListItem>
                                                        <asp:ListItem Text="QC Supervisor" Value="QC Supervisor"></asp:ListItem>
                                                    </asp:DropDownList>
                                                      <asp:RequiredFieldValidator InitialValue="0" ID="RequiredFieldValidator8" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="ddlType" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-4 col-xs-4">
                                                <div class="form-group has-feedback">
                                                    <label>Shift Code</label>
                                                    <asp:DropDownList ID="ddlShiftCode" runat="server" CssClass="form-control ">
                                                        <asp:ListItem Text="Select Shift" Value=""></asp:ListItem>
                                                        <asp:ListItem Text="S1" Value="S1"></asp:ListItem>
                                                        <asp:ListItem Text="S2" Value="S2"></asp:ListItem>
                                                        <asp:ListItem Text="S3" Value="S3"></asp:ListItem>
                                                    </asp:DropDownList>
                                                     <asp:RequiredFieldValidator InitialValue="0" ID="RequiredFieldValidator7" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="ddlShiftCode" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-4 col-xs-4">
                                                <div class="form-group has-feedback">
                                                    <label>Date</label>
                                                    <asp:TextBox ID="txtDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CaltxtStopDate" runat="server" Format="yyyy-MM-dd" Enabled="True"
                                                        TargetControlID="txtDate">
                                                    </asp:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="txtDate" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-4 col-xs-4 hide">
                                                <div class="form-group has-feedback">
                                                    <label>Created On</label>
                                                    <asp:TextBox ID="txtCreatedOn" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="yyyy-MM-dd" Enabled="True"
                                                        TargetControlID="txtCreatedOn">
                                                    </asp:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="txtCreatedOn" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-4 col-xs-4 hide">
                                                <div class="form-group has-feedback">
                                                    <label>Created By</label>
                                                    <asp:TextBox ID="txtCreatedBy" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="txtCreatedBy" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                <div class="form-group has-feedback" style="float: right;">
                                                    <asp:Button runat="server" ID="btnUpdateOperator" CssClass="btn btn-primary" Text="Update" OnClick="btnUpdateOperator_Click" ValidationGroup="g1" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
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

            </section>
        </div>
    </div>
</asp:Content>
