namespace EasyWorkout.Identity.Api
{
    public static class AuthConstants
    {
        public const string AdminUserPolicyName = "Admin";
        public const string AdminUserClaimName = "admin";

        public const string PaidMemberUserPolicyName = "PaidMember";
        public const string PaidMemberUserClaimName = "paid_member";

        public const string FreeMemberUserPolicyName = "FreeMember";
        public const string FreeMemberUserClaimName = "free_member";
    }
}
