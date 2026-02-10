namespace IMS.Infrastructure.Persistence.SD;

public class StaticData
{
    public const string MANAGER_ROLE = "SuperAdmin";
    public const string ADMIN_ROLE = "Admin";
    public const string MAIN_WAREHOUSE_ROLE = "Employee";

    // ضيف الـ IDs دي ضروري جداً
    public static readonly Guid AdminRoleId = Guid.NewGuid();
    public static readonly Guid ManagerRoleId = Guid.NewGuid();
    public static readonly Guid EmployeeRoleId = Guid.NewGuid();
    
    public static readonly Guid AdminUserId = Guid.NewGuid();
    public static readonly Guid MainWarehouseId = Guid.NewGuid();
}