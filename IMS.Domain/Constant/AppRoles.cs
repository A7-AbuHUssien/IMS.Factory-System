namespace IMS.Domain.Constant;

public static class AppRoles
{
    public const string Admin = "ADMIN";
    public const string Manager = "MANAGER";
    public const string WarehouseManager = "WAREHOUSE_MANAGER";
    public const string User = "USER";

    public static readonly string[] All = [Admin, Manager, WarehouseManager, User];
}