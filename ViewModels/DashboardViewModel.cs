namespace ERP_Consumer.ViewModels;

public class DashboardViewModel
{
    public int TotalCustomers { get; set; }
    public int TotalVehicles { get; set; }
    public int TotalCategories { get; set; }
    public int TotalServices { get; set; }
    public int TotalParts { get; set; }
    public bool ApiOnline { get; set; }
}
