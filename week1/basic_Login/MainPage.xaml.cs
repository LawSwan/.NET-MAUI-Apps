namespace basic_Login;

public partial class MainPage : ContentPage
{
    private const string ValidUserId = "Lawson";
    private const string ValidPassword = "Password1";

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnLoginClicked(object sender, EventArgs e)
    {
        string userId = txtUserName.Text;

        if (userId == ValidUserId && txtPassword.Text == ValidPassword)
        {
            lblMessage.Text = $"Login successful {userId}";
        }
        else
        {
            lblMessage.Text = $"Login failed {userId}";
        }
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        txtUserName.Text = string.Empty;
        txtPassword.Text = string.Empty;
        lblMessage.Text = string.Empty;
    }
}
