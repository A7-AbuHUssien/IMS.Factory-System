namespace IMS.Domain.Enums;

public enum TransactionSource
{
    ManualReceive = 1,        // استلام يدوي
    PurchaseOrder = 2,        // استلام من أمر شراء
    Sale = 3,                 // بيع
    SalesReturn = 4,          // مرتجع بيع
    InventoryAdjustment = 5,  // جرد
    Transfer = 6,             // تحويل بين مخازن
    Damage = 7,               // تالف
    SystemCorrection = 8      // تصحيح من النظام
}