using RunDiscord.Models.Settings;
using System.Net.Sockets;

namespace RunDiscord.Helpers
{
    public static class ProxyHelper
    {
        public static async Task WaitForProxyAsync(ProxySettings proxySettings,
            int retryDelayInSeconds = 1, CancellationToken cancellationToken = default)
        {
            var timeoutCts = new CancellationTokenSource(proxySettings.TimeoutInMilliseconds);
            var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);
            var linkedToken = linkedCts.Token;

            while (true)
            {
                try
                {
                    linkedToken.ThrowIfCancellationRequested();

                    using var client = new TcpClient();
                    var connectTask = client.ConnectAsync(proxySettings.Host, proxySettings.Port, linkedToken).AsTask();

                    var completedTask = await Task.WhenAny(
                        connectTask,
                        Task.Delay(TimeSpan.FromSeconds(retryDelayInSeconds), linkedToken))
                        .ConfigureAwait(false);

                    if (completedTask == connectTask)
                    {
                        await connectTask.ConfigureAwait(false);
                        return;
                    }
                }
                catch (Exception) when (!linkedToken.IsCancellationRequested)
                {
                    continue;
                }
                catch (Exception) when (timeoutCts.IsCancellationRequested)
                {
                    throw new TimeoutException(
                        $"Proxy at {proxySettings.Host}:{proxySettings.Port} did not start " +
                        $"within {TimeSpan.FromMilliseconds(proxySettings.TimeoutInMilliseconds).TotalSeconds} seconds.");
                }
            }
        }
    }
}
