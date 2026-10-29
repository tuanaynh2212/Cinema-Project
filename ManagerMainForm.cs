namespace CinemaManagement;

public partial class ManagerMainForm : RoleDashboardForm
{
    public ManagerMainForm() : this(new UserSession { UserId = 2, Username = "manager", FullName = "Quản lý rạp", Role = "Manager" }) { }
    public ManagerMainForm(UserSession user) : base(user, "Manager") { }
}
