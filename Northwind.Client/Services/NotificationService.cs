using Radzen;

namespace Northwind.Client.Services;

internal sealed class NotificationService
{
    private readonly Radzen.NotificationService radzenNotificationService;

    public NotificationService(Radzen.NotificationService radzenNotificationService)
    {
        this.radzenNotificationService = radzenNotificationService;
    }

    public void NotifyError(string summary, string detail)
    {
        this.radzenNotificationService.Notify(new NotificationMessage
        {
            Severity = NotificationSeverity.Error,
            Summary = summary,
            Detail = detail,
            Duration = 5000
        });
    }
}
