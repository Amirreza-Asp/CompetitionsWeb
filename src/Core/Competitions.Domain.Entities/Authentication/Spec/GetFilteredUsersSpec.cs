namespace Competitions.Domain.Entities.Authentication.Spec
{
    public class GetFilteredUsersSpec : BaseSpecification<User>
    {
        public GetFilteredUsersSpec ( String name , String family , Guid? roleId , String nationalCode , int skip , int take )
        {
            ApplyCriteria(entity =>
                entity.Role.Title != "User" &&
                ( String.IsNullOrEmpty(name) || entity.Name.Equals(name) ) &&
                ( String.IsNullOrEmpty(family) || entity.Family.Equals(family) ) &&
                ( !roleId.HasValue || entity.RoleId.Equals(roleId) ) &&
                ( String.IsNullOrEmpty(nationalCode) || entity.NationalCode.Value.Equals(nationalCode) ));


            ApplyPaging(skip , take);

            AddInclude(u => u.Role);
        }
    }
}
