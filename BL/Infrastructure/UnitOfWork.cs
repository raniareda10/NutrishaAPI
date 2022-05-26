using BL.Repositories;
using DL.DBContext;


namespace BL.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDBContext _ctx;
        public UnitOfWork(AppDBContext ctx)
        {
            _ctx = ctx;
            _ctx.ChangeTracker.LazyLoadingEnabled = true;
        }
        public UserRepository UserRepository => new UserRepository(_ctx);
        public UserRolesRepository UserRolesRepository => new UserRolesRepository(_ctx);
        public RoleRepository RoleRepository => new RoleRepository(_ctx);

        public NotificationRepository NotificationRepository => new NotificationRepository(_ctx);
        public ContactUsRepository ContactUsRepository => new ContactUsRepository(_ctx);

        public VerfiyCodeRepository VerfiyCodeRepository => new VerfiyCodeRepository(_ctx);
        public NotificationTypeRepository NotificationTypeRepository => new NotificationTypeRepository(_ctx);
        public NotificationUserRepository NotificationUserRepository => new NotificationUserRepository(_ctx);

        public UserGroupRepository UserGroupRepository => new UserGroupRepository(_ctx);
        public RolesGroupRepository RolesGroupRepository => new RolesGroupRepository(_ctx);
        public GroupRepository GroupRepository => new GroupRepository(_ctx);
        public JourneyPlanRepository JourneyPlanRepository => new JourneyPlanRepository(_ctx);
        public RiskRepository RiskRepository => new RiskRepository(_ctx);
        public GenderRepository GenderRepository => new GenderRepository(_ctx);
        public FrequencyRepository FrequencyRepository => new FrequencyRepository(_ctx);
        public AllergyRepository AllergyRepository => new AllergyRepository(_ctx);
        public GoalTypeRepository GoalTypeRepository => new GoalTypeRepository(_ctx);
        public UserGoalRepository UserGoalRepository => new UserGoalRepository(_ctx);
        public UserAllergyRepository UserAllergyRepository => new UserAllergyRepository(_ctx);
        public GoalRepository GoalRepository => new GoalRepository(_ctx);
        public UserRiskRepository UserRiskRepository => new UserRiskRepository(_ctx);
        public MealTypeRepository MealTypeRepository => new MealTypeRepository(_ctx);
        public IngredientRepository IngredientRepository => new IngredientRepository(_ctx);
        public FoodStepsRepository FoodStepsRepository => new FoodStepsRepository(_ctx);
        public SplashRepository SplashRepository => new SplashRepository(_ctx);

        public MealRepository MealRepository => new MealRepository(_ctx);
        virtual public DislikeMealRepository DislikeMealRepository => new DislikeMealRepository(_ctx);
        public MealStepsRepository MealStepsRepository => new MealStepsRepository(_ctx);
        public MealIngredientRepository MealIngredientRepository => new MealIngredientRepository(_ctx);
        public UserMealRepository UserMealRepository => new UserMealRepository(_ctx);
        public VideoRepository VideoRepository => new VideoRepository(_ctx);
        public MediaTypeRepository MediaTypeRepository => new MediaTypeRepository(_ctx);
        public AttachmentTypeRepository AttachmentTypeRepository => new AttachmentTypeRepository(_ctx);
        public SecUserRepository SecUserRepository => new SecUserRepository(_ctx);

        public void Dispose()
        {
            _ctx.Dispose();
        }

      

        public int Save()
        {
            return _ctx.SaveChanges();
        }
    }
}
