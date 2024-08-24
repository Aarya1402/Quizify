<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubjectDetails.aspx.cs" Inherits="Quizify.Subjects.SubjectDetails" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Subject Details - Quiz Web Application</title>
    <style>
        /* styles.css */

        /* General body styles */
        body {
            font-family: Arial, sans-serif;
            color: #333;
            background-color: #f9f9f9;
            margin: 0;
            padding: 0;
        }

        /* Header styles */
        header {
            background-color: #333;
            color: #fff;
            padding: 10px;
            text-align: center;
        }

        /* Main content area */
        main {
            margin: 20px;
        }

        /* Terms and Conditions section */
        .terms-and-conditions {
            background-color: #fff;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        }

            .terms-and-conditions h2 {
                color: #333;
                font-size: 24px;
                margin-bottom: 10px;
            }

            .terms-and-conditions p,
            .terms-and-conditions ul {
                color: #666;
                font-size: 16px;
                line-height: 1.6;
            }

            .terms-and-conditions ul {
                list-style-type: disc;
                margin-left: 20px;
            }

            /* Button and Checkbox styles */
            .terms-and-conditions .checkbox-container {
                margin-top: 20px;
            }

                .terms-and-conditions .checkbox-container label {
                    font-size: 16px;
                }

                .terms-and-conditions .checkbox-container input[type="checkbox"] {
                    margin-right: 10px;
                }

            .terms-and-conditions .submit-button {
                background-color: #333;
                color: #fff;
                padding: 10px 20px;
                border: none;
                border-radius: 4px;
                cursor: pointer;
                font-size: 16px;
            }

                .terms-and-conditions .submit-button:hover {
                    background-color: #555;
                }

        /* Footer styles */
        footer {
            background-color: #333;
            color: #fff;
            text-align: center;
            padding: 10px;
            position: fixed;
            bottom: 0;
            width: 100%;
        }
    </style>
</head>
<body>
    <!-- Header placeholder -->
    <header>
        <h1>Quizify</h1>
    </header>

    <form id="form1" runat="server">
        <main>
            <div class="terms-and-conditions">
                <h2>Terms and Conditions</h2>
                <p><strong>Welcome to Quizify!</strong> These Terms and Conditions outline the rules and regulations for the use of Quizify's services and website.</p>

                <p>By accessing or using our website, you agree to comply with and be bound by the following terms and conditions. If you do not agree with these terms, please do not use our website.</p>

                <h3>1. User Responsibilities</h3>
                <p>You are responsible for maintaining the confidentiality of your account information and for all activities that occur under your account. You agree to notify us immediately of any unauthorized use of your account or any other breach of security.</p>

                <h3>2. Prohibited Activities</h3>
                <p>You agree not to engage in any of the following prohibited activities:</p>
                <ul>
                    <li>Using our website for any unlawful purposes or activities.</li>
                    <li>Attempting to interfere with the proper functioning of our website.</li>
                    <li>Uploading or transmitting any viruses, worms, or harmful code.</li>
                    <li>Harassing or threatening other users.</li>
                </ul>

                <h3>3. Intellectual Property</h3>
                <p>All content, including but not limited to text, graphics, and logos, on our website is the property of Quizify or its licensors and is protected by copyright, trademark, and other intellectual property laws. You may not use, reproduce, or distribute any content from our website without our prior written consent.</p>

                <h3>4. Limitation of Liability</h3>
                <p>Quizify will not be liable for any indirect, incidental, special, or consequential damages arising from or related to your use of our website. Our total liability to you for any claim arising out of or related to these Terms and Conditions will be limited to the amount paid by you, if any, for accessing our website.</p>

                <h3>5. Changes to Terms</h3>
                <p>We reserve the right to modify these Terms and Conditions at any time. Any changes will be posted on this page with an updated effective date. Your continued use of our website after any changes indicates your acceptance of the new terms.</p>

                <h3>6. Governing Law</h3>
                <p>These Terms and Conditions are governed by and construed in accordance with the laws of the state or country where Quizify is based. Any disputes arising out of or related to these terms will be subject to the exclusive jurisdiction of the courts in that jurisdiction.</p>

                <p><strong>If you have any questions or concerns about these Terms and Conditions, please contact us at support@quizify.com.</strong></p>

                <div class="checkbox-container">
                    <asp:CheckBox ID="AgreeCheckBox" runat="server" Text="I agree to the terms and conditions" />
                </div>
                <asp:Button ID="SubmitButton" runat="server" Text="Register" CssClass="submit-button" OnClick="SubmitButton_Click" />
            </div>
            <div>
                <asp:Label runat="server" ID="SubjectLabel"  ></asp:Label>
            </div>
        </main>
    </form>

    <!-- Footer placeholder -->
    <footer>
        <p>&copy; 2024 Quizify. All rights reserved.</p>
    </footer>
</body>
</html>
