<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ShiftDetails.aspx.cs" Inherits="MFG_DigitalApp.ShiftDetails" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content-wrapper" style="min-height: 500px;">
        <div class="container">
            <asp:HiddenField ID="hdnfldVariable" runat="server" Value="" />
            <section class="content">

                <div class="box box-default">
                    <div class="box-body">
                        <div class="row">
                            <div class="col-md-4 col-sm-4 col-xs-4">
                                <div class="form-group has-feedback">
                                    <label>Shift</label>
                                    <asp:DropDownList ID="drpShift" runat="server" AutoPostBack="true" CssClass="form-control select2" OnSelectedIndexChanged="drpShift_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator InitialValue="0" ID="Reqshift" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="drpShift" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-4">
                                <div class="form-group has-feedback">
                                    <label>Plant</label>
                                    <asp:DropDownList ID="drpPlant" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="drpPlant_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:RequiredFieldValidator InitialValue="0" ID="ReqStation" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="drpPlant" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:GridView runat="server" ID="GrdStoppageReason" AutoGenerateColumns="false" EmptyDataText="No Records Found!!"
                                    Width="100%" CssClass="table table-striped table-bordered table-hover" ShowHeaderWhenEmpty="true" OnRowDataBound="GrdStoppageReason_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Line">
                                            <ItemTemplate>
                                                <asp:LinkButton Text='<%# Eval("Line") %>' runat="server" CommandArgument="<%# Container.DataItemIndex %>" ID="lnkLine" OnClick="lnkLine_Click" Font-Bold="true" ForeColor="#3333ff" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PONumber" HeaderText="Process Order" />
                                        <asp:BoundField DataField="MaterialDescrip" HeaderText="Material" />
                                        <asp:BoundField DataField="OrderQuantity" HeaderText="Order Quantity" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="ShiftProduction" HeaderText="Shift Production" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="TotalDeliveryQuantity" HeaderText="Total Delivery Quantity" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField HeaderText="Prod Qty MT" DataField="prodqtymt"/>
                                        <asp:BoundField HeaderText="Prod Qty MT"/>
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
    <script>

        getCurrentDate();


        function getCurrentDate() {

            //var curDate = new Date();

            //debugger;
            now = new Date();
            //UTCTime = now.toUTCString();
            var timezone = now.getTimezoneOffset() * 60000;
            var utc = now.getTime() + (now.getTimezoneOffset() * 60000);
            indotime = new Date(utc + (3600000 * 7));
            var CountryId = '<%= Session["CountryId"] %>';
            //alert(CountryId );
            if (CountryId == 1) {
                now = new Date();
                year = "" + now.getFullYear();
                month = "" + (now.getMonth() + 1); if (month.length == 1) { month = "0" + month; }
                day = "" + now.getDate(); if (day.length == 1) { day = "0" + day; }
                hour = "" + now.getHours(); if (hour.length == 1) { hour = "0" + hour; }
                minute = "" + now.getMinutes(); if (minute.length == 1) { minute = "0" + minute; }
                second = "" + now.getSeconds(); if (second.length == 1) { second = "0" + second; }
                var curDate = day + "-" + month + "-" + year + "  " + hour + ":" + minute + ":" + second;
                document.cookie = "ClientDateTime" + "=" + curDate;
                $('#<%=hdnfldVariable.ClientID%>').val(curDate);
            // hdnfldVariable.curDate = curDate;
            //alert(hdnfldVariable);
        }
        else if (CountryId == 2) {
            var now = new Date();
            var CurrentTime = new Date(indotime);
            //CurrentTime.setHours(CurrentTime.getHours());
            //CurrentTime.setMinutes(CurrentTime.getMinutes());

            var date = "" + CurrentTime.getDate(); if (date.length == 1) { date = "0" + date; }
            var hours = "" + CurrentTime.getHours(); if (hours.length == 1) { hours = "0" + hours; }
            var Minutes = "" + CurrentTime.getMinutes(); if (Minutes.length == 1) { Minutes = "0" + Minutes; }
            var second = "" + CurrentTime.getSeconds(); if (second.length == 1) { second = "0" + second; }
            var Month = "" + (CurrentTime.getMonth() + 1); if (Month.length == 1) { Month = "0" + Month; }
            var Year = "" + CurrentTime.getFullYear();
            var curDate = date + "-" + Month + "-" + Year + "  " + hours + ":" + Minutes + ":" + second;
            //var hdnfldVariable = document.getElementById('hdnfldVariable');
            //hdnfldVariable.value = curDate;
            //document.cookie = "ClientDateTime" + "=" + curDate;
            $('#<%=hdnfldVariable.ClientID%>').val(curDate);
                //alert(curDate);
                //alert(hdnfldVariable);
                setTimeout(getCurrentDate, 1000);
            }
        }
    </script>
</asp:Content>


