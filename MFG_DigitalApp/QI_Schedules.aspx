<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="QI_Schedules.aspx.cs" Inherits="MFG_DigitalApp.QI_Schedules" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" language="javascript"> functionCheckall(Checkbox) { var GridView1 = document.getElementById("<%=GrdScheduleList.ClientID %>"); 
            for (i = 1; i < GrdScheduleList.rows.length; i++) { GrdScheduleList.rows[i].cells[3].getElementsByTagName( "INPUT")[0].checked=Checkbox.checked; } } 
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
            ,.
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--  <asp:ScriptManager runat="server" />--%>
    <asp:UpdatePanel ID="UpdatePanle1" runat="server">
        <ContentTemplate>
            <div class="content-wrapper" style="min-height: 500px;">
                <div class="container">
                    <section class="content">
                        <div class="row" runat="server" id="HeaderButtons">
                            <div class="col-md-12" align="Right">
                                <asp:Button runat="server" ID="btnCreateSchedule" CssClass="btn btn-success" Text="Create Schedule" OnClick="btnCreateSchedule_Click" />
                                <asp:Button runat="server" ID="btnExport" CssClass="btn btn-primary" Text="Export to Excel" OnClick="btnExport_Click" />
                            </div>
                        </div>
                        <div class="box box-default">
                            <div class="box-body">
                                <div class="row" runat="server" id="completedFilter" style="margin-bottom: 15px">
                                    <div class="col-md-12">
                                        <div class="col-md-2 col-sm-2 col-xs-2">
                                            <label runat="server">Plant Code</label>
                                            <asp:DropDownList ID="drpPlant" Style="width: 100%" runat="server" CssClass="form-control select2" AutoPostBack="True" Width="100%" OnSelectedIndexChanged="drpPlant_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2 col-sm-2 col-xs-2">
                                            <label runat="server">Schedule Type</label>
                                            <asp:DropDownList ID="drpScheduleTypeId" Style="width: 100%" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="drpScheduleTypeId_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2 col-sm-2 col-xs-2">
                                            <label runat="server">Created By</label>
                                            <asp:TextBox runat="server" ID="txtCreatedBy" CssClass="form-control" />
                                        </div>
                                        <div class="col-md-2 col-sm-2 col-xs-2">
                                            <label>
                                            </label>
                                            <asp:Button runat="server" ID="BtnShow" CssClass="btn btn-success" Style="margin-top: 26px" Text="Show" OnClick="BtnShow_Click" />
                                        </div>
                                        <div class="col-md-2 col-sm-2 col-xs-2" align="Right">
                                            <label>
                                            </label>
                                            <asp:Button runat="server" ID="DeleteButton" CssClass="btn btn-success" Style="margin-top: 26px; margin-right: -203px;" Text="Delete Schedule" OnClick="DeleteButton_Click" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:GridView runat="server" ID="GrdScheduleList" AutoGenerateColumns="false" OnRowDeleting="GrdScheduleList_RowDeleting" OnRowDataBound="GrdScheduleList_RowDataBound" DataKeyNames="Id" EmptyDataText="No Records Found!!"
                                            Width="100%" CssClass="table table-striped table-bordered table-hover" ShowHeaderWhenEmpty="true" OnPageIndexChanging="OnPaging" AllowPaging="true" PageSize="10">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="CheckAll" AutoPostBack="true" OnCheckedChanged="chckchanged" runat="server" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="Select" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--  <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:RadioButton  runat="server" ID="rbCreate" AutoPostBack="true" OnCheckedChanged ="OnSelectedRecord"/>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="ScheduleNumber">
                                                    <ItemTemplate>
                                                        <asp:LinkButton Text='<%# Eval("ScheduleNumber") %>' runat="server" CommandArgument="<%# Container.DataItemIndex %>" ID="lnkScheduleNumber" OnClick="lnkScheduleNumber_Click" Font-Bold="true" ForeColor="#3333ff" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ScheduleDescription" HeaderText="ScheduleDescription" />
                                                <asp:BoundField DataField="ScheduleType" HeaderText="ScheduleType" />
                                                <asp:BoundField DataField="Plant" HeaderText="Plant" />
                                                <asp:BoundField DataField="Brand" HeaderText="Brand" />
                                                <asp:BoundField DataField="Variant" HeaderText="Variant" />
                                                <asp:BoundField DataField="Grammage" HeaderText="Grammage" />
                                                <asp:TemplateField HeaderText="CreatedOn">
                                                    <ItemTemplate>
                                                        <asp:Label ID="CreatedOn" runat="server"
                                                            Text='<%#Eval("CreatedOn", "{0:dd/MM/yyyy hh:mm:ss}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--                                        <asp:BoundField DataField="CreatedOn" HeaderText="Created On" ItemStyle-HorizontalAlign="Right" />--%>
                                                <asp:BoundField DataField="CreatedBy" HeaderText="Created By" ItemStyle-HorizontalAlign="Right" />
                                                <%--  <asp:CommandField ShowDeleteButton="True" HeaderText="Action" ControlStyle-CssClass="btn btn-info" ButtonType="Button" />--%>
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
    </asp:UpdatePanel>
</asp:Content>
