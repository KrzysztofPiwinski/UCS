namespace UCS.Web.ViewModels
{
    public class ChangePasswordViewModel : LayoutViewModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}