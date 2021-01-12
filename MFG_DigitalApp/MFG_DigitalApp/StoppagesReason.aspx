<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="StoppagesReason.aspx.cs" Inherits="MFG_DigitalApp.StoppagesReason" %>

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

        #myModalAlertComments {
            width: 600px;
            top: 0 !important;
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
                                <asp:Button runat="server" ID="btnProductionEntry" CssClass="btn btn-primary" Text="Productions" OnClick="btnProductionEntry_Click" />
                                <asp:Button runat="server" ID="btnRunDetails" CssClass="btn btn-primary" Text="Run Details" OnClick="btnRunDetails_Click" />
                                <asp:Button runat="server" ID="btnParameter" CssClass="btn btn-primary" Text="Parameter" OnClick="btnParameter_Click" />
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
                                    <div class="col-md-2 col-sm-2 col-xs-2">
                                        <div class="form-group has-feedback">
                                            <label>Date</label>
                                            <asp:TextBox ID="txtDate" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalStartDate" runat="server" Format="yyyy-MM-dd" Enabled="True"
                                                TargetControlID="txtDate">
                                            </asp:CalendarExtender>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="txtDate" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-md-2 col-sm-2 col-xs-2">
                                        <div class="form-group has-feedback">
                                            <label>Shift</label>
                                            <asp:DropDownList ID="drpShift" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="drpShift_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator InitialValue="0" ID="Reqshift" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="drpShift" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-md-2 col-sm-2 col-xs-2">
                                        <div class="form-group has-feedback">
                                            <label>Stoppage Time</label>
                                            <asp:DropDownList ID="drpTime" runat="server" CssClass="form-control select2">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="drpTime" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-md-2 col-sm-2 col-xs-2">
                                        <div class="form-group has-feedback">
                                            <label>Station</label>
                                            <asp:DropDownList ID="drpStation" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="drpStation_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator InitialValue="0" ID="ReqStation" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="drpStation" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-md-2 col-sm-2 col-xs-2">
                                        <div class="form-group has-feedback">
                                            <label>Reason Category</label>
                                            <asp:DropDownList ID="drpReasonCtg" runat="server" CssClass="form-control select2">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator InitialValue="0" ID="RequiredFieldValidator3" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="drpReasonCtg" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-md-2 col-sm-2 col-xs-2">
                                        <div class="form-group has-feedback">
                                            <label>Reason</label>
                                            <asp:DropDownList ID="drpReason" runat="server" CssClass="form-control select2">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator InitialValue="0" ID="ReqReason" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="drpReason" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2 col-sm-2 col-xs-2">
                                        <div class="form-group has-feedback">
                                            <label>Duration</label>
                                            <asp:DropDownList ID="ddlHour" runat="server" CssClass="form-control select2">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator InitialValue="0" ID="ReqHour" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="ddlHour" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-md-2 col-sm-2 col-xs-2">
                                        <div class="form-group has-feedback topmarg">
                                            <asp:DropDownList ID="ddlMinute" runat="server" CssClass="form-control select2">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator InitialValue="0" ID="ReqMinute" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="ddlMinute" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                        <div class="form-group has-feedback">
                                            <label>Activity</label>
                                            <asp:RadioButtonList ID="drpAcitivity" runat="server" RepeatDirection="Horizontal" CssClass="rbtnlbl rbl">
                                                <asp:ListItem>Planned </asp:ListItem>
                                                <asp:ListItem Selected="True">Unplanned</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                        <div class="form-group has-feedback topmarg">
                                            <asp:Button runat="server" ID="btnSubmit" CssClass="btn btn-success" Text="Add" OnClick="btnSubmit_Click" ValidationGroup="g1" />
                                            <asp:Button runat="server" ID="btnComment" CssClass="btn btn-success" Text="Comment" OnClick="btnComment_Click" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:GridView runat="server" ID="GrdStoppageReason" AutoGenerateColumns="false" EmptyDataText="No Records Found!!" DataKeyNames="AutoId"
                                            Width="100%" CssClass="table table-striped table-bordered table-hover" OnRowDeleting="GrdStoppageReason_RowDeleting" ShowHeaderWhenEmpty="true" OnRowDataBound="GrdStoppageReason_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="ShiftName" HeaderText="Shift" />
                                                <asp:BoundField DataField="StoppageTime" HeaderText="Stoppage Time" />
                                                <asp:BoundField DataField="StationDescription" HeaderText="Station" />
                                                <asp:BoundField DataField="ReasonDescription" HeaderText="Reason" />
                                                <asp:BoundField DataField="DownTime" HeaderText="Duration" />
                                                <asp:BoundField DataField="CreatedBy" HeaderText="Reported By" />
                                                <asp:BoundField DataField="EntryTime" HeaderText="Entry Time" />
                                                <asp:BoundField DataField="ReasonDescription" HeaderText="Comments" />
                                                <%-- <asp:TemplateField>
                                          <ItemTemplate>
                                          <asp:LinkButton ID="LinkDelete" runat="server"  OnClick="LinkDelete_Click" OnClientClick="return ConfirmOnDelete('Are you sure to delete this record?');">Delete</asp:LinkButton>
                                          </ItemTemplate>
                                     </asp:TemplateField>--%>
                                                <asp:CommandField ShowDeleteButton="True" ControlStyle-CssClass="btn btn-info" ButtonType="Button" />
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdReasoncode" runat="server" Value='<%#Eval("ReasonCode")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <PagerStyle CssClass="GridPager" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- ModalPopupExtender Comments -->
                        <asp:Button ID="btnPopupComments" runat="server" Style="display: none" />
                        <asp:ModalPopupExtender ID="MPComments" runat="server" TargetControlID="btnPopupComments"
                            PopupControlID="myModalAlertComments" CancelControlID="btnOkComments" BackgroundCssClass="modalBackground">
                        </asp:ModalPopupExtender>
                        <div id="myModalAlertComments">
                            <div class="modal-dialog" style="width: auto;">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h4>
                                            <label>Comments</label></h4>
                                        <asp:Label runat="server" ID="lblCommentsMessage" ForeColor="Red" Font-Bold="true"></asp:Label>
                                    </div>
                                    <div class="modal-body">
                                        <div style="width: 100%;">
                                            <div class="row">
                                                <div class="col-md-12 col-sm-12 col-xs-12">
                                                    <div class="form-group has-feedback">
                                                        <label>Enter Comment</label>
                                                        <asp:TextBox ID="txtComment" CssClass="form-control" TextMode="MultiLine" runat="server" Rows="3" MaxLength="500" ToolTip="Max 500 Char" onkeydown="count();" onkeyup="count();"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-4 col-sm-4 col-xs-4">
                                                    <label id="lblWordCount1"></label>
                                                    <label id="lblKeyInsight1">[Max 500 Char]</label>
                                                </div>
                                                <div class="col-md-8 col-sm-8 col-xs-8">
                                                    <div class="form-group has-feedback" style="float: right;">
                                                        <asp:Button runat="server" ID="btnClearComments" CssClass="btn btn-info" Text="Clear" OnClick="btnClearComments_Click" />
                                                        <asp:Button runat="server" ID="btnAddComments" CssClass="btn btn-primary" Text="Save" OnClick="btnAddComments_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button runat="server" ID="btnOkComments" CssClass="btn btn-default" Text="Close" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- ModalPopupExtender Comments -->
                    </section>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
    <script type="text/javascript">
        function count() {
            var txtInput = document.getElementById('MainContent_txtComment');
            var spanDisplay = document.getElementById('lblWordCount1');
            spanDisplay.innerHTML = txtInput.value.length;
        }
    </script>
</asp:Content>

