using Nameless.Objects;

namespace Nameless.Infrastructure {
    public interface IPubSubService {
        #region Methods

        void Subscribe<TNotification>(object recipient, Action<object, TNotification> handler)
            where TNotification : Notification;

        void Unsubscribe<TNotification>(object recipient)
            where TNotification : Notification;

        Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken)
            where TNotification : Notification;

        #endregion
    }
}
