<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Quizify.Subjects.Home" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Home - Quiz Web Application</title>
    <!-- Add your CSS files here -->
</head>
<body>
    <!-- Header placeholder -->
    <!-- Add your header here -->

    <form id="form1" runat="server">
        <div>
            <h1>Quiz Subjects</h1>
            <asp:GridView ID="SubjectsGridView" runat="server" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="Name" HeaderText="Subject Name" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HyperLink ID="SubjectLink" runat="server" Text='<%# Eval("Name") %>' NavigateUrl='<%# "SubjectDetails.aspx?id=" + Eval("Id") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>

    <!-- Footer placeholder -->
    <!-- Add your footer here -->
</body>
</html>
