<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ParameterDetails.aspx.cs" Inherits="MFG_DigitalApp.ParameterDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .rbl input[type="radio"] {
            margin-left: 15px;
            margin-right: 1px;
        }

        .topmarg {
            margin-top: 25px;
        }

        .btn {
            font-weight: bold !important;
            font-size: 12px !important;
        }

        .col-xs-1 {
            width: 5.33333333%;
        }
    </style>
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
                                <asp:Label runat="server" ID="lblline" Text=""></asp:Label>
                            </li>
                            <li class="breadcrumb-item">
                                <asp:Label runat="server" ID="lblshift" Text=""></asp:Label></li>
                            <li class="breadcrumb-item">
                                <asp:Label runat="server" ID="lbldate" Text=""></asp:Label></li>
                        </ol>
                        <div class="row" style="margin-top: 5px;">
                            <div class="col-md-5 col-sm-5 col-xs-5">
                                <asp:Button runat="server" ID="btnstoppages" CssClass="btn btn-primary" Text="Stoppages" OnClick="btnstoppages_Click" />
                                <asp:Button runat="server" ID="btnProductionEntry" CssClass="btn btn-primary" Text="Productions" OnClick="btnProductionEntry_Click" />
                                <asp:Button runat="server" ID="btnRunDetails" CssClass="btn btn-primary" Text="Run Details" OnClick="btnRunDetails_Click" />
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-4" style="padding-left: 0px; margin-left: -20px;">
                                <asp:DropDownList ID="drpShiftHeader" runat="server" AutoPostBack="true" CssClass="form-control select2" OnSelectedIndexChanged="drpShiftHeader_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3 col-sm-3 col-xs-3">
                            </div>
                        </div>
                        <div class="box box-default" style="margin-top: 7px;">
                            <div class="box-body">
                                <div class="row">
                                    <div class="col-sm-1">
                                        <div class="form-group has-feedback">
                                            <label>Date</label>
                                            <asp:TextBox ID="txtDate" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalStartDate" runat="server" Format="yyyy-MM-dd" Enabled="True"
                                                TargetControlID="txtDate">
                                            </asp:CalendarExtender>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="txtDate" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-sm-1">
                                        <div class="form-group has-feedback">
                                            <label>Shift</label>
                                            <asp:DropDownList ID="drpShift" runat="server" CssClass="form-control select2">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator InitialValue="0" ID="Reqshift" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="drpShift" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-sm-1">
                                        <div class="form-group has-feedback">
                                            <label>P.Time</label>
                                            <asp:DropDownList ID="ddlHour" runat="server" CssClass="form-control select2">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator InitialValue="" ID="ReqHour" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="ddlHour" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-sm-1">
                                        <div class="form-group has-feedback topmarg">
                                            <asp:DropDownList ID="ddlMinute" runat="server" CssClass="form-control select2">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator InitialValue="" ID="ReqMinute" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="ddlMinute" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-sm-2">
                                        <div class="form-group has-feedback">
                                            <label>Station</label>
                                            <asp:DropDownList ID="ddlStation" runat="server" CssClass="form-control select2" OnSelectedIndexChanged="ddlStation_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <%--<asp:RequiredFieldValidator InitialValue="" ID="RequiredFieldValidator6" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="ddlMinute" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                        </div>
                                    </div>
                                    <div class="col-sm-2">
                                        <div class="form-group has-feedback">
                                            <label>P. Type</label>
                                            <asp:DropDownList ID="drpParameterType" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="drpParameterType_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator InitialValue="" ID="RequiredFieldValidator5" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="drpParameterType" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-sm-2">
                                        <div class="form-group has-feedback">
                                            <label>Parameter</label>
                                            <asp:DropDownList ID="drpParameter" runat="server" CssClass="form-control select2">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator InitialValue="" ID="RequiredFieldValidator2" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="drpParameter" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-xs-1">
                                        <div class="form-group has-feedback">
                                            <label>A. Value</label>
                                            <asp:TextBox ID="txtValue" runat="server" CssClass="form-control" MaxLength="8" onkeypress="return validateFloatKeyPress(this,event);"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="txtValue" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-xs-1">
                                        <div class="form-group has-feedback">
                                            <label>UOM</label>
                                            <asp:TextBox ID="txtUOM" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="txtUOM" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-xs-1">
                                        <div class="form-group has-feedback">
                                            <label>Targ</label>
                                            <asp:TextBox ID="txtTarg" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="txtTarg" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                        <div class="form-group has-feedback">
                                            <label>Comments</label>
                                            <asp:TextBox ID="txtComments" runat="server" CssClass="form-control" ToolTip="Max 100 Char" MaxLength="100" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-2 col-sm-2 col-xs-2">
                                        <div class="form-group has-feedback topmarg">
                                            <asp:Button runat="server" ID="btnSubmit" CssClass="btn btn-success" Text="Add" ValidationGroup="g1" OnClick="btnSubmit_Click" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <label>Production Parameters :</label>
                                        <asp:GridView runat="server" ID="GrdProduction" AutoGenerateColumns="false" EmptyDataText="No Records Found!!" DataKeyNames="AutoId"
                                            Width="100%" CssClass="table table-striped table-bordered table-hover" ShowHeaderWhenEmpty="true">
                                            <Columns>
                                                <asp:BoundField HeaderText="Station Name" DataField="StationDescription" />
                                                <asp:BoundField DataField="ParameterTime" HeaderText="Parameter Time" />
                                                <asp:BoundField DataField="ParameterDesc" HeaderText="Parameter" />
                                                <asp:BoundField DataField="Value" HeaderText="Actual Value" />
                                                <asp:BoundField DataField="UOM" HeaderText="UOM" />
                                                <asp:BoundField DataField="Comments" HeaderText="Comments" />
                                                <%--    <asp:BoundField DataField="CreatedOn" HeaderText="CreatedOn" />--%>
                                                <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" />
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkPrdDelete" runat="server" OnClick="LinkPrdDelete_Click" CommandArgument='<%# Eval("AutoId") %>' OnClientClick="return confirm('Are you sure you want Delete?');">Delete</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle CssClass="GridPager" />
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <label>Quality  Parameters :</label>
                                        <asp:GridView runat="server" ID="GrdQuality" AutoGenerateColumns="false" EmptyDataText="No Records Found!!" DataKeyNames="AutoId"
                                            Width="100%" CssClass="table table-striped table-bordered table-hover" ShowHeaderWhenEmpty="true">
                                            <Columns>
                                                <asp:BoundField HeaderText="Station Name" DataField="StationDescription" />
                                                <asp:BoundField DataField="ParameterTime" HeaderText="Parameter Time" />
                                                <asp:BoundField DataField="ParameterDesc" HeaderText="Parameter" />
                                                <asp:BoundField DataField="Value" HeaderText="Actual Value" />
                                                <asp:BoundField DataField="UOM" HeaderText="UOM" />
                                                <asp:BoundField DataField="Comments" HeaderText="Comments" />
                                                <%-- <asp:BoundField DataField="CreatedOn" HeaderText="CreatedOn" />--%>
                                                <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" />
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkQlyDelete" runat="server" CommandArgument='<%# Eval("AutoId") %>' OnClick="LinkQlyDelete_Click" OnClientClick="return confirm('Are you sure you want Delete?');">Delete</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle CssClass="GridPager" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </section>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="drpShiftHeader" />
        </Triggers>
    </asp:UpdatePanel>
    <script type="text/javascript">
        function validateFloatKeyPress(el, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            var number = el.value.split('.');
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            //just one dot 
            if (number.length > 1 && charCode == 46) {
                return false;
            }
            //get the carat position
            var caratPos = getSelectionStart(el);
            var dotPos = el.value.indexOf(".");
            if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
                return false;
            }
            return true;
        }

    </script>
</asp:Content>
