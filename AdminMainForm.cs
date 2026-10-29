namespace CinemaManagement;

public partial class AdminMainForm : RoleDashboardForm
{
    public AdminMainForm() : this(new UserSession { UserId = 1, Username = "admin", FullName = "Quản trị viên", Role = "Admin" }) { }
    public AdminMainForm(UserSession user) : base(user, "Admin") { }
}
