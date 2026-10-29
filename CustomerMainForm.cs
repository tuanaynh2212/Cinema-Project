namespace CinemaManagement;

public partial class CustomerMainForm : RoleDashboardForm
{
    public CustomerMainForm() : this(new UserSession { UserId = 3, Username = "customer", FullName = "Khách hàng", Role = "Customer" }) { }
    public CustomerMainForm(UserSession user) : base(user, "Customer") { }
}
