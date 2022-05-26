using DL.CommonModels.Paging;

namespace NutrishaAPI.Validations.Shared
{
    public static class PagedValidator
    {
        public static bool IsValidPagedModel(this PagedModel model)
        {
            return model.PageNumber > 0 && model.PageSize > 0;
        }
    }
}