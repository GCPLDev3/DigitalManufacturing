<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOperator.aspx.cs" Inherits="MFG_DigitalApp.AddOperator" MasterPageFile="~/MasterPage.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box box-default" style="margin-left:10px;">
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
                                    <asp:BoundField DataField="EDPNumber" HeaderText="EDP No." />
                                    <asp:BoundField DataField="OperatorName" HeaderText="Name" />
                                    <asp:BoundField DataField="Type" HeaderText="Type" />
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
        <div class="box-body">
            <div class="row" style="margin-bottom: 10px; margin-top: 10px;">
                <div class="col-md-12 ">
                    <div class="col-md-3">
                        <asp:DropDownList ID="ddlPlantOperator" runat="server" CssClass="form-control"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvPlant" runat="server" ErrorMessage="Please select plant" ForeColor="Red" SetFocusOnError="true" ValidationGroup="validate" ControlToValidate="ddlPlantOperator" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtEdp" runat="server" placeholder="EDP Number" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvedp" runat="server" ErrorMessage="Please enter EDP number" ForeColor="Red" SetFocusOnError="true" ValidationGroup="validate" ControlToValidate="txtEdp"  Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtOperatorname" runat="server" placeholder="Oparator Name" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvoprname" runat="server" ErrorMessage="Please enter Operator name" ForeColor="Red" SetFocusOnError="true" ValidationGroup="validate" ControlToValidate="txtOperatorname"  Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-2">
                        <asp:DropDownList ID="drpType" runat="server" CssClass="form-control select2">
                                                                <%--<asp:ListItem Text="Select Type" Value=""></asp:ListItem>
                                                                <asp:ListItem Text="Operator" Value="Operator"></asp:ListItem>
                                                                <asp:ListItem Text="Shift Supervisor" Value="Shift Supervisor"></asp:ListItem>
                                                                <asp:ListItem Text="QC Supervisor" Value="QC Supervisor"></asp:ListItem>
                                                                <asp:ListItem Text="Maint Supervisor" Value="Maint Supervisor"></asp:ListItem>--%>
                                                            </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvType" runat="server" ErrorMessage="Please enter Type" ForeColor="Red" SetFocusOnError="true" ValidationGroup="validate" ControlToValidate="drpType"  Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-3"><asp:Button ID="btnSubmit" runat="server" Text="Add" OnClick="btnSubmit_Click" CssClass="btn btn-primary" ValidationGroup="validate" Width="100px"/>
                                <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" CssClass="btn btn-primary" Width="100px"/></div>
                    <%--<table class="table table-striped table-bordered table-hover">
                        <tr>
                            <td>
                                <asp:Label ID="lblPlant" Text="Plant" CssClass="form-group" runat="server"></asp:Label>
                            </td>
                            <td style="width:20%">
                                <asp:DropDownList ID="ddlPlantOperator" runat="server" CssClass="form-control"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblEdp" Text="EDP Number" runat="server" CssClass="form-group"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="txtEdp" runat="server" placeholder="EDP Number" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:Label ID="lblOperatorname" runat="server" Text="Operatore Name" CssClass="form-group"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="txtOperatorname" runat="server" placeholder="Oparator Name" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:Button ID="btnSubmit" runat="server" Text="Add" OnClick="btnSubmit_Click" CssClass="btn btn-primary" ValidationGroup="validate" />
                                <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" CssClass="btn btn-primary" />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td><asp:RequiredFieldValidator ID="rfvPlant" runat="server" ErrorMessage="Please select plant" SetFocusOnError="true" ValidationGroup="validate" ControlToValidate="ddlPlantOperator" Display="Dynamic"></asp:RequiredFieldValidator></td>
                            <td></td>
                            <td><asp:RequiredFieldValidator ID="rfvedp" runat="server" ErrorMessage="Please enter EDP number" SetFocusOnError="true" ValidationGroup="validate" ControlToValidate="txtEdp"  Display="Dynamic"></asp:RequiredFieldValidator></td>
                            <td></td>
                            <td><asp:RequiredFieldValidator ID="rfvoprname" runat="server" ErrorMessage="Please enter Operator name" SetFocusOnError="true" ValidationGroup="validate" ControlToValidate="txtOperatorname"  Display="Dynamic"></asp:RequiredFieldValidator></td>
                            <td></td>
                        </tr>
                    </table>--%>
                </div>
            </div>
        </div>
    </div>
    <div class="box box-default" style="margin-left:10px;">
        <div class="box-body" style="margin-left:10px;margin-right:10px;">
            <div class="row" style="margin-bottom: 10px; margin-top: 10px;">
                <div class="col-md-12">
                <div class="col-md-3"><asp:DropDownList ID="ddlPlant" runat="server" CssClass="form-control"></asp:DropDownList></div>
                        <div class="col-md-3"><asp:TextBox ID="txtsearch" runat="server" placeholder="EDP Number / Operator Name" CssClass="form-control" ></asp:TextBox></div>
                    <div class="col-md-3"><asp:DropDownList ID="ddlType" runat="server" CssClass="form-control"></asp:DropDownList></div>
                        <div class="col-md-3"><asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" CssClass="btn btn-primary"/>
                            <asp:Button ID="btnReset" runat="server" Text="Clear" OnClick="btnReset_Click" CssClass="btn btn-primary" />
                            <asp:Button ID="btnDelete" runat="server" Text="Delete Operator(s)" CssClass="btn btn-danger" OnClick="btnDelete_Click" OnClientClick="if(!UserDeleteConfirmation()) return false;" />
                        </div></div>
                </div>
            <div class="row">
            </div>
            <div class="row" style="margin-bottom: 10px; margin-top: 20px;">
                <div class="col-md-12">
                    <center>
                        <asp:GridView ID="grdOperator" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" DataKeyNames="Id"
                            OnRowCommand="grdOperator_RowCommand">
                            <EmptyDataTemplate>
                                <div align="center">No data found</div>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkCheckHeader" runat="server" onclick="HeaderCheckBoxClick(this);" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkCheck" runat="server" onclick="ChildCheckBoxClick(this);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("Id")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Plant">
                                    <ItemTemplate><asp:Label ID="lblPlant" runat="server" Text='<%#Eval("PlantCode") %>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="EDP Number">
                                    <ItemTemplate><asp:Label ID="lblEDP" runat="server" Text='<%#Eval("EDPNumber") %>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Operator Name">
                                    <ItemTemplate><asp:Label ID="lblOperator" runat="server" Text='<%#Eval("OperatorName") %>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Type">
                                    <ItemTemplate><asp:Label ID="lblType" runat="server" Text='<%#Eval("Type") %>'></asp:Label></ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:BoundField DataField="PlantCode" HeaderText="Plant" />
                                <asp:BoundField DataField="EDPNumber" HeaderText="EDP number" />
                                <asp:BoundField DataField="OperatorName" HeaderText="Operator Name" />
                                <asp:BoundField DataField="Type" HeaderText="Type" />--%>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:Button ID="btnEdit" CssClass="btn btn-info" Text="Edit" runat="server" CommandArgument='<%# Eval("Id").ToString()%>' CommandName="EditOperator" Visible='<%# Eval("Id").ToString() != "0" ? true : false %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:Button ID="btnDelete" CssClass="btn btn-info" Text="Delete" runat="server" CommandArgument='<%# Eval("Id").ToString()%>' CommandName="DeleteOperator" Visible='<%# Eval("Id").ToString() != "0" ? true : false %>' OnClientClick="return confirm('Are you sure you want to delete entry?');" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </center>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function HeaderCheckBoxClick(checkbox) {
            var gridView = document.getElementById("MainContent_grdOperator");
            for (i = 1; i < gridView.rows.length; i++) {
                gridView.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked
                    = checkbox.checked;
            }
        }

        function ChildCheckBoxClick(checkbox) {
            var atleastOneCheckBoxUnchecked = false;
            var gridView = document.getElementById("MainContent_grdOperator");

            for (i = 1; i < gridView.rows.length; i++) {
                if (gridView.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked
                    == false) {
                    atleastOneCheckBoxUnchecked = true;
                    break;
                }
            }

            gridView.rows[0].cells[0].getElementsByTagName("INPUT")[0].checked
                = !atleastOneCheckBoxUnchecked;
        }
    </script>
</asp:Content>
