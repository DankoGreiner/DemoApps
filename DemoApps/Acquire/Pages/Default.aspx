<%@ Page Title="" Language="C#" MasterPageFile="~/Acquire/ACQMaster.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DemoApps.Acquire.Pages.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:UpdatePanel ID="updMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            AACCQQUUIIRREE
 <hr />
            <asp:GridView ID="gvGameList" runat="server" Visible="false"></asp:GridView>
            <hr />

            <table border="0">
                <tr>
                    <td align="center">
                        <b>Hotels</b>
                    </td>
                    <td></td>
                    <td width="30px"></td>
                    <td>
                        <b>Stock price</b>
                    </td>
                    <td width="200px" align="center">
                        <b>Holders</b>
                    </td>
                </tr>

                <tr>
                    <td align="center">
                        <img src="Images/Sackson.png" height="50px" />
                    </td>
                    <td>
                        <asp:Button ID="btnStartSackson" runat="server" Text="Start" OnClick="btnStartSackson_Click" />
                        <asp:TextBox ID="txtSacksonCount" runat="server" Width="50px" Enabled="false" Text="0"></asp:TextBox>
                        <asp:Button ID="btnAddSackson" runat="server" Text="+" OnClick="btnAddSackson_Click" />
                        <asp:Button ID="btnResetSackson" runat="server" Text="Reset" OnClick="btnResetSackson_Click" />
                    </td>
                    <td></td>
                    <td align="right">
                        <asp:Label ID="lblSacksonPrice" runat="server"></asp:Label>
                    </td>
                    <td align="center">
                        <asp:Label ID="lblSacksonHolderValues" runat="server"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td align="center">
                        <img src="Images/Tower.png" height="50px" />
                    </td>
                    <td>
                        <asp:Button ID="btnStartTower" runat="server" Text="Start" OnClick="btnStartTower_Click" />
                        <asp:TextBox ID="txtTowerCount" runat="server" Width="50px" Enabled="false" Text="0"></asp:TextBox>
                        <asp:Button ID="btnAddTower" runat="server" Text="+" OnClick="btnAddTower_Click" />
                        <asp:Button ID="btnResetTower" runat="server" Text="Reset" OnClick="btnResetTower_Click" />
                    </td>
                    <td></td>
                    <td align="right">
                        <asp:Label ID="lblTowerPrice" runat="server"></asp:Label>
                    </td>
                    <td align="center">
                        <asp:Label ID="lblTowerHolderValues" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <img src="Images/American.png" height="50px" />
                    </td>
                    <td>
                        <asp:Button ID="btnStartAmerican" runat="server" Text="Start" OnClick="btnStartAmerican_Click" />
                        <asp:TextBox ID="txtAmericanCount" runat="server" Width="50px" Enabled="false" Text="0"></asp:TextBox>
                        <asp:Button ID="btnAddAmerican" runat="server" Text="+" OnClick="btnAddAmerican_Click" />
                        <asp:Button ID="btnResetAmerican" runat="server" Text="Reset" OnClick="btnResetAmerican_Click" />
                    </td>
                    <td></td>
                    <td align="right">
                        <asp:Label ID="lblAmericanPrice" runat="server"></asp:Label>
                    </td>
                    <td align="center">
                        <asp:Label ID="lblAmericanHolderValues" runat="server"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td align="center">
                        <img src="Images/Festival.png" height="50px" />
                    </td>
                    <td>
                        <asp:Button ID="btnStartFestival" runat="server" Text="Start" OnClick="btnStartFestival_Click" />
                        <asp:TextBox ID="txtFestivalCount" runat="server" Width="50px" Enabled="false" Text="0"></asp:TextBox>
                        <asp:Button ID="btnAddFestival" runat="server" Text="+" OnClick="btnAddFestival_Click" />
                        <asp:Button ID="btnResetFestival" runat="server" Text="Reset" OnClick="btnResetFestival_Click" />
                    </td>
                    <td></td>
                    <td align="right">
                        <asp:Label ID="lblFestivalPrice" runat="server"></asp:Label>
                    </td>
                    <td align="center">
                        <asp:Label ID="lblFestivalHolderValues" runat="server"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td align="center">
                        <img src="Images/Worldwide.png" height="50px" />
                    </td>
                    <td>
                        <asp:Button ID="btnStartWorldwide" runat="server" Text="Start" OnClick="btnStartWorldwide_Click" />
                        <asp:TextBox ID="txtWorldwideCount" runat="server" Width="50px" Enabled="false" Text="0"></asp:TextBox>
                        <asp:Button ID="btnAddWorldwide" runat="server" Text="+" OnClick="btnAddWorldwide_Click" />
                        <asp:Button ID="btnResetWorldwide" runat="server" Text="Reset" OnClick="btnResetWorldwide_Click" />
                    </td>
                    <td></td>
                    <td align="right">
                        <asp:Label ID="lblWorldwidePrice" runat="server"></asp:Label>
                    </td>
                    <td align="center">
                        <asp:Label ID="lbllWorldwideHolderValues" runat="server"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td colspan="3">
                        <hr />
                    </td>
                </tr>

                <tr>
                    <td align="center">
                        <img src="Images/Continental.png" height="50px" />
                    </td>
                    <td>
                        <asp:Button ID="btnStartContinental" runat="server" Text="Start" OnClick="btnStartContinental_Click" />
                        <asp:TextBox ID="txtContinentalCount" runat="server" Width="50px" Enabled="false" Text="0"></asp:TextBox>
                        <asp:Button ID="btnAddContinental" runat="server" Text="+" OnClick="btnAddContinental_Click" />
                        <asp:Button ID="btnResetContinental" runat="server" Text="Reset" OnClick="btnResetContinental_Click" />
                    </td>
                    <td></td>
                    <td align="right">
                        <asp:Label ID="lblContinentalPrice" runat="server"></asp:Label>
                    </td>
                    <td align="center">
                        <asp:Label ID="lblContinentalHolderValues" runat="server"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td align="center">
                        <img src="Images/Imperial.png" height="50px" />
                    </td>
                    <td>
                        <asp:Button ID="btnStartImperial" runat="server" Text="Start" OnClick="btnStartImperial_Click" />
                        <asp:TextBox ID="txtImperialCount" runat="server" Width="50px" Enabled="false" Text="0"></asp:TextBox>
                        <asp:Button ID="btnAddImperial" runat="server" Text="+" OnClick="btnAddImperial_Click" />
                        <asp:Button ID="btnResetImperial" runat="server" Text="Reset" OnClick="btnResetImperial_Click" />
                    </td>
                    <td></td>
                    <td align="right">
                        <asp:Label ID="lblImperialPrice" runat="server"></asp:Label>
                    </td>
                    <td align="center">
                        <asp:Label ID="lblImperialHolderValues" runat="server"></asp:Label>
                    </td>
                </tr>



            </table>

        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBbodyEnd" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphCustomPageScript" runat="server">
</asp:Content>
