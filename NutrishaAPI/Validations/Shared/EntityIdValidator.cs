using DL.HelperInterfaces;

namespace NutrishaAPI.Validations.Shared
{
    public static class EntityIdValidator
    {
        public static bool IsValidEntityId<T>(this T dto) where T: IEntityId
        {
            return dto.EntityId > 0;
        }
    }
}