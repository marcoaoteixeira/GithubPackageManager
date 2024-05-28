using CommunityToolkit.Mvvm.Messaging;
using Nameless.Infrastructure;
using Nameless.Objects;

namespace Nameless.Impl.Infrastructure {
    public sealed class PubSubService : IPubSubService {
        #region Private Read-Only Fields

        private readonly IMessenger _messenger;

        #endregion

        #region Public Constructors

        public PubSubService(IMessenger messenger) {
            ArgumentNullException.ThrowIfNull(messenger, nameof(messenger));

            _messenger = messenger;
        }

        #endregion

        #region IPubSubService Members

        public void Subscribe<TNotification>(object recipient, Action<object, TNotification> handler)
            where TNotification : Notification
            => _messenger.Register(recipient, (object handlerRecipient, TNotification handlerNotification) => handler(handlerRecipient, handlerNotification));

        public void Unsubscribe<TNotification>(object recipient)
            where TNotification : Notification
            => _messenger.Unregister<TNotification>(recipient);

        public Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
            where TNotification : Notification
            => Task.Run(() => _messenger.Send(notification), cancellationToken);

        #endregion
    }
}
