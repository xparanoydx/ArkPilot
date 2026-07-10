using ArkPilot.Services;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ArkPilot.Core
{
    public class TaskQueueService
    {
        private readonly ConcurrentQueue<(string Name, Func<Task> Task)> queue = new();

        private bool running;

        public async Task Enqueue(
            string name,
            Func<Task> task)
        {
            queue.Enqueue((name, task));

            LogService.Info($"Tâche ajoutée : {name}");

            if (!running)
                await ProcessQueue();
        }

        private async Task ProcessQueue()
        {
            running = true;

            while (queue.TryDequeue(out var item))
            {
                try
                {
                    LogService.Info($"▶ Début : {item.Name}");

                    await item.Task();

                    LogService.Info($"✔ Terminé : {item.Name}");
                }
                catch (Exception ex)
                {
                    LogService.Error($"✖ {item.Name} : {ex.Message}");
                }
            }

            running = false;
        }
    }
}