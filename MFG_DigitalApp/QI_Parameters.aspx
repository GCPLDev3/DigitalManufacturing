<%@ Import Namespace="System.Data " %>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="QI_Parameters.aspx.cs" Inherits="MFG_DigitalApp.QI_Parameters" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script>
        function AllowOnlyAlphaNumeric(e) {
            var key = e.keyCode;
            if (!((key == 8) || (key == 32) || (key == 46) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105)) || (event.shiftKey && (event.keyCode >= 48 && event.keyCode <= 57))) {
                e.preventDefault();
            }
        }
    </script>
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

        #myModalAlertStartAddParam {
            width: 600px;
            top: 0 !important;
        }

        .btn {
            font-weight: bold !important;
            font-size: 12px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content-wrapper" style="min-height: 500px;">
        <div class="container">
            <section class="content">

                <div class="row" runat="server" id="HeaderButtons">
                    <div class="col-md-12" align="Right">
                        <asp:Button runat="server" ID="btnCreateParameter" CssClass="btn btn-success" Text="Create Parameter" OnClick="btnCreateParameter_Click" />
                        <asp:Button runat="server" ID="btnExport" CssClass="btn btn-primary" Text="Export to Excel" OnClick="btnExport_Click" />
                    </div>
                </div>
                <div class="box box-default">
                    <div class="box-body">
                        <div class="row" style="margin-bottom: 15px">
                            <div class="col-md-12">
                                <div class="col-md-2 col-sm-2 col-xs-2">
                                    <label runat="server">Parameter name</label>
                                    <asp:TextBox runat="server" ID="txtParameterName" CssClass="form-control" />
                                </div>
                                <div class="col-md-2 col-sm-2 col-xs-2">
                                    <label runat="server">Parameter Type</label>
                                    <asp:DropDownList ID="drpParameterType" Style="width: 100%" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="drpParameterType_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 col-sm-2 col-xs-2">
                                    <label runat="server">Created By</label>
                                    <asp:TextBox runat="server" ID="txtCreatedBy" CssClass="form-control" />
                                </div>
                                <div class="col-md-2 col-sm-2 col-xs-2">
                                    <asp:Button runat="server" ID="BtnShow" CssClass="btn btn-success" Style="margin-top: 26px" Text="Show" OnClick="BtnShow_Click" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:GridView runat="server" ID="GrdParameterList" AutoGenerateColumns="false" DataKeyNames="Id" EmptyDataText="No Records Found!!" OnPageIndexChanging="OnPaging" AllowPaging="true" PageSize="10"
                                    Width="100%" CssClass="table table-striped table-bordered table-hover" OnRowDeleting="GrdParameterList_RowDeleting" OnRowDataBound="GrdParameterList_RowDataBound" ShowHeaderWhenEmpty="true">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Parameter Name">
                                            <ItemTemplate>
                                                <asp:LinkButton Text='<%# Eval("ParameterName") %>' runat="server" CommandArgument="<%# Container.DataItemIndex %>" ID="lnkParameterName" OnClick="lnkParameterName_Click" Font-Bold="true" ForeColor="#3333ff" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ParameterType" HeaderText="Parameter Type" />
                                        <asp:BoundField DataField="ToolsRequired" HeaderText="Tool Required" />
                                        <asp:TemplateField HeaderText="CreatedOn">
                                            <ItemTemplate>
                                                <asp:Label ID="CreatedOn" runat="server"
                                                    Text='<%#Eval("CreatedOn", "{0:dd/MM/yyyy hh:mm:ss}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--                                        <asp:BoundField DataField="CreatedOn" HeaderText="Created On" ItemStyle-HorizontalAlign="Right" />--%>
                                        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" ItemStyle-HorizontalAlign="Right" />
                                        <asp:CommandField ShowDeleteButton="True" HeaderText="Action" ControlStyle-CssClass="btn btn-info" ButtonType="Button" />
                                    </Columns>
                                    <PagerStyle CssClass="GridPager" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- ModalPopupExtender Start PO -->
                <asp:Button ID="btnPopupStartAddParam" runat="server" Style="display: none" />
                <asp:ModalPopupExtender ID="MPStartAddParam" runat="server" TargetControlID="btnPopupStartAddParam"
                    PopupControlID="myModalAlertStartAddParam" CancelControlID="btnCancel" BackgroundCssClass="modalBackground">
                </asp:ModalPopupExtender>
                <div id="myModalAlertStartAddParam">
                    <div class="modal-dialog" style="width: auto;">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h4>
                                    <label>Create Parameter</label></h4>
                                <asp:Label runat="server" ID="lblStartMessageAddParam" ForeColor="Red" Font-Bold="true"></asp:Label>
                            </div>
                            <div class="modal-body">
                                <div style="width: 100%;">

                                    <div class="row">
                                        <div class="col-md-4 col-sm-4 col-xs-4">
                                            <label>Parameter Name</label>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8">
                                            <div class="form-group has-feedback">
                                                <asp:HiddenField ID="hdnParameterId" runat="server" />
                                                <asp:TextBox ID="txtPName" runat="server" CssClass="form-control" MaxLength="40"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="ParameterNameRequired" runat="server" ControlToValidate="txtPName"
                                                    CssClass="failureNotification" ErrorMessage="Parameter Name is required." ToolTip="Parameter Name is required." ValidationGroup="AddParameterValidationGroup" ForeColor="Red">* Enter the Parameter Name</asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4 col-sm-4 col-xs-4">
                                            <label>Parameter Type</label>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8">
                                            <div class="form-group has-feedback">
                                                <asp:DropDownList ID="drpPType" runat="server" CssClass="form-control" AutoPostBack="false">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="drpPTypeValidator" CssClass="failureNotification" ValidationGroup="AddParameterValidationGroup" runat="server" ControlToValidate="drpPType" ToolTip="Please select parameter type" ErrorMessage="Please select parameter type" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-4 col-sm-4 col-xs-4">
                                            <label>Tool Required</label>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8">
                                            <div class="form-group has-feedback">
                                                <asp:TextBox ID="txtTool" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="txtToolValidator" runat="server" ControlToValidate="txtTool"
                                                    CssClass="failureNotification" ErrorMessage="Tool Details is required." ToolTip="Tool Details is required." ValidationGroup="AddParameterValidationGroup" ForeColor="Red">* Enter the Tool Required</asp:RequiredFieldValidator>
                                            </div>
                                        </div>

                                    </div>


                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button runat="server" ID="btnSaveParam" CssClass="btn btn-primary" Text="Save" ValidationGroup="AddParameterValidationGroup" OnClick="btnSaveCreateParameter_Click" />
                                <asp:Button runat="server" ID="btnCancel" CssClass="btn btn-default" Text="Close" />
                            </div>
                        </div>
                    </div>
                </div>

            </section>
        </div>
    </div>
</asp:Content>
