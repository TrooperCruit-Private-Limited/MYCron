using Microsoft.AspNetCore.Http;
using MYCentralModels.SQLite;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYCentralModels
{
    public static class CommonLibrary
    {

    }
    public class KeyValueModel
    {
        public required string Key { get; set; }
        public required string Value { get; set; }
    }
    public class BoolKeyValueModel
    {
        public required bool YesNo { get; set; }
        public required string Value { get; set; }
    }
    public class TransactionStatus
    {
        public int Status { get; set; }
        public string? Message { get; set; }
    }
    public class OTPModel
    {
        public string? OTP { get; set; }
        public string? OTPGeneratedOn { get; set; }
        public string? OTPValidityDurationMinutes { get; set; }
        public string? OTPValidUpTo { get; set; }
    }
    public class SignUp
    {
        public string? UserName { get; set; }
        public string? EMail { get; set; }
        public string? Phone { get; set; }
        public string? Password { get; set; }
    }
    public class SignIn
    {
        [DisplayName("EMail / Phone Number")]
        [Required(ErrorMessage = "EMail / Phone Number is required.")]
        public string? EMailorPhone { get; set; }
        [DisplayName("Password")]
        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }
    }
    public class AuthenticatedUser
    {
        public TransactionStatus? Status { get; set; }
        public UserSession? UserSessionData { get; set; }
    }
    public class Users
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? EMail { get; set; }
        public string? Phone { get; set; }
        public string? Password { get; set; }
        public string? OTP { get; set; }
        public DateTime OTPExpiryOn { get; set; }
        public DateTime SignedUpOn { get; set; }
        public DateTime LastSignedInOn { get; set; }
        public int Active { get; set; }
        public int UserType { get; set; }
    }
    [Table("EmployeesOnSite")]
    public class EmployeeOnSite
    {
        public int Id { get; set; }
        [Display(Name = "Employee Name")]
        public string? EmployeeName { get; set; }
        [Display(Name = "Designation")]
        public string? Designation { get; set; }
        [Display(Name = "Display Picture")]
        public string? DisplayPicture { get; set; }
        [Display(Name = "State")]
        public int Active { get; set; }
    }
    public class VisitUsInformationModel
    {
        public int Id { get; set; }
        public string? Icon { get; set; }
        public string? ImageType { get; set; }
        public string? Name { get; set; }
        public string? Link { get; set; }
        public int Active { get; set; }
    }
    public class SiteVisits
    {
        public int Id { get; set; }
        public string? VisitorIP { get; set; }
        public DateTime? VisitedOn { get; set; }
        public string? PageName { get; set; }
    }
}
