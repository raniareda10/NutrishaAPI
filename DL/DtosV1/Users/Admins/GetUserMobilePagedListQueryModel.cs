using DL.CommonModels;

namespace DL.DtosV1.Users.Admins
{
    public class GetUserMobilePagedListQueryModel : GetPagedListQueryModel
    {
        public bool OnlyUserWithoutPlan { get; set; }
        public bool UserWithAboutToFinishPlan { get; set; }
    }
}